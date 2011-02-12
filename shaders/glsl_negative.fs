uniform sampler2D tex;

void main()
{
  vec4 texColor = texture2D(tex, gl_TexCoord[0].st);
  gl_FragColor = vec4(1, 1, 1, 1) - texColor * gl_Color;
}
