// fragment shader

uniform sampler2D depthMap;
uniform float Near;
uniform float Far;

float texDepth (in vec4 coord) {
  float z = texture2DProj(depthMap, coord).r;
  return  z * Near / (Far - z * (Far - Near));
}

void main (void) {
  float limit = 0.001;
  float sampleDepth;
  float offset = 0.1;
  float ao = 0.0;
  float d = 0.0;
  float ao_min = 0.0;
  float ao_max = 1.0;
  float num_samples = 16.0;
  float fragDepth = clamp ((1.0 / gl_FragCoord.w - Near) / (Far - Near), ao_min, ao_max);
  vec4 Nor = vec4(gl_TexCoord[1].st, 0.0, 0.0) * 0.8 * offset;
  
  sampleDepth = texDepth (gl_TexCoord[0] + vec4(offset * -0.4, offset * 0.2, 0.0, 0.0) + Nor);
  d = clamp (fragDepth - sampleDepth, ao_min, ao_max);
  ao += min (d, limit);
  sampleDepth = texDepth (gl_TexCoord[0] + vec4(offset * 0.1, offset * 0.1, 0.0, 0.0) + Nor);
  d = clamp (fragDepth - sampleDepth, ao_min, ao_max);
  ao += min (d, limit);
  sampleDepth = texDepth (gl_TexCoord[0] + vec4(offset * 0.4, offset * -0.2, 0.0, 0.0) + Nor);
  d = clamp (fragDepth - sampleDepth, ao_min, ao_max);
  ao += min (d, limit);
  sampleDepth = texDepth (gl_TexCoord[0] + vec4(offset * -0.3, offset * -0.4, 0.0, 0.0) + Nor);
  d = clamp (fragDepth - sampleDepth, ao_min, ao_max);
  ao += min (d, limit);

  sampleDepth = texDepth (gl_TexCoord[0] + vec4(offset * -0.5, offset * 0.6, 0.0, 0.0) + Nor);
  d = clamp (fragDepth - sampleDepth, ao_min, ao_max);
  ao += min (d, limit);
  sampleDepth = texDepth (gl_TexCoord[0] + vec4(offset * 0.2, offset * 0.5, 0.0, 0.0) + Nor);
  d = clamp (fragDepth - sampleDepth, ao_min, ao_max);
  ao += min (d, limit);
  sampleDepth = texDepth (gl_TexCoord[0] + vec4(offset * 0.6, offset * -0.5, 0.0, 0.0) + Nor);
  d = clamp (fragDepth - sampleDepth, ao_min, ao_max);
  ao += min (d, limit);
  sampleDepth = texDepth (gl_TexCoord[0] + vec4(offset * -0.6, offset * -0.6, 0.0, 0.0) + Nor);
  d = clamp (fragDepth - sampleDepth, ao_min, ao_max);
  ao += min (d, limit);

  sampleDepth = texDepth (gl_TexCoord[0] + vec4(offset * -0.1, offset * 0.8, 0.0, 0.0) + Nor);
  d = clamp (fragDepth - sampleDepth, ao_min, ao_max);
  ao += min (d, limit);
  sampleDepth = texDepth (gl_TexCoord[0] + vec4(offset * 0.7, offset * 0.4, 0.0, 0.0) + Nor);
  d = clamp (fragDepth - sampleDepth, ao_min, ao_max);
  ao += min (d, limit);
  sampleDepth =texDepth (gl_TexCoord[0] + vec4(offset * -0.7, offset * -0.3, 0.0, 0.0) + Nor);
  d = clamp (fragDepth - sampleDepth, ao_min, ao_max);
  ao += min (d, limit);
  sampleDepth = texDepth (gl_TexCoord[0] + vec4(offset * 0.3, offset * -0.7, 0.0, 0.0) + Nor);
  d = clamp (fragDepth - sampleDepth, ao_min, ao_max);
  ao += min (d, limit);

  sampleDepth = texDepth (gl_TexCoord[0] + vec4(offset * 0.5, offset * 0.7, 0.0, 0.0) + Nor);
  d = clamp (fragDepth - sampleDepth, ao_min, ao_max);
  ao += min (d, limit);
  sampleDepth = texDepth (gl_TexCoord[0] + vec4(offset * -0.8, offset * 0.3, 0.0, 0.0) + Nor);
  d = clamp (fragDepth - sampleDepth, ao_min, ao_max);
  ao += min (d, limit);
  sampleDepth = texDepth (gl_TexCoord[0] + vec4(offset * 0.8, offset * -0.1, 0.0, 0.0) + Nor);
  d = clamp (fragDepth - sampleDepth, ao_min, ao_max);
  ao += min (d, limit);
  sampleDepth = texDepth (gl_TexCoord[0] + vec4(offset * -0.2, offset * -0.8, 0.0, 0.0) + Nor);
  d = clamp (fragDepth - sampleDepth, ao_min, ao_max);
  ao += min (d, limit);
  
  ao /= num_samples;

  gl_FragColor = pow(1.0 - ao, 300.0) * gl_Color;
  gl_FragColor.a = gl_Color.a;
}
