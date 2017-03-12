  \ math.calculator.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT

  \ Last modified: 201702220020
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ ROM calculator support.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ ===========================================================
  \ To-do

  \ XXX TODO -- Move the stack and make it configurable. The
  \ default location is limited by the small free memory left
  \ to BASIC.
  \
  \ XXX FIXME -- When the calculator stack is out of bounds,
  \ the calculator could issue a BASIC error and crash the
  \ system. Test it.
  \
  \ XXX TODO -- Add more control structures.
  \
  \ XXX TODO -- Test everything.
  \
  \ XXX TODO -- Document.
  \

( calculator )

need alias

wordlist constant calculator-wordlist

: calculator ( -- )
  calculator-wordlist >order  $C5 c, $EF c, ;
  \ Add `calculator-wordlist` to the search order and
  \ compile the assembler instructions to start the ROM
  \ calculator:
  \ ----
  \ push bc ; save the Forth IP
  \ rst $28 ; call the ROM calculator
  \ ----

calculator-wordlist >order
get-current  calculator-wordlist set-current

: end-calc ( -- ) $38 c, ;
  \ Compile the `end-calc` ROM calculator command.
  \ ----
  \ db $38 ; exit the ROM calculator
  \ ----

: end-calculator ( -- ) previous end-calc $C1 c, ;
  \ Restore the search order and
  \ compile the assembler instructions to exit the ROM calculator:
  \ ----
  \ db $38 ; `end-calc` ROM calculator command
  \ pop bc ; restore the Forth IP
  \ ----

-->

( calculator )

: + ( -- ) $0F c, ;
  \ Compile the `addition` ROM calculator command.
: - ( -- ) $03 c, ;
  \ Compile the `subtract` ROM calculator command.

: * ( -- ) $04 c, ;
  \ Compile the `multiply` ROM calculator command.
: / ( -- ) $05 c, ;
  \ Compile the `division` ROM calculator command.
: mod ( -- ) $32 c, ;
  \ Compile the `n-mod-m` ROM calculator command.

: ** ( -- ) $06 c, ;
  \ Compile the `to-power` ROM calculator command.
: sqrt ( -- ) $28 c, ;
  \ Compile the `sqr` ROM calculator command.

-->

( calculator )

: negate ( -- ) $1B c, ;
  \ Compile the `negate` ROM calculator command.
: sgn ( -- ) $29 c, ;
  \ Compile the `sgn` ROM calculator command.
: abs ( -- ) $2A c, ;
  \ Compile the `abs` ROM calculator command.

: int ( -- ) $27 c, ;
  \ Compile the `int` ROM calculator command.
: truncate ( -- ) $3A c, ;
  \ Compile the `truncate` ROM calculator command.

