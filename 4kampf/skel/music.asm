; Clinkster music converted from music.xrns 2015-05-05 23:34:28

%define USES_SINE 1
%define USES_SAWTOOTH 1
%define USES_SQUARE 1
%define USES_PARABOLA 0
%define USES_TRIANGLE 1
%define USES_NOISE 1
%define USES_VELOCITY 1
%define USES_LONG_NOTES 0
%define USES_DELAY 1
%define USES_PANNING 1
%define USES_INDEXDECAY 1
%define USES_GAIN 1

%define SUBSAMPLES_PER_TICK 15204
%define MAX_INSTRUMENT_SUBSAMPLES 1245184
%define MAX_TOTAL_INSTRUMENT_SAMPLES 851968
%define MAX_RELEASE_SUBSAMPLES 196608
%define TOTAL_SAMPLES 8126464
%define MAX_TRACK_INSTRUMENT_RENDERS 14

%define MAX_DELAY_LENGTH 30428
%define LEFT_DELAY_LENGTH 15214
%define RIGHT_DELAY_LENGTH 30428
%define DELAY_STRENGTH 0.39810718

%define NUMTRACKS 18
%define LOGNUMTICKS 12
%define MUSIC_LENGTH 2112
%define TICKS_PER_SECOND 11.60000000


	section instdata data align=1

_InstrumentData:
	; 00:  pad / 00|moody pad
	db	0,3,24,4,16,8,16,84,16,37,37,-96,0,0,0,14,4,-3,3,11
	db	38,127,64,0,2,127,32,0,1,127,32,127,8,0,-1
	; 01:  pad / 00|moody pad
	db	0,3,24,4,16,8,16,84,16,37,37,-96,0,0,0,14,4,-3,3,11
	db	41,127,64,0,3,127,32,0,0,127,32,127,8,0,-1
	; 02:  pad / 00|moody pad
	db	0,3,24,4,16,8,16,84,16,37,37,-96,0,0,0,14,4,-3,3,11
	db	45,127,64,0,2,127,32,0,1,127,32,127,8,0,-1
	; 03:  pad / 00|moody pad
	db	0,3,24,4,16,8,16,84,16,37,37,-96,0,0,0,14,4,-3,3,11
	db	62,96,16,0,-1
	; 04:  pad / 00|moody pad
	db	0,3,24,4,16,8,16,84,16,37,37,-96,0,0,0,14,4,-3,3,11
	db	65,96,16,0,-1
	; 05:  pad / 00|moody pad
	db	0,3,24,4,16,8,16,84,16,37,37,-96,0,0,0,14,4,-3,3,11
	db	69,96,16,0,-1
	; 06:  sqrlead / 01|tha sqr lead
	db	2,2,8,8,61,56,16,32,0,63,63,0,12,0,0,-67,-8,-16,-4,8
	db	74,127,1,0,2,127,1,0,1,127,1,0,1,127,1,0,0,127,1,0,1,127,1,0,1,127,1,0,2,127,1,0,1,127,1,0,-1
	; 07:  snare2 / 05|snare3
	db	2,0,22,20,0,6,22,0,1,59,59,0,40,0,0,0,16,-59,-9,-1
	db	36,127,1,0,-1
	; 08:  sqrsubbb / 0B|square_su
	db	2,1,4,4,16,48,4,16,9,45,45,0,0,0,0,-49,0,-31,-27,2
	db	26,127,6,127,2,0,2,127,10,127,4,0,1,127,6,127,2,0,6,127,12,127,6,127,2,0,2,127,12,127,6,0,1,127,12,127,6,127,2,0,-1
	db	-1
	; 09:  closed hh / 06|closed_hh1
	db	0,0,13,23,0,5,12,37,2,57,27,55,15,0,0,0,30,-59,-6,-19
	db	72,127,67,127,4,127,3,127,1,0,-1
	; 10:  closed hh 2 / 07|closed_hh2
	db	0,0,28,20,0,4,12,37,2,45,45,55,8,0,0,0,64,-59,-6,-19
	db	72,127,3,127,2,0,-1
	; 11:  kickdrum / 02|kickdrum
	db	0,0,0,0,0,0,1,12,0,48,48,24,0,-70,0,0,0,-59,2,-27
	db	24,127,1,0,6,127,1,0,-1
	; 12:  snare1 / 04|snare2
	db	0,4,16,8,16,16,20,4,-2,32,32,8,0,-64,0,28,53,-104,-4,1
	db	60,127,1,0,-1
	; 13:  snare1 / 03|snare
	db	0,4,26,10,0,6,20,4,2,32,32,-2,0,-70,0,0,45,-59,-9,6
	db	60,127,1,0,-1
	; 14:  ride / 08|ride
	db	3,0,32,23,0,5,12,37,11,19,53,55,22,0,0,0,22,-59,-14,1
	db	72,127,2,0,-1
	; 15:  perc_shak_snar / 09|shaker
	db	0,0,13,23,4,4,32,64,0,32,32,57,15,16,0,0,48,-17,-6,-19
	db	67,127,1,0,-1
	; 16:  perc_shak_snar / 05|snare3
	db	2,0,22,20,0,6,22,0,1,42,42,0,40,0,0,0,16,-59,-9,-1
	db	50,127,10,127,1,0,-1
	; 17:  sub / 0A|subbass
	db	0,0,11,0,0,0,1,0,-6,38,38,0,0,0,0,0,16,-59,15,5
	db	26,127,10,127,9,127,6,127,4,0,2,127,10,127,4,0,1,127,10,127,6,127,4,0,6,96,12,96,6,0,2,96,12,96,6,0,1,96,12,0,-1
	db	-1,-1

	section notepos data align=1

