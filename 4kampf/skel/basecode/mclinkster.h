// 4kampfpanzerin basecode
// Fell, 2012-2014

#pragma once

#include "../clinksterwriter/clinkster.h"


__forceinline float _clinkster_frame() {
	float pos = Clinkster_GetPosition();

#ifdef USE_4KLANG_ENV_SYNC
	for (int i = 0; i < Clinkster_NumTracks; i++) {
		syncVal[i] = Clinkster_GetInstrumentTrigger(i, pos);
	}
#endif
	return pos / Clinkster_TicksPerSecond;
}

__forceinline void _clinkster_init() {
	Clinkster_GenerateMusic();
	Clinkster_StartMusic();
}
