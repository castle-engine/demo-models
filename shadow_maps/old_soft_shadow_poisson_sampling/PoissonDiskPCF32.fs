// Simple soft shadows.
// Adapted mainly from nVidia SDK 9.
// by Victor Amat.

#version 110
#define ONEOVER4 0.25
#define ONEOVER16 0.0625
#define ONEOVER32 0.03125
#define ONEOVER64 0.015625
#define MAPSIZE 0.0009765625     // 1/1024 --change accordingly to shadowMap size
#define SCALE  24.0              // Increase to widen penumbra

//uniform vec3 lightPos;
uniform sampler2DShadow shadowMap;
uniform sampler3D offsetsMap;

void main(void) {

  //vec3 Pos = gl_TexCoord[1].xyz;
  //vec3 Light = normalize(lightPos - Pos);
  //vec3 Normal = gl_TexCoord[2].xyz;
  float shadowCoeff = 1.0;

  //if (dot(Normal, Light) > -0.2)
  //{
    float filterSize = SCALE * MAPSIZE * gl_TexCoord[0].q;
    vec2 offsetXYScale = vec2(ONEOVER64);    // Size of the screen-aligned pattern
    vec4 sampleCoord = gl_TexCoord[0];
    vec3 offsetCoord = vec3(0.0);
    float shadow = 0.0;
    vec4 offset;
    offsetCoord.xy = gl_FragCoord.xy * offsetXYScale;

    // samples 1,2
    offset = texture3D(offsetsMap, offsetCoord) - 0.5;  // lookup in offsetsMap
    sampleCoord.st = gl_TexCoord[0].st + (offset.xy * filterSize);
    shadow += shadow2DProj(shadowMap, sampleCoord).x;   // lookup in shadowMap
    sampleCoord.st = gl_TexCoord[0].st + (offset.zw * filterSize);
    shadow += shadow2DProj(shadowMap, sampleCoord).x;   // lookup in shadowMap

    // samples 3,4
    offsetCoord.z += ONEOVER16;
    offset = texture3D(offsetsMap, offsetCoord) - 0.5;  // lookup in offsetsMap
    sampleCoord.st = gl_TexCoord[0].st + (offset.xy * filterSize);
    shadow += shadow2DProj(shadowMap, sampleCoord).x;   // lookup in shadowMap
    sampleCoord.st = gl_TexCoord[0].st + (offset.zw * filterSize);
    shadow += shadow2DProj(shadowMap, sampleCoord).x;   // lookup in shadowMap

    shadowCoeff = shadow * ONEOVER4;

    if (shadowCoeff > 0.0001 && shadowCoeff < 0.9999)   // penumbra region
    {
      for (int i = 0; i < 14; i++)   // samples 5-32
      {
        offsetCoord.z += ONEOVER16;
        offset = texture3D(offsetsMap, offsetCoord) - 0.5;  // lookup in offsetsMap
        sampleCoord.st = gl_TexCoord[0].st + (offset.xy * filterSize);
        shadow += shadow2DProj(shadowMap, sampleCoord).x;   // lookup in shadowMap
        sampleCoord.st = gl_TexCoord[0].st + (offset.zw * filterSize);
        shadow += shadow2DProj(shadowMap, sampleCoord).x;   // lookup in shadowMap
      }
      shadowCoeff = shadow * ONEOVER32;
    }
  //}
  gl_FragColor = vec4(0, 0, 0, 1);
  gl_FragColor += mix (
    gl_FrontLightModelProduct.sceneColor + gl_FrontLightProduct[0].ambient,
    gl_Color,
    shadowCoeff
  );
}
