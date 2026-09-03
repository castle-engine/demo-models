#!/usr/bin/env python3
"""
Generate text_literal.ifc: a small IFC4 model that displays text,
using IfcTextLiteral and IfcTextLiteralWithExtent inside IfcAnnotation.

This follows the conventions used by BonsaiBIM and FreeCAD, so that the
resulting file can be opened by them, as well as by Castle Game Engine.

Requires ifcopenshell ("pip install ifcopenshell").
Afterwards convert to IFC JSON encoding using
https://github.com/michaliskambi/ifcJSON :

  python ifcJSON/file_converters/ifc2json.py -i text_literal.ifc -o text_literal.ifcjson

Claude generated, though reviewed. Trust this accordingly.
"""

import time

import ifcopenshell
import ifcopenshell.guid

f = ifcopenshell.file(schema="IFC4")


def guid():
    return ifcopenshell.guid.new()


def point(coords):
    return f.create_entity("IfcCartesianPoint", Coordinates=[float(c) for c in coords])


def direction(coords):
    return f.create_entity("IfcDirection", DirectionRatios=[float(c) for c in coords])


def axis3d(location=(0, 0, 0), axis=None, ref_direction=None):
    return f.create_entity(
        "IfcAxis2Placement3D",
        Location=point(location),
        Axis=direction(axis) if axis else None,
        RefDirection=direction(ref_direction) if ref_direction else None,
    )


def local_placement(location=(0, 0, 0), relative_to=None, axis=None, ref_direction=None):
    return f.create_entity(
        "IfcLocalPlacement",
        PlacementRelTo=relative_to,
        RelativePlacement=axis3d(location, axis, ref_direction),
    )


# --- owner history (optional in IFC4, but authoring applications like to see it) ---

person = f.create_entity("IfcPerson", FamilyName="Kamburelis", GivenName="Michalis")
organization = f.create_entity("IfcOrganization", Name="Castle Game Engine")
person_and_org = f.create_entity(
    "IfcPersonAndOrganization", ThePerson=person, TheOrganization=organization
)
application = f.create_entity(
    "IfcApplication",
    ApplicationDeveloper=organization,
    Version="7.0",
    ApplicationFullName="Castle Game Engine",
    ApplicationIdentifier="CGE",
)
owner_history = f.create_entity(
    "IfcOwnerHistory",
    OwningUser=person_and_org,
    OwningApplication=application,
    ChangeAction="ADDED",
    CreationDate=int(time.time()),
)

# --- units: metric, lengths in meters ---

units = f.create_entity(
    "IfcUnitAssignment",
    Units=[
        f.create_entity("IfcSIUnit", UnitType="LENGTHUNIT", Name="METRE"),
        f.create_entity("IfcSIUnit", UnitType="AREAUNIT", Name="SQUARE_METRE"),
        f.create_entity("IfcSIUnit", UnitType="VOLUMEUNIT", Name="CUBIC_METRE"),
        f.create_entity("IfcSIUnit", UnitType="PLANEANGLEUNIT", Name="RADIAN"),
    ],
)

# --- representation contexts ---

model_context = f.create_entity(
    "IfcGeometricRepresentationContext",
    ContextType="Model",
    CoordinateSpaceDimension=3,
    Precision=1e-5,
    WorldCoordinateSystem=axis3d(),
)
body_context = f.create_entity(
    "IfcGeometricRepresentationSubContext",
    ContextIdentifier="Body",
    ContextType="Model",
    ParentContext=model_context,
    TargetView="MODEL_VIEW",
)
# Annotations (like our texts) belong in a subcontext with
# ContextIdentifier = 'Annotation'. This is what BonsaiBIM and FreeCAD use.
annotation_context = f.create_entity(
    "IfcGeometricRepresentationSubContext",
    ContextIdentifier="Annotation",
    ContextType="Model",
    ParentContext=model_context,
    TargetView="MODEL_VIEW",
)

project = f.create_entity(
    "IfcProject",
    GlobalId=guid(),
    OwnerHistory=owner_history,
    Name="Text Literal Test",
    RepresentationContexts=[model_context],
    UnitsInContext=units,
)

