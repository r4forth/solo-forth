  \ flow.conditionals.negative.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702280002

  \ -----------------------------------------------------------
  \ Description

  \ Negative conditionals.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ Latest changes

  \ 2016-11-24: Compact the code, saving one block. Add
  \ conditional compilation for `need`. Rename `-branch` to
  \ `+branch` and move it to its own module.
  \
  \ 2016-12-20: Rename `jpnext` to `jpnext,` after the change
  \ in the kernel.
  \
  \ 2017-01-19: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-02-27: Improve documentation.

  \ XXX TODO -- Compilation stack comments.

( -if -while -until -exit )

[unneeded] -if ?( need +branch

: -if ( f -- )
  postpone +branch >mark ; immediate compile-only ?)

  \ doc{
  \
  \ -if ( f -- )
  \
  \ Faster and smaller alternative to the idiom `0< if`.
  \
  \ ``-if`` is an `immediate` and `compile-only` word.
  \
  \ }doc

[unneeded] -while ?( need -if need cs-swap

: -while ( f -- )
  postpone -if  postpone cs-swap ; immediate compile-only ?)

  \ doc{
  \
  \ -while ( f -- )
  \
  \ Faster and smaller alternative to the idiom `0< while`.
  \
  \ ``-while`` is an `immediate` and `compile-only` word.
  \
  \ }doc

[unneeded] -until ?( need +branch

: -until ( n -- )
  postpone +branch <resolve ; immediate compile-only ?)

  \ doc{
  \
  \ -until ( n -- )
  \
  \ Faster and smaller alternative to the idiom `0< until`.
  \
  \ ``-until`` is an `immediate` and `compile-only` word.
  \
  \ }doc

[unneeded] -exit ?(

code -exit ( n -- ) ( R: nest-sys | -- nest-sys | )
  E1 c,  CB c, 7C c,  C2 c, ' exit ,  jpnext, end-code ?)
  \ pop hl
  \ bit 7,h ; negative?
  \ jp nz,exit_xt ; exit if negative
  \ jp next

  \ doc{
  \
  \ -exit ( n -- ) ( R: nest-sys | -- nest-sys | )
  \
  \ If _n_ is negative, return control to the calling definition,
  \ specified by _nest-sys_.
  \
  \ `-exit` is not intended to be used within a do-loop. Use
  \ `0< if unloop exit then` instead.
  \
  \ In Solo Forth `-exit` can be used in interpretation mode to
  \ stop the interpretation of a block.
  \
  \ }doc

  \ vim: filetype=soloforth