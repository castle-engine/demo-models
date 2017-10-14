/* Use normal vector Z (were in camera space, so (0, 0, 1) just points
   at camera) as color alpha. Inspiration: Blender "Map Input Nor" trick. */

varying vec3 normal;
uniform vec3 baseColor;

void main(void)
{
  float factor = 1.0 - pow(normal.z, 2.0);
  gl_FragColor = vec4(baseColor, factor);
}
