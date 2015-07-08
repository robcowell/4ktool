// This is the default vert shader.

// It shows how to use 4kampf's camera system to get an RM ray dir
// when composing scenes (ofc you use a different means of cam 
// control in the final prod).

// Alternatively, see the frag shader comments to see how to calculate
// ray dir there instead.

uniform vec3 u;		// If Standard Uniforms are enabled, this gives {xres, yres, time}
// uniform vec3 cp,up,fd;	// If the camera system is enabled, these uniforms give cam position, up and forward vectors, use in the tool
//vec3 cp=vec3(0,0,0),up=vec3(0,1,0),fd=vec3(0,0,1); // workaround for issue in rendering the prod, as there is no cam system export atm
CAMVARS
uniform float ev[6];		// If 4klang envelopes are enabled, this gives envelopes for tracks 0, 1 and 2

// If not using the cam system, you would create these another way :)
//vec3 cp=vec3(5367.5,-1215.21,-2706.44);
//vec3 up=vec3(-.36,.83,-.43);
//vec3 fd=vec3(-.53,-.56,-.64);	

varying vec2 uv;		// uv coord
varying vec3 rd;		// ray direction for RM

float scale = 0.41;
void main(){
	gl_Position=gl_Vertex;
	uv=(gl_Vertex.xy+1.)/2.;

	// If the Sync Tracker is enabled, sync code gets inserted in place of the below comment; available from any shader.
	//#SYNCCODE#
	
	vec3 q=cross(fd,up);
	rd=mat3(q,cross(q,fd),fd)*vec3(gl_Vertex.x*u.x/u.y*scale,gl_Vertex.y*scale,u.y/u.x);
}
