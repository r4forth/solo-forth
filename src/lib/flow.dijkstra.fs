  \ flow.dijkstra.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 20160324

  \ -----------------------------------------------------------
  \ Description

  \ The Dijkstra control structures.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.


( {if if} if> |if| )

  \ Dijkstra Guarded Command Control Structures

  \ Credit:
  \
  \ Adapted from: Dijkstra Guarded Command Control Structures
  \ M. Edward Borasky, 1996-08-03. Listing in "Towards a
  \ Discipline of ANS Forth Programming". Originally published
  \ on Forth Dimensions (volume 18, number 4, pp 5-14).
  \ Adapted to hForth v0.9.9 by Wonyong Koh.

need cs-roll

: {if ( -- 0 ) 0 ; immediate compile-only
  \ start a conditional
  \ put counter on stack

: if>
  \ ( count -- count+1 )
  \ ( c: -- orig1 )
  1+ >r postpone if  r> ; immediate compile-only
  \ right-arrow for {if ... if}

: |if|
  \ ( count -- count )
  \ ( c: orig ... orig1 -- orig ... orig2 )
  >r postpone ahead \ new orig
  1 cs-roll postpone then \ resolve old orig
  r> ; immediate compile-only
  \ bar for {if ... if}

: if} \ end of conditional
  \ ( count -- )
  \ ( c: orig1 ... orign -- )
  >r  postpone ahead
  1 cs-roll postpone then \ resolve old orig
  -22 postpone literal postpone throw
    \ 'control structure mismatch'
  r> 0 ?do  postpone then  loop ; immediate compile-only
  \ end a conditional

( {do do} do> |do| )

  \ Dijkstra Guarded Command Control Structures

  \ Credit:
  \
  \ Adapted from: Dijkstra Guarded Command Control Structures
  \ M. Edward Borasky, 1996-08-03. Listing in "Towards a
  \ Discipline of ANS Forth Programming" Originally published
  \ on Forth Dimensions (number 18, number 4, pages 5-14).
  \ Adapted to hForth v0.9.9 by Wonyong Koh

need cs-pick need cs-roll

: {do ( c: -- dest ) postpone begin ; immediate compile-only
  \ start a loop

: do> ( c: dest -- orig1 dest )
  postpone if  1 cs-roll ; immediate compile-only
  \ right arrow for {do ... od}

: |do| ( c: orig1 dest -- dest )
  0 cs-pick postpone again \ resolve a copy of dest
  1 cs-roll postpone then \ resolve old orig
 ; immediate compile-only
  \ bar for {do ... do}

: do} ( c: orig dest -- )
  \ end of loop
  postpone again \ resolve dest
  postpone then \ resolve orig
 ; immediate compile-only
  \ end a loop

  \ vim: filetype=soloforth