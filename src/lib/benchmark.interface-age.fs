  \ benchmark.interface-age.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702221226

  \ ===========================================================
  \ Description

  \ The Interface-Age benchmark.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( interface-age-benchmark )

  \ Credit:
  \
  \ Code adapted from: Forth Dimensions (volume 17, number 4,
  \ page 11, 1995-11).

  \ Interface Age Benchmark, 1985-11-16.  This is the Interface
  \ Age benchmark program described in Appendix D of the
  \ forthCMP Manual.

  \ 2015-12-24. Modified: no printing.

need bench{ need 2/

: (interface-age-benchmark) ( n -- )
  dup 2/ 1+ swap cr
  1 ?do
    dup i 1 rot 2 do
      drop dup 0 i um/mod dup
      0=  if  drop drop 1 leave  then
      1 = if    drop 1
          else  dup 0= if  drop 0 leave  then
                0< 0= if  1  then
          then
    loop
    \ if  .  else  drop  then  \ XXX OLD
    2drop  \ XXX NEW
  loop  drop ; -->

( interface-age-benchmark )

: interface-age-benchmark ( n -- )
  bench{ (interface-age-benchmark) }bench. ;

  cr
  \  <------------------------------>
  .( To run the interface age) cr
  .( benchmark type:) cr
  .(   n interface-age-benchmark  ) cr
  .( where _n_ is the number of) cr
  .( iterations. The original code) cr
  .( used 5000 iterations.) cr

  \ 2015-12-24
  \
  \ Times Frames (1 frame = 50th of second)
  \ ----- -----------------------------------
  \       ITC           DTC
  \       ------------  ------------
  \ 05000 80091 (1.00)  72445 (0.90)

  \ 2016-03-16 XXX TODO --
  \
  \ Times Frames (1 frame = 50th of second)
  \ ----- --------------------------------------
  \       jp pushhl        push hl + jp (ix) [1]
  \       ------------     ---------------------
  \ 05000 72445 (1.00)     71914 (0.99)
  \
  \ [1] Changed only in the kernel.

( Interface Age Benchmark program )

  \ XXX TODO -- test

  \ Forth Dimensions (volume 2, number 4, page 112)

: bench ( -- )
  dup 2 / 1+ swap ." Starting " cr
  1 do dup i 1 rot
    2 do drop dup i /mod
      dup 0= if  drop drop 1 leave
      else  1 = if drop 1
            else  dup 0 > if  drop 1
                  else  0= if  0 leave  then
                  then
            then
      then
    loop
    if  4 .r  else  drop  then
  loop  drop cr ." Finished " ;

  \ vim: filetype=soloforth
