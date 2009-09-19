uniform sampler2DShadow shadowMap;

float offset = 2 * gl_TexCoord[0].q / 512.0;

void addSample(vec4 point)
{
  /* add ambient term */
  //if (0 < shadow2DProj(shadowMap, gl_TexCoord[0]+point).r)
	//	gl_FragColor += gl_Color;
	//else
	//	gl_FragColor += gl_FrontLightModelProduct.sceneColor + gl_FrontLightProduct[1].ambient;
		gl_FragColor += max (
			shadow2DProj(shadowMap, gl_TexCoord[0]+point).r*gl_Color,
			gl_FrontLightModelProduct.sceneColor + gl_FrontLightProduct[1].ambient);
}

void main (void) {
	gl_FragColor = vec4(0);
	for (float i=-2; i<2; i++)
		for (float j=-2; j<2; j++)
			addSample (vec4(offset*i,offset*j,0,0));
	gl_FragColor /= 16;
}