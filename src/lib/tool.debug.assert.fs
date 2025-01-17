  \ tool.debug.assert.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201807202342
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Versions of the `assert` debugging tool.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( assert assert= assert( )

unneeding assert
?\ : assert ( n -- ) 0= if  abort  then ;

unneeding assert=
?\ need assert  : assert= ( a b -- ) = assert ;

  \ Credit:
  \
  \ `assert` and `assert=` are taken from Brad Nelson's code:
  \
  \ http://bradn123.github.io/literateforth/out/events.fs

unneeding assert( ?exit

  \ Credit:
  \
  \ Documentation and public-domain code of `assert(` from
  \ Gforth.

  \ It is a good idea to make your programs self-checking, in
  \ particular, if you use an assumption (e.g., that a certain
  \ field of a data structure is never zero) that may become
  \ wrong during maintenance.  Gforth supports assertions for
  \ this purpose. They are used like this:

  \      assert( FLAG )

  \ The code between `assert(' and `)' should compute a flag,
  \ that should be true if everything is alright and false
  \ otherwise. It should not change anything else on the stack.
  \ The overall stack effect of the assertion is `( -- )'. E.g.

  \   assert( 1 1 + 2 = ) \ what we learn in school
  \   assert( dup 0<> ) \ the top of stack should not be zero
  \   assert( false ) \ this code should not be reached

  \ The need for assertions is different at different times.
  \ During debugging, we want more checking, in production we
  \ sometimes care more for speed. Therefore, assertions can be
  \ turned off, i.e., the assertion becomes a comment.
  \ Depending on the importance of an assertion and the time it
  \ takes to check it, you may want to turn off some assertions
  \ and keep others turned on. Gforth provides several levels
  \ of assertions for this purpose:

  \ Note that the `assert-level' is evaluated at compile-time,
  \ not at run-time. I.e., you cannot turn assertions on or off
  \ at run-time, you have to set the `assert-level'
  \ appropriately before compiling a piece of code. You can
  \ compile several pieces of code at several `assert-level's
  \ (e.g., a trusted library at level 1 and newly written code
  \ at level 3).

variable assert-level ( -- a ) 1 assert-level !

  \ doc{
  \
  \ assert-level ( -- a )
  \
  \ A `variable`. _a_ is the address of a cell containing the
  \ highest assertions that are turned on (0..3).  Its default
  \ value is 1: all assertions above 1 are turned off.
  \
  \ Origin: Gforth.
  \
  \ See: `assert(`.
  \
  \ }doc

: assertn ( n -- ) assert-level @ > if  postpone (  then ;

  \ doc{
  \
  \ assertn ( n -- ) "assert-n"
  \
  \ If the contents of `assert-level` is greater than _n_, then
  \ parse and discard the input stream to the next right paren
  \ (the end of the assertion); else do nothing.  ``assertn``
  \ is the common factor of `assert0(`, `assert1(`, `assert2(`,
  \ and `assert3(`.
  \
  \ Origin: Gforth.
  \
  \ See: `assert(`.
  \
  \ }doc

: assert0( ( -- ) 0 assertn ; immediate

  \ doc{
  \
  \ assert0( ( -- ) "assert-zero"
  \
  \ Start an important assertion.  Important assertions should
  \ always be turned on.
  \
  \ ``assert0(`` is an `immediate` word.
  \
  \ Origin: Gforth.
  \
  \ See: `assert-level`, `assert(`, `assert1(`, `assert2(`,
  \ assert3(`, `)`.
  \
  \ }doc

: assert1( ( -- ) 1 assertn ; immediate

  \ doc{
  \
  \ assert1( ( -- ) "assert-one"
  \
  \ Start a normal assertion.  Normal assertions are turned on
  \ by default.
  \
  \ ``assert1(`` is an `immediate` word.
  \
  \ Origin: Gforth.
  \
  \ See: `assert-level`, `assert(`, `assert0(`, `assert2(`,
  \ `assert3(`, `)`.
  \
  \ }doc

: assert2( ( -- ) 2 assertn ; immediate

  \ doc{
  \
  \ assert2( ( -- ) "assert-two"
  \
  \ Start a debugging assertion.
  \
  \ ``assert2(`` is an `immediate` word.
  \
  \ Origin: Gforth.
  \
  \ See: `assert-level`, `assert(`, `assert0(`, `assert1(`,
  \ `assert3(`, `)`.
  \
  \ }doc

: assert3( ( -- ) 3 assertn ; immediate

  \ doc{
  \
  \ assert3( ( -- ) "assert-three"
  \
  \ Start a slow assertion.  Slow assertions are those you may
  \ not want to turn on in normal debugging; you would turn
  \ them on mainly for thorough checking.
  \
  \ ``assert3(`` is an `immediate` word.
  \
  \ Origin: Gforth.
  \
  \ See: `assert-level`, `assert(`, `assert0(`, `assert1(`,
  \ `assert2(`, `)`.
  \
  \ }doc

: assert( ( -- ) postpone assert1( ; immediate

  \ doc{
  \
  \ assert( ( -- ) "assert-paren"
  \
  \ Start a normal assertion.  Normal assertion are turned on
  \ by default. ``assert(`` is equivalent to `assert1(`.
  \
  \ ``assert(`` is an `immediate` word.
  \
  \ Origin: Gforth.
  \
  \ See: `assert-level`, `assert0(`, `assert1(`,
  \ `assert2(`, assert3(`, `)`.
  \
  \ }doc

: (endassert ( f -- ) 0= #-262 ?throw ;

: ) ( f -- ) postpone (endassert ; immediate

  \ doc{
  \
  \ ) ( f -- ) "close-paren"
  \
  \ End an assertion.
  \
  \ ``)`` is an `immediate` word.
  \
  \ Origin: Gforth.
  \
  \ See: `assert(`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-11-14: Improve documentation.
  \
  \ 2016-12-30: Compact the code, saving one block.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-02-27: Improve documentation.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-12-09: Improve documentation.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-09: Add words' pronunciaton.
  \
  \ 2018-04-14: Fix markup in documentation.
  \
  \ 2018-06-04: Update: remove trailing closing paren from
  \ word names. Link `variable` in documentation.
  \
  \ 2018-07-20: Fix typo in documentation markup.

  \ vim: filetype=soloforth
