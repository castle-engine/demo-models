vec4 color_from_intensity_original(float intensity)
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

/* Kambi note: color_from_intensity_original doesn't work like it should on
   Radeon with closed ATI
   drivers on Linux MacBookPro. Don't know why, seems like comparison
   "intensity > ..." is always true unless "..." is 0, and
   constant vector values also work only as 0 / 1.
   Effectively, seems that float constants are rounded up to integer
   values.... Weird.
   Seems that on Mesa, the same problem is present... possibly this
   is some shortcoming of older GLSL versions ?

   On the same graphic card on Mac OS X work fine... so it seems a problem
   in OpenGL implementation in fglrx.

   So color_from_intensity_alt is another implementation of
   color_from_intensity. A little worse looking, but does the trick
   and manages to avoid any "if" call. */
vec4 color_from_intensity_alt(float intensity)
{
  intensity = ceil(intensity * 4.0) / 4.0;
  return vec4(intensity, intensity, 0.0, 1.0);
}

/* Yet another version of color_from_intensity, the best one:
   completely equivalent to color_from_intensity_original,
   and avoiding fglrx problems. */
vec4 color_from_intensity(float intensity)
{
  vec4 result = vec4(1, 1, 0, 1);
  if (intensity * 100 <= 95)
  {
    result /= 4.0;
    if (intensity * 100 > 50)
      result *= 3.0; else
    if (intensity * 100 > 25)
      result *= 2.0;
    /* else stays as vec4(0.25, 0.25, 0, 1); */
  }

  return result;
}