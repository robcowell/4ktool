// 4kampfpanzerin basecode
// Fell, 2012-2014

#pragma once

#include "../4kampfpanzerin.h"
#include "../4klang.h"
#include <windows.h>
#include <mmsystem.h>
#include "mmreg.h"

#pragma data_seg(".wavefmt")
static WAVEFORMATEX WaveFMT = {
#ifdef FLOAT_32BIT	
	WAVE_FORMAT_IEEE_FLOAT,
#else
	WAVE_FORMAT_PCM,
#endif		
	2,												// channels
	SAMPLE_RATE,									// samples per sec
	SAMPLE_RATE*sizeof(SAMPLE_TYPE) * 2,				// bytes per sec
	sizeof(SAMPLE_TYPE) * 2,							// block alignment
	sizeof(SAMPLE_TYPE) * 8,							// bits per sample
	0												// I forget... but it isnae important
};

#pragma data_seg(".wavehdr")
static SAMPLE_TYPE lpSoundBuffer[MAX_SAMPLES * 2];
static WAVEHDR WaveHDR = { (LPSTR)lpSoundBuffer, MAX_SAMPLES*sizeof(SAMPLE_TYPE) * 2, 0, 0, 0, 0, 0, 0 };
static MMTIME MMTime = { TIME_SAMPLES, 0 };
static HWAVEOUT hWaveOut;

__forceinline float _4klang_frame() {
	waveOutGetPosition(hWaveOut, &MMTime, sizeof(MMTIME));
	int index = ((MMTime.u.sample >> 8) << 5);
#ifdef USE_4KLANG_ENV_SYNC
	for (int i=0; i<MAX_INSTRUMENTS; i++) {
		syncVal[i] = (&_4klang_envelope_buffer)[index];
		index+=2;
}
#endif
	return ((float)MMTime.u.sample) / (SAMPLE_RATE);
}

__forceinline void _4klang_init() {

#ifdef USE_SOUND_THREAD
	// thx to xTr1m/blu-flame for providing a smarter and smaller way to create the thread :)
	CreateThread(0, 0, (LPTHREAD_START_ROUTINE)_4klang_render, lpSoundBuffer, 0, 0);
#else
	_4klang_render(lpSoundBuffer);
#endif
	waveOutOpen(&hWaveOut, WAVE_MAPPER, &WaveFMT, NULL, 0, CALLBACK_NULL);
	waveOutPrepareHeader(hWaveOut, &WaveHDR, sizeof(WaveHDR));
	waveOutWrite(hWaveOut, &WaveHDR, sizeof(WaveHDR));
}