#pragma warning( disable : 4309 4996 ) // Truncated const value on WaveHeader init, fopen bitching

#define WIN32_LEAN_AND_MEAN
#define WIN32_EXTRA_LEAN
#include "windows.h"
#include "mmsystem.h"
#include "mmreg.h"
#include "stdio.h"
#include "math.h"
#include "../4klang.h"
#include "../4kampfpanzerin.h"

SAMPLE_TYPE	lpSoundBuffer[MAX_SAMPLES*2];  // (Stereo)

int __cdecl main(int argc, char* argv[]) {
	fprintf(stdout, "\nRendering your musical masterpiece (may take a min)...\n");
	fflush(stdout);
	_4klang_render(lpSoundBuffer);
	fprintf(stdout, "Writing wav file...\n");
	fflush(stdout);
	char WaveHeader[44] = {
		'R', 'I', 'F', 'F',
		0, 0, 0, 0,				// filled below
		'W', 'A', 'V', 'E',
		'f', 'm', 't', ' ',
		16, 0, 0, 0,
		1, 0,
		2, 0,
		0x44, 0xac, 0, 0,
		0x10, 0xB1, 0x02, 0,
		4, 0,
		16, 0,
		'd', 'a', 't', 'a',
		0, 0, 0, 0				// filled below
	};

	*((DWORD*)(&WaveHeader[4])) = MAX_SAMPLES * 2 * 2 + 36;	// size of the rest of the file in bytes
	*((DWORD*)(&WaveHeader[40])) = MAX_SAMPLES * 2 * 2;		// size of raw sample data to come
	FILE *file = fopen("music.wav", "wb");
	if (!file) {
		fprintf(stdout, "Couldn't open the wav for writing :(\n");
		return 1;
	}

	fwrite(WaveHeader, 1, 44, file);
	for (int i = 0; i < MAX_SAMPLES*2; i++) {
		// convert and clip each sample
#ifdef FLOAT_32BIT
		int iin = (int)(lpSoundBuffer[i]*32767);
#else
		int iin = (int)(lpSoundBuffer[i]);
#endif
		if (iin > 32767) iin = 32767;
		if (iin < -32767) iin = -32767;
		short iout = iin;
		fwrite(&iout, 2, 1, file);
	}
	fclose(file);

#ifdef USE_4KLANG_ENV_SYNC
	if (_4klang_envelope_buffer) {
		fprintf(stdout, "Dumping envelopes...\n");
		fflush(stdout);
		FILE *envFiles[MAX_INSTRUMENTS];
		char filename[16];
		for (int i = 0; i < MAX_INSTRUMENTS; i++) {
			sprintf(filename, "envelope-%i.dat", i);
			envFiles[i] = fopen(filename, "w");
			if (!envFiles[i]) {
				fprintf(stdout, "Couldn't open %s for writing :(\n", filename);
				return 1;
			}
		}
		for (int sample = 0; sample < MAX_SAMPLES; sample+=256) {
			int index = ((sample >> 8) << 5);
			for (int i = 0; i < MAX_INSTRUMENTS; i++) {
				float f = (&_4klang_envelope_buffer)[index];
				if (isnan(f))
					f = 0;
				fprintf(envFiles[i], "%f,", f);
				index += 2;
			}
		}
		for (int i = 0; i < MAX_INSTRUMENTS; i++)
			fclose(envFiles[i]);
		fprintf(stdout, "%i instruments dumped\n", MAX_INSTRUMENTS);
	} else
			fprintf(stdout, "(No envelopes found)\n");
#else
	fprintf(stdout, "(Envelope rendering disabled in Options)\n");
#endif

	fprintf(stdout, "4klang render complete chief!\n");
	return 0;
}
