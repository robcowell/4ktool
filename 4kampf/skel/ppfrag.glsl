// This is the default postprocessing shader :)

uniform sampler2D fr;	// The rendered frame
uniform vec3 un;		// Standard uniforms
varying vec2 uv;		// uv

void main() {
	// Alternatively method to calculate uv here if needed:
	//vec2 uv=.5+(gl_FragCoord.xy/vec2(un.x,un.y)-.5);
	
	vec4 c=texture2D(fr,uv);
	
	// Film grain
	float q=uv.x*uv.y*sin(un.z)*7777.;
	c*=1.+clamp(.1+mod(mod(q,223.)*mod(q,11.),.01)*100.,0.,5.)/5.;
	
	// Flicker
	c*=.97+.03*sin(600.*un.z);
	
	// Scanlines
	c*=.95+.05*sin(9.*un.z+uv.y*980.);
	
	// Vignette
    c*=.4+8.*uv.x*uv.y*(1.-uv.x)*(1.-uv.y);					
	//c*=sin(uv.x)*sin(1.-uv.x)*sin(uv.y)*sin(1.-uv.y)*16.;
	
	gl_FragColor=vec4(c.xyz, 1.);
}
