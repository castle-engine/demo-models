#version 120
uniform vec3 light;
uniform vec3 eye;
uniform vec3 reflectEye;
uniform vec3 center;
uniform vec2 translation;
uniform vec2 scale;
uniform float frame;
uniform sampler3D wavesMap;
uniform sampler2D refractMap;
uniform sampler2D reflectMap;
uniform sampler2D shadowMap;
uniform mat4 reflectViewMatrix;
varying vec4 castle_TexCoord0;
varying vec4 castle_TexCoord1;
#define HEIGHT (center.y - 1.17)

void main(void) {
  vec3 Inc = normalize(castle_TexCoord0.xyz - eye);
  vec3 Normal = texture3D(wavesMap, vec3(castle_TexCoord1.st, frame)).xzy;
  Normal.xz = (Normal.xz - 0.5) * 0.35;
  Normal.z *= -1.0;
  Normal = normalize(Normal);

  // Refracted rays
  vec3 Refract = refract(Inc, Normal, 0.75);
  // intersection between refracted ray and the pool floor
  vec3 P = castle_TexCoord0.xyz - 1.17 / Refract.y * Refract;
  // if P is out of the floor, check for the pool walls
  if (abs(P.z + 6.05) > 4.9775) {
    float f = clamp(P.z, -11.0275, -1.0725);
    P = castle_TexCoord0.xyz + (f - castle_TexCoord0.z) / Refract.z * Refract;
  }
  if (P.x > 24.2275)
    P = castle_TexCoord0.xyz + (24.2275 - castle_TexCoord0.x) / Refract.x * Refract;
  vec3 V = P - center;
  // intersection between a line from P to the center of projection, and the water surface
  vec3 T = center - HEIGHT / V.y * V;
  // texture coordinates adjusted to refractMap bounds
  vec2 texcoord_3 = (T.xz - translation) * scale;
  vec4 refractedColor = texture2D(refractMap, texcoord_3);

  // (faked) Caustics
  // intersection between a line through P parallel to the light rays, and the water surface
  vec3 U = P + (1.17 - P.y) / light.y * light;
  // texture coordinates adjusted to wavesMap bounds
  vec2 texcoord_5 = (U.xz - translation) * scale * 4.0;
  float shadow = texture2D(shadowMap, texcoord_3).r;
  refractedColor *= mix(1.0, texture3D(wavesMap, vec3(texcoord_5, frame)).a * 2.0, shadow);

  // Reflected rays
  vec3 Reflect = reflect(Inc, Normal);
  //Reflect.y = max(Reflect.y, 0);
  // intersection between a line parallel to the reflected vector from the mirrored camera, and the water surface
  vec3 Q = reflectEye + (1.17 - reflectEye.y) / Reflect.y * Reflect;
  // diminish the effect
  Q = (Q + castle_TexCoord0.xyz) * 0.5;
  // coordinates of the previous point in mirrored camera - space
  vec4 texcoord_4 = reflectViewMatrix * vec4(Q, 1.0);
  // adjusts (s,t) to 0 - 1 interval
  texcoord_4.st = (1 - texcoord_4.st / texcoord_4.p) * 0.5;
  texcoord_4.p *= -1.0;
  vec4 reflectedColor = texture2DProj(reflectMap, texcoord_4);
  float FresnelTerm = 0.985 - dot(-Inc, Normal);
  // faster than pow(1 - cos, 5)
  float FrSqr = FresnelTerm * FresnelTerm;
  FresnelTerm *= FrSqr * FrSqr;
  gl_FragColor = mix(refractedColor, reflectedColor, FresnelTerm + 0.08);
  gl_FragColor.a = 1.0;
}
