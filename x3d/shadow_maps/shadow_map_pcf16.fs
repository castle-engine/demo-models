/* Simple PCF with 16 samples */

uniform sampler2DShadow shadowMap;

void main(void)
{
  float offset = gl_TexCoord[0].w / 512.0;

  gl_FragColor = (
    (
      shadow2DProj(shadowMap, gl_TexCoord[0] + vec4(-offset * 1.5, -offset * 1.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[0] + vec4(-offset * 1.5, -offset * 0.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[0] + vec4(-offset * 0.5, -offset * 0.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[0] + vec4(-offset * 0.5, -offset * 1.5, 0.0, 0.0)).r +

      shadow2DProj(shadowMap, gl_TexCoord[0] + vec4(-offset * 1.5,  offset * 1.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[0] + vec4(-offset * 1.5,  offset * 0.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[0] + vec4(-offset * 0.5,  offset * 0.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[0] + vec4(-offset * 0.5,  offset * 1.5, 0.0, 0.0)).r +

      shadow2DProj(shadowMap, gl_TexCoord[0] + vec4( offset * 1.5,  offset * 1.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[0] + vec4( offset * 1.5,  offset * 0.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[0] + vec4( offset * 0.5,  offset * 0.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[0] + vec4( offset * 0.5,  offset * 1.5, 0.0, 0.0)).r +

      shadow2DProj(shadowMap, gl_TexCoord[0] + vec4( offset * 1.5, -offset * 1.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[0] + vec4( offset * 1.5, -offset * 0.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[0] + vec4( offset * 0.5, -offset * 0.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[0] + vec4( offset * 0.5, -offset * 1.5, 0.0, 0.0)).r
    )
    / 16.0 ) * gl_Color;
}
