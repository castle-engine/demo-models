void main(void)
{
  /* some pretty dumb deformation of vertex positions */
  vec4 v = gl_Vertex;
  v[0] += v[1];
  v[2] += v[1];
  gl_Position = gl_ModelViewProjectionMatrix * v;

  gl_FrontColor = gl_Color;

  gl_TexCoord[0] = gl_MultiTexCoord0;
}
