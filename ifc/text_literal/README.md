Text in IFC, using
[IfcTextLiteral](https://standards.buildingsmart.org/IFC/RELEASE/IFC4_3/HTML/lexical/IfcTextLiteral.htm)
and
[IfcTextLiteralWithExtent](https://standards.buildingsmart.org/IFC/RELEASE/IFC4_3/HTML/lexical/IfcTextLiteralWithExtent.htm).

This is the IFC counterpart of the X3D `Text` node.

The model contains 4 texts (and nothing else -- the texts are the point here).
Each text is an
`IfcAnnotation` (with `ObjectType = 'TEXT'`) whose representation
(`RepresentationIdentifier = 'Annotation'`, `RepresentationType = 'Annotation2D'`,
in a subcontext with `ContextIdentifier = 'Annotation'`) contains the text item.
This is exactly how [BonsaiBIM](https://bonsaibim.org/) and
[FreeCAD](https://www.freecad.org/) store text, so this file can be opened by them too.

The 4 texts deliberately differ:

- *A: literal placement* --- position is in the `IfcTextLiteral.Placement`,
  the containing `IfcAnnotation` is at zero.

- *B: object placement* --- position is in the `IfcAnnotation.ObjectPlacement`,
  the `IfcTextLiteral.Placement` is an identity placement.
  This is what BonsaiBIM and FreeCAD do when they write text.

- *C: with extent* --- uses `IfcTextLiteralWithExtent`, so it also has
  `IfcPlanarExtent` and `BoxAlignment`. This is the exact entity BonsaiBIM writes.

- *D: rotated* --- the `IfcTextLiteral.Placement` is rotated, so the text stands
  vertically (in the XZ plane). It is also a multi-line text (the literal contains
  a newline).

The texts are large (a few meters) because we display them with the default X3D
font size 1.0, which means 1 meter in this model (the model units are meters).

Both encodings of the same model are here: `text_literal.ifc` (STEP encoding,
IFC4) and `text_literal.ifcjson` (JSON encoding, this is the one that
[Castle Game Engine can read](https://castle-engine.io/ifc)).

`text_literal.x3dv` is not part of the testcase --- it only wraps the model in
an X3D `Viewpoint`, to give a good camera for the screenshot. Regenerate the
screenshot from it:

```
../../regenerate_screenshots.sh text_literal.x3dv
```

## Regenerating

`generate.py` creates `text_literal.ifc` (it needs
[IfcOpenShell](https://ifcopenshell.org/), `pip install ifcopenshell`).
Then convert to the JSON encoding using
[our fork of the ifcJSON Python scripts](https://github.com/michaliskambi/ifcJSON):

```
python3 generate.py
python3 ifcJSON/file_converters/ifc2json.py -i text_literal.ifc -o text_literal.ifcjson
```

## Notes about other applications

Research below about BonsaiBIM and FreeCAD was done by Claude -- use limited trust.

- *FreeCAD* imports `IfcTextLiteral` as a `Draft Text` object. Note that it reads
  only the `IfcTextLiteral.Placement`, ignoring the placement of the containing
  `IfcAnnotation` --- so texts B and C may land at the origin.
  FreeCAD also treats `;` inside the literal as a line separator
  (we don't --- we honor only real newlines).

- *BonsaiBIM* reads the literals (they are visible in the text properties panel,
  and in drawings) but does not display them in the Blender 3D viewport ---
  on import, elements whose only geometry is an `IfcTextLiteral` are created
  as objects without any mesh.
