// 4kampfpanzerin basecode
// Fell and Skomp, 2012-2015

#pragma once

#define WIN32_LEAN_AND_MEAN
#define WIN32_EXTRA_LEAN

#include <windows.h>
#include <mmsystem.h>
#include "mmreg.h"
#include "music.h"
#ifndef USE_CLINKSTER
#include "m4klang.h"
#else
#include "mclinkster.h"
#endif


#ifdef USE_4KLANG_ENV_SYNC
float syncVal[MAX_INSTRUMENTS];
#endif


#pragma code_seg(".musicFuncs")
#ifndef USE_CLINKSTER
#define MusicFrame(t) _4klang_frame(t);
#else
#define MusicFrame(t) _clinkster_frame(t);
#endif

// envelope macro is missing atm

__forceinline void MusicInit() {
#ifndef USE_CLINKSTER
	_4klang_init();
#else
	_clinkster_init();
#endif
}


