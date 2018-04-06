// 4kampfpanzerin basecode
// Fell and Skomp, 2012-2015

#pragma once

#define WIN32_LEAN_AND_MEAN
#define WIN32_EXTRA_LEAN

#include <windows.h>
#include <mmsystem.h>
#include <mmreg.h>

#include "4kampfpanzerin.h"
#include "music.h"

#include <windows.h>
#include <mmsystem.h>
#include "mmreg.h"

#if		SYNTH==_4KLANG
#include "m4klang.h"
#define MusicFrame(t) _4klang_frame(t);

#elif	SYNTH==CLINKSTER
#include "mclinkster.h"
#define MusicFrame(t) _clinkster_frame(t);

#elif	SYNTH==OIDOS
#include "moidos.h"
#define MusicFrame(t) _oidos_frame(t);

#endif

#ifdef USE_4KLANG_ENV_SYNC
float syncVal[MAX_INSTRUMENTS];
#endif



// envelope macro is missing atm

#pragma code_seg(".musicFuncs")
__forceinline void MusicInit() {
#if		SYNTH==_4KLANG
	_4klang_init();
#elif	SYNTH==CLINKSTER
	_clinkster_init();
#elif	SYNTH==OIDOS
	_oidos_init();
#endif
}

