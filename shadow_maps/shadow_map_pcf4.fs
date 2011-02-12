/* Simple PCF with 4 samples */

uniform sampler2DShadow shadowMap;

void main(void)
{
  float offset = gl_TexCoord[0].w / 512.0;

  gl_FragColor = (
    (
      shadow2DProj(shadowMap, gl_TexCoord[0] + vec4(-offset, -offset, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[0] + vec4(-offset,  offset, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[0] + vec4( offset,  offset, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[0] + vec4( offset, -offset, 0.0, 0.0)).r
    )
    / 4.0 ) * gl_Color;
}
