/* Vertex shader effect for caustics underwater.
   It merely passes values to fragment shader.

   Uses Castle Game Engine shader effects
   ( https://castle-engine.io/shaders )
   to cooperate with engine shaders.
*/

// Do not redeclare castle_MultiTexCoord* on OpenGLES
#ifndef GL_ES
attribute vec4 castle_MultiTexCoord0;
attribute vec4 castle_MultiTexCoord1;
attribute vec4 castle_MultiTexCoord2;
#endif

varying vec4 tex_coord0;
varying vec4 tex_coord1;
varying vec4 tex_coord2;

void PLUG_vertex_eye_space(const in vec4 vertex_eye, const in vec3 normal_eye)
{
  tex_coord0 = castle_MultiTexCoord0;
  tex_coord1 = castle_MultiTexCoord1;
  tex_coord2 = castle_MultiTexCoord2;
}
