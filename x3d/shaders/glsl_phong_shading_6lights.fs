/* Based on shaders/glsl_phong_shading.fs, 
   adjusted to sum over 6 directional lights. */

varying vec3 N;
varying vec3 v;

void main (void)
{
  vec3 E = normalize(-v); // we are in Eye Coordinates, so EyePos is (0,0,0)

  gl_FragColor = gl_FrontLightModelProduct.sceneColor;

  for (int i = 0; i < 6; i++)
  {
    vec3 L = gl_LightSource[i].position.xyz; // directional light, so L is just position.xyz
    vec3 R = normalize(-reflect(L,N));

    //calculate Ambient Term:
    vec4 Iamb = gl_FrontLightProduct[i].ambient;

    //calculate Diffuse Term:
    vec4 Idiff = gl_FrontLightProduct[i].diffuse * max(dot(N,L), 0.0);

    // calculate Specular Term:
    vec4 Ispec = gl_FrontLightProduct[i].specular
      * pow(max(dot(R,E),0.0),
        /* 0.3 * gl_FrontMaterial.shininess was here, but on Radeon with closed
           ATI drivers on Linux MacBookPro this produced always white
           result... Somehow, "gl_FrontMaterial.shininess / 3.0" which
           calculates almost the same in almost the same way...) works OK. */
           gl_FrontMaterial.shininess / 3.0);

    // write Total Color:
    gl_FragColor += Iamb + Idiff + Ispec;
  }
}
