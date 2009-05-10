uniform samplerCube envMap;
uniform sampler2D normalMap;
uniform mat4 cameraInverseMatrix;

varying vec3 vertex_to_camera;
varying vec3 vertex_to_light;
varying mat3 normal_matrix;

void main(void)
{
  vec3 normal = texture2D(normalMap, gl_TexCoord[0].st).xyz;
  /* Unpack normal.xy. Blender generates normal maps with Z always > 0,
     so do not unpack Z. Hm, actually, I do not see any visible difference?

     Note: This is normal in tangent space. It so happens that on our
     water surface, this is also valid in object space, because our
     surface is flat on Z=const (and relation of xy to texture st doesn't
     really matter, as it's just a noise for waves, doesn't matter much
     how we map it). So simply transforming by normal_matrix gets us
     into eye-space, and we're happy. */
  normal.xy = normal.xy * 2.0 - vec2(1.0, 1.0);
  normal = normal_matrix * normal;

  float diffuse = max(dot(normalize(vertex_to_light), normal), 0.0);

  /* This will be needed to make later "refractedColor" (input to refract)
     normalize and to make "dot" when calculating refraction_amount fair.

     Note: it's not needed for calculating reflected (color and vector),
     since reflect and textureCube work fine with unnormalized vectors. */
  vec3 to_camera = normalize(vertex_to_camera);

  vec3 reflected = reflect(to_camera, normal);

  /* We have to multiply by cameraInverseMatrix, to get "reflected" from
     eye-space to world-space. Our cube map is generated in world space.
     "reflected" is direction, so 4th component should be 0 in homog coords. */
  reflected = (cameraInverseMatrix * vec4(reflected, 0.0)).xyz;

  /* TODO: why doesn the reflected need to be negated? Yeah, it works,
     but as far as I think this shouldn't be needed. */
  vec3 reflectedColor = textureCube(envMap, -reflected).rgb;

  vec3 refracted = refract(to_camera, normal, 1.1);
  refracted = (cameraInverseMatrix * vec4(refracted, 0.0)).xyz;
  vec3 refractedColor = textureCube(envMap, refracted).rgb;

  /* How much refraction / reflection do I see?
     to_camera is direction from current position to camera.
     Note that we know I look *into* the water, so we can assume
     angle between to_camera and normal is <= 90 degrees.
     When it's 0 -> I look straight into the water, so pick refraction.
     When it's 90 deg -> I look along the water surface, so pick reflection. */
  float refraction_amount = dot(to_camera, normal);

  /* fake reflectedColor to be lighter. This just Looks Better. */
  reflectedColor *= 1.5;

  gl_FragColor.rgb = 
    mix(reflectedColor, refractedColor, refraction_amount) *
    vec3(gl_FrontMaterial.diffuse) * diffuse;

  /* alpha is simply constant, not scaled by lighting */
  gl_FragColor.a = 1.0;
}
