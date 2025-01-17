  \ graphics.cls.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201804121531
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words that clear the screen with special
  \ effects.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( cls-chars0 )

need assembler

code cls-chars0 ( -- )

  b push, 6 b ld#,
  rbegin  b push,  57FF h ldp#,
          rbegin   20 c ld#, a and,
                   rbegin   nop, m sla,
                            nop, m rlc,
                            nop, m sla,
                            nop, h decp, c dec,
                   z? runtil
                   3F a ld#, h cp,
          z? runtil
          b pop, rstep

  b pop, jpnext, end-code

  \ Credit:
  \
  \ Code adapted from a routine written by Anselmo Moreno
  \ Lorente, published on Microhobby, issue 121 (1987-03), page
  \ 7:
  \
  \ http://microhobby.org/numero121.htm
  \ http://microhobby.speccy.cz/mhf/121/MH121_07.jpg

  \ XXX FIXME -- 2017-01-02: it does nothing

  \
  \ cls-chars0 ( -- ) "c-l-s-chars-zero"
  \
  \ Clear the screen by rotating all bytes of the bitmap.
  \
  \ See: `cls-chars1`.
  \

( cls-chars1 )

need assembler

code cls-chars1 ( -- )

  b push, 08 b ld#,

  rbegin

    4000 h ldp#,  \ screen bitmap address
    rbegin
      m srl, h incp,
      m sla, h incp,
      58 a ld#, h cp,
    z? runtil

  rstep

  b pop,  jpnext, end-code

  \ Credit:
  \
  \ Code adapted from a routine written by Antonio Adolfo Sanz,
  \ published on Microhobby, issue 197 (1990-03), page 24:
  \
  \ http://microhobby.org/numero197.htm
  \ http://microhobby.speccy.cz/mhf/197/MH197_24.jpg

  \ doc{
  \
  \ cls-chars1 ( -- ) "c-l-s-chars-one"
  \
  \ Clear the screen by rotating all bytes of the bitmap.
  \
  \ }doc

( horizontal-curtain )

  \ Credit:
  \
  \ Code adapted from a routine written by Alejandro Mora,
  \ published on Microhobby, issue 128 (1987-05), page 7:
  \ http://microhobby.org/numero128.htm
  \ http://microhobby.speccy.cz/mhf/128/MH128_07.jpg

need assembler

code horizontal-curtain ( b -- )

  d pop, b push,

  e a ld, 5800 d ldp#, 5AFF h ldp#,
  0C b ld#,
  rbegin  b push, 20 b ld#,
          rbegin  a m ld, d stap,
                  b push,  02 b ld#,
                  rbegin  b push,  FF b ld#, rbegin  rstep
                          b pop,  rstep
                  b pop, d incp, h decp,  rstep
          b pop,  rstep

  b pop,  jpnext, end-code


  \ doc{
  \
  \ horizontal-curtain ( b -- )
  \
  \ Wash the screen with the given color attribute _b_ from the
  \ top and bottom rows to the middle.
  \
  \ See: `vertical-curtain`.
  \
  \ }doc

( vertical-curtain )

  \ Credit:
  \
  \ Code adapted from a routine written by Alejandro Mora,
  \ published on Microhobby, issue 128 (1987-05), page 7:
  \ http://microhobby.org/numero128.htm
  \ http://microhobby.speccy.cz/mhf/128/MH128_07.jpg

need assembler

code vertical-curtain ( b -- )

  h pop, h push,

  e a ld, 5800 h ldp#, 5AFF h ldp#, 10 b ld#,
  rbegin  h push, 18 b ld#, h push, h push,
          rbegin  a m ld, h stap, h push, 02 b ld#,
                  rbegin  h push, FF b ld#,  rbegin  rstep
                          h pop,  rstep
                  20 b ld#,
                  rbegin  h incp, h decp,  rstep
                  h pop,  rstep

          h pop, h pop, h pop, h incp, h decp,  rstep

  h pop, jpnext, end-code

  \ doc{
  \
  \ vertical-curtain ( b -- )
  \
  \ Wash the screen with the given color attribute _b_ from the
  \ left and right columns to the middle.
  \
  \ See: `horizontal-curtain`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-12-20: Rename `jpnext` to `jpnext,` after the change
  \ in the kernel.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2018-02-15: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-04-12: Deactivate documentation of `cls-chars0`.

  \ vim: filetype=soloforth
