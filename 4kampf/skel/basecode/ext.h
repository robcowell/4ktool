#pragma once

#include <GL/gl.h>
#include "glext.h"


#ifdef DEBUG
#define NUM_FUNCS 8
#else
#define NUM_FUNCS 6
#endif

#define oglCreateShaderProgramv         ((PFNGLCREATESHADERPROGRAMVPROC)myglfunc[0])
#define oglGenFramebuffers				((PFNGLGENFRAMEBUFFERSPROC)myglfunc[1])
#define oglBindFramebuffer				((PFNGLBINDFRAMEBUFFERPROC)myglfunc[2])
#define oglUseProgram		            ((PFNGLUSEPROGRAMPROC)myglfunc[3])
#define oglUniform3fv            ((PFNGLUNIFORM3FVPROC)myglfunc[4])
#define oglFramebufferTexture2D			((PFNGLFRAMEBUFFERTEXTURE2DPROC)myglfunc[5])

#ifdef DEBUG
#define oglGetProgramiv          ((PFNGLGETPROGRAMIVPROC)myglfunc[6])
#define oglGetProgramInfoLog     ((PFNGLGETPROGRAMINFOLOGPROC)myglfunc[7])
#endif


static const char *strs[] = {
	"glCreateShaderProgramv",
	"glGenFramebuffers",
	"glBindFramebuffer",
	"glUseProgram",
	"glUniform3fv",
	"glFramebufferTexture2D",
	//--
#ifdef DEBUG
	"glGetProgramiv",
	"glGetProgramInfoLog",
#endif
};


void *myglfunc[NUM_FUNCS];

int EXT_Init(void)
{
	for (int i = 0; i<NUM_FUNCS; i++)
	{
		myglfunc[i] = wglGetProcAddress(strs[i]);
		if (!myglfunc[i])
			return(0);
	}
	return(1);
}