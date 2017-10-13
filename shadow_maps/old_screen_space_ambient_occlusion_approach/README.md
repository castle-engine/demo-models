This directory demonstrates our first implementation of
Screen Space Ambient Occlusion, using a little trick with GeneratedShadowMap.
The main code is inside the fragment shader in myssao.fs .

Started by Victor Amat (thousand thanks!),
somewhat adjusted later by Michalis Kambrelis (to work with shader effects,
https://castle-engine.sourceforge.io/compositing_shaders.php ).

**This approach is now deprecated.**

A better way to implement SSAO in Castle Game Engine is to use screen effects:
https://castle-engine.sourceforge.io/x3d_extensions_screen_effects.php .
In fact, a simple SSAO shader is already built-in in Castle Game Engine,
just turn on SceneManager.ScreenScapeAmbientOcclusion := true in code.
You can test it with the checkbox in view3dscene "View -> Screen Effects" submenu.
