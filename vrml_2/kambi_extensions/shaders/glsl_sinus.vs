void main(void)
{
  vec4 v = gl_Vertex;
  v.y = sin(v.x);
  gl_Position = gl_ModelViewProjectionMatrix * v;

  gl_FrontColor = gl_Color;
}
