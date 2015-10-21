// 4kampfpanzerin basecode
// Fell and Skomp, 2012-2015

#pragma once

#define WIN32_LEAN_AND_MEAN
#define WIN32_EXTRA_LEAN

#include <windows.h>
#include <GL/gl.h>
#include <GL/glu.h>
#include <stdio.h>
#include "glext.h"
#include "../4klang.h"
#include "../4kampfpanzerin.h"

#define XRES 1280.
#define YRES 720.

#ifdef USE_4KLANG_ENV_SYNC
extern float syncVal[MAX_INSTRUMENTS];
#endif

#pragma data_seg(".introStaticData")
static GLuint sceneProg;
#ifdef USE_PP
static GLuint ppProg, rtt, fbo;
#endif

#pragma code_seg(".introShaderBuildingStuff")
void AddShaderToProg(GLuint prog, const char **source, GLuint shaderType) {
	GLuint id = ((PFNGLCREATESHADERPROC)wglGetProcAddress("glCreateShader"))(shaderType); 
	((PFNGLSHADERSOURCEPROC)wglGetProcAddress("glShaderSource"))(id, 1, source, 0);
    ((PFNGLCOMPILESHADERPROC)wglGetProcAddress("glCompileShader"))(id);
	((PFNGLATTACHSHADERPROC)wglGetProcAddress("glAttachShader"))(prog, id);
#ifdef DEBUG
	int	result;
    char info[60000];
	((PFNGLGETOBJECTPARAMETERIVARBPROC)wglGetProcAddress("glGetObjectParameterivARB"))(id, GL_OBJECT_COMPILE_STATUS_ARB, &result);
	((PFNGLGETINFOLOGARBPROC)wglGetProcAddress("glGetInfoLogARB"))(id, 60000, NULL, (char *)info);
	fprintf(stdout, "SHADER COMPILE LOG (Prog %i):\n", prog);
	fprintf(stdout, info);
#endif
}

#ifdef USE_PP
GLuint BuildShaderProg(const char **fragShaderSource) {
	GLuint prog = ((PFNGLCREATEPROGRAMPROC)wglGetProcAddress("glCreateProgram"))();
	AddShaderToProg(prog, vertShader, GL_VERTEX_SHADER);
	AddShaderToProg(prog, fragShaderSource, GL_FRAGMENT_SHADER);
	((PFNGLLINKPROGRAMPROC)wglGetProcAddress("glLinkProgram"))(prog);
	return prog;
}
#endif

#pragma code_seg(".introInit")
__forceinline void IntroInit() {
#ifdef USE_PP
	sceneProg = BuildShaderProg(fragShader);
	ppProg = BuildShaderProg(ppShader);

	((PFNGLGENFRAMEBUFFERSPROC)wglGetProcAddress("glGenFramebuffers"))(1, &fbo);
	((PFNGLBINDFRAMEBUFFERPROC)wglGetProcAddress("glBindFramebuffer"))(GL_FRAMEBUFFER, fbo);
	glGenTextures(1, &rtt);
	glBindTexture(GL_TEXTURE_2D, rtt);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, (GLsizei)XRES, (GLsizei)YRES, 0, GL_RGBA, GL_HALF_FLOAT, NULL);
	((PFNGLFRAMEBUFFERTEXTURE2DPROC)wglGetProcAddress("glFramebufferTexture2D"))(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D, rtt, 0);
#else
	sceneProg = ((PFNGLCREATEPROGRAMPROC)wglGetProcAddress("glCreateProgram"))();
	AddShaderToProg(sceneProg, vertShader, GL_VERTEX_SHADER);
	AddShaderToProg(sceneProg, fragShader, GL_FRAGMENT_SHADER);
	((PFNGLLINKPROGRAMPROC)wglGetProcAddress("glLinkProgram"))(sceneProg);
	((PFNGLUSEPROGRAMPROC)wglGetProcAddress("glUseProgram"))(sceneProg);
#endif
}

#pragma code_seg(".introFrameFunc")
__forceinline void IntroFrame(float t) {
#ifdef USE_PP
	((PFNGLBINDFRAMEBUFFERPROC)wglGetProcAddress("glBindFramebuffer"))(GL_FRAMEBUFFER, fbo);
	((PFNGLUSEPROGRAMPROC)wglGetProcAddress("glUseProgram"))(sceneProg);
#endif
#ifdef USE_STANDARD_UNIFORMS
	((PFNGLUNIFORM3FPROC)wglGetProcAddress("glUniform3f"))(((PFNGLGETUNIFORMLOCATIONPROC)wglGetProcAddress("glGetUniformLocation"))(sceneProg, "u"), XRES, YRES, t);
#endif
#ifdef USE_4KLANG_ENV_SYNC
	((PFNGLUNIFORM1FVPROC)wglGetProcAddress("glUniform1fv"))(((PFNGLGETUNIFORMLOCATIONPROC)wglGetProcAddress("glGetUniformLocation"))(sceneProg, "ev"), MAX_INSTRUMENTS, syncVal);
#endif
	glRects(-1, -1, 1, 1);
#ifdef USE_PP	
	((PFNGLBINDFRAMEBUFFERPROC)wglGetProcAddress("glBindFramebuffer"))(GL_FRAMEBUFFER, 0);
	((PFNGLUSEPROGRAMPROC)wglGetProcAddress("glUseProgram"))(ppProg);
	glBindTexture(GL_TEXTURE_2D, rtt);
#ifdef USE_STANDARD_UNIFORMS
	((PFNGLUNIFORM3FPROC)wglGetProcAddress("glUniform3f"))(((PFNGLGETUNIFORMLOCATIONPROC)wglGetProcAddress("glGetUniformLocation"))(ppProg, "u"), XRES, YRES, t);
#endif
	((PFNGLUNIFORM1IPROC)wglGetProcAddress("glUniform1i"))(((PFNGLGETUNIFORMLOCATIONPROC)wglGetProcAddress("glGetUniformLocation"))(ppProg, "fr"), 0);
	glRects(-1, -1, 1, 1);
#endif
}

