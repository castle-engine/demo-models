varying mat3 normal_matrix;
varying vec2 normalMapTexCooord;
void PLUG_vertex_process(const in vec4 vertex_eye,
  const in vec3 normal_eye)
{
  normal_matrix = gl_NormalMatrix;
  normalMapTexCooord = gl_Vertex.xy + vec2(1.0) / 2.0;
}
