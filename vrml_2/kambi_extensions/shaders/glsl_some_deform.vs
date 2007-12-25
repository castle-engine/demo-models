/* TODO: why does this messes up depth buffer ? */

void main(void)
{
  gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
  gl_Position[0] += gl_Vertex[1];
  gl_Position[2] += gl_Vertex[1];

  gl_FrontColor = gl_Color;

  gl_TexCoord[0] = gl_MultiTexCoord0;
}
