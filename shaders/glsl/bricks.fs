/*
  Shader source from
  "Introduction to the OpenGL Shading Language"
  presentation by Randi Rost, 3DLabs (GLSLOverview2005.pdf)
*/

#ifdef GL_ES
precision mediump float;
#endif

uniform vec3 BrickColor;
uniform vec3 MortarColor;
uniform vec2 BrickSize;
uniform vec2 BrickPct;

varying vec2 bricks_position;
varying vec3 color;

void main(void)
{
  vec2 position, useBrick;

  position = bricks_position / BrickSize;

  if (fract(position.y * 0.5) > 0.5) {
    position.x += 0.5;
  }

  position = fract(position);

  useBrick = step(position, BrickPct);

  vec3 fragment_color = color *
    mix(MortarColor, BrickColor, useBrick.x * useBrick.y);
  gl_FragColor = vec4(fragment_color, 1.0);
}
