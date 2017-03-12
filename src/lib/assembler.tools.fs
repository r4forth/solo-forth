  \ assembler.tools.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT

  \ Last modified: 201702220028

  \ ===========================================================
  \ Description

  \ Z80 assembler tools, independent from the actual assembler.

  \ ===========================================================
  \ Authors

  \ Original code by Frank Sergeant, for Pygmy Forth.
  \
  \ Adapted to Solo Forth by Marcos Cruz (programandala.net),
  \ 2015, 2016.

  \ ===========================================================--
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( << >> )

  \ For dumping assembled code to screen.

  \ Credit:
  \
  \ Code adapted from Pygmy Forth.

  \ XXX TODO finish adapt

need @c+ need for

: << ( -- a depth ) here depth ;
: >> ( a depth -- )
  depth 1- - #-258 ?throw cr base @ >r hex
  dup 4 u.r space  here over - for  c@+ 3 u.r  step drop
  r> base !  space ;

  \ vim: filetype=soloforth

