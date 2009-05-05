uniform samplerCube envMap;
uniform sampler2D normalMap;
uniform mat4 cameraInverseMatrix;
varying float diffuse;
varying vec3 vertex_to_camera;
varying vec3 normal;

void main(void)
{
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

  gl_FragColor.rgb = reflectedColor *
    vec3(texture2D(normalMap, gl_TexCoord[0].st)) * diffuse;

  /* alpha is simply constant, not scaled by lighting */
  gl_FragColor.a = 1.0;
}
