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

/* Kambi note: color_from_intensity doesn't work like it should on
   Radeon with closed ATI
   drivers on Linux MacBookPro. Don't know why, seems like comparison
   "intensity > ..." is always true unless "..." is 0, and
   constant vector values also work only as 0 / 1.
   Effectively, seems that float constants are rounded up to integer
   values.... Weird.

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
