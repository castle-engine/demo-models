// Inspired by http://www.clockworkcoders.com/oglsl/tutorial5.htm

varying vec3 N;
varying vec3 v;

#define light_position vec3(3.0, 3.0, 3.0)
#define light_material_ambient vec4(0.1)
#define light_material_diffuse vec4(1.0, 1.0, 0.0, 1.0)
#define light_material_specular vec4(1.0)
#define material_shininess 1.0

void main (void)
{
  vec3 L = normalize(light_position - v);
  vec3 E = normalize(-v); // we are in Eye Coordinates, so EyePos is (0,0,0)
  vec3 R = normalize(-reflect(L,N));

  //calculate Ambient Term:
  vec4 Iamb = light_material_ambient;

  //calculate Diffuse Term:
  vec4 Idiff = light_material_diffuse * max(dot(N,L), 0.0);

  // calculate Specular Term:
  vec4 Ispec = light_material_specular
    * pow(max(dot(R,E),0.0), material_shininess * 0.3);

  // write Total Color:
  gl_FragColor = Iamb + Idiff + Ispec;
}
