/* This is a demo of a trick headlight in fragment shader.

  Idea: I want to make a headlight that looks like a spot light.
  Doing this by normal OpenGL spot light makes really ugly effect,
  thanks to Gouraud shading. It's possible to calculate any spot light
  per-pixel in shader... but we can also do a trick.
  Since a spot light from head is essentially just enlighting a circle
  in the middle of the screen, we can do this enlighting in fragment
  shader, using gl_FragCoord.

  Actually, it could be implemented much faster as a texture
  projected on 2D screen. This will work without shader,
  and calculating dim amount (sqrt(distance) etc.) will be precalculated
  in texture. Also, this will allow me to render all objects
  normally, using fixed-function pipeline, instead of forcing me to
  switch everything to shader.
  Well, this is just a demo of how to do this in GLSL shader.
  One day I may do this as a texture and use in real-world game.
*/


const int screen_width = 800;
const int screen_height = 600;

/* Headlight outer and inner radius in pixels, between them we'll
   interpolate light. */
const float headlight_outer_radius = 300.0;
const float headlight_inner_radius = 100.0;

/* Larger, the darker the scene will be around headlight */
const float max_light_dim = 3.0;

void main()
{
  gl_FragColor = gl_Color;

  vec2 distance_xy = gl_FragCoord.xy -
    vec2(float(screen_width ) / 2.0,
         float(screen_height) / 2.0);

  /* first, let distance be actually sqr(real distance) */
  float distance = distance_xy.x * distance_xy.x +
                   distance_xy.y * distance_xy.y;

  if (distance > headlight_outer_radius * headlight_outer_radius)
  {
    gl_FragColor /= max_light_dim;
  } else
  if (distance > headlight_inner_radius * headlight_inner_radius)
  {
    /* Now calculate sqrt to have real distance */
    distance = sqrt(distance);

    /* Change distance to be 0..1 within inner...outer headlight radius */
    distance = mix(1.0, 0.0,
      (              distance - headlight_inner_radius) /
      (headlight_outer_radius - headlight_inner_radius));

    /* Much more smooth drop-off, sinusoidal */
    distance = sin((3.14 / 2) * distance);

    gl_FragColor /= mix(max_light_dim, 1.0, distance);
  }
}
