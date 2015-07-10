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

vec3 ro(vec3 i,vec3 a){
    vec3 h=cos(a),j=sin(a);
         return mat3(h.y*h.z,-h.y*j.z,j.y,
                    h.x*j.z+h.z*j.x*j.y,h.x*h.z-j.x*j.y*j.z,-h.y*j.x,
                    j.x*j.z-h.x*h.z*j.y,h.z*j.x+h.x*j.y*j.z,h.x*h.y)*i;
}

float scale = 0.41;
void main(){
	gl_Position=gl_Vertex;
	uv=(gl_Vertex.xy+1.)/2.;

	// If the Sync Tracker is enabled, sync code gets inserted in place of the below comment; available from any shader.
	//#SYNCCODE#
	
	rd=ro(vec3(gl_Vertex.x*u.x/u.y*scale,gl_Vertex.y*scale,u.y/u.x),cr);
}
