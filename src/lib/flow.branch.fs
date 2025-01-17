  \ flow.branch.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803052149
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Alternative branch words.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ XXX TODO -- Compilation stack comments.

( -branch +branch )

unneeding -branch ?(
code -branch ( n -- )
  E1 c,  CB c, 7C c,  C2 c, ' branch ,  03 c, 03 c,
    \ pop hl
    \ bit 7,h ; negative?
    \ jp nz,branch_xt ; branch if negative
    \ inc bc
    \ inc bc ; skip the inline branch address
  jpnext, end-code ?)

  \ doc{
  \
  \ -branch ( n -- ) "minus-branch"
  \
  \ A run-time procedure to branch conditionally. If  _n_ is
  \ negative, the following in-line address is copied to IP to
  \ branch forward or backward.
  \
  \ ``-branch`` is compiled by `+if` and `+until`.
  \
  \ See: `branch`, `?branch`, `0branch`, `+branch`.
  \
  \ }doc

unneeding +branch ?(
code +branch ( n -- )
  E1 c,  CB c, 7C c,  CA c, ' branch ,  03 c, 03 c,
    \ pop hl
    \ bit 7,h ; positive?
    \ jp z,branch_xt ; branch if positive
    \ inc bc
    \ inc bc ; skip the inline branch address
  jpnext, end-code ?)

  \ doc{
  \
  \ +branch ( n -- ) "plus-branch"
  \
  \ A run-time procedure to branch conditionally. If  _n_ is
  \ positive, the following in-line address is copied to IP to
  \ branch forward or backward.
  \
  \ ``+branch`` is compiled by `-if` and `-until`.
  \
  \ See: `branch`, `?branch`, `0branch`, `-branch`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-11-24: Move `+branch` from
  \ <flow.conditionals.negative.fsb>.  Add `-branch`. Improve
  \ documentation.
  \
  \ 2016-12-20: Rename `jpnext` to `jpnext,` after the change
  \ in the kernel.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-05-05: Improve documentation.
  \
  \ 2018-02-04: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.

  \ vim: filetype=soloforth

