vec4 color_from_intensity(float intensity)
{
  // Decide which shade to used based on intensity
  if (intensity > 0.95)
    return vec4(1, 1, 0, 1); else
  if (intensity > 0.5)
    return vec4(0.75, 0.75, 0, 1); else
  if (intensity > 0.25)
    return vec4(0.5, 0.5, 0, 1); else
    return vec4(0.25, 0.25, 0, 1);
}
