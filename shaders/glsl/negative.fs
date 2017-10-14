uniform sampler2D tex;
varying vec4 tex_coord;

void main()
{
  vec4 texColor = texture2D(tex, tex_coord.st);
  gl_FragColor = vec4(1, 1, 1, 1) - texColor;
}
