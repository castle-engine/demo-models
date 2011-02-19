uniform sampler3D white_noise;

const float white_noise_size = 8.0;

/* Linear-interpolated 2D noise.

   Make sure white_noise filtering is AVG_PIXEL for speed.
   Actually GPU does linear interpolation, and this is only a thin
   wrapper over texture3D call, to test that this is similar
   to noise_2d_cos */
float noise_2d_lin(const vec2 v)
{
  return texture3D(white_noise,
    vec3((v.xy * 2.0 + vec2(1.0)) / (white_noise_size * 2.0), 0.0)).r;
}

/* Cosine-interpolated 2D noise.

   Make sure white_noise filtering is NEAREST_PIXEL for speed,
   as it will be queried anyway only for complete pixel values.
   In fact, the standard bilinear filtering will smooth it
   unnecessarily, probably because we just do ''/ white_noise_size'',
   so we don't hit texel center (we should shift by
   0.5 / white_noise_size). */
float noise_2d_cos(const vec2 v)
{
  vec2 f = floor(v);
  vec2 c = ceil(v);

  vec2 c_fr = fract(v);
  c_fr = (cos(vec2(314.0 / 100.0) * (vec2(1.0) - c_fr)) + vec2(1.0)) / vec2(2.0);
  vec2 f_fr = vec2(1.0) - c_fr;

  return
    texture3D(white_noise, vec3(f.xy    , 0.0) / white_noise_size).r * f_fr.x * f_fr.y +
    texture3D(white_noise, vec3(c.x, f.y, 0.0) / white_noise_size).r * c_fr.x * f_fr.y +
    texture3D(white_noise, vec3(f.x, c.y, 0.0) / white_noise_size).r * f_fr.x * c_fr.y +
    texture3D(white_noise, vec3(c.xy    , 0.0) / white_noise_size).r * c_fr.x * c_fr.y;
}

/* Cosine-interpolated 3D noise.
   Just like 2D version. Since everything is on vectors,
   this is trivially similar to noise_2d_cos.
   (We still keep separate noise_2d_cos, as it may be faster.) */
float noise_3d_cos(const vec3 v)
{
  vec3 f = floor(v);
  vec3 c = ceil(v);

  vec3 c_fr = fract(v);
  c_fr = (cos(vec3(314.0 / 100.0) * (vec3(1.0) - c_fr)) + vec3(1.0)) / vec3(2.0);
  vec3 f_fr = vec3(1.0) - c_fr;

  return
    texture3D(white_noise, vec3(f.xyz        ) / white_noise_size).r * f_fr.x * f_fr.y * f_fr.z +
    texture3D(white_noise, vec3(c.x, f.y, f.z) / white_noise_size).r * c_fr.x * f_fr.y * f_fr.z +
    texture3D(white_noise, vec3(f.x, c.y, f.z) / white_noise_size).r * f_fr.x * c_fr.y * f_fr.z +
    texture3D(white_noise, vec3(c.xy    , f.z) / white_noise_size).r * c_fr.x * c_fr.y * f_fr.z +

    texture3D(white_noise, vec3(f.xy    , c.z) / white_noise_size).r * f_fr.x * f_fr.y * c_fr.z +
    texture3D(white_noise, vec3(c.x, f.y, c.z) / white_noise_size).r * c_fr.x * f_fr.y * c_fr.z +
    texture3D(white_noise, vec3(f.x, c.y, c.z) / white_noise_size).r * f_fr.x * c_fr.y * c_fr.z +
    texture3D(white_noise, vec3(c.xyz        ) / white_noise_size).r * c_fr.x * c_fr.y * c_fr.z;
}
