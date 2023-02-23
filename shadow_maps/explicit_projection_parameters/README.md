# Setting up properties on light source that determine shadow map usage

When setting up shadow maps ( https://castle-engine.io/x3d_extensions_shadow_maps.php ) at the beginning it's easiest to just add

```
shadows TRUE
```

to the light source and let projection parameters be auto-calculated.

However this is not optimal. It means that shadow map tries to account for the whole scene (whole shadow caster), while possibly your actual shadow caster, that matters, is much smaller.

This example shows this case.

- The statue of monkey stands on a big plane.

- While it works by default, with auto-calculated parameters (`directional_params_auto.x3dv`, `spot_params_auto.x3dv`)...

- ...but using explicit parameters to control the light source projection is better. Shadow map then really focuses on the useful scene parts (monkey statue) not everything (like large ground plane). `directional_params_explicit.x3dv` and `spot_params_explicit.x3dv` show the end result.

How to do this?

1. Temporarily remove the big plane from `base.blend`, export to `base.gltf`.

2. Open in view3dscene `directional_params_auto.x3dv`. Let it autocalculate best parameters, for just monkey.

2. Open the projection settings using view3dscene _"Lights Editor -> Directional Light -> Shadows Settings"_. Copy stuff using _"Copy "Current projection for shadow maps" as X3D to clipboard"_.

3. Paste it into `directional_params_explicit.x3dv`.

4. Now you can restore `base.blend` and `base.gltf` to contain the big plane. `directional_params_explicit.x3dv` will keep using the forced projection parameters.
