/* Worse version of toon shader, calculates intensity per-vertex
   and interpolates intensity.
   This is worse than the other version (per-pixel), is done just
   to actually see *how* it's worse.

   By Kambi, based on per-pixel version and
   http://www.lighthouse3d.com/opengl/glsl/.
*/

varying float intensity;

void main()
{
  intensity = dot(
    normalize(vec3(gl_LightSource[0].position)),
    normalize(gl_NormalMatrix * gl_Normal));
  gl_Position = ftransform();
}