#!/usr/bin/env python3
"""
Claude generated. Trust this accordingly.

Generate text_literal_visible_in_bonsai_bim.ifc: like text_literal.ifc,
but arranged so that BonsaiBIM actually *displays* the texts.

BonsaiBIM never turns an IfcTextLiteral into 3D geometry. It draws texts only
as a viewport overlay, and only for annotations that belong to a "drawing".
So, compared to text_literal.ifc, this file adds:

- An IfcAnnotation with ObjectType = 'DRAWING', which BonsaiBIM turns into
  a Blender camera. It needs a 'Body' representation in the Model / MODEL_VIEW
  context, holding an IfcCsgSolid of an IfcBlock: that box is the camera
  volume (X, Y give the camera size, Z the far clipping plane).
- An EPset_Drawing property set on that drawing, with TargetView, Scale etc.
- An IfcGroup with ObjectType = 'DRAWING', grouping the drawing together with
  all the texts. This is how BonsaiBIM knows which annotations belong to
  which drawing.
- An IfcGroup with ObjectType = 'DRAWINGS', the parent group of all drawings.

and changes:

- All texts are IfcTextLiteralWithExtent, never plain IfcTextLiteral.
  BonsaiBIM reads literal.BoxAlignment unconditionally, so a plain
  IfcTextLiteral would raise an error in its viewport overlay code.
- All texts are positioned by IfcAnnotation.ObjectPlacement, not by
  IfcTextLiteral.Placement. BonsaiBIM draws the text at the object origin
  and ignores the placement of the literal.
- Every text gets an EPset_Annotation property set with Classes = 'title'.
  This is how BonsaiBIM expresses the font size ('small', 'regular', 'large',
  'header', 'title' = 1.8, 2.5, 3.5, 5, 7 mm on the paper).

Requires ifcopenshell ("pip install ifcopenshell"). See README.md.
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


def local_placement(location=(0, 0, 0), relative_to=None):
    return f.create_entity(
        "IfcLocalPlacement", PlacementRelTo=relative_to, RelativePlacement=axis3d(location)
    )


# --- owner history ---

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
# The drawing (camera) geometry goes here.
body_context = f.create_entity(
    "IfcGeometricRepresentationSubContext",
    ContextIdentifier="Body",
    ContextType="Model",
    ParentContext=model_context,
    TargetView="MODEL_VIEW",
)
# The texts go here. BonsaiBIM writes exactly such a subcontext:
# 'Annotation' identifier, PLAN_VIEW target view, under the 3D Model context.
annotation_context = f.create_entity(
    "IfcGeometricRepresentationSubContext",
    ContextIdentifier="Annotation",
    ContextType="Model",
    ParentContext=model_context,
    TargetView="PLAN_VIEW",
)

project = f.create_entity(
    "IfcProject",
    GlobalId=guid(),
    OwnerHistory=owner_history,
    Name="Text Literal Test (visible in BonsaiBIM)",
    RepresentationContexts=[model_context],
    UnitsInContext=units,
)

# --- spatial structure ---

site_placement = local_placement()
site = f.create_entity("IfcSite", GlobalId=guid(), OwnerHistory=owner_history, Name="Site",
                       ObjectPlacement=site_placement, CompositionType="ELEMENT")
building_placement = local_placement(relative_to=site_placement)
building = f.create_entity("IfcBuilding", GlobalId=guid(), OwnerHistory=owner_history, Name="Building",
                           ObjectPlacement=building_placement, CompositionType="ELEMENT")
storey_placement = local_placement(relative_to=building_placement)
storey = f.create_entity("IfcBuildingStorey", GlobalId=guid(), OwnerHistory=owner_history, Name="Storey",
                         ObjectPlacement=storey_placement, CompositionType="ELEMENT")


def aggregate(parent, children):
    f.create_entity("IfcRelAggregates", GlobalId=guid(), OwnerHistory=owner_history,
                    RelatingObject=parent, RelatedObjects=children)


aggregate(project, [site])
aggregate(site, [building])
aggregate(building, [storey])


def add_pset(product, name, properties):
    """Add a property set. Values are either plain (bool -> IfcBoolean,
    str -> IfcLabel) or a ("IfcXxx", value) tuple to force the IFC type."""
    props = []
    for prop_name, value in properties.items():
        if isinstance(value, tuple):
            nominal = f.create_entity(value[0], value[1])
        elif isinstance(value, bool):
            nominal = f.create_entity("IfcBoolean", value)
        else:
            nominal = f.create_entity("IfcLabel", str(value))
        props.append(
            f.create_entity("IfcPropertySingleValue", Name=prop_name, NominalValue=nominal)
        )
    pset = f.create_entity("IfcPropertySet", GlobalId=guid(), OwnerHistory=owner_history,
                           Name=name, HasProperties=props)
    f.create_entity("IfcRelDefinesByProperties", GlobalId=guid(), OwnerHistory=owner_history,
                    RelatedObjects=[product], RelatingPropertyDefinition=pset)
    return pset


# --- the drawing, i.e. the camera through which BonsaiBIM shows the texts ---

# Camera volume: DRAWING_WIDTH x DRAWING_HEIGHT, looking down along -Z,
# from the drawing placement down to DRAWING_DEPTH below it.
DRAWING_WIDTH = 30.0
DRAWING_HEIGHT = 20.0
DRAWING_DEPTH = 10.0

camera_block = f.create_entity(
    "IfcBlock",
    Position=axis3d((-DRAWING_WIDTH / 2, -DRAWING_HEIGHT / 2, -DRAWING_DEPTH)),
    XLength=DRAWING_WIDTH,
    YLength=DRAWING_HEIGHT,
    ZLength=DRAWING_DEPTH,
)
drawing = f.create_entity(
    "IfcAnnotation",
    GlobalId=guid(),
    OwnerHistory=owner_history,
    Name="PLAN",
    ObjectType="DRAWING",
    # 6 meters above the texts, without rotation, so we look straight down.
    # BonsaiBIM writes the drawing placement as absolute (PlacementRelTo = nothing).
    ObjectPlacement=f.create_entity(
        "IfcLocalPlacement",
        RelativePlacement=axis3d((-6, 0, 6), axis=(0, 0, 1), ref_direction=(1, 0, 0)),
    ),
    Representation=f.create_entity(
        "IfcProductDefinitionShape",
        Representations=[
            f.create_entity(
                "IfcShapeRepresentation",
                ContextOfItems=body_context,
                RepresentationIdentifier="Body",
                RepresentationType="CSG",
                Items=[f.create_entity("IfcCsgSolid", TreeRootExpression=camera_block)],
            )
        ],
    ),
)
# The full set of properties BonsaiBIM writes for a drawing.
# The resource paths point to files BonsaiBIM creates in the project directory;
# they are not needed to just display the drawing, but we keep them for fidelity.
add_pset(drawing, "EPset_Drawing", {
    "TargetView": "PLAN_VIEW",
    "Scale": "1/100",
    "HumanScale": "1:100",
    "HasUnderlay": False,
    "HasLinework": True,
    "HasAnnotation": True,
    "GlobalReferencing": True,
    "Stylesheet": ("IfcText", "drawings/assets/default.css"),
    "Markers": ("IfcText", "drawings/assets/markers.svg"),
    "Symbols": ("IfcText", "drawings/assets/symbols.svg"),
    "Patterns": ("IfcText", "drawings/assets/patterns.svg"),
    "ShadingStyles": ("IfcText", "drawings/assets/shading_styles.json"),
    "CurrentShadingStyle": "Blender Default",
})

# BonsaiBIM also links every drawing to an IfcDocumentInformation.
drawing_document = f.create_entity(
    "IfcDocumentInformation", Identification="X", Name="PLAN", Scope="DRAWING"
)
f.create_entity("IfcRelAssociatesDocument", GlobalId=guid(), OwnerHistory=owner_history,
                RelatedObjects=[drawing], RelatingDocument=drawing_document)

# --- the texts ---

texts = []


def add_text(name, literal, position, box_alignment):
    text_item = f.create_entity(
        "IfcTextLiteralWithExtent",
        Literal=literal,
        # BonsaiBIM draws the text at the object origin, so keep this identity
        Placement=axis3d(),
        Path="RIGHT",
        Extent=f.create_entity("IfcPlanarExtent", SizeInX=6.0, SizeInY=1.0),
        BoxAlignment=box_alignment,
    )
    annotation = f.create_entity(
        "IfcAnnotation",
        GlobalId=guid(),
        OwnerHistory=owner_history,
        Name=name,
        ObjectType="TEXT",
        ObjectPlacement=local_placement(position, relative_to=storey_placement),
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
    # 'title' = 7 mm on the paper, the largest font size BonsaiBIM defines
    add_pset(annotation, "EPset_Annotation", {"Classes": "title"})
    texts.append(annotation)
    return annotation


add_text("Text A", "A: bottom-left", (-12, 4.5, 0.01), "bottom-left")
add_text("Text B", "B: center", (-12, 1.5, 0.01), "center")
add_text("Text C", "C: top-right", (-12, -1.5, 0.01), "top-right")
add_text("Text D", "D: multi-line\nsecond line", (-12, -5.5, 0.01), "bottom-left")

# --- groups: this is what ties the texts to the drawing ---

drawing_group = f.create_entity("IfcGroup", GlobalId=guid(), OwnerHistory=owner_history,
                                Name="PLAN", ObjectType="DRAWING")
f.create_entity("IfcRelAssignsToGroup", GlobalId=guid(), OwnerHistory=owner_history,
                RelatedObjects=[drawing] + texts, RelatingGroup=drawing_group)

drawings_group = f.create_entity("IfcGroup", GlobalId=guid(), OwnerHistory=owner_history,
                                 Name="DRAWINGS", ObjectType="DRAWINGS")
f.create_entity("IfcRelAssignsToGroup", GlobalId=guid(), OwnerHistory=owner_history,
                RelatedObjects=[drawing_group], RelatingGroup=drawings_group)

# Note: only the texts are placed in the spatial structure, not the drawing.
# This is what BonsaiBIM does too (it finds drawings by their ObjectType and
# group, not through the spatial structure). It also keeps the drawing's camera
# volume away from viewers that display everything in the spatial structure,
# like Castle Game Engine.
f.create_entity("IfcRelContainedInSpatialStructure", GlobalId=guid(), OwnerHistory=owner_history,
                RelatingStructure=storey, RelatedElements=texts)

f.write("text_literal_visible_in_bonsai_bim.ifc")
print("Written text_literal_visible_in_bonsai_bim.ifc")
