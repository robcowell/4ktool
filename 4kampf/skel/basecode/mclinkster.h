// 4kampfpanzerin basecode
// Fell, 2012-2014

#pragma once

#include "../Clinkster/easy_exe/clinkster.h"


#define _clinkster_frame(t) t = Clinkster_GetPosition() / Clinkster_TicksPerSecond * 44000;

/*#ifdef USE_4KLANG_ENV_SYNC
	for (int i = 0; i < Clinkster_NumTracks; i++) {
		syncVal[i] = Clinkster_GetInstrumentTrigger(i, Clinkster_GetPosition());
	}
#endif
	return pos / Clinkster_TicksPerSecond * 44000;
}
*/

__forceinline void _clinkster_init() {
	Clinkster_GenerateMusic();
	Clinkster_StartMusic();
}