: re-stack ( r -- r' ) $3D c, ;
  \ Compile the `re-stack` ROM calculator command.

: zero ( -- ) $A0 c, ;
  \ Compile the ROM calculator command that stacks 0.
: one ( -- ) $A1 c, ;
  \ Compile the ROM calculator command that stacks 1.
: half ( -- ) $A2 c, ;
  \ Compile the ROM calculator command that stacks 1/2.
: pi2/ ( -- ) $A3 c, ;
  \ Compile the ROM calculator command that stacks pi/2.
: ten ( -- ) $A4 c, ;
  \ Compile the ROM calculator command that stacks 10.

-->

( calculator )

: ln ( -- ) $25 c, ;
  \ Compile the `ln` ROM calculator command.
: exp ( -- ) $26 c, ;
  \ Compile the `exp` ROM calculator command.

: acos ( -- ) $23 c, ;
  \ Compile the `acos` ROM calculator command.
: asin ( -- ) $22 c, ;
  \ Compile the `asin` ROM calculator command.
: atan ( -- ) $24 c, ;
  \ Compile the `atan` ROM calculator command.
: cos ( -- ) $20 c, ;
  \ Compile the `cos` ROM calculator command.
: sin ( -- ) $1F c, ;
  \ Compile the `sin` ROM calculator command.
: tan ( -- ) $21 c, ;
  \ Compile the `tan` ROM calculator command.

-->

( calculator )

: drop ( -- ) $02 c, ;
  \ Compile the `delete` ROM calculator command.

: dup ( -- ) $31 c, ;
  \ Compile the `duplicate` ROM calculator command.

: swap ( -- ) $01 c, ;
  \ Compile the `exchange` ROM calculator command.

: >mem ( n -- ) $C0 [ also forth ] + [ previous ] c, ;
  \ Compile the `st-mem` ROM calculator command for memory
  \ number _n_ (0..5). Note: The floating-point stack TOS is
  \ copied, not moved.

: mem> ( n -- ) $E0 [ also forth ] + [ previous ] c, ;
  \ Compile the `get-mem` ROM calculator command for memory
  \ number _n_ (0..5).

: over ( -- )
  2 >mem drop 1 >mem 2 mem> 1 mem> ;
  \ Compile the ROM calculator commands to do `over`.

: 2dup ( -- )
  2 >mem drop 1 >mem drop 1 mem> 2 mem>  1 mem> 2 mem> ;
  \ Compile the ROM calculator commands to do `2dup`.

-->

( calculator )

: 0= ( -- ) $30 c, ;
  \ Compile the `not` ROM calculator command.

: 0< ( -- ) $36 c, ;
  \ Compile the `less-0` ROM calculator command.

: 0> ( -- ) $37 c, ;
  \ Compile the `greater-0` ROM calculator command.

-->

( calculator )

-->  \ XXX TMP -- ignore this block

  \ XXX FIXME -- These commands always return true.
  \
  \ 2016-04-20:
  \
  \ After some research, it seems the reason is the numbers are
  \ compared as strings.  Some commands of the ROM calculator
  \ are used to compare numbers and strings, and the routine
  \ checks the parameters before doing the comparison.
  \
  \ Somehow the ROM routine at $353B gets confused because the
  \ command is not restored from $5C67 (the BREG system
  \ variable).
  \
  \ I examined the source of the ROM calculator and followed
  \ its execution using the debugger of the Fuse emulator, in
  \ BASIC and Forth. So far I got the following clues:
  \
  \ $335E: the command in B is saved to $5C67. This is at the
  \ start of the calculator, so it doesn't makes sense the
  \ first time, because the command is not in B. In Forth, B
  \ contains the high part of the IP ($78 at the time of
  \ writing). But this address is a re-entry point, forced by
  \ the calculator by manipulating the Z80 stack.
  \
  \ $336C: the command is in A, ok.
  \
  \ $338C: the command is modified for indexing, ok.
  \
  \ $339D: the command should be restored by `ld bc,($5C66)`,
  \ which is the low part of STKEND and the high part of BREG.
  \ The register B should contain the command, but not right
  \ after the first entry into the calculator.
  \
  \ $33A1: BREG is in B, which in Forth is $78, bad.
  \
  \ $33A1: Restore the ROM calculator literal: `ld a,($5C67)`.
  \ This is not executed by Forth's `f=`, but it is when the
  \ BASIC command `print 1=1` is interpreted.
  \
  \ $353B: B contains $78, not the command. The routine does a
  \ string comparison. But in BASIC, at this point register B
  \ contains the command.

: = ( -- ) $0E c, ;
  \ Compile the `nos-eql` ROM calculator command.

: <> ( -- ) $0B c, ;
  \ Compile the `nos-neql` ROM calculator command.

: > ( -- ) $0C c, ;
  \ Compile the `no-grtr` ROM calculator command.

: < ( -- ) $0D c, ;
  \ Compile the `no-less` ROM calculator command.

: <= ( -- ) $09 c, ;
  \ Compile the `no-l-eql` ROM calculator command.

: >= ( -- ) $0A c, ;
  \ Compile the `no-gr-eql` ROM calculator command.

-->

( calculator )

: ?branch ( -- ) $00 c, ;
  \ Compile the `jump-true` ROM calculator command.

: 0branch ( -- ) 0= ?branch ;
  \ Compile the ROM calculator commands to do a branch if
  \ the TOS of the calculator stack is zero.

: branch ( -- ) $33 c, ;
  \ Compile the `jump` ROM calculator command.

-->

( calculator )

: >mark ( -- a ) here 0 c, ;

  \ Compile space for the displacement of a ROM calculator
  \ forward branch which will later be resolved by
  \ `>resolve`.
  \
  \ Typically used after either `branch` or
  \ `?branch`.

: from-here ( a -- n )
  here [ also forth ] swap - [ previous ] ;
  \ Calculate the displacement _n_ from the current data-space
  \ pointer to address _a_.

: >resolve ( a -- )
  [ also forth ] dup [ previous ] from-here
  [ also forth ] swap [ previous ] c! ;

  \ Resolve a ROM calculator forward branch by placing the
  \ displacement to the current position into the space
  \ compiled by `>mark`.

' here alias <mark ( -- a )

  \ Leave the address of the current data-space pointer as the
  \ destination of a ROM calculator backward branch which will
  \ later be resolved by `<resolve`.
  \
  \ Typically used before either `branch` or `?branch`.

: <resolve ( a -- ) from-here c, ;

  \ Resolve a ROM calculator backward branch by compiling the
  \ displacement from the current position to address _a_,
  \ which was left by `<mark`.

: if ( -- a ) 0branch >mark ;

: else ( a1 -- a2 )
  branch >mark [ also forth ] swap [ previous ] >resolve ;

' >resolve alias then ( a -- )

set-current  previous
  \ restore the current word list and the search order

  \ ===========================================================
  \ Change log

  \ 2015-09-23: Start. Main development, as part of the
  \ floating-point module.
  \
  \ 2016-04-11: Revision. Code reorganized. Improvements.
  \
  \ 2016-04-13: Fixes and improvements. First usable version.
  \
  \ 2016-04-18: Extracted the code from the floating-point
  \ module, in order to reuse it. Much improved. Added `if then
  \ else`. Added `int`.
  \
  \ 2016-04-20: Improved `2dup`. Commented out `=`, `<>`, `<`,
  \ `<=`, `>` and `>=`, which can not be used yet.
  \
  \ 2016-10-28: Fix block title that caused `>=` and other
  \ calculator operators be found by `need` instead of the
  \ integer ones, because this module comes before the integer
  \ operators in the library disk.

  \ vim: filetype=soloforth
