varying vec3 vertex_to_camera;

void PLUG_vertex_eye_space(const in vec4 vertex_eye,
   const in vec3 normal_eye)
{
  /* That's easy, since in eye space camera position is always (0, 0, 0). */
  vertex_to_camera = normalize(-vec3(vertex_eye));
}
