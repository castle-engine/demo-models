const float Intensity = 12.0;

void main (void)
{
  float RedGreenX   = screenf_x() + Intensity;
  float BlueX       = screenf_x() - Intensity;
  gl_FragColor = vec4(
    screenf_get_color(vec2(RedGreenX, screenf_y())).xy,
    screenf_get_color(vec2(BlueX    , screenf_y())).z,
    1.0
  );
}
