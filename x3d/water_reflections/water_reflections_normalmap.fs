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
     so do not unpack Z. TODO: does it actually make any visible difference? */
  normal.xy = normal.xy * 2.0 - vec2(1.0, 1.0);
  normal = normal_matrix * normal;

  float diffuse = max(dot(normalize(vertex_to_light), normal), 0.0);

  vec3 reflected = reflect(vertex_to_camera, normal);

  /* We have to multiply by cameraInverseMatrix, to get "reflected" from
     eye-space to world-space. Our cube map is generated in world space.
     "reflected" is direction, so 4th component should be 0 in homog coords. */
  reflected = (cameraInverseMatrix * vec4(reflected, 0.0)).xyz;

  /* We didn't care about normalizing "reflected" (or any vector on the way),
     because it doesn't matter for textureCube.
     TODO: why doesn the reflected need to be negated? Yeah, it works,
     but as far as I think this shouldn't be needed. */
  vec3 reflectedColor = textureCube(envMap, -reflected).rgb;

  vec3 refracted = refract(vertex_to_camera, normal, 1.1);
  refracted = (cameraInverseMatrix * vec4(refracted, 0.0)).xyz;
  vec3 refractedColor = textureCube(envMap, refracted).rgb;

  gl_FragColor.rgb = (reflectedColor * 0.5 + 
    refractedColor * 0.5 * vec3(gl_FrontMaterial.diffuse)) * diffuse;

  /* alpha is simply constant, not scaled by lighting */
  gl_FragColor.a = 1.0;
}
