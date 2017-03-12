  \ game.siderator_2.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ The sample game Siderator 2.

  \ XXX UNDER DEVELOPMENT

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2009, 2010, 2013, 2015,
  \ 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ ===========================================================
  \ To-do

  \ XXX TODO -- less stars!
  \ XXX FIXME -- The craft's autodestruction creates a new star.
  \ XXX FIXME -- 2016-08-05: now stars appear at the middle of
  \ the screen

( siderator )

only forth definitions  decimal

need random need randomize need udg: need inkey
need between need ocr need frames@ need inverse
need j need between need rows need last-column
need last-row need vocabulary
need cyan need white need set-ink

vocabulary siderator
also siderator definitions

  \ Game variables and constants:

variable x          variable speed
variable parsecs    variable record  record off

999 constant max-speed
'5' constant left-key  '8' constant right-key

  \ System variables and addresses:

8192 constant 'screen \ XXX OLD

  \ Common words:

: pause ( -- ) begin  inkey  until ;  -->

( siderator )

  \ Graphics

15360 constant charset  \ ROM charset

: char>a ( c -- a ) 8 * charset + ;

: udg>a ( c -- a ) 128 - 8 * os-udg @ + ;

: char>udg ( c0 c1 -- ) swap char>a swap udg>a 8 cmove ;

128 constant star0-udg  '*' star0-udg char>udg

%00011000
%00001000
%00011000
%00010000
%00011000
%00001000
%00011000
%00010000 129 udg: star1-udg  -->

( siderator )

130 constant star2-udg
'|' star2-udg char>udg

%00001000
%00000000
%00001000
%00000000
%00001000
%00000000
%00001000
%00000000 131 udg: star3-udg  -->

( siderator )

%10000001
%10000001
%11000011
%11100111
%11111111
%01100110
%00111100
%00011000 132 udg: craft-udg  -->

( siderator )

  \ Keyboard

0 constant first-column

: pressed? ( c -- f ) inkey = ;

: left ( col -- col' )
  left-key pressed? + first-column max ;

: right ( col -- col' )
  right-key pressed? - last-column min ;

: rudder ( -- ) x @ right left x ! ;

  \ Stars

4 constant #stars

: star-coords ( -- gx gy ) last-column last-row ;

: .star ( c -- )
  [ last-column 1+ ] literal random last-row at-xy
  1 bright. emit 0 bright. ;

: stars/speed ( -- n ) speed @ #stars 1- max-speed */ 1+ ;

: scroll ( -- ) star-coords at-xy cr cr ;  -->

( siderator )

: .stars ( -- )
  stars/speed dup [ star0-udg 1- ] literal + swap 0
  ?do  dup .star  loop  drop ;

: star= ( c -- f ) star0-udg star3-udg between ;

: star<> ( c -- f ) star= 0= ;

  \ Craft

rows 2 / constant craft-y

: craft-coords ( -- y x ) x @ craft-y ;

: at-craft ( -- ) craft-coords at-xy ;

: -craft ( -- ) at-craft space ;

: .craft ( -- )
  at-craft craft-udg cyan set-ink emit white set-ink ;

  \ Speed, parsecs, record

: .datum ( u -- ) s>d <# # # # #> type space ;

: delay ( -- ) max-speed speed @ - 2 / 0  ?do  loop ;

: .speed ( -- ) ." Speed:" speed @ .datum ;  -->

( siderator )

: +speed ( u1 -- u2 )
  dup 10 / 1 max  parsecs @ 4 mod 0= abs *  + max-speed min ;

: faster ( -- ) speed @ +speed speed ! ;

: .parsecs ( -- ) ." Parsecs:" parsecs @ .datum ;

: farther ( -- ) 1 parsecs +! ;

: .record ( -- ) ." Record:" record @ .datum ;

: .info ( -- ) home .speed .parsecs .record ;

  \ End

: blast-delay ( -- ) 32 0  ?do  loop ;

: (blast) ( -- )
  .craft blast-delay at-craft star0-udg emit blast-delay ;

: blast ( -- ) 256 0  ?do  (blast)  loop ;

: halt ( -- )
  32 0  ?do  24 0 ?do
    i j ocr star= if  i j at-xy  star0-udg emit  then
  loop  loop ;  -->
  \ XXX TODO

( siderator )

: safe? ( -- f ) craft-coords swap ocr star<> ;

: continue? ( -- f ) safe? break-key? 0= and ;

: new-record ( -- )
  parsecs @ record @ >  if  parsecs @ record !  then ;

: game-over ( -- )
  blast halt  11 dup at-xy ." GAME OVER"
  new-record .info first-column last-row at-xy
  default-colors ;

  \ Instructions

: about ( -- )
  cr ." Siderator 2: Jugdement Day"  cr
  cr ." By programandala.net"
  cr ." Version: 0.1.0+20160325" ;

-->

( siderator )

: objective ( -- )
  cr ." Your objective is to travel as"
  cr ." much parsecs as possible"
  cr ." while dodging the stars."
  cr ." Anyway you're supposed to die"
  cr ." before the 1000th parsec"
  cr ." because four digits would ruin"
  cr ." the score panel." ;

  \ Instructions

: keys ( -- ) cr ." Rudder keys: "
                left-key emit space right-key emit
                cr ." Autodestruction key: Break" ;

: instructions ( -- ) objective cr keys ;

: wait ( -- ) cr cr ." Press any key to start." pause ;  -->

( siderator )

  \ Init

: init-colors ( -- )
  white attr!  0 inverse  0 border ;

: init-screen ( -- )
  init-colors cls about cr instructions wait cls ;

: 4+- ( n1 -- n2 ) 9 random 4 - + ;

: init ( -- )
  frames@ s>d randomize  udg-ocr
  init-screen  15 4+- x ! parsecs off  speed off ;

: run ( -- )
  init  begin  -craft scroll  faster farther .info  continue?
        while  rudder .craft .stars delay  repeat  game-over ;

  \ ===========================================================
  \ Change log

  \ 2015-09-02: Start, with the code of the Jupiter ACE
  \ version: http://programandala.net/en.program.siderator.html
  \
  \ 2016-05-02: Compact two blocks to save space in the
  \ library.
  \
  \ 2016-05-18: Need `vocabulary`, which has been moved to the
  \ library.
  \
  \ 2016-06-01: Replace `char` with char notation.
  \
  \ 2016-08-05: Compact the code, but no block is saved. Add
  \ missing `need inverse`.
  \
  \ 2016-08-05: Compact the code, saving three blocks.
  \
  \ 2017-01-31: Update the attribute and color words.
  \
  \ 2017-02-19: Replace `do`, which has been moved to the
  \ library, with `?do`.

  \ vim: filetype=soloforth
