  \ data.array.bit.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201802051709
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words to create and manage bit arrays.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ Credit

  \ Based on:
  \
  \ Bit manipulations in Forth
  \ http://www.forth.org/svfig/Len/bits.htm
  \ First Presented at North Bay Forth Interest Group,
  \ 1989-09-09.  Updated 1996-09-18.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( bitmasks bits>bytes bit-array !bit @bit )

need align need alias need cset need creset

create bitmasks 128 c, 64 c, 32 c, 16 c, 8 c, 4 c, 2 c, 1 c,

  \ doc{
  \
  \ bitmasks ( -- ca )
  \
  \ Address of an 8-byte table containing the bitmasks for bits
  \ 0..7 as used by `bit-array`.
  \
  \ }doc

: bits>bytes ( n1 -- n2 ) 8 /mod swap 0<> - ;

  \ doc{
  \
  \ bits>bytes ( n1 -- n2 ) "bits-to-bytes"
  \
  \ Return the number of bytes _n2_ needed to hold _n1_ bits.
  \ Used by `bit-array`.
  \
  \ }doc

: bit-array ( n "name" -- )
  create bits>bytes allot align
  does> ( n -- b ca )
    ( n dfa ) swap 8 /mod >r bitmasks + c@ swap r> + ;

  \ doc{
  \
  \ bit-array ( n "name" -- )
  \
  \ Create a bit-array _name_ to hold _n_ bits, with the
  \ execution semantics defined below. The bits are stored in
  \ order: array bit 0 is bit 7 of the first byte of the array;
  \ array bit 7 is bit 0 of the first byte of the array; array
  \ bit 8 is bit 7 of the second byte of the array; array bit
  \ 15 is bit 0 of the second byte of the array, etc.
  \
  \ name ( n -- b ca )
  \
  \ Return bitmak _b_ and address _ca_ of bit _n_ of the array.
  \
  \ See: `@bit`, `!bit`, `bits>bytes`, `bitmasks`.
  \
  \ }doc

: !bit ( f b ca -- ) rot if  cset exit  then  creset ;

  \ doc{
  \
  \ !bit ( f b ca -- ) "store-bit"
  \
  \ Store flag _f_ in an element of a bit-array, represented by
  \ address _ca_ and bitmask _b_.
  \
  \ See: `@bit`, `bit-array`.
  \
  \ }doc

' c@and? alias @bit ( b ca -- f )

  \ doc{
  \
  \ @bit ( b ca -- f ) "fetch-bit"
  \
  \ Fetch _f_ from an element of a bit-array, represented by
  \ address _ca_ and bitmask _b_.
  \
  \ ``@bit`` is an alias of `c@and?`.
  \
  \ See: `!bit`, `bit-array`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-11-23: Start. Adapt, fix and improve the original code
  \ (http://www.forth.org/svfig/Len/bits.htm).
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".
  \
  \ 2017-12-09: Improve documentation.
  \
  \ 2018-02-05: Improve documentation: add pronunciation to
  \ words that need it.

  \ vim: filetype=soloforth
