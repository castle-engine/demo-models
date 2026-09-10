#!/usr/bin/env python3
"""
Generate text_literal_alignment.ifc: a small IFC4 model that shows all
9 possible IfcBoxAlignment values of IfcTextLiteralWithExtent.

Each text is accompanied by 2 guides, drawn using IfcPolyline:

- a cross at the text placement (IfcTextLiteral.Placement combined with
  the IfcAnnotation.ObjectPlacement),

- the rectangle of the extent (IfcTextLiteralWithExtent.Extent), positioned
  as the BoxAlignment says.

This makes it easy to see the meaning of BoxAlignment: it names the corner
(or edge middle) of the extent box that lies at the placement, and the text
is aligned to that very same corner.

Requires ifcopenshell ("pip install ifcopenshell").
Afterwards convert to IFC JSON encoding using
https://github.com/michaliskambi/ifcJSON :

  python ifcJSON/file_converters/ifc2json.py -i text_literal_alignment.ifc -o text_literal_alignment.ifcjson

Claude generated. Trust this accordingly.
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


def polyline(points):
    return f.create_entity("IfcPolyline", Points=[point(p) for p in points])


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
    Name="Text Literal Alignment Test",
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

# --- the annotations ---

products = []


def add_annotation(name, items, at, object_type=None):
    """Add one IfcAnnotation with the given representation items."""
    annotation = f.create_entity(
        "IfcAnnotation",
        GlobalId=guid(),
        OwnerHistory=owner_history,
        Name=name,
        ObjectType=object_type,
        ObjectPlacement=local_placement(at, relative_to=storey_placement),
        Representation=f.create_entity(
            "IfcProductDefinitionShape",
            Representations=[
                f.create_entity(
                    "IfcShapeRepresentation",
                    ContextOfItems=annotation_context,
                    RepresentationIdentifier="Annotation",
                    RepresentationType="Annotation2D",
                    Items=items,
                )
            ],
        ),
    )
    products.append(annotation)
    return annotation


# Size of the IfcPlanarExtent box of every text below.
SIZE_X = 8.0
SIZE_Y = 2.0

# Half-size of the cross drawn at the text placement.
CROSS = 0.5


def box_corner(box_alignment):
    """Lower-left corner of the extent box, relative to the text placement.

    This follows the IfcBoxAlignment definition: the named corner of the box
    lies exactly at the placement.
    """
    if box_alignment == "center":
        vertical, horizontal = "middle", "middle"
    else:
        vertical, horizontal = box_alignment.split("-")

    x = {"left": 0.0, "middle": -SIZE_X / 2, "right": -SIZE_X}[horizontal]
    y = {"bottom": 0.0, "middle": -SIZE_Y / 2, "top": -SIZE_Y}[vertical]
    return x, y


def add_text(name, literal, at, box_alignment, path="RIGHT", guides=True):
    """Add one text (IfcAnnotation with IfcTextLiteralWithExtent inside).

    When guides, add a second IfcAnnotation that draws the placement cross
    and the extent rectangle, using IfcPolyline.
    """
    add_annotation(
        name,
        [
            f.create_entity(
                "IfcTextLiteralWithExtent",
                Literal=literal,
                # Keep the literal placement identity, put the position
                # in the IfcAnnotation.ObjectPlacement.
                # This is what BonsaiBIM and FreeCAD do.
                Placement=axis3d(),
                Path=path,
                Extent=f.create_entity(
                    "IfcPlanarExtent", SizeInX=SIZE_X, SizeInY=SIZE_Y
                ),
                BoxAlignment=box_alignment,
            )
        ],
        # Lift the texts a bit above the guides, to avoid Z-fighting.
        (at[0], at[1], 0.01),
        object_type="TEXT",
    )

    if guides:
        x, y = box_corner(box_alignment)
        add_annotation(
            name + " guides",
            [
                # cross at the text placement
                polyline([(-CROSS, 0, 0), (CROSS, 0, 0)]),
                polyline([(0, -CROSS, 0), (0, CROSS, 0)]),
                # rectangle of the extent
                polyline(
                    [
                        (x, y, 0),
                        (x + SIZE_X, y, 0),
                        (x + SIZE_X, y + SIZE_Y, 0),
                        (x, y + SIZE_Y, 0),
                        (x, y, 0),
                    ]
                ),
            ],
            (at[0], at[1], 0.0),
        )


# 1. All 9 IfcBoxAlignment values, laid out in the same 3x3 arrangement
# as the figure in the IfcBoxAlignment specification.

ALIGNMENTS = [
    ["top-left", "top-middle", "top-right"],
    ["middle-left", "center", "middle-right"],
    ["bottom-left", "bottom-middle", "bottom-right"],
]

for row_index, row in enumerate(ALIGNMENTS):
    for column_index, box_alignment in enumerate(row):
        add_text(
            "Text " + box_alignment,
            box_alignment,
            (column_index * 16.0, -row_index * 8.0),
            box_alignment,
        )

# 2. Multi-line texts, to show how the text grows when lines are added:
# down from a "top-..." placement, up from a "bottom-..." placement,
# in both directions from a "middle-..." placement.

for column_index, box_alignment in enumerate(["top-left", "center", "bottom-left"]):
    add_text(
        "Multi-line " + box_alignment,
        "multi-line\n" + box_alignment + "\ngrows this way",
        (column_index * 16.0, -28.0),
        box_alignment,
    )

# 3. All 4 IfcTextPath values (writing directions).
#
# Note: Castle Game Engine ignores the X3D fields expressing the writing
# direction (horizontal, leftToRight, topToBottom) when rendering, it always
# renders horizontal left-to-right text. But it does account for the Path when
# converting the BoxAlignment to the X3D justify (which is relative to the
# writing direction). So these 4 texts are deliberately *not* placed correctly
# by our renderer -- they show the X3D data is right, and the renderer
# has yet to catch up. See README.md.

for column_index, path in enumerate(["RIGHT", "LEFT", "UP", "DOWN"]):
    add_text(
        "Path " + path,
        "path " + path,
        (column_index * 16.0, -38.0),
        "bottom-left",
        path=path,
    )

f.create_entity(
    "IfcRelContainedInSpatialStructure",
    GlobalId=guid(),
    OwnerHistory=owner_history,
    RelatingStructure=storey,
    RelatedElements=products,
)

f.write("text_literal_alignment.ifc")
print("Written text_literal_alignment.ifc")