_NotePositions:
	; 00:  pad / 00|moody pad
	; position 0 - pattern 0
	db	0,32
	; position 1 - pattern 1
	db	32
	; position 2 - pattern 2
	db	64,32
	; position 3 - pattern 3
	db	32
	; position 4 - pattern 4
	db	64,32
	; position 5 - pattern 5
	db	32
	; position 6 - pattern 6
	db	64,32
	; position 7 - pattern 7
	db	32
	; position 8 - pattern 8
	db	64,32
	; position 9 - pattern 9
	db	32
	; position 10 - pattern 10
	db	64,32
	; position 11 - pattern 11
	db	32
	; position 12 - pattern 12
	db	64,32
	; position 13 - pattern 13
	db	32
	; position 14 - pattern 14
	db	64,32
	; position 15 - pattern 15
	db	32
	; position 24 - pattern 24
	db	-3,64,32
	; position 25 - pattern 25
	db	32
	; position 26 - pattern 26
	db	64,32
	; position 27 - pattern 27
	db	32
	; position 28 - pattern 28
	db	64,32
	; position 29 - pattern 29
	db	32
	; position 30 - pattern 30
	db	64,32
	; position 31 - pattern 31
	db	32
	; position 32 - pattern 32
	db	64

	; 01:  pad / 00|moody pad
	; position 0 - pattern 0
	db	0,32
	; position 1 - pattern 1
	db	32
	; position 2 - pattern 2
	db	64,32
	; position 3 - pattern 3
	db	32
	; position 4 - pattern 4
	db	64,32
	; position 5 - pattern 5
	db	32
	; position 6 - pattern 6
	db	64,32
	; position 7 - pattern 7
	db	32
	; position 8 - pattern 8
	db	64,32
	; position 9 - pattern 9
	db	32
	; position 10 - pattern 10
	db	64,32
	; position 11 - pattern 11
	db	32
	; position 12 - pattern 12
	db	64,32
	; position 13 - pattern 13
	db	32
	; position 14 - pattern 14
	db	64,32
	; position 15 - pattern 15
	db	32
	; position 24 - pattern 24
	db	-3,64,32
	; position 25 - pattern 25
	db	32
	; position 26 - pattern 26
	db	64,32
	; position 27 - pattern 27
	db	32
	; position 28 - pattern 28
	db	64,32
	; position 29 - pattern 29
	db	32
	; position 30 - pattern 30
	db	64,32
	; position 31 - pattern 31
	db	32
	; position 32 - pattern 32
	db	64

	; 02:  pad / 00|moody pad
	; position 0 - pattern 0
	db	0,32
	; position 1 - pattern 1
	db	32
	; position 2 - pattern 2
	db	64,32
	; position 3 - pattern 3
	db	32
	; position 4 - pattern 4
	db	64,32
	; position 5 - pattern 5
	db	32
	; position 6 - pattern 6
	db	64,32
	; position 7 - pattern 7
	db	32
	; position 8 - pattern 8
	db	64,32
	; position 9 - pattern 9
	db	32
	; position 10 - pattern 10
	db	64,32
	; position 11 - pattern 11
	db	32
	; position 12 - pattern 12
	db	64,32
	; position 13 - pattern 13
	db	32
	; position 14 - pattern 14
	db	64,32
	; position 15 - pattern 15
	db	32
	; position 24 - pattern 24
	db	-3,64,32
	; position 25 - pattern 25
	db	32
	; position 26 - pattern 26
	db	64,32
	; position 27 - pattern 27
	db	32
	; position 28 - pattern 28
	db	64,32
	; position 29 - pattern 29
	db	32
	; position 30 - pattern 30
	db	64,32
	; position 31 - pattern 31
	db	32
	; position 32 - pattern 32
	db	64

	; 03:  pad / 00|moody pad
	; position 3 - pattern 3
	db	-1,240
	; position 7 - pattern 7
	db	-2,0
	; position 11 - pattern 11
	db	-2,0
	; position 15 - pattern 15
	db	-2,0
	; position 27 - pattern 27
	db	-4,0
	; position 31 - pattern 31
	db	-2,0

	; 04:  pad / 00|moody pad
	; position 3 - pattern 3
	db	-1,240
	; position 7 - pattern 7
	db	-2,0
	; position 11 - pattern 11
	db	-2,0
	; position 15 - pattern 15
	db	-2,0
	; position 27 - pattern 27
	db	-4,0
	; position 31 - pattern 31
	db	-2,0

	; 05:  pad / 00|moody pad
	; position 3 - pattern 3
	db	-1,240
	; position 7 - pattern 7
	db	-2,0
	; position 11 - pattern 11
	db	-2,0
	; position 15 - pattern 15
	db	-2,0
	; position 27 - pattern 27
	db	-4,0
	; position 31 - pattern 31
	db	-2,0

	; 06:  sqrlead / 01|tha sqr lead
	; position 4 - pattern 4
	db	-2,0,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2
	; position 5 - pattern 5
	db	2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2
	; position 6 - pattern 6
	db	2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2
	; position 7 - pattern 7
	db	2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2
	; position 8 - pattern 8
	db	2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2
	; position 9 - pattern 9
	db	2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2
	; position 10 - pattern 10
	db	2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2
	; position 11 - pattern 11
	db	2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2
	; position 12 - pattern 12
	db	2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2
	; position 13 - pattern 13
	db	2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2
	; position 14 - pattern 14
	db	2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2
	; position 15 - pattern 15
	db	2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2
	; position 24 - pattern 24
	db	-3,2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2
	; position 25 - pattern 25
	db	2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2
	; position 26 - pattern 26
	db	2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2
	; position 27 - pattern 27
	db	2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2
	; position 28 - pattern 28
	db	2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2
	; position 29 - pattern 29
	db	2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2
	; position 30 - pattern 30
	db	2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2
	; position 31 - pattern 31
	db	2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2,2,2,2,6,2,2
	; position 32 - pattern 32
	db	2,2,2

	; 07:  snare2 / 05|snare3
	; position 8 - pattern 8
	db	-3,4,6,10,6,10,6,10,6
	; position 9 - pattern 9
	db	10,6,10,6,10,6,10,6
	; position 10 - pattern 10
	db	10,6,10,6,10,6,10,6
	; position 11 - pattern 11
	db	10,6,10,6,10,6,10,6
	; position 12 - pattern 12
	db	10,6,10,6,10,6,10,6
	; position 13 - pattern 13
	db	10,6,10,6,10,6,10,6
	; position 14 - pattern 14
	db	10,6,10,6,10,6,10,6
	; position 15 - pattern 15
	db	10,6,10,6,10,6,10,6
	; position 16 - pattern 16
	db	10,6,10,6,10,6,10,6
	; position 17 - pattern 17
	db	10,6,10,6,10,6,10,6
	; position 18 - pattern 18
	db	10,6,10,6,10,6,10,6
	; position 19 - pattern 19
	db	10,6,10,6,10,6,10,6
	; position 20 - pattern 20
	db	10,6,10,6,10,6,10,6
	; position 21 - pattern 21
	db	10,6,10,6,10,6,10,6
	; position 22 - pattern 22
	db	10,6,10,6,10,6,10,6
	; position 23 - pattern 23
	db	10,6,10,6,10,6,10,6
	; position 24 - pattern 24
	db	10,6,10,6,10,6,10,6
	; position 25 - pattern 25
	db	10,6,10,6,10,6,10,6
	; position 26 - pattern 26
	db	10,6,10,6,10,6,10,6
	; position 27 - pattern 27
	db	10,6,10,6,10,6,10,6
	; position 28 - pattern 28
	db	10,6,10,6,10,6,10,6
	; position 29 - pattern 29
	db	10,6,10,6,10,6,10,6

	; 08:  sqrsubbb / 0B|square_su
	; position 8 - pattern 8
	db	-3,0,4,12,6,8,2,4,12,6
	; position 9 - pattern 9
	db	10,4,12,6,8,2,4,12,6,6
	; position 10 - pattern 10
	db	4,4,12,6,8,2,4,12,6
	; position 11 - pattern 11
	db	10,4,12,6,8,2,4,12,6,6
	; position 12 - pattern 12
	db	4,4,12,6,8,2,4,12,6
	; position 13 - pattern 13
	db	10,4,12,6,8,2,4,12,6,6
	; position 14 - pattern 14
	db	4,4,12,6,8,2,4,12,6
	; position 15 - pattern 15
	db	10,4,12,6,8,2,4,12,6,6
	; position 16 - pattern 16
	db	4,4,12,6,8,2,4,12,6
	; position 17 - pattern 17
	db	10,4,12,6,8,2,4,12,6,6
	; position 18 - pattern 18
	db	4,4,12,6,8,2,4,12,6
	; position 19 - pattern 19
	db	10,4,12,6,8,2,4,12,6,6
	; position 20 - pattern 20
	db	4,4,12,6,8,2,4,12,6
	; position 21 - pattern 21
	db	10,4,12,6,8,2,4,12,6,6
	; position 22 - pattern 22
	db	4,4,12,6,8,2,4,12,6
	; position 23 - pattern 23
	db	10,4,12,6,8,2,4,12,6,6

	; 09:  closed hh / 06|closed_hh1
	; position 2 - pattern 2
	db	-1,130,6,6,4,6,6,4,6,6,4,6,6,1
	; position 3 - pattern 3
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 4 - pattern 4
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 5 - pattern 5
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 6 - pattern 6
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 8 - pattern 8
	db	67,6,6,4,6,6,4,6,6,4,6,6,1
	; position 9 - pattern 9
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 10 - pattern 10
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 11 - pattern 11
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 12 - pattern 12
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 13 - pattern 13
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 14 - pattern 14
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 15 - pattern 15
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 16 - pattern 16
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 17 - pattern 17
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 18 - pattern 18
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 19 - pattern 19
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 20 - pattern 20
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 21 - pattern 21
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 22 - pattern 22
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 23 - pattern 23
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 24 - pattern 24
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 25 - pattern 25
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 26 - pattern 26
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 27 - pattern 27
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 28 - pattern 28
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 29 - pattern 29
	db	3,6,6,4,6,6,4,6,6,4,6,6,1
	; position 30 - pattern 30
	db	3,6,6,4,6,6,4,6,6,4,6,6,1

	; 10:  closed hh 2 / 07|closed_hh2
	; position 4 - pattern 4
	db	-2,0,32
	; position 5 - pattern 5
	db	32,32
	; position 6 - pattern 6
	db	32,32
	; position 8 - pattern 8
	db	96,32
	; position 9 - pattern 9
	db	32,32
	; position 10 - pattern 10
	db	32,32
	; position 11 - pattern 11
	db	32,32
	; position 12 - pattern 12
	db	32,32
	; position 13 - pattern 13
	db	32,32
	; position 14 - pattern 14
	db	32,32
	; position 15 - pattern 15
	db	32,32
	; position 16 - pattern 16
	db	32,32
	; position 17 - pattern 17
	db	32,32
	; position 18 - pattern 18
	db	32,32
	; position 19 - pattern 19
	db	32,32
	; position 20 - pattern 20
	db	32,32
	; position 21 - pattern 21
	db	32,32
	; position 22 - pattern 22
	db	32,32
	; position 23 - pattern 23
	db	32,32
	; position 24 - pattern 24
	db	32,32
	; position 25 - pattern 25
	db	32,32
	; position 26 - pattern 26
	db	32,32
	; position 27 - pattern 27
	db	32,32
	; position 28 - pattern 28
	db	32,32
	; position 29 - pattern 29
	db	32,32
	; position 30 - pattern 30
	db	32,32

	; 11:  kickdrum / 02|kickdrum
	; position 4 - pattern 4
	db	-2,0,6,10,6,10,6,10,6
	; position 5 - pattern 5
	db	10,6,10,6,10,6,10,6,4
	; position 6 - pattern 6
	db	6,6,10,6,10,6,10,6
	; position 8 - pattern 8
	db	74,6,10,6,10,6,10,6
	; position 9 - pattern 9
	db	10,6,10,6,10,6,10,6,4
	; position 10 - pattern 10
	db	6,6,10,6,10,6,10,6
	; position 11 - pattern 11
	db	10,6,10,6,10,6,10,6,4
	; position 12 - pattern 12
	db	6,6,10,6,10,6,10,6
	; position 13 - pattern 13
	db	10,6,10,6,10,6,10,6,4
	; position 14 - pattern 14
	db	6,6,10,6,10,6,10,6
	; position 16 - pattern 16
	db	74,6,10,6,10,6,10,6
	; position 17 - pattern 17
	db	10,6,10,6,10,6,10,6,4
	; position 18 - pattern 18
	db	6,6,10,6,10,6,10,6
	; position 19 - pattern 19
	db	10,6,10,6,10,6,10,6,4
	; position 20 - pattern 20
	db	6,6,10,6,10,6,10,6
	; position 21 - pattern 21
	db	10,6,10,6,10,6,10,6,4
	; position 22 - pattern 22
	db	6,6,10,6,10,6,10,6
	; position 23 - pattern 23
	db	10,6,10,6,10,6
	; position 24 - pattern 24
	db	26,6,10,6,10,6,10,6
	; position 25 - pattern 25
	db	10,6,10,6,10,6,10,6,4
	; position 26 - pattern 26
	db	6,6,10,6,10,6,10,6
	; position 27 - pattern 27
	db	10,6,10,6,10,6,10,6,4

	; 12:  snare1 / 04|snare2
	; position 8 - pattern 8
	db	-3,4,54
	; position 9 - pattern 9
	db	10,54
	; position 10 - pattern 10
	db	10,54
	; position 11 - pattern 11
	db	10,54
	; position 12 - pattern 12
	db	10,54
	; position 13 - pattern 13
	db	10,54
	; position 14 - pattern 14
	db	10,54
	; position 15 - pattern 15
	db	10,54
	; position 16 - pattern 16
	db	10,54
	; position 17 - pattern 17
	db	10,54
	; position 18 - pattern 18
	db	10,54
	; position 19 - pattern 19
	db	10,54
	; position 20 - pattern 20
	db	10,54
	; position 21 - pattern 21
	db	10,54
	; position 22 - pattern 22
	db	10,54
	; position 23 - pattern 23
	db	10,54
	; position 24 - pattern 24
	db	10,54
	; position 25 - pattern 25
	db	10,54
	; position 26 - pattern 26
	db	10,54
	; position 27 - pattern 27
	db	10,54
	; position 28 - pattern 28
	db	10,54
	; position 29 - pattern 29
	db	10,54

	; 13:  snare1 / 03|snare
	; position 8 - pattern 8
	db	-3,10,10,6,10,6,10
	; position 9 - pattern 9
	db	22,10,6,10,6,10
	; position 10 - pattern 10
	db	22,10,6,10,6,10
	; position 11 - pattern 11
	db	22,10,6,10,6,10
	; position 12 - pattern 12
	db	22,10,6,10,6,10
	; position 13 - pattern 13
	db	22,10,6,10,6,10
	; position 14 - pattern 14
	db	22,10,6,10,6,10
	; position 15 - pattern 15
	db	22,10,6,10,6,10
	; position 16 - pattern 16
	db	22,10,6,10,6,10
	; position 17 - pattern 17
	db	22,10,6,10,6,10
	; position 18 - pattern 18
	db	22,10,6,10,6,10
	; position 19 - pattern 19
	db	22,10,6,10,6,10
	; position 20 - pattern 20
	db	22,10,6,10,6,10
	; position 21 - pattern 21
	db	22,10,6,10,6,10
	; position 22 - pattern 22
	db	22,10,6,10,6,10
	; position 23 - pattern 23
	db	22,10,6,10,6,10
	; position 24 - pattern 24
	db	22,10,6,10,6,10
	; position 25 - pattern 25
	db	22,10,6,10,6,10
	; position 26 - pattern 26
	db	22,10,6,10,6,10
	; position 27 - pattern 27
	db	22,10,6,10,6,10
	; position 28 - pattern 28
	db	22,10,6,10,6,10
	; position 29 - pattern 29
	db	22,10,6,10,6,10

	; 14:  ride / 08|ride
	; position 6 - pattern 6
	db	-2,128,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 7 - pattern 7
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 8 - pattern 8
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 9 - pattern 9
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 10 - pattern 10
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 11 - pattern 11
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 12 - pattern 12
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 13 - pattern 13
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 14 - pattern 14
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 15 - pattern 15
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 16 - pattern 16
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 17 - pattern 17
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 18 - pattern 18
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 19 - pattern 19
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 20 - pattern 20
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 21 - pattern 21
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 22 - pattern 22
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 23 - pattern 23
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 24 - pattern 24
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 25 - pattern 25
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 26 - pattern 26
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 27 - pattern 27
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 28 - pattern 28
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 29 - pattern 29
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4
	; position 30 - pattern 30
	db	4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4

	; 15:  perc_shak_snar / 09|shaker
	; position 4 - pattern 4
	db	-2,10,2,14,2,14,2,14,2
	; position 5 - pattern 5
	db	14,2,14,2,14,2,14,2
	; position 6 - pattern 6
	db	14,2,14,2,14,2,14,2
	; position 7 - pattern 7
	db	14,2,14,2,14,2,14
	; position 8 - pattern 8
	db	16,2,14,2,14,2,14,2
	; position 9 - pattern 9
	db	14,2,14,2,14,2,14,2
	; position 10 - pattern 10
	db	14,2,14,2,14,2,14,2
	; position 11 - pattern 11
	db	14,2,14,2,14,2,14,2
	; position 12 - pattern 12
	db	14,2,14,2,14,2,14,2
	; position 13 - pattern 13
	db	14,2,14,2,14,2,14,2
	; position 14 - pattern 14
	db	14,2,14,2,14,2,14,2
	; position 15 - pattern 15
	db	14,2,14,2,14,2,14
	; position 16 - pattern 16
	db	16,2,14,2,14,2,14,2
	; position 17 - pattern 17
	db	14,2,14,2,14,2,14,2
	; position 18 - pattern 18
	db	14,2,14,2,14,2,14,2
	; position 19 - pattern 19
	db	14,2,14,2,14,2,14,2
	; position 20 - pattern 20
	db	14,2,14,2,14,2,14,2
	; position 21 - pattern 21
	db	14,2,14,2,14,2,14,2
	; position 22 - pattern 22
	db	14,2,14,2,14,2,14,2
	; position 23 - pattern 23
	db	14,2,14,2,14,2,14
	; position 24 - pattern 24
	db	16,2,14,2,14,2,14,2
	; position 25 - pattern 25
	db	14,2,14,2,14,2,14,2
	; position 26 - pattern 26
	db	14,2,14,2,14,2,14,2
	; position 27 - pattern 27
	db	14,2,14,2,14,2,14,2
	; position 28 - pattern 28
	db	14,2,14,2,14,2,14,2
	; position 29 - pattern 29
	db	14,2,14,2,14,2,14,2
	; position 30 - pattern 30
	db	14,2,14,2,14,2,14,2

	; 16:  perc_shak_snar / 05|snare3
	; position 4 - pattern 4
	db	-2,30,32
	; position 5 - pattern 5
	db	32,32,1
	; position 6 - pattern 6
	db	31,32
	; position 7 - pattern 7
	db	32,30,1,1
	; position 8 - pattern 8
	db	32,32
	; position 9 - pattern 9
	db	32,32
	; position 10 - pattern 10
	db	32,32
	; position 11 - pattern 11
	db	32,32
	; position 12 - pattern 12
	db	32,32
	; position 13 - pattern 13
	db	32,32
	; position 14 - pattern 14
	db	32,32
	; position 15 - pattern 15
	db	32,30,1,1
	; position 16 - pattern 16
	db	32,32
	; position 17 - pattern 17
	db	32,32
	; position 18 - pattern 18
	db	32,32
	; position 19 - pattern 19
	db	32,32
	; position 20 - pattern 20
	db	32,32
	; position 21 - pattern 21
	db	32,32
	; position 22 - pattern 22
	db	32,32
	; position 23 - pattern 23
	db	32,30,1,1
	; position 24 - pattern 24
	db	32,32
	; position 25 - pattern 25
	db	32,32
	; position 26 - pattern 26
	db	32,32
	; position 27 - pattern 27
	db	32,32
	; position 28 - pattern 28
	db	32,32
	; position 29 - pattern 29
	db	32,32
	; position 30 - pattern 30
	db	32,32

	; 17:  sub / 0A|subbass
	; position 8 - pattern 8
	db	-3,0,4,12,6,10,4,12,6
	; position 9 - pattern 9
	db	10,4,12,6,10,4,12,6
	; position 10 - pattern 10
	db	10,4,12,6,10,4,12,6
	; position 11 - pattern 11
	db	10,4,12,6,10,4,12,6
	; position 12 - pattern 12
	db	10,4,12,6,10,4,12,6
	; position 13 - pattern 13
	db	10,4,12,6,10,4,12,6
	; position 14 - pattern 14
	db	10,4,12,6,10,4,12,6
	; position 15 - pattern 15
	db	10,4,12,6,10,4,12,6
	; position 16 - pattern 16
	db	10,4,12,6,10,4,12,6
	; position 17 - pattern 17
	db	10,4,12,6,10,4,12,6
	; position 18 - pattern 18
	db	10,4,12,6,10,4,12,6
	; position 19 - pattern 19
	db	10,4,12,6,10,4,12,6
	; position 20 - pattern 20
	db	10,4,12,6,10,4,12,6
	; position 21 - pattern 21
	db	10,4,12,6,10,4,12,6
	; position 22 - pattern 22
	db	10,4,12,6,10,4,12,6
	; position 23 - pattern 23
	db	10,4,12,6,10,4,12,6
	; position 24 - pattern 24
	db	10,4,12,6,10,4,12,6
	; position 25 - pattern 25
	db	10,4,12,6,10,4,12,6
	; position 26 - pattern 26
	db	10,4,12,6,10,4,12,6
	; position 27 - pattern 27
	db	10,4,12,6,10,4,12,6
	; position 28 - pattern 28
	db	10,4,12,6,10,4,12,6
	; position 29 - pattern 29
	db	10,4,12,6,10,4,12,6
	; position 30 - pattern 30
	db	10,4,12,6,10,4,12,6
	; position 31 - pattern 31
	db	10,4,12,6,10,4,12,6


	section notesamp data align=1

