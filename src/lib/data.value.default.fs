  \ data.value.default.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702280020

  \ -----------------------------------------------------------
  \ Description

  \ This module provides:
  \
  \ - Standard `value` and `to` for single-cell values.
  \ - Non-standard `2value` and `2to` for double-cell values.
  \ - Non-standard `cvalue` and `cto` for character values.
  \
  \ Note: There's a standard implementation of `value`,
  \ `2value` and `to` in module "data.value.standard.fsb",
  \ but it's bigger and slower.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

  \ 2015: First versions.
  \
  \ 2015-09-25: Benchmark all versions.
  \
  \ 2015-10-07: Add `cvalue` and `cto`.
  \
  \ 2016-03-24: Split the code into several library modules.
  \
  \ 2016-05-11: Combine three library modules into one. Rewrote
  \ all words as aliases. Document them.
  \
  \ 2016-05-13: Fix `to`, `2to` and `cto`: `immediate` was
  \ missing.
  \
  \ 2016-08-02: Fix comment.
  \
  \ 2017-02-27: Improve documentation.

( value to 2value 2to cvalue cto )

need alias

[unneeded] value [unneeded] to and ?(

' constant alias value ( x "name"  -- )

  \ doc{
  \
  \ value ( x "name" -- )
  \
  \ Create a definition _name_ with the following execution
  \ semantics: place _x_ on the stack.
  \
  \ See `to`.
  \
  \ Origin: Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ }doc

need !>  ' !> alias to immediate ?)

  \ doc{
  \
  \ to
  \   Interpretation: ( x "name" -- )
  \   Compilation: ( "name" -- )
  \   Execution: ( x -- )
  \
  \ ``to`` is an `immediate` word.
  \
  \ Interpretation:
  \
  \ Parse _name_, which is the name of a word created by
  \ `value`, and make _x_ its value.
  \
  \ Compilation:
  \
  \ Parse _name_, which is a word created by `value`, and
  \ append the execution execution semantics given below to the
  \ current definition.
  \
  \ Execution:
  \
  \ Make _x_ the current value of value _name_.
  \
  \ }doc

[unneeded] 2value [unneeded] 2to and ?(

' 2constant alias 2value ( xd "name"  -- )

  \ doc{
  \
  \ 2value ( xd "name" -- )
  \
  \ Create a definition _name_ with the following execution
  \ semantics: place _xd_ on the stack.
  \
  \ Note: This word is not the standard `2value` of Forth-94
  \ and Forth-2012. In Solo Forth, words created with this
  \ version of `2value` must be modified using the non-standard
  \ word `2to` instead of `to`.
  \
  \ See `2to`.
  \
  \ }doc

need 2!>  ' 2!> alias 2to immediate ?)

  \ doc{
  \
  \ 2to
  \   Interpretation: ( xd "name" -- )
  \   Compilation: ( "name" -- )
  \   Execution: ( xd -- )
  \
  \ ``2to`` is an `immediate` word.
  \
  \ Interpretation:
  \
  \ Parse _name_, which is the name of a word created by
  \ `2value`, and make _xd_ its value.
  \
  \ Compilation:
  \
  \ Parse _name_, which is a word created by `2value`, and
  \ append the execution execution semantics given below to the
  \ current definition.
  \
  \ Execution:
  \
  \ Make _xd_ the current value of double-cell value _name_.
  \
  \ }doc

[unneeded] cvalue [unneeded] cto and ?(

need cconstant ' cconstant alias cvalue ( c "name"  -- )

  \ doc{
  \
  \ cvalue ( c "name" -- )
  \
  \ Create a definition _name_ with the following execution
  \ semantics: place _c_ on the stack.
  \
  \ See `cto`.
  \
  \ }doc

need c!>  ' c!> alias cto immediate ?)

  \ doc{
  \
  \ cto
  \   Interpretation: ( c "name" -- )
  \   Compilation: ( "name" -- )
  \   Execution: ( c -- )
  \
  \ ``cto`` is an `immediate` word.
  \
  \ Interpretation:
  \
  \ Parse _name_, which is the name of a word created by
  \ `cvalue`, and make _c_ its value.
  \
  \ Compilation:
  \
  \ Parse _name_, which is a word created by `cvalue`, and
  \ append the execution execution semantics given below to the
  \ current definition.
  \
  \ Execution:
  \
  \ Make _c_ the current value of the character value
  \ _name_.
  \
  \ }doc

  \ vim: filetype=soloforth