# --- spatial structure: project -> site -> building -> storey ---

site_placement = local_placement()
site = f.create_entity(
    "IfcSite",
    GlobalId=guid(),
    OwnerHistory=owner_history,
    Name="Site",
    ObjectPlacement=site_placement,
    CompositionType="ELEMENT",
)
building_placement = local_placement(relative_to=site_placement)
building = f.create_entity(
    "IfcBuilding",
    GlobalId=guid(),
    OwnerHistory=owner_history,
    Name="Building",
    ObjectPlacement=building_placement,
    CompositionType="ELEMENT",
)
storey_placement = local_placement(relative_to=building_placement)
storey = f.create_entity(
    "IfcBuildingStorey",
    GlobalId=guid(),
    OwnerHistory=owner_history,
    Name="Storey",
    ObjectPlacement=storey_placement,
    CompositionType="ELEMENT",
)


def aggregate(parent, children):
    f.create_entity(
        "IfcRelAggregates",
        GlobalId=guid(),
        OwnerHistory=owner_history,
        RelatingObject=parent,
        RelatedObjects=children,
    )


aggregate(project, [site])
aggregate(site, [building])
aggregate(building, [storey])

# --- the texts ---

products = []


def add_text(
    name,
    literal,
    object_placement_at=(0, 0, 0),
    literal_placement=None,
    path="RIGHT",
    extent=None,
    box_alignment=None,
):
    """Add one IfcAnnotation with a text literal inside."""
    if literal_placement is None:
        literal_placement = axis3d()

    if extent is None:
        text_item = f.create_entity(
            "IfcTextLiteral", Literal=literal, Placement=literal_placement, Path=path
        )
    else:
        text_item = f.create_entity(
            "IfcTextLiteralWithExtent",
            Literal=literal,
            Placement=literal_placement,
            Path=path,
            Extent=f.create_entity(
                "IfcPlanarExtent", SizeInX=float(extent[0]), SizeInY=float(extent[1])
            ),
            BoxAlignment=box_alignment,
        )

    annotation = f.create_entity(
        "IfcAnnotation",
        GlobalId=guid(),
        OwnerHistory=owner_history,
        Name=name,
        # 'TEXT' is what BonsaiBIM and FreeCAD use to mark a text annotation
        ObjectType="TEXT",
        ObjectPlacement=local_placement(object_placement_at, relative_to=storey_placement),
        Representation=f.create_entity(
            "IfcProductDefinitionShape",
            Representations=[
                f.create_entity(
                    "IfcShapeRepresentation",
                    ContextOfItems=annotation_context,
                    RepresentationIdentifier="Annotation",
                    RepresentationType="Annotation2D",
                    Items=[text_item],
                )
            ],
        ),
    )
    products.append(annotation)
    return annotation


# A: position expressed by the placement of the IfcTextLiteral itself.
add_text(
    "Text A",
    "A: literal placement",
    literal_placement=axis3d((-12, 4.5, 0.01)),
)

# B: position expressed by the placement of the containing IfcAnnotation,
# the IfcTextLiteral placement is identity. This is what BonsaiBIM and FreeCAD do.
add_text(
    "Text B",
    "B: object placement",
    object_placement_at=(-12, 1.5, 0.01),
    path="LEFT",
)

# C: IfcTextLiteralWithExtent, which is what BonsaiBIM writes.
add_text(
    "Text C",
    "C: with extent",
    object_placement_at=(-12, -1.5, 0.01),
    extent=(6.0, 1.0),
    box_alignment="bottom-left",
)

# D: rotated placement, so the text stands vertically (in the XZ plane).
# Also multi-line, to test that we honor the newlines inside the literal.
add_text(
    "Text D",
    "D: rotated\nand multi-line",
    object_placement_at=(-12, -5.5, 0.01),
    literal_placement=axis3d(axis=(0, -1, 0), ref_direction=(1, 0, 0)),
)

f.create_entity(
    "IfcRelContainedInSpatialStructure",
    GlobalId=guid(),
    OwnerHistory=owner_history,
    RelatingStructure=storey,
    RelatedElements=products,
)

f.write("text_literal.ifc")
print("Written text_literal.ifc")
