varying mat3 normal_matrix;
varying vec2 normalMapTexCooord;

void PLUG_fragment_eye_space(const vec4 vertex, inout vec3 normal)
{
  normal = vec3(0.0, 0.0, 1.0);
  /* PLUG: water_normal_object_space (normal, normalMapTexCooord) */
  if (!gl_FrontFacing) normal.z = -normal.z;

  normal = normal_matrix * normal;
}
