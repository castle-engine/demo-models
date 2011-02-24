uniform sampler2D normalMap;

void PLUG_water_normal_object_space(inout vec3 normal,
  const in vec2 normalMapTexCooord)
{
  normal = texture2D(normalMap, normalMapTexCooord).xyz;

  /* Unpack normal.xy. Blender generates normal maps with Z always > 0,
     so do not unpack Z. Hm, actually, I do not see any visible difference?

     Note: This is normal in tangent space. It so happens that on our
     water surface, this is also valid in object space, because our
     surface is flat on Z=const (and relation of xy to texture st doesn't
     really matter, as it's just a noise for waves, doesn't matter much
     how we map it). So simply transforming by normal_matrix gets us
     into eye-space, and we're happy. */
  normal.xy = normal.xy * 2.0 - vec2(1.0, 1.0);
}
