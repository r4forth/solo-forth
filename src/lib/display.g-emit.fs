  \ display.g-emit.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803082255
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words to display characters at high resolution coordinates.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( (g-emit g-emit g-type )

unneeding (g-emit ?( need assembler need g-emit_
                      need os-chars need os-coords

code (g-emit ( c -- )
  h pop, l a ld, b push, os-coords b ftp, os-chars d ftp,
  g-emit_ call, b pop, next ix ldp#,  jpnext,
  end-code ?)

  \ doc{
  \
  \ (g-emit ( c -- ) "paren-g-emit"
  \
  \ Display character _c_ (32..127) at the current graphic
  \ coordinates.
  \
  \ The character is printed with overprinting (equivalent to
  \ ``1 overprint``).
  \
  \ See: `g-emit`, `g-emit_`.
  \
  \ }doc

unneeding g-emit ?(

need g-emit-udg need (g-emit need g-emitted

: g-emit ( c -- )
  dup last-font-char c@ > if   g-emit-udg
                          else (g-emit then g-emitted ; ?)

  \ doc{
  \
  \ g-emit ( gx gy c -- )
  \
  \ Display character _c_ (32..255) at the current graphic
  \ coordinates.  If _c_ greater than `last-font-char` from the
  \ UDG font, otherwise it is printed from the main font.
  \
  \ The character is printed with overprinting (equivalent to
  \ ``1 overprint``).
  \
  \ See: `g-emit-udg`, `(g-emit`, `g-type`.
  \
  \ }doc

unneeding g-type  ?( need g-emit

: g-type ( ca len -- )
  bounds ?do  i c@ g-emit  loop ; ?)

  \ doc{
  \
  \ g-type ( ca len -- )
  \
  \ If _len_ is greater than zero, display the character string
  \ _ca len_ at the current graphic coordinates.
  \
  \ See: `g-emit`.
  \
  \ }doc

( g-cr g-emitted g-emit-udg )

unneeding g-cr ?( need g-y need g-at-xy need pixels-scroll-up

: g-cr ( -- )
  0 g-y 8 - dup 7 < if    7 swap - pixels-scroll-up 7
                    then  g-at-xy ; ?)

  \ doc{
  \
  \ g-cr ( -- ) "g-c-r"
  \
  \ Move the graphic coordinates to the next character row.
  \
  \ See: `g-at-xy`, `g-emit`.
  \
  \ }doc

unneeding g-emitted ?( need g-x need g-at-x need g-cr

: g-emitted ( -- ) g-x 8 + dup [ 255 6 - ] cliteral <
                     if  g-at-x exit  then  drop g-cr ; ?)

  \ doc{
  \
  \ g-emitted ( -- )
  \
  \ Update the current graphic coordinates after printing a
  \ character at them.
  \
  \ See `g-emit`, `g-cr`, `g-at-xy`.
  \
  \ }doc

unneeding g-emit-udg ?( need assembler need g-emit_
                           need os-udg need os-coords

code g-emit-udg ( c -- )
  h pop, l a ld, b push,
  os-coords b ftp, os-udg d ftp, g-emit_ call,
  b pop, next ix ldp#, jpnext, end-code ?)

  \ doc{
  \
  \ g-emit-udg ( c -- ) "g-emit-u-d-g"
  \
  \ Display UDG _c_ (0..255) at the current graphic
  \ coordinates, from the font pointed by system variable
  \ `os-udg`, which contains the address of the first UDG
  \ bitmap (0).
  \
  \ The UDG character is printed with overprinting (equivalent
  \ to ``1 overprint``).
  \
  \ See: `g-emit`, `g-emit_`.
  \
  \ }doc

( g-emit_ )

need assembler need gxy>scra_

create g-emit_ ( -- a ) asm

  0 h ld#, a l ld, h addp, h addp, h addp, d addp,
  h push, ix pop, b h ldp, h push, 8 c ld#,

  rbegin

    h pop, h dec, h push, h inc,
      \ next line

    b push, h b ldp, gxy>scra_ call, b pop,
      \ convert the coords H (x) and L (y) to an address in HL
      \ and a bit in A

    a b ld, a xor, b or, 0 ix a ftx,
    nz? rif  exde, 0 h ld#, a l ld, 8 a ld#, b sub, a b ld,
             rbegin  h addp,  rstep  exde,
             m a ld, d xor, a m ld,
             h incp, e a ld, rthen

    m xor, a m ld, ix incp, c dec,
      \ next char scan, one screen line less

  z? runtil  h pop, ret, end-asm

  \ Credit:
  \
  \ Code Adapted from "Smooth Move",
  \ written by Simon N. Goodwin,
  \ published in Todospectrum, issue 2 (1984-10), page 16.
  \ http://microhobby.speccy.cz/zxsf/revistas-ts.htm

  \ doc{
  \
  \ g-emit_ ( -- a ) "g-emit-underscore"
  \
  \ Address of a machine code routine that prints an 8x8 bits
  \ character at graphic coordinates. Used by `g-emit-udg`.
  \
  \ Input registers:
  \
  \ - DE = address of the first char (0) bitmap in a charset
  \ - A = char code (0..255)
  \ - B = y coordinate
  \ - C = x coordinate
  \
  \ Modifies: AF BC HL IX DE
  \
  \ See: `g-emit`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-09-03: First version.
  \
  \ 2016-04-23: Rename "hires-" prefix to "g-". Rename
  \ `g-emit-udg` to `g-emit-0udg`, because zero-index is used,
  \ instead of the default UDG char codes 128..255. Add
  \ `g-emit-udg` for codes 128..255. Improve documentation.
  \ First versions of `g-emitted` and `g-cr`.
  \
  \ 2016-10-11: Add `first-udg`.
  \
  \ 2016-12-20: Rename `jpnext` to `jpnext,` after the change
  \ in the kernel.
  \
  \ 2017-01-02: Convert all assembler words from `z80-asm` to
  \ `z80-asm,`.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-02-01: Compact the code. Make words individually
  \ accessible to `need`.  Finish `g-cr` and `g-emitted`.
  \ Improve documentation. Fix `g-emit-routine` (a bug
  \ introduced during the assembly conversion one month ago).
  \
  \ 2017-02-03: Fix `g-cr` and improve it with scroll.
  \
  \ 2017-02-04: Adapt to 0-index-only UDG, after the changes in
  \ the kernel: Remove `g-emit-udg`; rename `g-emit-0udg` to
  \ `g-emit-udg`. Compact the code, saving one block.
  \
  \ 2017-02-17: Update cross references.  Change markup of
  \ inline code that is not a cross reference.
  \
  \ 2017-03-04: Update naming convention of Z80 routines, after
  \ the changes in the kernel.
  \
  \ 2017-03-13: Update name: `(pixel-addr)` to `gxy>scra_`.
  \
  \ 2017-05-07: Improve documentation.
  \
  \ 2018-01-25: Improve `g-emit` to use `last-font-char`.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-08: Add words' pronunciaton.

  \ vim: filetype=soloforth
