// This is the default vert shader.

// It shows how to use 4kampf's camera system to get an RM ray dir
// when composing scenes (ofc you use a different means of cam 
// control in the final prod).

uniform vec3 u;			// If Standard Uniforms are enabled, this gives {xres, yres, time}
uniform vec3 cp,cr;		// If the camera system is enabled, these give cam pos and rotation

varying vec2 uv;		// uv coord
varying vec3 rd;		// ray direction for RM


float scale = 0.41;
void main(){
	// Must be set
	gl_Position=gl_Vertex;

	// Calculate uv
	uv=(gl_Vertex.xy+1.)/2.;

	// Calculate ray direction
	vec3 h=cos(cr),j=sin(cr);
	rd=mat3(
		h.y*h.z,-h.y*j.z,j.y,
		h.x*j.z+h.z*j.x*j.y,h.x*h.z-j.x*j.y*j.z,-h.y*j.x,
		j.x*j.z-h.x*h.z*j.y,h.z*j.x+h.x*j.y*j.z,h.x*h.y
	)*vec3(gl_Vertex.x*u.x/u.y*scale,gl_Vertex.y*scale,u.y/u.x);
}
