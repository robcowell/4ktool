// 4kampfpanzerin basecode
// Fell and Skomp, 2012-2015

// This version of main gets used in Debug build, so has regular win32 entrypoint

#define WIN32_LEAN_AND_MEAN
#define WIN32_EXTRA_LEAN

#include <windows.h>
#include <windowsx.h>
#include <mmsystem.h>
#include <GL/gl.h>
#include <string.h>
#include <stdio.h>
#include <math.h>
#include "intro.h"

#include "music.h"

#include "../4kampfpanzerin.h"

typedef struct {
    HINSTANCE   hInstance;
    HDC         hDC;
    HGLRC       hRC;
    HWND        hWnd;
    int         full;
    char        wndclass[5];
} WININFO;

static const PIXELFORMATDESCRIPTOR pfd = {
    sizeof(PIXELFORMATDESCRIPTOR), 1, PFD_DRAW_TO_WINDOW|PFD_SUPPORT_OPENGL|PFD_DOUBLEBUFFER, PFD_TYPE_RGBA,
    32, 0, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 0, 32, 0, 0, PFD_MAIN_PLANE, 0, 0, 0, 0
};

static WININFO wininfo = {  
	0,0,0,0,0, {"TRSI"}
};

static LRESULT CALLBACK WndProc(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam) {
	if(uMsg==WM_SYSCOMMAND && (wParam==SC_SCREENSAVE || wParam==SC_MONITORPOWER))
		return(0);

	if(uMsg==WM_CLOSE || uMsg==WM_DESTROY || (uMsg==WM_KEYDOWN && wParam==VK_ESCAPE)) {
		PostQuitMessage(0);
        return(0);
	}

    if(uMsg==WM_SIZE)
        glViewport(0, 0, lParam&65535, lParam>>16);

    if((uMsg==WM_CHAR || uMsg==WM_KEYDOWN) && wParam==VK_ESCAPE) {
        PostQuitMessage(0);
        return(0);
    }

    return(DefWindowProc(hWnd,uMsg,wParam,lParam));
}

static void WindowEnd(WININFO *info) {
    if(info->hRC) {
        wglMakeCurrent(0, 0);
        wglDeleteContext(info->hRC);
    }

    if(info->hDC) 
		ReleaseDC(info->hWnd, info->hDC);
    if(info->hWnd) 
		DestroyWindow(info->hWnd);

    UnregisterClass(info->wndclass, info->hInstance);

    if(info->full) {
        ChangeDisplaySettings(0, 0);
		while(ShowCursor(1)<0); // show cursor
    }
}

static int WindowInit(WININFO *info) {
	unsigned int	PixelFormat;
    DWORD			dwExStyle, dwStyle;
    DEVMODE			dmScreenSettings;
    RECT			rec;
    WNDCLASS		wc;

    ZeroMemory(&wc, sizeof(WNDCLASS));
    wc.style         = CS_OWNDC|CS_HREDRAW|CS_VREDRAW;
    wc.lpfnWndProc   = WndProc;
    wc.hInstance     = info->hInstance;
    wc.lpszClassName = info->wndclass;
    wc.hbrBackground =(HBRUSH)CreateSolidBrush(0);
	
    if(!RegisterClass(&wc))
        return(0);

    if(info->full) {
        dmScreenSettings.dmSize       = sizeof(DEVMODE);
        dmScreenSettings.dmFields     = DM_BITSPERPEL|DM_PELSWIDTH|DM_PELSHEIGHT;
        dmScreenSettings.dmBitsPerPel = 32;
        dmScreenSettings.dmPelsWidth  = (int)XRES;
        dmScreenSettings.dmPelsHeight = (int)YRES;

        if(ChangeDisplaySettings(&dmScreenSettings,CDS_FULLSCREEN)!=DISP_CHANGE_SUCCESSFUL)
            return(0);

        dwExStyle = WS_EX_APPWINDOW;
        dwStyle   = WS_VISIBLE | WS_POPUP;

		while(ShowCursor(0)>=0);	// hide cursor
    } else {
        dwExStyle = 0;
        dwStyle   = WS_VISIBLE | WS_CAPTION | WS_SYSMENU | WS_MAXIMIZEBOX | WS_OVERLAPPED;
		dwStyle   = WS_VISIBLE | WS_OVERLAPPEDWINDOW|WS_POPUP;
    }

    rec.left   = 0;
    rec.top    = 0;
    rec.right  = (int)XRES;
    rec.bottom = (int)YRES;

    AdjustWindowRect(&rec, dwStyle, 0);

    info->hWnd = CreateWindowEx(dwExStyle, wc.lpszClassName, "Precalcing!", dwStyle,
                               (GetSystemMetrics(SM_CXSCREEN)-rec.right+rec.left)>>1,
                               (GetSystemMetrics(SM_CYSCREEN)-rec.bottom+rec.top)>>1,
                               rec.right-rec.left, rec.bottom-rec.top, 0, 0, info->hInstance, 0);

    if(!info->hWnd)
        return(0);

    if(!(info->hDC=GetDC(info->hWnd)))
        return(0);

    if(!(PixelFormat=ChoosePixelFormat(info->hDC,&pfd)))
        return(0);

    if(!SetPixelFormat(info->hDC,PixelFormat,&pfd))
        return(0);

    if(!(info->hRC=wglCreateContext(info->hDC)))
        return(0);

    if(!wglMakeCurrent(info->hDC,info->hRC))
        return(0);

    return(1);
}

static char wintitle[120];

extern float syncVal[MAX_INSTRUMENTS];

int main(){
    MSG msg;
    int done=0;
    WININFO *info = &wininfo;

    info->hInstance = GetModuleHandle(0);

	if(MessageBox(0, "I'm going owl hunting - did you say \"hoo\"?", "4kampfpanzerin Debug Prod Build", MB_YESNO|MB_ICONQUESTION)==IDYES) info->full++;

    if(!WindowInit(info)) {
        WindowEnd(info);
        MessageBox(0, "Gaaah window init error!","TRSI have errorzed your shit",MB_OK|MB_ICONEXCLAMATION);
        return(0);
    }

	IntroInit();	
	MusicInit();

	int fps = 0, frameCount = 0;
	float t, fpsCountStartTime=0;
	while(!done) {
		t = MusicFrame();

		if (t - fpsCountStartTime > 1.0f) {
			fps = frameCount;
			frameCount = 0;
			fpsCountStartTime = t;
		}
		frameCount++;
		if (t>PROD_LENGTH)
			done = true;
        while(PeekMessage(&msg,0,0,0,PM_REMOVE)) {
            if(msg.message==WM_QUIT)
				done=1;

		    TranslateMessage(&msg);
            DispatchMessage(&msg);
        }
        IntroFrame(t);
		sprintf(wintitle, "FPS:%i; t:%.2f", fps, t);
		SetWindowText(info->hWnd, wintitle);
        SwapBuffers(info->hDC);
    }

	WindowEnd(info);
    return(0);
}