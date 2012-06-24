Tests in this directory are not 2-manifold 3D models.
They will not render shadow volumes with view3dscene >= 3.12.0.

Old view3dscene <= 3.11.0 will render shadows,
but with possible artifacts from particular viewpoints.

New view3dscene >= 3.12.0 will simply not render shadows at all,
disabling shadow casting for non-manifold models (with some border edges).
To see shadows in these models, you need to
modify castle_game_engine/src/x3d/opengl/castlescene.pas sources, and either:

1. Ignore the check for BorderEdges.Count in TCastleScene.RenderShadowVolume,
   and render non-manifold models with possible artifacts, or

2. Change AllowSilhouetteOptimization constant to false. This makes
   the shadow volumes working on any model (not necessarily manifold),
   but usually with unacceptably slow speed.

Since none of these two options are really useful, by default
view3dscene >= 3.12.0 just doesn't render shadow volumes for non-manifold models.
You are forced to make your models manifold if you want shadow volumes,
models that are almost-but-not-perfectly-manifold
(as it often happens, and it seems to work --- until it fails for
a particular point of view) are not allowed.
