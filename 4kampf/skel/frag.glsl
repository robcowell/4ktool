// This is the default frag shader.

uniform vec3 un,cp;					// Standard uniforms; time in .z. Also using cam pos cp.
varying vec3 rd;					// Ray direction

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
	// Alternative method to calculate raydir here:
 	//vec3 rd=normalize(vec3((gl_FragCoord.xy-un.xy/2.0)/un.y,-1.));
	//rd*=transpose(mat3(cross(fd,up),up,-fd));
	
	vec3 p=cp,r=normalize(rd);
	for(int i=0;i<128;i++){
		d=h(p)-M;
		if(d<M)break;
		p+=r*d;
	}
	if(d<M){
		vec3 dc=vec3(.6,.3,.7)*h(p-.1)*16.,
		n=nr(p);
		dc=mix(dc/2.,dc,ao(p,n,.5));
		gl_FragColor=vec4(dc,1.);
	}else
		gl_FragColor=vec4(abs(sin(un.z)),.1,.4,1.);
}
