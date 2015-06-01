// 4kampfpanzerin basecode
// Fell, 2012-2014

// This version of main gets used in Release build, so has Crinkler-ready entrypoint

#define WIN32_LEAN_AND_MEAN
#define WIN32_EXTRA_LEAN
#include <windows.h>
#include <mmsystem.h>
#include "intro.h"
#include "music.h"
#include "../4kampfpanzerin.h"

#pragma data_seg(".pfd")
/*
static const PIXELFORMATDESCRIPTOR pfd = {
    sizeof(PIXELFORMATDESCRIPTOR), 1, PFD_DRAW_TO_WINDOW|PFD_SUPPORT_OPENGL|PFD_DOUBLEBUFFER, PFD_TYPE_RGBA,
    32, 0, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 0, 32, 0, 0, PFD_MAIN_PLANE, 0, 0, 0, 0
};
*/
static const PIXELFORMATDESCRIPTOR pfd = { 0, 0, PFD_SUPPORT_OPENGL | PFD_DOUBLEBUFFER };

/*
#pragma data_seg(".screenSettings")
static DEVMODE screenSettings = {
	{0},0,0,156,0,0x001c0000,{0},0,0,0,0,0,{0},0,32,(DWORD)XRES,(DWORD)YRES,{0},0,
    #if(WINVER >= 0x0400)
    0,0,0,0,0,0,
    #if (WINVER >= 0x0500) || (_WIN32_WINNT >= 0x0400)
    0,0
    #endif
    #endif
};*/

extern "C" {
	int _fltused = 0;
}

#pragma code_seg(".codeInMain")
void entrypoint() {
	//ChangeDisplaySettings(&screenSettings,CDS_FULLSCREEN);
    ShowCursor(0);
	//HWND hWnd = CreateWindow("static",0,WS_POPUP|WS_VISIBLE,0,0,(int)XRES,(int)YRES,0,0,0,0);
	//HWND hWnd = CreateWindow("edit",0,WS_POPUP|WS_VISIBLE|WS_MAXIMIZE,0,0,0,0,0,0,0,0);
	HWND hWnd = CreateWindow((LPCSTR)0xC018, 0, WS_POPUP | WS_VISIBLE | WS_MAXIMIZE, 0, 0, 0, 0, 0, 0, 0, 0);
	//HWND hWnd = CreateWindow((LPCSTR)0xC019, 0, WS_POPUP | WS_VISIBLE | WS_MAXIMIZE, 0, 0, 0, 0, 0, 0, 0, 0);
	HDC hDC = GetDC(hWnd);
    SetPixelFormat(hDC,ChoosePixelFormat(hDC,&pfd),&pfd);
    wglMakeCurrent(hDC,wglCreateContext(hDC));

	IntroInit();
	//SwapBuffers(hDC);	// Remind me why this was here? lol
    MusicInit();

	float t=0;
	do {
		t=MusicFrame();
		IntroFrame(t);
        //wglSwapLayerBuffers(hDC, WGL_SWAP_MAIN_PLANE); 
		SwapBuffers(hDC);
		PeekMessage(0,0,0,0,PM_REMOVE);
    } while (!GetAsyncKeyState(VK_ESCAPE) && t<PROD_LENGTH);

    ExitProcess(0);
}
