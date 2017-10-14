/* Absolutely trivial GLSL fragment shader combining two textures
   (using max, so chooses the brighter color from two textures). */

uniform sampler2D texture_squirrel;
uniform sampler2D texture_brick;
varying vec4 tex_coord;

void main()
{
  gl_FragColor = max(
    texture2D(texture_squirrel, tex_coord.st),
    texture2D(texture_brick   , tex_coord.st));
}
