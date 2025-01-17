  \ graphics.scroll.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803052149
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words to scroll and pan the screen.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( (pixel-pan-right pixel-pan-right pixels-pan-right )

unneeding (pixel-pan-right ?( need assembler

create (pixel-pan-right ( -- a ) asm
  4000 h ldp#, C0 c ld#,
    \ HL = screen bitmap address
    \ C = pixel rows
  rbegin  m srl, h incp, 1F b ld#,
            \ Rotate the first byte and point HL to the next
            \ B = remaining char columns (=bytes)
          rbegin  m rr, h incp,  rstep c dec,
            \ Rotate the remaining bytes of the pixel row
  z? runtil ret, end-asm ?)

  \ Credit:
  \
  \ Code adapted from a routine written by Antonio Adolfo Sanz,
  \ published on Microhobby, issue 197 (1990-03), page 24:
  \ http://microhobby.org/numero197.htm
  \ http://microhobby.speccy.cz/mhf/197/MH197_24.jpg

  \ doc{
  \
  \ (pixel-pan-right ( -- a ) "paren-pixel-pan-right"
  \
  \ Return the address _a_ of a Z80 routine that pans the whole
  \ screen one pixel to the right.
  \
  \ Note: The BC register (the Forth IP) is not preserved.
  \ This is intended, in order to save time when this routine
  \ is called in a loop. Therefore the calling code must save
  \ the BC register.
  \
  \ See: `pixel-pan-right`, `pixel-scroll-up`.
  \
  \ }doc

unneeding pixel-pan-right ?(

need (pixel-pan-right need assembler

code pixel-pan-right ( -- )
  b push, (pixel-pan-right call, b pop, jpnext, end-code ?)

  \ doc{
  \
  \ pixel-pan-right ( -- )
  \
  \ Pan the whole screen one pixel to the right.
  \ ``pixel-pan-right`` is a wrapper that calls
  \ `(pixel-pan-right` saving the BC register.
  \
  \ See `(pixel-pan-right`, `pixels-pan-right`,
  \ `pixel-scroll-up`.
  \
  \ }doc

unneeding pixels-pan-right ?( need pixel-pan-right

: pixels-pan-right ( n -- ) 0 ?do  pixel-pan-right  loop ; ?)

  \ doc{
  \
  \ pixels-pan-right ( u -- )
  \
  \ Pan the whole screen _u_ pixels to the right.
  \
  \ See `pixel-pan-right`, `pixels-scroll-up`.
  \
  \ }doc

( (pixel-scroll-up pixel-scroll-up pixels-scroll-up )

unneeding (pixel-scroll-up ?( need assembler

create (pixel-scroll-up ( -- a ) asm
  4000 h ldp#, BF b ld#, rbegin
    b push, h d ldp, h inc, h a ld, F8 and#, h cp,
    z? rif  08 b ld#, b sub,             rra, rra, rra, a h ld,
            0020 b ldp#, b addp, h a ld, rla, rla, rla, a h ld,
    rthen   h push, 0020 b ldp#, ldir, h pop, b pop,
  rstep b m ld, 57E1 d ldp#, 0020 b ldp#, ldir, ret, end-asm ?)

  \ Credit:
  \
  \ Code adapted and improved from a routine written by Iv�n
  \ Sansa, published on Microhobby, issue 122 (1987-03), page
  \ 7: http://microhobby.org/numero122.htm
  \ http://microhobby.speccy.cz/mhf/122/MH122_07.jpg

  \ doc{
  \
  \ (pixel-scroll-up ( -- a ) "paren-pixel-scroll-up"
  \
  \ Return the address _a_ of a Z80 routine that scrolls the
  \ whole screen one pixel up.
  \
  \ Note: The BC register (the Forth IP) is not preserved.
  \ This is intended, in order to save time when this routine
  \ is called in a loop. Therefore the calling code must save
  \ the BC register.
  \
  \ See: `pixel-scroll-up`.
  \
  \ }doc

unneeding pixel-scroll-up ?(

need assembler need (pixel-scroll-up

code pixel-scroll-up ( -- )
  b push, (pixel-scroll-up call, b pop, jpnext, end-code ?)

  \ doc{
  \
  \ pixel-scroll-up ( -- )
  \
  \ Scroll the whole screen one pixel up. ``pixel-scroll-up``
  \ is a wrapper that calls `(pixel-scroll-up` saving the BC
  \ register.
  \
  \ See: `pixel-pan-right`, `pixels-scroll-up`.
  \
  \ }doc

unneeding pixels-scroll-up ?( need pixel-scroll-up

: pixels-scroll-up ( n -- )
  0 ?do  pixel-scroll-up  loop ; ?)

  \ doc{
  \
  \ pixels-scroll-up ( u -- )
  \
  \ Scroll the whole screen _u_ pixels up.
  \
  \ See: `pixel-scroll-up`, `pixels-pan-right`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-12-20: Rename `jpnext` to `jpnext,` after the change
  \ in the kernel.
  \
  \ 2017-01-04: Convert `scroll-1px-up` and `(scroll-1px-up)`
  \ from `z80-asm` to `z80-asm,`. Improve documentation. Fix:
  \ add missing `ret,` to `(scroll-1px-up)`.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-02-02: Make words individually accessible to `need`.
  \ Improve documentation. Add `scroll-#px-up`.
  \
  \ 2017-02-03: Rename `scroll-1px-right` to `pixel-pan-right`;
  \ `scroll-1px-up` to `pixel-scroll-up`; `scroll-#px-up` to
  \ `pixels-scroll-up`. Add `pixels-pan-right`. Factor
  \ `(pixel-pan-right` from `pixel-pan-right`. Improve
  \ documentation. Fix `(pixel-scroll-up` to erase the last
  \ pixel row of the screen.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2018-02-17: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.

  \ vim: filetype=soloforth
