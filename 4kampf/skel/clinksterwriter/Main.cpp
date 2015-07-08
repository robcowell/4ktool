#include "clinkster.h"

#include <stdio.h>
#include <Windows.h>

void printTime(float ticks) {
	int sec = (int)(ticks / Clinkster_TicksPerSecond);
	printf("%d:%02d", sec / 60, sec % 60);
}

int main(int argc, char **argv)
{
	setbuf(stdout, NULL);
	fprintf(stdout, "Generating...\n");
	Clinkster_GenerateMusic();

	fprintf(stdout, "Saving...\n");
	FILE *outfile = fopen("music.wav", "wb");
	fwrite(Clinkster_WavFileHeader, 1, sizeof(Clinkster_WavFileHeader), outfile);
	fwrite(Clinkster_MusicBuffer, 1, Clinkster_WavFileHeader[10], outfile);
	fclose(outfile);

	fprintf(stdout, "Success, music written to file \"music.wav\"");
	exit(0);
	return 0;
}
