uniform mat4 castle_ModelViewMatrix;
uniform mat4 castle_ProjectionMatrix;
attribute vec4 castle_Vertex;

void main(void)
{
  /* some example deformation of vertex positions */
  vec4 v = castle_Vertex;
  /*
  v[0] += v[1];
  v[2] += v[1];
  equivalent : */
  v.xz += v.y;

  gl_Position = castle_ProjectionMatrix * (castle_ModelViewMatrix * v);
}
