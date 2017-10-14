uniform mat4 castle_ModelViewMatrix;
uniform mat4 castle_ProjectionMatrix;
attribute vec4 castle_Vertex;

void main(void)
{
  vec4 v = castle_Vertex;
  v.y = sin(v.x);
  gl_Position = castle_ProjectionMatrix * (castle_ModelViewMatrix * v);
}
