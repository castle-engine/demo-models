# Animated piece of cloth, with bump mapping

The piece of cloth is just a square with animation created using Blender's _soft body_ physics.

It was exported (see details below) and adjusted by hand to display properly the animation and bump mapping in X3D.

## Texture credits

rock_d01:

- from http://opengameart.org/content/generic-rock-texture
- by Sindwiller, https://opengameart.org/users/sindwiller
- license: GPL 3.0, CC-BY-SA 3.0

## Details how cloth_anim.x3dv was made

- Exported from Blender to [castle-anim-frames](https://castle-engine.io/castle_animation_frames.php) using our own Blender exporter.

    Note that `castle-anim-frames` format is deprecated now and our Blender exporter to it is no longer available (it doesn't work with latest Blender). We advise to use glTF for all animation purposes.

- Converted castle-anim-frames to single file using our tool `kanim_to_interpolators coord_MOD_Plane cloth.kanim cloth_anim.wrl`

    Note that `kanim_to_interpolators` is no longer available. Again, we advise to just use glTF for all animation purposes now.

- gzipped, for smaller size on disk

- Adjusted by hand:
    - headlight
    - Appearance with normal map
    - later upgraded to X3D 4

Made by Michalis.
