#pragma once

#include "../oidos/player/oidos.h"

#define _oidos_frame(t) t = Oidos_GetPosition() / Oidos_TicksPerSecond * 44000;

__forceinline void _oidos_init() {
	Oidos_FillRandomData();
	Oidos_GenerateMusic();
	Oidos_StartMusic();
}
