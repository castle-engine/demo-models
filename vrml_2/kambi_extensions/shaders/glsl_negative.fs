uniform sampler2D tex;

void main()
{
  vec4 texColor = texture2D(tex, gl_TexCoord[0]);
  gl_FragColor = 1 - texColor * gl_Color;
}