_NoteSamples:
	; 00:  pad / 00|moody pad
	; position 0 - pattern 0
	db	2,1
	; position 1 - pattern 1
	db	0
	; position 2 - pattern 2
	db	2,1
	; position 3 - pattern 3
	db	0
	; position 4 - pattern 4
	db	2,1
	; position 5 - pattern 5
	db	0
	; position 6 - pattern 6
	db	2,1
	; position 7 - pattern 7
	db	0
	; position 8 - pattern 8
	db	2,1
	; position 9 - pattern 9
	db	0
	; position 10 - pattern 10
	db	2,1
	; position 11 - pattern 11
	db	0
	; position 12 - pattern 12
	db	2,1
	; position 13 - pattern 13
	db	0
	; position 14 - pattern 14
	db	2,1
	; position 15 - pattern 15
	db	0
	; position 24 - pattern 24
	db	2,1
	; position 25 - pattern 25
	db	0
	; position 26 - pattern 26
	db	2,1
	; position 27 - pattern 27
	db	0
	; position 28 - pattern 28
	db	2,1
	; position 29 - pattern 29
	db	0
	; position 30 - pattern 30
	db	2,1
	; position 31 - pattern 31
	db	0
	; position 32 - pattern 32
	db	3
	db	-1

	; 01:  pad / 00|moody pad
	; position 0 - pattern 0
	db	2,1
	; position 1 - pattern 1
	db	0
	; position 2 - pattern 2
	db	2,1
	; position 3 - pattern 3
	db	0
	; position 4 - pattern 4
	db	2,1
	; position 5 - pattern 5
	db	0
	; position 6 - pattern 6
	db	2,1
	; position 7 - pattern 7
	db	0
	; position 8 - pattern 8
	db	2,1
	; position 9 - pattern 9
	db	0
	; position 10 - pattern 10
	db	2,1
	; position 11 - pattern 11
	db	0
	; position 12 - pattern 12
	db	2,1
	; position 13 - pattern 13
	db	0
	; position 14 - pattern 14
	db	2,1
	; position 15 - pattern 15
	db	0
	; position 24 - pattern 24
	db	2,1
	; position 25 - pattern 25
	db	0
	; position 26 - pattern 26
	db	2,1
	; position 27 - pattern 27
	db	0
	; position 28 - pattern 28
	db	2,1
	; position 29 - pattern 29
	db	0
	; position 30 - pattern 30
	db	2,1
	; position 31 - pattern 31
	db	0
	; position 32 - pattern 32
	db	3
	db	-1

	; 02:  pad / 00|moody pad
	; position 0 - pattern 0
	db	2,1
	; position 1 - pattern 1
	db	0
	; position 2 - pattern 2
	db	2,1
	; position 3 - pattern 3
	db	0
	; position 4 - pattern 4
	db	2,1
	; position 5 - pattern 5
	db	0
	; position 6 - pattern 6
	db	2,1
	; position 7 - pattern 7
	db	0
	; position 8 - pattern 8
	db	2,1
	; position 9 - pattern 9
	db	0
	; position 10 - pattern 10
	db	2,1
	; position 11 - pattern 11
	db	0
	; position 12 - pattern 12
	db	2,1
	; position 13 - pattern 13
	db	0
	; position 14 - pattern 14
	db	2,1
	; position 15 - pattern 15
	db	0
	; position 24 - pattern 24
	db	2,1
	; position 25 - pattern 25
	db	0
	; position 26 - pattern 26
	db	2,1
	; position 27 - pattern 27
	db	0
	; position 28 - pattern 28
	db	2,1
	; position 29 - pattern 29
	db	0
	; position 30 - pattern 30
	db	2,1
	; position 31 - pattern 31
	db	0
	; position 32 - pattern 32
	db	3
	db	-1

	; 03:  pad / 00|moody pad
	; position 3 - pattern 3
	db	0
	; position 7 - pattern 7
	db	0
	; position 11 - pattern 11
	db	0
	; position 15 - pattern 15
	db	0
	; position 27 - pattern 27
	db	0
	; position 31 - pattern 31
	db	0
	db	-1

	; 04:  pad / 00|moody pad
	; position 3 - pattern 3
	db	0
	; position 7 - pattern 7
	db	0
	; position 11 - pattern 11
	db	0
	; position 15 - pattern 15
	db	0
	; position 27 - pattern 27
	db	0
	; position 31 - pattern 31
	db	0
	db	-1

	; 05:  pad / 00|moody pad
	; position 3 - pattern 3
	db	0
	; position 7 - pattern 7
	db	0
	; position 11 - pattern 11
	db	0
	; position 15 - pattern 15
	db	0
	; position 27 - pattern 27
	db	0
	; position 31 - pattern 31
	db	0
	db	-1

	; 06:  sqrlead / 01|tha sqr lead
	; position 4 - pattern 4
	db	2,4,6,2,4,6,2,6,4,6,4,8,1,3,5,1,3,5,1,5,3,7,5,3
	; position 5 - pattern 5
	db	0,1,3,0,1,3,0,3,1,3,1,6,0,1,3,0,1,3,0,3,1,6,3,1
	; position 6 - pattern 6
	db	2,4,6,2,4,6,2,6,4,6,4,8,1,3,5,1,3,5,1,5,3,7,5,3
	; position 7 - pattern 7
	db	0,1,3,0,1,3,0,3,1,3,1,6,0,1,3,0,1,3,0,3,1,6,3,1
	; position 8 - pattern 8
	db	2,4,6,2,4,6,2,6,4,6,4,8,1,3,5,1,3,5,1,5,3,7,5,3
	; position 9 - pattern 9
	db	0,1,3,0,1,3,0,3,1,3,1,6,0,1,3,0,1,3,0,3,1,6,3,1
	; position 10 - pattern 10
	db	2,4,6,2,4,6,2,6,4,6,4,8,1,3,5,1,3,5,1,5,3,7,5,3
	; position 11 - pattern 11
	db	0,1,3,0,1,3,0,3,1,3,1,6,0,1,3,0,1,3,0,3,1,6,3,1
	; position 12 - pattern 12
	db	2,4,6,2,4,6,2,6,4,6,4,8,1,3,5,1,3,5,1,5,3,7,5,3
	; position 13 - pattern 13
	db	0,1,3,0,1,3,0,3,1,3,1,6,0,1,3,0,1,3,0,3,1,6,3,1
	; position 14 - pattern 14
	db	2,4,6,2,4,6,2,6,4,6,4,8,1,3,5,1,3,5,1,5,3,7,5,3
	; position 15 - pattern 15
	db	0,1,3,0,1,3,0,3,1,3,1,6,0,1,3,0,1,3,0,3,1,6,3,1
	; position 24 - pattern 24
	db	2,4,6,2,4,6,2,6,4,6,4,8,1,3,5,1,3,5,1,5,3,7,5,3
	; position 25 - pattern 25
	db	0,1,3,0,1,3,0,3,1,3,1,6,0,1,3,0,1,3,0,3,1,6,3,1
	; position 26 - pattern 26
	db	2,4,6,2,4,6,2,6,4,6,4,8,1,3,5,1,3,5,1,5,3,7,5,3
	; position 27 - pattern 27
	db	0,1,3,0,1,3,0,3,1,3,1,6,0,1,3,0,1,3,0,3,1,6,3,1
	; position 28 - pattern 28
	db	2,4,6,2,4,6,2,6,4,6,4,8,1,3,5,1,3,5,1,5,3,7,5,3
	; position 29 - pattern 29
	db	0,1,3,0,1,3,0,3,1,3,1,6,0,1,3,0,1,3,0,3,1,6,3,1
	; position 30 - pattern 30
	db	2,4,6,2,4,6,2,6,4,6,4,8,1,3,5,1,3,5,1,5,3,7,5,3
	; position 31 - pattern 31
	db	0,1,3,0,1,3,0,3,1,3,1,6,0,1,3,0,1,3,0,3,1,6,3,1
	; position 32 - pattern 32
	db	2,4,6
	db	-1

	; 07:  snare2 / 05|snare3
	; position 8 - pattern 8
	db	0,0,0,0,0,0,0,0
	; position 9 - pattern 9
	db	0,0,0,0,0,0,0,0
	; position 10 - pattern 10
	db	0,0,0,0,0,0,0,0
	; position 11 - pattern 11
	db	0,0,0,0,0,0,0,0
	; position 12 - pattern 12
	db	0,0,0,0,0,0,0,0
	; position 13 - pattern 13
	db	0,0,0,0,0,0,0,0
	; position 14 - pattern 14
	db	0,0,0,0,0,0,0,0
	; position 15 - pattern 15
	db	0,0,0,0,0,0,0,0
	; position 16 - pattern 16
	db	0,0,0,0,0,0,0,0
	; position 17 - pattern 17
	db	0,0,0,0,0,0,0,0
	; position 18 - pattern 18
	db	0,0,0,0,0,0,0,0
	; position 19 - pattern 19
	db	0,0,0,0,0,0,0,0
	; position 20 - pattern 20
	db	0,0,0,0,0,0,0,0
	; position 21 - pattern 21
	db	0,0,0,0,0,0,0,0
	; position 22 - pattern 22
	db	0,0,0,0,0,0,0,0
	; position 23 - pattern 23
	db	0,0,0,0,0,0,0,0
	; position 24 - pattern 24
	db	0,0,0,0,0,0,0,0
	; position 25 - pattern 25
	db	0,0,0,0,0,0,0,0
	; position 26 - pattern 26
	db	0,0,0,0,0,0,0,0
	; position 27 - pattern 27
	db	0,0,0,0,0,0,0,0
	; position 28 - pattern 28
	db	0,0,0,0,0,0,0,0
	; position 29 - pattern 29
	db	0,0,0,0,0,0,0,0
	db	-1

	; 08:  sqrsubbb / 0B|square_su
	; position 8 - pattern 8
	db	5,11,12,4,13,3,9,10,2
	; position 9 - pattern 9
	db	1,6,7,0,8,1,6,7,0,3
	; position 10 - pattern 10
	db	5,11,12,4,13,3,9,10,2
	; position 11 - pattern 11
	db	1,6,7,0,8,1,6,7,0,3
	; position 12 - pattern 12
	db	5,11,12,4,13,3,9,10,2
	; position 13 - pattern 13
	db	1,6,7,0,8,1,6,7,0,3
	; position 14 - pattern 14
	db	5,11,12,4,13,3,9,10,2
	; position 15 - pattern 15
	db	1,6,7,0,8,1,6,7,0,3
	; position 16 - pattern 16
	db	5,11,12,4,13,3,9,10,2
	; position 17 - pattern 17
	db	1,6,7,0,8,1,6,7,0,3
	; position 18 - pattern 18
	db	5,11,12,4,13,3,9,10,2
	; position 19 - pattern 19
	db	1,6,7,0,8,1,6,7,0,3
	; position 20 - pattern 20
	db	5,11,12,4,13,3,9,10,2
	; position 21 - pattern 21
	db	1,6,7,0,8,1,6,7,0,3
	; position 22 - pattern 22
	db	5,11,12,4,13,3,9,10,2
	; position 23 - pattern 23
	db	1,6,7,0,8,1,6,7,0,3
	db	-1

	; 09:  closed hh / 06|closed_hh1
	; position 2 - pattern 2
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 3 - pattern 3
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 4 - pattern 4
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 5 - pattern 5
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 6 - pattern 6
	db	3,3,3,3,3,1,3,3,3,3,3,3,0
	; position 8 - pattern 8
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 9 - pattern 9
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 10 - pattern 10
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 11 - pattern 11
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 12 - pattern 12
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 13 - pattern 13
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 14 - pattern 14
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 15 - pattern 15
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 16 - pattern 16
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 17 - pattern 17
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 18 - pattern 18
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 19 - pattern 19
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 20 - pattern 20
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 21 - pattern 21
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 22 - pattern 22
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 23 - pattern 23
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 24 - pattern 24
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 25 - pattern 25
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 26 - pattern 26
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 27 - pattern 27
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 28 - pattern 28
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 29 - pattern 29
	db	3,3,3,3,3,1,3,3,3,3,3,3,2
	; position 30 - pattern 30
	db	3,3,3,3,3,1,3,3,3,3,3,3,3
	db	-1

	; 10:  closed hh 2 / 07|closed_hh2
	; position 4 - pattern 4
	db	0,1
	; position 5 - pattern 5
	db	0,1
	; position 6 - pattern 6
	db	0,1
	; position 8 - pattern 8
	db	0,1
	; position 9 - pattern 9
	db	0,1
	; position 10 - pattern 10
	db	0,1
	; position 11 - pattern 11
	db	0,1
	; position 12 - pattern 12
	db	0,1
	; position 13 - pattern 13
	db	0,1
	; position 14 - pattern 14
	db	0,1
	; position 15 - pattern 15
	db	0,1
	; position 16 - pattern 16
	db	0,1
	; position 17 - pattern 17
	db	0,1
	; position 18 - pattern 18
	db	0,1
	; position 19 - pattern 19
	db	0,1
	; position 20 - pattern 20
	db	0,1
	; position 21 - pattern 21
	db	0,1
	; position 22 - pattern 22
	db	0,1
	; position 23 - pattern 23
	db	0,1
	; position 24 - pattern 24
	db	0,1
	; position 25 - pattern 25
	db	0,1
	; position 26 - pattern 26
	db	0,1
	; position 27 - pattern 27
	db	0,1
	; position 28 - pattern 28
	db	0,1
	; position 29 - pattern 29
	db	0,1
	; position 30 - pattern 30
	db	0,1
	db	-1

	; 11:  kickdrum / 02|kickdrum
	; position 4 - pattern 4
	db	1,0,0,0,1,0,0,1
	; position 5 - pattern 5
	db	1,0,0,0,1,0,0,0,1
	; position 6 - pattern 6
	db	1,0,0,0,1,0,0,1
	; position 8 - pattern 8
	db	1,0,0,0,1,0,0,1
	; position 9 - pattern 9
	db	1,0,0,0,1,0,0,0,1
	; position 10 - pattern 10
	db	1,0,0,0,1,0,0,1
	; position 11 - pattern 11
	db	1,0,0,0,1,0,0,0,1
	; position 12 - pattern 12
	db	1,0,0,0,1,0,0,1
	; position 13 - pattern 13
	db	1,0,0,0,1,0,0,0,1
	; position 14 - pattern 14
	db	1,0,0,0,1,0,0,1
	; position 16 - pattern 16
	db	1,0,0,0,1,0,0,1
	; position 17 - pattern 17
	db	1,0,0,0,1,0,0,0,1
	; position 18 - pattern 18
	db	1,0,0,0,1,0,0,1
	; position 19 - pattern 19
	db	1,0,0,0,1,0,0,0,1
	; position 20 - pattern 20
	db	1,0,0,0,1,0,0,1
	; position 21 - pattern 21
	db	1,0,0,0,1,0,0,0,1
	; position 22 - pattern 22
	db	1,0,0,0,1,0,0,1
	; position 23 - pattern 23
	db	1,0,0,0,1,0
	; position 24 - pattern 24
	db	1,0,0,0,1,0,0,1
	; position 25 - pattern 25
	db	1,0,0,0,1,0,0,0,1
	; position 26 - pattern 26
	db	1,0,0,0,1,0,0,1
	; position 27 - pattern 27
	db	1,0,0,0,1,0,0,0,1
	db	-1

	; 12:  snare1 / 04|snare2
	; position 8 - pattern 8
	db	0,0
	; position 9 - pattern 9
	db	0,0
	; position 10 - pattern 10
	db	0,0
	; position 11 - pattern 11
	db	0,0
	; position 12 - pattern 12
	db	0,0
	; position 13 - pattern 13
	db	0,0
	; position 14 - pattern 14
	db	0,0
	; position 15 - pattern 15
	db	0,0
	; position 16 - pattern 16
	db	0,0
	; position 17 - pattern 17
	db	0,0
	; position 18 - pattern 18
	db	0,0
	; position 19 - pattern 19
	db	0,0
	; position 20 - pattern 20
	db	0,0
	; position 21 - pattern 21
	db	0,0
	; position 22 - pattern 22
	db	0,0
	; position 23 - pattern 23
	db	0,0
	; position 24 - pattern 24
	db	0,0
	; position 25 - pattern 25
	db	0,0
	; position 26 - pattern 26
	db	0,0
	; position 27 - pattern 27
	db	0,0
	; position 28 - pattern 28
	db	0,0
	; position 29 - pattern 29
	db	0,0
	db	-1

	; 13:  snare1 / 03|snare
	; position 8 - pattern 8
	db	0,0,0,0,0,0
	; position 9 - pattern 9
	db	0,0,0,0,0,0
	; position 10 - pattern 10
	db	0,0,0,0,0,0
	; position 11 - pattern 11
	db	0,0,0,0,0,0
	; position 12 - pattern 12
	db	0,0,0,0,0,0
	; position 13 - pattern 13
	db	0,0,0,0,0,0
	; position 14 - pattern 14
	db	0,0,0,0,0,0
	; position 15 - pattern 15
	db	0,0,0,0,0,0
	; position 16 - pattern 16
	db	0,0,0,0,0,0
	; position 17 - pattern 17
	db	0,0,0,0,0,0
	; position 18 - pattern 18
	db	0,0,0,0,0,0
	; position 19 - pattern 19
	db	0,0,0,0,0,0
	; position 20 - pattern 20
	db	0,0,0,0,0,0
	; position 21 - pattern 21
	db	0,0,0,0,0,0
	; position 22 - pattern 22
	db	0,0,0,0,0,0
	; position 23 - pattern 23
	db	0,0,0,0,0,0
	; position 24 - pattern 24
	db	0,0,0,0,0,0
	; position 25 - pattern 25
	db	0,0,0,0,0,0
	; position 26 - pattern 26
	db	0,0,0,0,0,0
	; position 27 - pattern 27
	db	0,0,0,0,0,0
	; position 28 - pattern 28
	db	0,0,0,0,0,0
	; position 29 - pattern 29
	db	0,0,0,0,0,0
	db	-1

	; 14:  ride / 08|ride
	; position 6 - pattern 6
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 7 - pattern 7
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 8 - pattern 8
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 9 - pattern 9
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 10 - pattern 10
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 11 - pattern 11
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 12 - pattern 12
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 13 - pattern 13
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 14 - pattern 14
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 15 - pattern 15
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 16 - pattern 16
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 17 - pattern 17
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 18 - pattern 18
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 19 - pattern 19
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 20 - pattern 20
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 21 - pattern 21
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 22 - pattern 22
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 23 - pattern 23
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 24 - pattern 24
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 25 - pattern 25
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 26 - pattern 26
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 27 - pattern 27
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 28 - pattern 28
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 29 - pattern 29
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	; position 30 - pattern 30
	db	0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	db	-1

	; 15:  perc_shak_snar / 09|shaker
	; position 4 - pattern 4
	db	0,0,0,0,0,0,0,0
	; position 5 - pattern 5
	db	0,0,0,0,0,0,0,0
	; position 6 - pattern 6
	db	0,0,0,0,0,0,0,0
	; position 7 - pattern 7
	db	0,0,0,0,0,0,0
	; position 8 - pattern 8
	db	0,0,0,0,0,0,0,0
	; position 9 - pattern 9
	db	0,0,0,0,0,0,0,0
	; position 10 - pattern 10
	db	0,0,0,0,0,0,0,0
	; position 11 - pattern 11
	db	0,0,0,0,0,0,0,0
	; position 12 - pattern 12
	db	0,0,0,0,0,0,0,0
	; position 13 - pattern 13
	db	0,0,0,0,0,0,0,0
	; position 14 - pattern 14
	db	0,0,0,0,0,0,0,0
	; position 15 - pattern 15
	db	0,0,0,0,0,0,0
	; position 16 - pattern 16
	db	0,0,0,0,0,0,0,0
	; position 17 - pattern 17
	db	0,0,0,0,0,0,0,0
	; position 18 - pattern 18
	db	0,0,0,0,0,0,0,0
	; position 19 - pattern 19
	db	0,0,0,0,0,0,0,0
	; position 20 - pattern 20
	db	0,0,0,0,0,0,0,0
	; position 21 - pattern 21
	db	0,0,0,0,0,0,0,0
	; position 22 - pattern 22
	db	0,0,0,0,0,0,0,0
	; position 23 - pattern 23
	db	0,0,0,0,0,0,0
	; position 24 - pattern 24
	db	0,0,0,0,0,0,0,0
	; position 25 - pattern 25
	db	0,0,0,0,0,0,0,0
	; position 26 - pattern 26
	db	0,0,0,0,0,0,0,0
	; position 27 - pattern 27
	db	0,0,0,0,0,0,0,0
	; position 28 - pattern 28
	db	0,0,0,0,0,0,0,0
	; position 29 - pattern 29
	db	0,0,0,0,0,0,0,0
	; position 30 - pattern 30
	db	0,0,0,0,0,0,0,0
	db	-1

	; 16:  perc_shak_snar / 05|snare3
	; position 4 - pattern 4
	db	1,1
	; position 5 - pattern 5
	db	1,1,0
	; position 6 - pattern 6
	db	1,1
	; position 7 - pattern 7
	db	1,1,1,1
	; position 8 - pattern 8
	db	1,1
	; position 9 - pattern 9
	db	1,1
	; position 10 - pattern 10
	db	1,1
	; position 11 - pattern 11
	db	1,1
	; position 12 - pattern 12
	db	1,1
	; position 13 - pattern 13
	db	1,1
	; position 14 - pattern 14
	db	1,1
	; position 15 - pattern 15
	db	1,1,1,1
	; position 16 - pattern 16
	db	1,1
	; position 17 - pattern 17
	db	1,1
	; position 18 - pattern 18
	db	1,1
	; position 19 - pattern 19
	db	1,1
	; position 20 - pattern 20
	db	1,1
	; position 21 - pattern 21
	db	1,1
	; position 22 - pattern 22
	db	1,1
	; position 23 - pattern 23
	db	1,1,1,1
	; position 24 - pattern 24
	db	1,1
	; position 25 - pattern 25
	db	1,1
	; position 26 - pattern 26
	db	1,1
	; position 27 - pattern 27
	db	1,1
	; position 28 - pattern 28
	db	1,1
	; position 29 - pattern 29
	db	1,1
	; position 30 - pattern 30
	db	1,1
	db	-1

	; 17:  sub / 0A|subbass
	; position 8 - pattern 8
	db	8,13,7,6,5,11,12,4
	; position 9 - pattern 9
	db	3,9,2,1,3,9,10,0
	; position 10 - pattern 10
	db	8,13,7,6,5,11,12,4
	; position 11 - pattern 11
	db	3,9,2,1,3,9,10,0
	; position 12 - pattern 12
	db	8,13,7,6,5,11,12,4
	; position 13 - pattern 13
	db	3,9,2,1,3,9,10,0
	; position 14 - pattern 14
	db	8,13,7,6,5,11,12,4
	; position 15 - pattern 15
	db	3,9,2,1,3,9,10,0
	; position 16 - pattern 16
	db	8,13,7,6,5,11,12,4
	; position 17 - pattern 17
	db	3,9,2,1,3,9,10,0
	; position 18 - pattern 18
	db	8,13,7,6,5,11,12,4
	; position 19 - pattern 19
	db	3,9,2,1,3,9,10,0
	; position 20 - pattern 20
	db	8,13,7,6,5,11,12,4
	; position 21 - pattern 21
	db	3,9,2,1,3,9,10,0
	; position 22 - pattern 22
	db	8,13,7,6,5,11,12,4
	; position 23 - pattern 23
	db	3,9,2,1,3,9,10,0
	; position 24 - pattern 24
	db	8,13,7,6,5,11,12,4
	; position 25 - pattern 25
	db	3,9,2,1,3,9,10,0
	; position 26 - pattern 26
	db	8,13,7,6,5,11,12,4
	; position 27 - pattern 27
	db	3,9,2,1,3,9,10,0
	; position 28 - pattern 28
	db	8,13,7,6,5,11,12,4
	; position 29 - pattern 29
	db	3,9,2,1,3,9,10,0
	; position 30 - pattern 30
	db	8,13,7,6,5,11,12,4
	; position 31 - pattern 31
	db	3,9,2,1,3,9,10,0
	db	-1

