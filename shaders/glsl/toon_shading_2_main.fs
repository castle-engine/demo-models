/* This is much like, glsl_toon_shading.fs, but it's splitted
   into two files, just to test GLSL functions and the fact that
   you can attach more than 1 fragment shader and that VRML engine
   allows this.

   Also, you can freely use either color_from_intensity_alt or
   color_from_intensity. color_from_intensity_alt looks worse, but avoids
   "if" and so works even on fglrx. */

vec4 color_from_intensity(float intensity);
vec4 color_from_intensity_alt(float intensity);

varying vec3 normal;
varying vec4 vertex_eye;

void main()
{
  float intensity;
  // Normalize the normal, again
  vec3 n = normalize(normal);
  // Assume a headlight, so light direction in eye space is -vertex_eye
  vec3 lDir = normalize(-vertex_eye.xyz);
  // Compute light intensity using dot product
  intensity = dot(lDir,n);
  // Compute light intensity using dot product
  intensity = dot(lDir,n);
  // Finally, set destination colour
  gl_FragColor = color_from_intensity(intensity);
}
