/* Varying vectors below are given in in eye-space.
   Always normalized, to make "fair" interpolation. */
varying vec3 vertex_to_camera;
varying vec3 vertex_to_light;
varying vec3 normal;

void main(void)
{
  gl_TexCoord[0] = /* gl_TextureMatrix[0] * (not needed) */ gl_MultiTexCoord0;

  gl_Position = ftransform();

  normal = normalize(gl_NormalMatrix * gl_Normal);
  vec3 light_position = vec3(0.0, 0.0, 0.0);
  vec3 vertex = vec3(gl_ModelViewMatrix * gl_Vertex);
  vertex_to_light = normalize(light_position - vertex);
  /* That's easy, since in eye space camera position is always (0, 0, 0). */
  vertex_to_camera = normalize(- vertex);
}
