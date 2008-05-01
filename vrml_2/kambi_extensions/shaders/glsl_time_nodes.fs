/* Making an alpha-bands moving on the object.
   Idea: see human telenodes from tremulous (http://tremulous.net/). */

uniform float alpha1;
uniform float alpha2;

/* Passed from vertex shader, simply one component of vertex position. */
varying float height;

/* Passed from vertex shader, color calculated from lights as usual. */
varying vec3 color;

void main(void)
{
  float h = fract(height);
  /* Originally this was:

       if (h > 0.5)
         h = 1.0 - (h - 0.5) * 2.0; else
         h *= 2.0;

     but, of course, fglrx under Linux sucks and rounds float consts
     to ints (I fought with this in other shaders, see toon shader
     comments; yes, this is an evident fglrx bug, yes, only on Radeon with
     fglrx drivers under Linux (same GPU under Mac OS X works fine)). */

  if (h * 2.0 > 1.0)
    h = 1.0 - (h * 2.0 - 1.0); else
    h *= 2.0;

  h = pow(h, 5.0);

  gl_FragColor = vec4(color, mix(alpha1, alpha2, h));
}
