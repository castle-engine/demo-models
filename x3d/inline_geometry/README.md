## InlineGeometry demo

The main model to open is `inline_geometry.x3dv`.

### Making of teapot_and_monkey.x3d

`teapot_and_monkey.x3d` was exported from Blender using X3D Exporter, then edited by hand to:

- Add `DEF="Monkey"` and `DEF="Teapot"` to the geometry nodes (`IndexedFaceSet`), so that we can refer to them from `InlineGeometry` nodes.

- Add `creaseAngle="3"` also, to make it look smooth. (Blender exporter does not set this.)

- Also, `<Material DEF="MA_Material_001" />` was fixed (for some reason, Blender exporter made material `USE` without corresponding `DEF`. The material doesn't really matter for `InlineGeometry` demo, but it was makingt the model incorrect.)

When editing this in Blender, you must be sure to rotate the _geometry coordinates_ such that proper "up" is +Y.

Note: While Blender X3D exporter performs the rotation +Z -> +Y automatically, it does so by rotating the objects (using `Transform.rotation`), so it is completely ignored when you use only geometry by `InlineGeometry` (as it, by design, ignores all transforms of the geometry in the referenced file). To properly rotate the geometry, rotate it to have "up" in +Y and apply the rotation in Blender (Ctrl-A, _"Apply Rotation"_), so that the geometry coordinates are actually rotated.