/* This tests passing various VRML types to various
   GLSL types. The GLSL code makes sure to use all possible GLSL types.
   Color depends only on int(gl_FragCoord.x) % 36, for each uniform type
   tested.

   If all is white, then the test succeded.
   If you see any other colors (typically, red vertical stripes),
   then some uniform is not correctly set.

   Note: to have % operator available, this needs

   - OpenGLES: #version 300 es
   - OpenGL: #version 130

   CGE will declare such versions (even higher) if possible on this GPU.
*/

uniform bool my_bool;
uniform int my_long;
uniform int my_int32;

uniform vec3 my_color;
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

/* Now repeat this for array types */
uniform bool a_bool[2];
uniform int a_long[2];
uniform int a_int32[2];
uniform vec3 a_color[2];
uniform vec4 a_rotation[2];
uniform float a_float[2];
uniform vec2 a_vec2[2];
uniform vec3 a_vec3[2];
uniform vec4 a_vec4[2];
uniform mat3 a_mat3[2];
uniform mat4 a_mat4[2];
uniform float a_time[2];
uniform float a_double[2];
uniform vec2 a_dvec2[2];
uniform vec3 a_dvec3[2];
uniform vec4 a_dvec4[2];
uniform mat3 a_dmat3[2];
uniform mat4 a_dmat4[2];

