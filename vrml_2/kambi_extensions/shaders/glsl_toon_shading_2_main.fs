/* This is much like, glsl_toon_shading.fs, but it's splitted
   into two files, just to test GLSL functions and the fact that
   you can attach more than 1 fragment shader and that VRML engine
   allows this. */

vec4 color_from_intensity(float intensity);

varying vec3 normal;
void main()
{
  float intensity;
  // Normalize the normal, again
  vec3 n = normalize(normal);
  // Normalize light direction and convert to a vec3
  vec3 lDir = normalize(vec3(gl_LightSource[0].position));
  // Compute light intensity using dot product
  intensity = dot(lDir,n);
  // Compute light intensity using dot product
  intensity = dot(lDir,n);
  // Finally, set destination colour
  gl_FragColor = color_from_intensity(intensity);
}
