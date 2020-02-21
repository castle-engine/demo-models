uniform samplerCube envMap;
uniform mat3 cameraRotationInverseMatrix;

varying vec3 vertex_to_camera;

void PLUG_main_texture_apply(inout vec4 fragment_color, const in vec3 normal)
{
  /* This will be needed to make later "refractedColor" (input to refract)
     normalize and to make "dot" when calculating refraction_amount fair.

     Note: it's not needed for calculating reflected (color and vector),
     since reflect and textureCube work fine with unnormalized vectors. */
  vec3 to_camera = normalize(vertex_to_camera);

  vec3 reflected = reflect(to_camera, normal);

  /* We have to multiply by cameraRotationInverseMatrix, to get "reflected" from
     eye-space to world-space. Our cube map is generated in world space. */
  reflected = cameraRotationInverseMatrix * reflected;

  /* Why doesn't the reflected need to be negated? Yeah, it works, but why? */
  vec3 reflectedColor = textureCube(envMap, -reflected).rgb;

  vec3 refracted = refract(to_camera, normal, 1.1);
  refracted = cameraRotationInverseMatrix * refracted;
  vec3 refractedColor = textureCube(envMap, refracted).rgb;

  /* How much refraction / reflection do I see?
     to_camera is direction from current position to camera.
     Note that we know I look *into* the water, so we can assume
     angle between to_camera and normal is <= 90 degrees.

     When it's 0 (so refraction_amount = cos 0.0 = 1.0)
       -> I look straight into the water, so pick refraction.
     When it's 90 deg (so refraction_amount = cos 90 deg = 0.0)
       -> I look along the water surface, so pick reflection. */
  float refraction_amount = dot(to_camera, normal);

  /* fake reflectedColor to be lighter. This just Looks Better. */
  reflectedColor *= 1.5;

  fragment_color.rgb *=
    mix(reflectedColor, refractedColor, refraction_amount);
}
