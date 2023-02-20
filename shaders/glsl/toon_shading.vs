// Inspired by LightHouse3D.com tutorial, see
// http://www.comp.leeds.ac.uk/vvr/papers/gpu-vvr2.pdf

uniform mat4 castle_ModelViewMatrix;
uniform mat4 castle_ProjectionMatrix;
uniform mat3 castle_NormalMatrix;
attribute vec4 castle_Vertex;
attribute vec3 castle_Normal;

varying vec3 normal; // normal vector
varying vec4 vertex_eye; // vertex, in eye-space

void main()
{
  // Transform the normal to view coordinates
  // and normalize it
  normal = normalize(castle_NormalMatrix * castle_Normal);
  // Finally, transform vertex position
  vertex_eye = castle_ModelViewMatrix * castle_Vertex;
  gl_Position = castle_ProjectionMatrix * vertex_eye;
}
