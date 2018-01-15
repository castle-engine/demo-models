/* Fragment shader effect for caustics underwater.

   Uses Castle Game Engine shader effects
   ( https://castle-engine.sourceforge.io/compositing_shaders.php )
   to cooperate with engine shaders.
*/

uniform bool onProjShadowMap;
uniform bool onRefractTex;
uniform sampler2D textureMap;
uniform sampler3D wavesMap;
uniform sampler2DShadow shadowMap;
uniform float frame;

varying vec4 tex_coord0;
varying vec4 tex_coord1;
varying vec4 tex_coord2;

void PLUG_texture_apply(inout vec4 fragment_color, const in vec3 normal)
{
  float shadow = shadow2DProj(shadowMap, tex_coord2).x;
  if (onProjShadowMap) {
    fragment_color = vec4(shadow);
    return;
  }
  fragment_color *= texture2D(textureMap, tex_coord0.st);
  if (!onRefractTex) {
    float caustics = texture3D(wavesMap, vec3(tex_coord1.st, frame)).a * 2.0;
    fragment_color *= mix(1.0, caustics, shadow);
  }
}
