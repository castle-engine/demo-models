uniform sampler2D texture;
uniform sampler2DShadow shadowMap;

void main(void)
{
  float offset = gl_TexCoord[1].w / 1024.0;

  /* PCF with 4x4 kernel */
  float shadow =
    (
      shadow2DProj(shadowMap, gl_TexCoord[1] + vec4(-offset * 1.5, -offset * 1.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[1] + vec4(-offset * 1.5, -offset * 0.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[1] + vec4(-offset * 0.5, -offset * 0.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[1] + vec4(-offset * 0.5, -offset * 1.5, 0.0, 0.0)).r +

      shadow2DProj(shadowMap, gl_TexCoord[1] + vec4(-offset * 1.5,  offset * 1.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[1] + vec4(-offset * 1.5,  offset * 0.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[1] + vec4(-offset * 0.5,  offset * 0.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[1] + vec4(-offset * 0.5,  offset * 1.5, 0.0, 0.0)).r +

      shadow2DProj(shadowMap, gl_TexCoord[1] + vec4( offset * 1.5,  offset * 1.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[1] + vec4( offset * 1.5,  offset * 0.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[1] + vec4( offset * 0.5,  offset * 0.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[1] + vec4( offset * 0.5,  offset * 1.5, 0.0, 0.0)).r +

      shadow2DProj(shadowMap, gl_TexCoord[1] + vec4( offset * 1.5, -offset * 1.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[1] + vec4( offset * 1.5, -offset * 0.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[1] + vec4( offset * 0.5, -offset * 0.5, 0.0, 0.0)).r +
      shadow2DProj(shadowMap, gl_TexCoord[1] + vec4( offset * 0.5, -offset * 1.5, 0.0, 0.0)).r
    )
    / 16.0;

  gl_FragColor = texture2D(texture, gl_TexCoord[0].st) * gl_Color;
  gl_FragColor.rgb *= max(0.1, shadow);
  /* add some ambient light */
  gl_FragColor += vec4(0.05, 0.05, 0.05, 0.0);
}
