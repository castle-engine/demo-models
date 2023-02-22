varying mat3 normal_matrix;
varying vec2 normalMapTexCooord;

#ifndef GL_ES
uniform mat3 castle_NormalMatrix;
#endif

void PLUG_vertex_object_space(const in vec4 vertex, const in vec3 normal)
{
  normal_matrix = castle_NormalMatrix;
  /* funny float consts are to workaround fglrx bugs. */
  normalMapTexCooord = (vertex.xy * 2.0 + vec2(1.0)) / 2.0;
}
