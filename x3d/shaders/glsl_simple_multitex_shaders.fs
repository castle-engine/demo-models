/* Absolutely trivial GLSL fragment shader combining two textures
   (using max, so chooses the lighter color from two textures). */

uniform sampler2D texture_squirrel;
uniform sampler2D texture_brick;

void main()
{
  gl_FragColor = gl_Color *
    max(
      texture2D(texture_squirrel, gl_TexCoord[0].st),
      texture2D(texture_brick   , gl_TexCoord[1].st));
}