void main(void)
{
  /* Reason for the first "int(gl_FragCoord.x) % 36 < 18":
     fglrx (closed-source ATI (Radeon) Linux drivers) bug.
     "if-else" clauses with more than 33
     parts produce very weird results, some random parts of pixels
     get black then (not only columns, black diagonal stripes pass through
     whole screen where shaders run).
  */

  if (int(gl_FragCoord.x) % 36 < 18)
  {
    if (int(gl_FragCoord.x) % 36 == 0)
    {
      /* my_bool should be true, so color should be white. */
      gl_FragColor = vec4(
        1.0,
        (my_bool ? 1.0 : 0.0),
        (my_bool ? 1.0 : 0.0), 1.0);
    } else
    if (int(gl_FragCoord.x) % 36 == 1)
    {
      gl_FragColor = vec4(
        1.0,
        (my_long == 666 ? 1.0 : 0.0),
        (my_long == 666 ? 1.0 : 0.0), 1.0);
    } else
    if (int(gl_FragCoord.x) % 36 == 2)
    {
      gl_FragColor = vec4(
        1.0,
        (my_int32 == 1234 ? 1.0 : 0.0),
        (my_int32 == 1234 ? 1.0 : 0.0), 1.0);
    } else
    if (int(gl_FragCoord.x) % 36 == 3)
    {
      gl_FragColor = vec4(my_color, 1.0);
    } else
    if (int(gl_FragCoord.x) % 36 == 4)
    {
      gl_FragColor = my_rotation;
    } else
    if (int(gl_FragCoord.x) % 36 == 5)
    {
      gl_FragColor = vec4(1.0, my_float, my_float, 1.0);
    } else
    if (int(gl_FragCoord.x) % 36 == 6)
    {
      gl_FragColor = vec4(1.0, my_vec2.x, my_vec2.y, 1.0);
    } else
    if (int(gl_FragCoord.x) % 36 == 7)
    {
      gl_FragColor = vec4(my_vec3, 1.0);
    } else
    if (int(gl_FragCoord.x) % 36 == 8)
    {
      gl_FragColor = my_vec4;
    } else
    if (int(gl_FragCoord.x) % 36 == 9)
    {
      gl_FragColor = vec4(my_mat3[0], 1.0);
    } else
    if (int(gl_FragCoord.x) % 36 == 10)
    {
      gl_FragColor = my_mat4[0];
    } else
    if (int(gl_FragCoord.x) % 36 == 11)
    {
      gl_FragColor = vec4(1.0, my_double, my_double, 1.0);
    } else
    if (int(gl_FragCoord.x) % 36 == 12)
    {
      gl_FragColor = vec4(1.0, my_time, my_time, 1.0);
    } else
    if (int(gl_FragCoord.x) % 36 == 13)
    {
      gl_FragColor = vec4(1.0, my_dvec2.x, my_dvec2.y, 1.0);
    } else
    if (int(gl_FragCoord.x) % 36 == 14)
    {
      gl_FragColor = vec4(my_dvec3, 1.0);
    } else
    if (int(gl_FragCoord.x) % 36 == 15)
    {
      gl_FragColor = my_dvec4;
    } else
    if (int(gl_FragCoord.x) % 36 == 16)
    {
      gl_FragColor = vec4(my_dmat3[0], 1.0);
    } else
    if (int(gl_FragCoord.x) % 36 == 17)
    {
      gl_FragColor = my_dmat4[0];
    }
  } else

  /* Double-precision types */

  if (int(gl_FragCoord.x) % 36 == 18)
  {
    /* a_bool should be true, so color should be white. */
    gl_FragColor = vec4(
      1.0,
      (a_bool[1] ? 1.0 : 0.0),
      (a_bool[1] ? 1.0 : 0.0), 1.0);
  } else
  if (int(gl_FragCoord.x) % 36 == 19)
  {
    gl_FragColor = vec4(
      1.0,
      (a_long[1] == 666 ? 1.0 : 0.0),
      (a_long[1] == 666 ? 1.0 : 0.0), 1.0);
  } else
  if (int(gl_FragCoord.x) % 36 == 20)
  {
    gl_FragColor = vec4(
      1.0,
      (a_int32[1] == 1234 ? 1.0 : 0.0),
      (a_int32[1] == 1234 ? 1.0 : 0.0), 1.0);
  } else
  if (int(gl_FragCoord.x) % 36 == 21)
  {
    gl_FragColor = vec4(a_color[1], 1.0);
  } else
  if (int(gl_FragCoord.x) % 36 == 22)
  {
    gl_FragColor = a_rotation[1];
  } else
  if (int(gl_FragCoord.x) % 36 == 23)
  {
    gl_FragColor = vec4(1.0, a_float[1], a_float[1], 1.0);
  } else
  if (int(gl_FragCoord.x) % 36 == 24)
  {
    gl_FragColor = vec4(1.0, a_vec2[1].x, a_vec2[1].y, 1.0);
  } else
  if (int(gl_FragCoord.x) % 36 == 25)
  {
    gl_FragColor = vec4(a_vec3[1], 1.0);
  } else
  if (int(gl_FragCoord.x) % 36 == 26)
  {
    gl_FragColor = a_vec4[1];
  } else
  if (int(gl_FragCoord.x) % 36 == 27)
  {
    gl_FragColor = vec4(a_mat3[1][0], 1.0);
  } else
  if (int(gl_FragCoord.x) % 36 == 28)
  {
    gl_FragColor = a_mat4[1][0];
  } else
  if (int(gl_FragCoord.x) % 36 == 29)
  {
    gl_FragColor = vec4(1.0, a_double[1], a_double[1], 1.0);
  } else
  if (int(gl_FragCoord.x) % 36 == 30)
  {
    gl_FragColor = vec4(1.0, a_time[1], a_time[1], 1.0);
  } else
  if (int(gl_FragCoord.x) % 36 == 31)
  {
    gl_FragColor = vec4(1.0, a_dvec2[1].x, a_dvec2[1].y, 1.0);
  } else
  if (int(gl_FragCoord.x) % 36 == 32)
  {
    gl_FragColor = vec4(a_dvec3[1], 1.0);
  } else
  if (int(gl_FragCoord.x) % 36 == 33)
  {
    gl_FragColor = a_dvec4[1];
  } else
  if (int(gl_FragCoord.x) % 36 == 34)
  {
    gl_FragColor = vec4(a_dmat3[1][0], 1.0);
  } else
  if (int(gl_FragCoord.x) % 36 == 35)
  {
    gl_FragColor = a_dmat4[1][0];
  } else

    gl_FragColor = vec4(0.0, 0.0, 0.0, 0.0);
}
