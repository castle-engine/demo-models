/* Vertex shader effect for water with caustics.
   This merely passes variables for the fragment shader in waterShader.fs
   that does the actual work.

   Uses Castle Game Engine shader effects
   ( https://castle-engine.sourceforge.io/compositing_shaders.php )
   to cooperate with engine shaders.
*/

varying vec3 water_coord;
attribute vec2 water_tex3d_coord;
varying vec2 water_tex3d_coord_frag;

void PLUG_vertex_object_space(
  const in vec4 vertex_object, const in vec3 normal_object)
{
  water_coord = vec3(vertex_object);
  water_tex3d_coord_frag = water_tex3d_coord;
}
