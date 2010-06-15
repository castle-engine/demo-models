//#define PCF4
//#define PCF4_BILINEAR

#define SIZE 1024.0

uniform sampler2D texture;
uniform sampler2DShadow shadowMap;

void main(void)
{
#ifdef PCF4_BILINEAR

  /* We have to scale up/down by texture SIZE to make the floor/fract
     perform real bilinear filtering.
     This also means that we have to handle xy and z separately. */
  vec2 tc_full = SIZE * gl_TexCoord[1].xy / gl_TexCoord[1].w;
  float z = gl_TexCoord[1].z / gl_TexCoord[1].w;

  vec2 tc = floor(tc_full.xy);
  vec2 f = fract(tc_full.xy);
  vec2 f1 = vec2(1.0, 1.0) - f;

  float shadow = (
    shadow2D(shadowMap, vec3( tc.x        / SIZE,  tc.y        / SIZE, z)).r * f1.x * f1.y +
    shadow2D(shadowMap, vec3( tc.x        / SIZE, (tc.y + 1.0) / SIZE, z)).r * f1.x *  f.y +
    shadow2D(shadowMap, vec3((tc.x + 1.0) / SIZE,  tc.y        / SIZE, z)).r *  f.x * f1.y +
    shadow2D(shadowMap, vec3((tc.x + 1.0) / SIZE, (tc.y + 1.0) / SIZE, z)).r *  f.x *  f.y
    ) / 2.0; /* TODO: why /2.0 instead of /4.0 needed? */

#elif defined(PCF4)

  /* PCF with 2x2 kernel */
  float offset = gl_TexCoord[1].w / SIZE;
  float shadow = (
    shadow2DProj(shadowMap, gl_TexCoord[1] + vec4(-offset, -offset, 0.0, 0.0)).r +
    shadow2DProj(shadowMap, gl_TexCoord[1] + vec4(-offset,  offset, 0.0, 0.0)).r +
    shadow2DProj(shadowMap, gl_TexCoord[1] + vec4( offset,  offset, 0.0, 0.0)).r +
    shadow2DProj(shadowMap, gl_TexCoord[1] + vec4( offset, -offset, 0.0, 0.0)).r
    ) / 4.0;

#else

  /* PCF with 4x4 kernel */
  float offset = gl_TexCoord[1].w / SIZE;
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

#endif

  gl_FragColor = texture2D(texture, gl_TexCoord[0].st) * gl_Color;
  /* Use max to make texture and gl_Color slightly visible even inside shadows.
     Do not write 0.1 directly, to avoid fglrx bugs. */
  gl_FragColor.rgb *= max(1.0 / 10.0, shadow);
  /* add some ambient light */
  gl_FragColor += vec4(0.05, 0.05, 0.05, 0.0);
}
