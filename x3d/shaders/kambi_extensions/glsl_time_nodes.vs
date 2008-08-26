uniform float time;

varying float height;
varying vec3 color;

void main(void)
{
  /* Light calculation mostly copied and only slightly adjusted
     from other GLSL shaders here.
     Nothing interesting, we just calculate diffuse + specular
     like usual. */

  vec3 ecPosition = vec3(gl_ModelViewMatrix * gl_Vertex);
  vec3 tnorm      = normalize(gl_NormalMatrix * gl_Normal);
  vec3 lightVec   = normalize(gl_LightSource[0].position.xyz - ecPosition);
  vec3 reflectVec = reflect(-lightVec, tnorm);
  vec3 viewVec    = normalize(-ecPosition);
  float diffuse   = max(dot(lightVec, tnorm), 0.0);
  float spec      = 0.0;
  if (diffuse > 0.0)
  {
      spec = max(dot(reflectVec, viewVec), 0.0);
      spec = pow(spec, 16.0);
  }
  color = (vec3(gl_FrontLightProduct[0].diffuse) * diffuse +
           vec3(gl_FrontLightProduct[0].specular) * spec);

  /* This is the real job of vertex shader: to pass "height"
     for fragment shader. */
  height = (gl_Vertex.y  + time / 20.0) * 30.0;

  gl_Position = ftransform();
}
