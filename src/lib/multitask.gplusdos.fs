  \ multitask.gplusdos.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 20160324

  \ ===========================================================
  \ Description

  \ The Jiffy tool for multitasking on
  \ G+DOS.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.


( jiffy! jiffy@ -jiffy )

  \ Note: This code is specific for G+DOS.

  \ Credit:
  \
  \ Idea inspired by an article by Paul King, published on
  \ Format (volume 2, number 3, 1988-10).
  \
  \ XXX TODO link to the WoS archive ftp, when available

need !dosvar need @dosvar

: jiffy! ( a -- ) 16 !dosvar ;
  \ Set the Z80 routine to be called by G+DOS after the OS
  \ interrupts routine, every 50th of a second.

: jiffy@ ( -- a ) 16 @dosvar ;
  \ Get the current Z80 routine that is called by G+DOS after
  \ the OS interrupts routine, every 50th of a second.

: -jiffy ( -- ) 8335 jiffy! ;
  \ Deactivate the jiffy call, setting its default value
  \ (a noop routine in the RAM of the +D interface).

  \ vim: filetype=soloforth
