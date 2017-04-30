These files allow testing X3DOM ( https://www.x3dom.org/ )
implementation of CommonSurfaceShader.

If you open these HTML files from disk (through file:// protocol):
- In Firefox, you will see the textures automatically.
  (they need to be present in subdirectory of this dir, not in parent directories,
  that's why we copied all the necessary textures to the textures/ subdirectory).
- In Google Chrome, you need to run it with
  google-chrome-stable --allow-file-access-from-files --allow-file-access
  (see http://stackoverflow.com/questions/30661761/viewing-a-x3dom-file-locally )
  Or use a local web server.

The X3DOM documentation of CommonSurfaceShader is on
https://doc.x3dom.org/author/Shaders/CommonSurfaceShader.html
The Castle Game Engine documentation of CommonSurfaceShader, for comparison, is on
https://castle-engine.sourceforge.io/x3d_implementation_texturing_extensions.php#section_ext_common_surface_shader
