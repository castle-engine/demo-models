uniform mat4 castle_ModelViewMatrix;
uniform mat4 castle_ProjectionMatrix;
uniform mat3 castle_NormalMatrix;
attribute vec4 castle_Vertex;
attribute vec3 castle_Normal;

uniform float time;

varying float height;
varying vec3 color;

void main(void)
{
  /* Simple light calculation: diffuse and specular for a headlight. */
  vec4 vertex_eye = castle_ModelViewMatrix * castle_Vertex;
  vec3 normal = normalize(castle_NormalMatrix * castle_Normal);
  vec3 light_dir = normalize(-vertex_eye.xyz);
  float diffuse = max(dot(light_dir, normal), 0.0);
  float specular = 0.0;
  if (diffuse > 0.0) {
    vec3 reflection_dir = reflect(-light_dir, normal);
    vec3 view_dir = light_dir;
    specular = max(dot(reflection_dir, view_dir), 0.0);
    specular = pow(specular, 16.0);
  }
  color =
    vec3(1.0, 1.0, 1.0) * diffuse +
    vec3(1.0) * specular;

  /* pass "height" to fragment shader, based on time */
  height = (castle_Vertex.y  + time / 20.0) * 30.0;

  gl_Position = castle_ProjectionMatrix * vertex_eye;
}
