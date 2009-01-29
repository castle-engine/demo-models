/* This is used to test ability to pass various VRML types to various
   GLSL types. The GLSL code doesn't make much sense on it's own,
   it just makes sure to use all possible GLSL types, to test them.

   If all is white, then the test is sucess.
   If you see and red (or other) colors,
   then some uniform is not correctly set. */

uniform bool my_bool;
uniform int my_long;
uniform int my_int32;

uniform vec4 my_color;
uniform vec4 my_rotation;

uniform float my_float;
uniform vec2 my_vec2;
uniform vec3 my_vec3;
uniform vec4 my_vec4;
uniform mat3 my_mat3;
uniform mat4 my_mat4;

/* Double-precision (in VRML) are converted to single-precision for GLSL,
   since GLSL doesn't support double precision. */
uniform float my_time;
uniform float my_double;
uniform vec2 my_dvec2;
uniform vec3 my_dvec3;
uniform vec4 my_dvec4;
uniform mat3 my_dmat3;
uniform mat4 my_dmat4;

void main(void)
{
  if (int(gl_FragCoord[0]) % 18 == 0)
  {
    /* my_bool should be true, so color should be white. */
    gl_FragColor = vec4(
      1.0,
      (my_bool ? 1.0 : 0.0),
      (my_bool ? 1.0 : 0.0), 1.0);
  } else
  if (int(gl_FragCoord[0]) % 18 == 1)
  {
    gl_FragColor = vec4(
      1.0,
      (my_long == 666 ? 1.0 : 0.0),
      (my_long == 666 ? 1.0 : 0.0), 1.0);
  } else
  if (int(gl_FragCoord[0]) % 18 == 2)
  {
    gl_FragColor = vec4(
      1.0,
      (my_int32 == 1234 ? 1.0 : 0.0),
      (my_int32 == 1234 ? 1.0 : 0.0), 1.0);
  } else
  if (int(gl_FragCoord[0]) % 18 == 3)
  {
    gl_FragColor = my_color;
  } else
  if (int(gl_FragCoord[0]) % 18 == 4)
  {
    gl_FragColor = my_rotation;
  } else
  if (int(gl_FragCoord[0]) % 18 == 5)
  {
    gl_FragColor = vec4(1.0, my_float, my_float, 1.0);
  } else
  if (int(gl_FragCoord[0]) % 18 == 6)
  {
    gl_FragColor = vec4(1.0, my_vec2.x, my_vec2.y, 1.0);
  } else
  if (int(gl_FragCoord[0]) % 18 == 7)
  {
    gl_FragColor = vec4(my_vec3, 1.0);
  } else
  if (int(gl_FragCoord[0]) % 18 == 8)
  {
    gl_FragColor = my_vec4;
  } else
  if (int(gl_FragCoord[0]) % 18 == 9)
  {
    gl_FragColor = vec4(my_mat3[0], 1.0);
  } else
  if (int(gl_FragCoord[0]) % 18 == 10)
  {
    gl_FragColor = my_mat4[0];
  } else
  if (int(gl_FragCoord[0]) % 18 == 11)
  {
    gl_FragColor = vec4(1.0, my_double, my_double, 1.0);
  } else
  if (int(gl_FragCoord[0]) % 18 == 12)
  {
    gl_FragColor = vec4(1.0, my_time, my_time, 1.0);
  } else
  if (int(gl_FragCoord[0]) % 18 == 13)
  {
    gl_FragColor = vec4(1.0, my_dvec2.x, my_dvec2.y, 1.0);
  } else
  if (int(gl_FragCoord[0]) % 18 == 14)
  {
    gl_FragColor = vec4(my_dvec3, 1.0);
  } else
  if (int(gl_FragCoord[0]) % 18 == 15)
  {
    gl_FragColor = my_dvec4;
  } else
  if (int(gl_FragCoord[0]) % 18 == 16)
  {
    gl_FragColor = vec4(my_dmat3[0], 1.0);
  } else
  if (int(gl_FragCoord[0]) % 18 == 17)
  {
    gl_FragColor = my_dmat4[0];
  } else
    gl_FragColor = vec4(0.0, 0.0, 0.0, 0.0);
}
