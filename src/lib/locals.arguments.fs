  \ locals.arguments.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020

  \ -----------------------------------------------------------
  \ Description

  \ An implementation of nestable locals, with a predefined set
  \ of ten variables which return their contents.

  \ -----------------------------------------------------------
  \ Authors

  \ Original code by Marc Perkel, published on Forth Dimensions
  \ (volume 3, number 6, page 185, 1982-03).
  \
  \ Adapted to Solo Forth and improved by Marcos Cruz
  \ (programandala.net), 2015, 2016.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

  \ 2015-11-14: Start.
  \
  \ 2016-04-09: Fixed, improved, renamed, documented, finished.
  \
  \ 2017-02-17: Fix markup in comment.
  \
  \ 2017-02-19: Replace `do`, which has been moved to the
  \ library, with `?do`.

( arguments results )

need cell/

variable >args
  \ address of the current arguments in the data stack

create arg-actions ] @ ! +! [
  \ execution table of the argument actions

variable arg-action  arg-action off
  \ id (offset) of the argument action:
  \ 0 = fetch; 2 (1 cell) = store; 4 (2 cells) = add

: arg: ( +n "name" -- )
  create  c,
  does> ( -- x ) ( x -- )
    \ ( pfa | x pfa )
    c@ >args @ swap -
    arg-action @ arg-actions + perform  arg-action off ;
  \ create a new argument _name_ with offset _+n_

$00 arg: l0 $02 arg: l1 $04 arg: l2 $06 arg: l3 $08 arg: l4
$0A arg: l5 $0C arg: l6 $0E arg: l7 $10 arg: l8 $12 arg: l9

-->

( arguments results )

: toarg ( -- ) cell arg-action ! ;

  \ doc{
  \
  \ toarg ( -- )
  \
  \ Set the store action for the next local variable. Used with
  \ locals created by `arguments`.  See `arguments` for a usage
  \ example.
  \
  \ }doc

: +toarg ( -- ) [ 2 cells ] literal arg-action ! ;

  \ doc{
  \
  \ +toarg ( -- )
  \
  \ Set the add action for the next local variable. Used with
  \ locals created by `arguments`.  See `arguments` for a usage
  \ example.
  \
  \ }doc

: arguments ( i*x +n -- j*x )
  r> >args @ >r >r
  cells sp@ + dup >args ! [ 10 cells ] literal - sp@ swap -
  cell/ 0 ?do  0  loop ; compile-only

  \ doc{
  \
  \ arguments ( i*x +n -- j*x )
  \
  \ Define the number _+n_ of arguments to take from the stack
  \ and assign them to the first local variables from `l0` to
  \ `l9`.
  \
  \ The local variables are modified with `toarg`, `+toarg`,
  \ and returned  with `results`.
  \
  \ Example: The phrase `3 arguments` assigns the names of
  \ local variables `l0` through `l9` to ten stack positions,
  \ with `l0`, `l1` and `l2` returning the top 3 stack values
  \ that were there before `3 arguments` was executed. `l3`
  \ through `l9` are zero-filled and the stack pointer is set
  \ to just below `l9`.
  \
  \ `l0` through `l9` act as local variables returning their
  \ contents, not their addresses.  To write them you precede
  \ them with the word `toarg`. For example `5 toarg l4` writes
  \ a 5 into `l4`. Execution of `l4` returns 5 to the stack.
  \
  \ After all calculating is done, the phrase `3 results`
  \ leaves that many results on the stack relative to the stack
  \ position when `arguments` was executed. All intermediate
  \ stack values are lost, which is good because you can leave
  \ the stack "dirty" and it doesn't matter.
  \
  \ Usage example:

  \ ----
  \ : test ( length width height -- length' volume surface )
  \   3 arguments
  \   l0 l1 * toarg l5       \ surface
  \   l5 l2 * toarg l4       \ volume
  \   $2000 +toarg l0        \ length+$2000
  \   l4 toarg l1            \ volume
  \   l5 toarg l2            \ surface
  \   3 results ;
  \ ----

  \ }doc

: results ( +n -- )
  cells >args @ swap - sp@ -
  cell/ 0 ?do  drop  loop
  r> r> >args ! >r ; compile-only

  \ doc{
  \
  \ results ( +n -- )
  \
  \ Define the number _+n_ of local variables to leave on the
  \ stack as results. Used with locals created by `arguments`.
  \ See `arguments` for a usage example.
  \
  \ }doc

  \ vim: filetype=soloforth