/* This is just like water_normalmap.vs, but we scale a little differently.
   As usual: funny float consts are to workaround fglrx bugs. */

varying mat3 normal_matrix;
varying vec2 normalMapTexCooord;
void PLUG_vertex_process(const in vec4 vertex_eye,
  const in vec3 normal_eye)
{
  normal_matrix = gl_NormalMatrix;
  normalMapTexCooord = (gl_Vertex.xy * 2.0 + vec2(1.0)) / 2.0;
}
