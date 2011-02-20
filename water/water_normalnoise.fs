varying mat3 normal_matrix;
varying vec2 normalMapTexCooord;
uniform float time;

vec3 vertex_noised(const float x, const float y)
{
  vec3 result = vec3(x, y, time / 10.0);
  result.z = (
    noise_3d_cos(result * 2.0) +
    noise_3d_cos(result * 4.0) / 2.0 +
    noise_3d_cos(result * 8.0) / 4.0
    ) / 10.0;
  return result;
}

void PLUG_fragment_normal_eye(inout vec3 normal_eye_fragment)
{
  /* We calculate normal from a random noise */
  vec3 vertex   = vertex_noised(normalMapTexCooord.x      , normalMapTexCooord.y      );
  vec3 vertex_y = vertex_noised(normalMapTexCooord.x      , normalMapTexCooord.y + 1.0 / 100.0);
  vec3 vertex_x = vertex_noised(normalMapTexCooord.x + 1.0 / 100.0, normalMapTexCooord.y      );

  normal_eye_fragment = normalize(cross(vertex_x - vertex, vertex_y - vertex));
}
