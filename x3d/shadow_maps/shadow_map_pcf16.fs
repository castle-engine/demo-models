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

  /* Uncomment to keep some non-zero color.

     (Teoretically, scaling gl_Color makes no sense. If the light
     is shadowed, then it doesn't light at all -> so it should not be scaled,
     it should be set to zero. Ambient term, if any, should only be added.
     But in practice, this hides the model shape in the shadow,
     so it's ugly.

     So we fake global lighting nicer by scaling gl_Color.)
  */
  /*  gl_FragColor += gl_Color / 5.0; */
}
