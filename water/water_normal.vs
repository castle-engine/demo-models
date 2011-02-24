varying mat3 normal_matrix;
varying vec2 normalMapTexCooord;
void PLUG_vertex_process(const in vec4 vertex_eye,
  const in vec3 normal_eye)
{
  normal_matrix = gl_NormalMatrix;
  /* funny float consts are to workaround fglrx bugs. */
  normalMapTexCooord = (gl_Vertex.xy * 2.0 + vec2(1.0)) / 2.0;
}
