  \ assembler.labels.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220028

  \ -----------------------------------------------------------
  \ Description

  \ Local labels for the Z80 assembler.

  \ -----------------------------------------------------------
  \ Credit

  \ 8086/8 Assembler for hForth
  \
  \ This 8088 Assembler has been rewritten by Sheen Lee for hForth
  \
  \ ............................................
  \ This 8088 Assembler was originally written by Mike Perry and
  \ Steve Pollack.  It has been rewritten by Martin Tracy
  \ and rewritten again by Rick VanNorman (to adapt it to a
  \ 32-bit environment).
  \ Programmers who are familiar with the original F83 assembler
  \ will find the following major differences:
  \
  \ 1. the mode  #) is now simply )
  \ 2. the mode S#) has disappeared.
  \ 3. conditional macros have been replaced by local labels.
  \ 4. REPZ and REPNZ are now REPE and REPNE.
  \ 5. JZ  JNZ  JC  JNC  and more error checks have been added.
  \ 6. the JMP and CALL instructions now have an indirect mode:
  \
  \    MYLABEL  # JMP  means  JMP to this label, but
  \    MYVECTOR ) JMP  means  JMP indirectly through this address.
  \ ............................................

  \ Further modifications by Wonyong Koh
  \
  \ 1996. 11. 29.
  \ Revise ';CODE' for control-flow stack.
  \ 1996. 4. 15.
  \ ';CODE' is fixed. END-CODE is changed.
  \ 1995. 11. 27.
  \ ';CODE' is redefined following the change of 'DOES>' and 'doCREATE'.
  \
  \ o 'MOV', 'JMP', etc are renamed to 'MOV,', 'JMP,', etc. You can
  \   use Standard Forth words 'AND', 'OR', 'XOR' between 'CODE' and
  \   'END-CODE' with no confliction.
  \ o ANS Standard word ';CODE' is added.
  \ o The definition of '1MI' for hForth 8086 ROM Model is better to be
  \ : 1MI RAM/ROM@ ROM CREATE C, RAM/ROM!  DOES> C@ xb, ;
  \   rather than
  \ : 1MI CREATE C,  DOES> C@ xb, ;
  \   However, I did not bother and simply put 'ROM' and 'RAM' in
  \   'ASM8086.F' since '1MI' won't be used in any other places.

  \ -----------------------------------------------------------
  \ History

  \ 2016-11-14: Code extracted from hForth (version 0.9.7,
  \ 1997-06-02). First changes to adapt the source layout to
  \ the style used in Solo Forth.
  \
  \ 2016-12-06: Revise source layout, comments, stack comments.
  \ Divide source into blocks. Rename `xhere` and `codeb!`
  \ after the hForth documentation. Use `?rel`, which is
  \ already in the assemblers, instead of the original code.
  \ Prepare for future development.
  \
  \ 2016-12-26: Review, try, test. Factor. Integrate init into
  \ `asm`. Replace original error checking with `?throw` and
  \ specific exception codes.
  \
  \ 2017-02-21: Need `?rel`. No need to load the whole
  \ assembler. Improve documentation.

  \ -----------------------------------------------------------
  \ To-do

  \ 2016-12-26: Document. Simplify. Save memory: Store label
  \ numbers in bytes.

( l: l# )

get-order get-current

root-wordlist assembler-wordlist forth-wordlist 3 set-order
forth-wordlist set-current

#16 constant max-l#

2 cells  max-l# 2* cells +  constant /fwds
  \ cell structure:
  \   +0    = address of the next free pair
  \   +1    = ? \ XXX TODO --
  \   +2..x = pairs

create fwds ( -- a ) /fwds allot
  \ Unresolved forward reference associative array.

: init-fwds ( -- )
  fwds /fwds erase
  [ fwds 2 cells + ] literal fwds ! ;  init-fwds

max-l# cells constant /bwds

create bwds ( -- a ) /bwds allot
  \ Resolved label value array.

: init-bwds ( -- ) bwds /bwds erase ;  init-bwds

: init-l ( -- ) init-fwds init-bwds ;

' init-l ' init-asm defer!  -->

( l: l# )

: lpush ( a n -- )
  fwds 2@ = #-285 ?throw  \ full?
  fwds @ 2!  [ 2 cells ] literal fwds +! ;
  \ Push unresolved reference to address _a_, label _n_.

: lpop ( n -- a true | n false )
  >r fwds @ fwds cell+ cell+
  begin  2dup = 0= ( end start ) while
    dup @  r@ = if  \ found
      dup cell+ @ ( a ) >r
      swap cell- cell-  dup fwds ! 2@ rot 2!
        \ promote last pair
      r> r> ( a key ) -1 or ( a true ) exit
    then  cell+ cell+
  repeat  2drop r> 0 ;
  \ Pop any unresolved references of label _n_.

: ?l# ( n -- ) [ max-l# 1- ] 1literal u> #-283 ?throw ;

  \ doc{
  \
  \ ?l# ( -- )
  \
  \ If assembler label _n_ is out of range, throw exception
  \ #-283.
  \
  \ See also: `l?`, `l:`.
  \
  \ }doc

  \ If label number _n_ is out of range, throw exception #-283.

: l? ( n -- a | 0 ) dup ?l#  cells bwds + @ ;

  \ doc{
  \
  \ l? ( n -- a | 0 )
  \
  \ If assembler label _n_ is resolved, return its address _a_.
  \ Otherwise return zero.
  \
  \ See also: `?l#`, `l:`, `l#`.
  \
  \ }doc

-->

( l: l# )

assembler-wordlist set-current

need ?rel

: l: ( n -- )
  dup l? #-284 ?throw  \ should be unknown
  here  over cells bwds + ! \ now known
  begin  dup lpop ( a true | n false ) while
    here over - 1-  swap over ?rel c!  \ resolve ref
  repeat  2drop ;

  \ doc{
  \
  \ l: ( n -- )
  \
  \ Assign `here` to assembler label _n_.  Resolve any forward
  \ references.  Assume 8-bit relative displacements.
  \
  \ See also: `l#`, `?rel`.
  \
  \ }doc

: l# ( n -- a )
  dup l? ?dup 0= if  here 1+ 2dup swap lpush  then  nip ;

  \ doc{
  \
  \ l# ( n -- a )
  \
  \ Retrieve the address _a_ of assembler label _n_.
  \
  \ See also: `l:`.
  \
  \ }doc

set-current set-order

  \ vim: filetype=soloforth