uniform float Intensity;

void main (void)
{
  gl_FragColor = vec4(
    (
      2.0 * screenf_get_color(vec2( screenf_x(),             screenf_y()             )).xyz +
      1.0 * screenf_get_color(vec2( screenf_x() + Intensity, screenf_y()             )).xyz +
      1.0 * screenf_get_color(vec2( screenf_x() - Intensity, screenf_y()             )).xyz +
      1.0 * screenf_get_color(vec2( screenf_x(),             screenf_y() + Intensity )).xyz +
      1.0 * screenf_get_color(vec2( screenf_x(),             screenf_y() - Intensity )).xyz
    ) / 6.0,
    1.0
  );
}
