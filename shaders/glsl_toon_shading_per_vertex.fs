varying float intensity;

vec4 color_from_intensity(float intensity)
{
  vec4 result = vec4(1.0, 1.0, 0.0, 1.0);
  if (intensity * 100.0 <= 95.0)
  {
    result /= 4.0;
    if (intensity * 100.0 > 50.0)
      result *= 3.0; else
    if (intensity * 100.0 > 25.0)
      result *= 2.0;
    /* else stays as vec4(0.25, 0.25, 0, 1); */
  }

  return result;
}

void main()
{
  gl_FragColor = color_from_intensity(intensity);
}
