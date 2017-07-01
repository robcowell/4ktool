// This is the default frag shader.

#version 450 // base code needs this

// setting u to location 0 is required by the basecode
layout(location=0)uniform vec3 u;					// Standard uniforms; time in .z. Also using cam pos cp.
layout(location=1)uniform vec3 cp;
layout(location=2)uniform vec3 cr;
//in vec3 rd;					// Ray direction
out vec3 c;

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
		c=dc;
	}else
		c=vec3(abs(sin(u.z)),.1,.4);
}
