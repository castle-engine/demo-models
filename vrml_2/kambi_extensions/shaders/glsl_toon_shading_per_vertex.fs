varying float intensity;

void main()
{
  vec4 color;

  // Decide which shade to used based on intensity
  if (intensity > 0.95)
     color = vec4(1, 1, 0, 1);
  else if (intensity > 0.5)
     color = vec4(0.75, 0.75, 0, 1);
  else if (intensity > 0.25)
     color = vec4(0.5, 0.5, 0, 1);
  else
     color = vec4(0.25, 0.25, 0, 1);

  gl_FragColor = color;
}