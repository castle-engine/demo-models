uniform float time;

/* 1 / wave_height_inverse is wave height (in object space).
   Using inverse, to workaround fglrx bugs. */
const float wave_height_inverse = 50.0;

vec3 vertex_noised(const float x, const float y)
{
  vec3 result = vec3(x, y, time / 5.0);
  result.z = (
    noise_3d_cos(result * 2.1) +
    noise_3d_cos(result * 4.3) / 2.2 +
    noise_3d_cos(result * 7.8) / 3.7
    ) / wave_height_inverse;
  return result;
}

void PLUG_water_normal_object_space(inout vec3 normal,
  const in vec2 normalMapTexCooord)
{
  /* We calculate normal from a random noise */
  vec3 vertex   = vertex_noised(normalMapTexCooord.x      , normalMapTexCooord.y      );
  vec3 vertex_y = vertex_noised(normalMapTexCooord.x      , normalMapTexCooord.y + 1.0 / 100.0);
  vec3 vertex_x = vertex_noised(normalMapTexCooord.x + 1.0 / 100.0, normalMapTexCooord.y      );

  normal = normalize(cross(vertex_x - vertex, vertex_y - vertex));
}
