  \ flow.ccase.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201806041131
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ `ccase` is a positional control structure that uses the
  \ position of a character in a string as key, and has a
  \ mandatory default option at the end.  Data space used
  \ (without requirements): 69 bytes.
  \
  \ `ccase0` is a simpler variant of `ccase` that uses the
  \ first compiled option as a mandatory default option.  Data
  \ space used (without requirements): 44 bytes.
  \
  \ `?ccase` is a simpler variant of `ccase` with no default
  \ option.  Data space used (without requirements): 34 bytes.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016, 2017, 2018.

  \ ===========================================================
  \ Credit

  \ `?ccase` was inspired by MMSFORTH's `acase`.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( ?ccase )

need alias need char-position?

' >resolve alias end?ccase  immediate compile-only
  \ Compilation: ( C: orig -- )
  \ Run-time: ( -- )

  \ doc{
  \
  \ end?ccase "end-question-case"
  \   Compilation: ( C: orig -- )
  \   Run-time: ( -- )
  \
  \ End of a `?ccase` control structure.
  \ See `?ccase` for a usage example.
  \
  \ ``end?ccase`` is an `immediate` and `compile-only` word.
  \
  \ }doc

: (?ccase ( c ca len -- )
  rot char-position? if  2+ cells r@ + perform  then ;

  \ doc{
  \
  \ (?ccase ( c ca len -- ) "paren-question-c-case"
  \
  \ Run-time procedure compiled by `?ccase`.  If _c_ is in the
  \ string _ca len_, execute the n-th word compiled after
  \ `?ccase`, where _n_ is the position of the first _c_ in the
  \ string (0..len-1).  If _c_ is not in _ca len_, do nothing.
  \
  \ }doc

: ?ccase
  \ Compilation: ( C: -- orig )
  \ Run-time: ( c ca len -- )
  postpone (?ccase postpone ahead ; immediate compile-only

  \ doc{
  \
  \ ?ccase "question-c-case"
  \   Compilation: ( C: -- orig )
  \   Run-time: ( c ca len --)

  \
  \ Start a ``?ccase``..`end?ccase` structure. If _c_ is in the
  \ string _ca len_, execute the n-th word compiled after
  \ ``?ccase``, where _n_ is the position of the first _c_ in
  \ the string (0..len-1), then continue after `end?ccase`.  If
  \ _c_ is not in _ca len_, just continue after `end?ccase`.
  \
  \ ``?ccase`` is an `immediate` and `compile-only` word.
  \
  \ Usage example:

  \ ----
  \ : .a   ( -- ) ." Letter A" ;
  \ : .b   ( -- ) ." Letter B" ;
  \ : .c   ( -- ) ." Letter C" ;
  \
  \ : letter ( c -- )
  \   s" abc" ?ccase  .a .b .c  end?ccase  ."  The End" cr ;
  \ ----
  \
  \ See: `ccase`, `ccase0`.
  \
  \ }doc

( ccase0 )

need alias need char-position?

' >resolve alias endccase0  immediate compile-only
  \ Compilation: ( C: orig -- )
  \ Run-time: ( -- )

  \ doc{
  \
  \ endccase0 "end-c-case-zero"
  \   Compilation: ( C: orig -- )
  \   Run-time: ( -- )
  \
  \ End of a `ccase0` control structure.
  \ See `ccase0` for a usage example.
  \
  \ ``endcase0`` is an `immediate` and `compile-only` word.
  \
  \ }doc

: (ccase0 ( c ca len -- )
  rot char-position? if    ( +n ) 3 +
                           \ character found:
                           \ calculate the cells offset to the option
                     else  2
                           \ character not found:
                           \ leave cells offset to the default option
                     then  cells r@ + perform ;

  \ doc{
  \
  \ (ccase0 ( c ca len -- ) "paren-c-case-zero"
  \
  \ Run-time procedure compiled by `ccase0`.  If _c_ is in the
  \ string _ca len_, execute the n-th word compiled after
  \ `ccase0`, where _n_ is the position of the first _c_ in the
  \ string (0..len-1) plus 1.  If _c_ is not in _ca len_,
  \ execute the word compiled right after `ccase0`.
  \
  \ }doc

: ccase0
  \ Compilation: ( C: -- orig )
  \ Run-time: ( c ca len -- )
  postpone (ccase0 postpone ahead ; immediate compile-only

  \ doc{
  \
  \ ccase0 "c-case-zero"
  \   Compilation: ( C: -- orig )
  \   Run-time: ( c ca len -- )

  \
  \ Start a ``ccase0``..`endccase` structure. If _c_ is in the
  \ string _ca len_, execute the n-th word compiled after
  \ ``ccase0``, where _n_ is the position of the first _c_ in
  \ the string (0..len-1) plus 1, then continue after
  \ ``endccase0``.  If _c_ is not in _ca len_, execute the word
  \ compiled right after ``ccase0``, then continue after
  \ `endccase0`.
  \
  \ ``ccase0`` is an `immediate` and `compile-only` word.
  \
  \ Usage example:

  \ ----
  \ : .a    ( -- ) ." Letter A" ;
  \ : .b    ( -- ) ." Letter B" ;
  \ : .c    ( -- ) ." Letter C" ;
  \ : .nope ( -- ) ." Nope!" ;
  \
  \ : letter ( c -- )
  \   s" abc" ccase0  .nope .a .b .c  endccase0
  \   ."  The End" cr ;
  \ ----
  \
  \ See: `ccase` `?ccase`.
  \
  \ }doc

( ccase )

need char-position?

: endccase
  \ Compilation: ( C: orig1 orig2 -- )
  \ Run-time: ( -- )
  here cell- swap !
    \ resolve _orig2_, the address of the default option
  >resolve
    \ resolve _orig1_, the branch to `endccase`
 ; immediate compile-only

  \ doc{
  \
  \ endccase "end-c-case"
  \   Compilation: ( C: orig1 orig2 -- )
  \   Run-time: ( -- )

  \
  \ End of a `ccase` control structure.
  \ See `ccase` for a usage example.
  \
  \ ``endccase`` is an `immediate` and `compile-only` word.
  \
  \ }doc

: (ccase ( c ca len -- )
  rot char-position? if    ( +n ) 3 + cells r@ +
                           \ character found:
                           \ calculate address of the option
                     else  r@ cell+ cell+ @
                           \ character not found:
                           \ calculate address of the default option
                     then  perform ;

  \ doc{
  \
  \ (ccase ( c ca len -- ) "paren-c-case"
  \
  \ Run-time procedure compiled by `ccase`.  If _c_ is in the
  \ string _ca len_, execute the n-th word compiled after
  \ `ccase`, where _n_ is the position of the first _c_ in the
  \ string (0..len-1).  If _c_ is not in _ca len_,
  \ execute the word compiled right before `endccase`.
  \
  \ }doc

: ccase
  \ Compilation: ( C: -- orig1 orig2 )
  \ Run-time: ( c ca len -- )
  postpone (ccase  postpone ahead  >mark
  ; immediate compile-only

  \ doc{
  \
  \ ccase "c-case"
  \   Compilation: ( C: -- orig1 orig2 )
  \   Run-time: ( c ca len -- )
  \
  \ Start a ``ccase``..`endccase` structure. If _c_ is in the
  \ string _ca len_, execute the n-th word compiled after
  \ ``ccase``, where _n_ is the position of the first _c_ in the
  \ string (0..len-1) plus 1, then continue after `endccase`.
  \ If _c_ is not in _ca len_, execute the word compiled right
  \ after `ccase`, then continue after `endccase`.
  \
  \ ``ccase`` is an `immediate` and `compile-only` word.
  \
  \ Usage example:

  \ ----
  \ : .a    ( -- ) ." Letter A" ;
  \ : .b    ( -- ) ." Letter B" ;
  \ : .c    ( -- ) ." Letter C" ;
  \ : .nope ( -- ) ." Nope!" ;
  \
  \ : letter ( c -- )
  \   s" abc" ccase  .a .b .c .nope  endccase
  \   ."  The End" cr ;
  \ ----
  \
  \ See: `ccase0`, `?ccase`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-04-28: Write `?ccase`, `ccase0`.
  \
  \ 2016-11-26: Improve `(ccase)`.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-02-27: Fix and improve documentation.
  \
  \ 2018-02-04: Fix documentation layout. Improve
  \ documentation: add pronunciation to words that need it.
  \
  \ 2018-06-04: Update: remove trailing closing paren from
  \ word names.

  \ vim: filetype=soloforth
