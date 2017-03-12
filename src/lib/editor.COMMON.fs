  \ editor.COMMON.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ This module contains code common to any editor.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( r# top )

variable r#

  \ doc{
  \
  \ r# ( -- a )
  \
  \ A variable that holds the location of the editing cursor,
  \ an offset from the top of the current block. Its default
  \ value is zero.
  \
  \ Origin: fig'Forth's user variable `r#`.
  \
  \ }doc

: top ( -- ) r# off ;  top

  \ doc{
  \
  \ top ( -- )
  \
  \ Position the editing cursor at the top of the block.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-11-22: Start, with code extracted from
  \ <editor.specforth.fsb> and <editor.blocked.fsb>.
  \
  \ 2016-12-08: Rename the module filename with uppercase
  \ "COMMON", after the new convention.

  \ vim: filetype=soloforth
