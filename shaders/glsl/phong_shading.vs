uniform mat4 castle_ModelViewMatrix;
uniform mat4 castle_ProjectionMatrix;
uniform mat3 castle_NormalMatrix;
attribute vec4 castle_Vertex;
attribute vec3 castle_Normal;

varying vec3 N;
varying vec3 v;

void main(void)
{
   vec4 vertex_eye = castle_ModelViewMatrix * castle_Vertex;
   v = vec3(vertex_eye);

   N = normalize(castle_NormalMatrix * castle_Normal);

   gl_Position = castle_ProjectionMatrix * vertex_eye;
}
