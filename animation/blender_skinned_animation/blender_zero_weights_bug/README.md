# Blender -> glTF exporter had a bug in Blender < 3.2

## Before Blender 3.2

Blender could write weights=(0,0,0,0) for some vertexes.

This is not correct (glTF spec says that weights should sum to 1.0).

Solution that works: Transform it with weight 1 by the joint number 0
(relying that Blender put root joint at this position).

See for background:

- https://github.com/KhronosGroup/glTF/issues/1213
- https://github.com/KhronosGroup/glTF-Blender-IO/issues/308
- https://github.com/KhronosGroup/glTF-Blender-IO/issues/308#issuecomment-531355129

    _"""it's not exactly a satisfying fix, but in practice using 1, 0, 0, 0 when the weights would otherwise be zero has avoided these issues in threejs."""_

    (CGE note: This is not really the full workaround, you also need to use bone 0.)

- https://github.com/KhronosGroup/glTF/pull/1352
- https://github.com/Franck-Dernoncourt/NeuroNER/issues/91

**Castle Game Engine can workaround this problem reliably.** We do it in both CPU-based and GPU-based animation calculation. We assume the vertex is transformed by the root bone, so we transform it with weight 1 by the joint number 0.

## Effect since Blender 3.2

They fixed it in Blender 3.2, https://github.com/KhronosGroup/glTF-Blender-IO/pull/1552 .

Though note that the fix only works if you use the _"Vertex Groups"_, not _"Bone Envelopes"_ option at _"Armature"_ modifier. With _"Bone Envelopes"_, the glTF is correct but shows different animation (just assuming as if _"Vertex Groups"_ was used, it seems). No big deal I guess, since most practical models use _"Vertex Groups"_ anyway.

