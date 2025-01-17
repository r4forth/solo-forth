  \ data.array.noble.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201807212109
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Toolset for one- and two-dimensional arrays by Julian V.
  \ Noble:

  \ Well, as I am sure everyone knows to the point of hurling
  \ (regurgitating) by now, I chose a format that looks
  \ somewhat like Fortran. I could not use the right
  \ parenthesis, ) , because it was taken as the closure for a
  \ parenthesized comment. Could not use the [ ]'s because they
  \ also have a definite meaning (turn compiler off and on by
  \ switching STATE).  So I was left with what I could do using
  \ curly braces { and }.

  \ Now why did I want an array notation that looks like
  \ Fortran? There were several reasons:

  \ 1. I wanted (at that time) to sell Forth to the Fortran
  \ community.  (Silly me! But I was young then.)

  \ 2. I wanted a notation that unequivocally said "I am an
  \ array!" I had already written a number of linear equations
  \ and other matrix ops programs and kept running into the
  \ problems of

  \  a. different constructors for each data type;
  \
  \  b. reading the program when it was done.

  \ 3. I wanted something that would be completely transparent,
  \ would permit address arithmetic, and would not require tons
  \ of comments to be maintainable.

  \ Eventually I hit on the Forthish solution of defining an
  \ array with a header that contained its size and its data
  \ size. Then I wrote a de-referencing operator that would dig
  \ into the header and calculate the address of the datum
  \ being indexed. The notation for this was

  \ v{ I } ( -- adr of v[I] )

  \ The left curly brace in an array's name was simply
  \ syntactic sugar.  But at some point I realized that by
  \ naming 2-dimensional arrays something like M{{ (2 curly
  \ braces) I could have the name say "I am a 2-dimensional
  \ array." Then one dereferences such by saying

  \ m{{ i j }}

  \ --that is, the 2-dim dereferencing operator expects a base
  \ address and two indices and produces the correct address of
  \ the I,Jth element.

  \ For those who want it, go to my home page (under
  \ construction) then to the link "Computational Methods in
  \ the Physical Sciences", and thence to "Forth system and
  \ example programs". There you can find the file arrays.f
  \ that does all of this stuff. I have bounds checking in that
  \ version because it was intended for student use.  Experts
  \ can delete that stuff.

  \ ...........................................................
  \ Usage examples

  \ 20 2 floats 1array a{
  \   \ complex vector
  \
  \ 20 20 1 floats 2array m{{
  \   \ real matrix
  \
  \ 20 1 cells 1array irow{
  \   \ single-length, integer-valued vector
  \
  \ m{{ i j }} ( -- adr[m_ij] )
  \   \ to dereference

  \ ...........................................................
  \ References

  \ http://forth.sourceforge.net/techniques/
  \ http://forth.sourceforge.net/techniques/arrays-jvn/
  \ http://forth.sourceforge.net/techniques/arrays-jvn/index-v.txt
  \ http://www.phys.virginia.edu/classes/551.jvn.fall01/arrays.f

  \ ===========================================================
  \ Authors

  \ Copyright (C) 2001 Julian V. Noble

  \ Modified for Solo Forth by Marcos Cruz (programandala.net),
  \ 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ License of the original version:

  \ ---------------------------------------------------
  \     (c) Copyright 2001  Julian V. Noble.          \
  \       Permission is granted by the author to      \
  \       use this software for any application pro-  \
  \       vided this copyright notice is preserved.   \
  \ ---------------------------------------------------

  \ License of this version:

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( 1array } 2array }} )

unneeding 1array ?( need } need align

: 1array ( n1 n2 "name" -- )
  create 2dup , , * allot align ; ?)

  \ doc{
  \
  \ 1array ( n1 n2 "name" -- ) "one-array"
  \
  \ Define a 1-dimension array _name_ with _n1_ items of
  \ _n2_ bytes each.
  \
  \ See: `}`, `array>items`, `2array`.
  \
  \ }doc

unneeding } ?( need <=

: array>items ( a -- n ) cell+ @ ;

  \ doc{
  \
  \ array>items ( a -- n ) "array-to-items"
  \
  \ Convert address of array _a_ to its number of items _n_.
  \
  \ See: `1array`.
  \
  \ }doc

: } ( a1 n -- a2 )
  over array>items over <= over 0< or #-272 ?throw
  over @ * + cell+ cell+ ; ?)

  \ XXX TODO -- remove bounds checking

  \ doc{
  \
  \ } ( a1 n -- a2 ) "right-curly-bracket"
  \
  \ If in range, return address _a2_ of the _n_ item of the
  \ 1-cell array _a1_.  Otherwise `throw` an exception #-272
  \ ("array index out of range").
  \
  \ See: `1array`, `array>items`.
  \
  \ }doc

unneeding 2array ?( need } need align

: 2array ( n1 n2 n3 "name" -- )
  create >r tuck , ( n2 n1 ) r@ , * dup , r> * allot align ;

  \ doc{
  \
  \ 2array ( n1 n2 n3 "name" -- ) "two-array"
  \
  \ Define a 2-dimension array _name_ with _n1 x n2_ items of
  \ _n3_ bytes each.
  \
  \ See: `}}`, `1array`.
  \
  \ }doc

: }} ( a1 n1 n2 -- a2 ) 2>r cell+ dup cell- @  r> * r> + } ;

?)

  \ doc{
  \
  \ }} ( a1 n1 n2 -- a2 ) "double-right-curly-bracket"
  \
  \ Return address _a2_ of the _n1,n2_ item of the 2-dimension
  \ array _a1_.  Data  stored row-wise.
  \
  \ See: `2array`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-11-15: Adapted to Solo Forth.
  \
  \ 2016-04-03: Header reorganized after the original
  \ documentation.
  \
  \ 2016-05-07: Make block titles compatible with `indexer`.
  \
  \ 2016-08-05: Remove `long` and `wide` (syntactic sugar).
  \ Combine both blocks into one. Rewrite all stack comments
  \ and word descriptions.
  \
  \ 2016-11-23: Complete documentation. Improve conditional
  \ compilation for `need`.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2018-02-05: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-09: Update notation "address units" to "bytes".
  \
  \ 2018-07-21: Improve documentation, linking `throw`.

  \ vim: filetype=soloforth
