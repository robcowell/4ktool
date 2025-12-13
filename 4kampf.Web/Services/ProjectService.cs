namespace _4kampf.Web.Services;

public class ProjectService
{
    public string DefaultVertexShader { get; } = @"#version 300 es
// This is the default vert shader for WebGL.

// It shows how to use 4kampf's camera system to get an RM ray dir
// when composing scenes (ofc you use a different means of cam 
// control in the final prod).

precision mediump float;

uniform vec3 u;			// If Standard Uniforms are enabled, this gives {xres, yres, time}
uniform vec3 cp,cr;		// If the camera system is enabled, these give cam pos and rotation

in vec2 a_position;		// Position attribute

out vec2 uv;			// uv coord
out vec3 rd;			// ray direction for RM

float scale = 0.41;
void main(){
	// Must be set
	gl_Position = vec4(a_position, 0.0, 1.0);

	// Calculate uv
	uv = (a_position + 1.0) / 2.0;

	// Calculate ray direction
	vec3 h = cos(cr), j = sin(cr);
	rd = mat3(
		h.y*h.z, -h.y*j.z, j.y,
		h.x*j.z+h.z*j.x*j.y, h.x*h.z-j.x*j.y*j.z, -h.y*j.x,
		j.x*j.z-h.x*h.z*j.y, h.z*j.x+h.x*j.y*j.z, h.x*h.y
	) * vec3(a_position.x*u.x/u.y*scale, a_position.y*scale, u.y/u.x);
}";

    public string DefaultFragmentShader { get; } = @"#version 300 es
// This is the default frag shader for WebGL.

precision mediump float;

uniform vec3 u;					// Standard uniforms; time in .z. Also using cam pos cp.
uniform vec3 cp;
uniform vec3 cr;

in vec2 uv;
in vec3 rd;						// Ray direction

out vec4 c;

float d,M=.001;						// Epsilon

vec3 rp(vec3 p,vec3 c){				// Domain repeat util
	return mod(p,c)-c/2.;
}

float h(vec3 p){					// The distance function
	p=rp(p,vec3(15));
	return length(p)-3.;
}

vec3 nr(vec3 p){					// Estimate normal at p
	vec2 e=vec2(M,0);
	return vec3(h(p+e.xyy)-h(p-e.xyy),h(p+e.yxy)-h(p-e.yxy),h(p+e.yyx)-h(p-e.yyx));
}

float ao(vec3 p,vec3 n,float s){	// Do AO for point p with normal n and strength s
	float e=1.,a=1.;
	for(float i=1.;i<4.;i++){
		e*=2.;
		a-=(i*s-h(p+n*i*s))/e;
	}
	return a;
}

void main(){						// Entrypoint	
	vec3 v=gl_FragCoord.xyz/u.xyz-.5,q=cos(cr),ppp=sin(cr),g=v+.5,						// Calc ray dir
		r=normalize(mat3(
			q.y*q.z,0,ppp.y,
			q.z*ppp.x*ppp.y,q.x*q.z,-q.y*ppp.x,
			-q.x*q.z*ppp.y,q.z*ppp.x,q.x*q.y
		)*vec3(v.x*u.x/u.y*.2,v.y*.2,u.y/u.x)),p=cp;
	
	for(int i=0;i<128;i++){
		d=h(p)-M;
		if(d<M)break;
		p+=r*d;
	}
	if(d<M){
		vec3 dc=vec3(.6,.3,.7)*h(p-.1)*16.,
		n=nr(p);
		dc=mix(dc/2.,dc,ao(p,n,.5));
		c=vec4(dc, 1.0);
	}else
		c=vec4(vec3(abs(sin(u.z)),.1,.4), 1.0);
}";

    public string DefaultPostProcessShader { get; } = @"#version 300 es
// This is the default postprocessing shader for WebGL

precision mediump float;

uniform sampler2D fr;	// The rendered frame
uniform vec3 u;			// Standard uniforms

in vec2 uv;				// uv

out vec4 fragColor;

void main() {
	// Alternatively method to calculate uv here if needed:
	//vec2 uv=.5+(gl_FragCoord.xy/vec2(u.x,u.y)-.5);
	
	vec4 c=texture(fr,uv);
	
	// Film grain
	float q=uv.x*uv.y*sin(u.z)*7777.;
	c*=1.+clamp(.1+mod(mod(q,223.)*mod(q,11.),.01)*100.,0.,5.)/5.;
	
	// Flicker
	c*=.97+.03*sin(600.*u.z);
	
	// Scanlines
	c*=.95+.05*sin(9.*u.z+uv.y*980.);
	
	// Vignette
    c*=.4+8.*uv.x*uv.y*(1.-uv.x)*(1.-uv.y);					
	//c*=sin(uv.x)*sin(1.-uv.x)*sin(uv.y)*sin(1.-uv.y)*16.;
	
	fragColor=vec4(c.xyz, 1.);
}";
}

