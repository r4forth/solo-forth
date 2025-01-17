" soloforth.vim
" Vim syntax file
" Language: Solo Forth (for ZX Spectrum 128 and compatible)
" Author:   Marcos Cruz (programandala.net)
" License:  Vim license (GPL compatible)
" URL:      http://programandala.net/en.program.solo_forth.html
" Updated:  2018-03-28
"
" See change log at the end of the file.

" This file is part of Solo Forth
" http://programandala.net/en.program.solo_forth.html

" --------------------------------------------------------------
" Usage

" Possible locations of this file:

" ~/.vim/syntax/soloforth.vim
" /usr/share/vim/vimcurrent/syntax/soloforth.vim

" --------------------------------------------------------------

" For version 5.x: Clear all syntax items
" For version 6.x: Quit when a syntax file was already loaded
if version < 600
    syntax clear
elseif exists("b:current_syntax")
    finish
endif

" Synchronization method
syn sync ccomment
syn sync maxlines=200

" F+D Forth is case insensitive:
syn case ignore

" Some special, non-Forth keywords
syn keyword soloforthTodo contained TODO
syn keyword soloforthTodo contained FIXME
syn keyword soloforthTodo contained XXX
syn match soloforthTodo contained 'Copyright\(\s([Cc])\)\=\(\s[0-9]\{2,4}\)\='

" Characters allowed in keywords
if version >= 600
    setlocal iskeyword=33-255
else
    set iskeyword=33-255
endif

" Keywords

" XXX TODO
" syn keyword soloforthGraphics emmited
" syn keyword soloforthGraphics emmited-charset
" syn keyword soloforthGraphics draw
" syn keyword soloforthGraphics emmited-chars
" syn keyword soloforthHardware bleep
" syn keyword soloforthHardware link

" XXX OLD
"syn keyword soloforthFlow abort"

syn keyword soloforthCharacterInput #tib
syn keyword soloforthCharacterInput accept
syn keyword soloforthCharacterInput break-key?
syn keyword soloforthCharacterInput expect
syn keyword soloforthCharacterInput key
syn keyword soloforthCharacterInput key?
syn keyword soloforthCharacterInput query
syn keyword soloforthCharacterInput span
syn keyword soloforthCharacterInput tib
syn keyword soloforthCharacterOutput .
syn keyword soloforthCharacterOutput .r
syn keyword soloforthCharacterOutput 0.r
syn keyword soloforthCharacterOutput 0d.r
syn keyword soloforthCharacterOutput bl
syn keyword soloforthCharacterOutput cr
syn keyword soloforthCharacterOutput d.
syn keyword soloforthCharacterOutput d.r
syn keyword soloforthCharacterOutput emit
syn keyword soloforthCharacterOutput emits
syn keyword soloforthCharacterOutput fartype
syn keyword soloforthCharacterOutput home
syn keyword soloforthCharacterOutput printing
syn keyword soloforthCharacterOutput space
syn keyword soloforthCharacterOutput spaces
syn keyword soloforthCharacterOutput type
syn keyword soloforthCharacterOutput u.
syn keyword soloforthCharacterOutput u.r
syn keyword soloforthConversion #
syn keyword soloforthConversion #>
syn keyword soloforthConversion #s
syn keyword soloforthConversion /hold
syn keyword soloforthConversion <#
syn keyword soloforthConversion >number
syn keyword soloforthConversion digit?
syn keyword soloforthConversion dpl
syn keyword soloforthConversion hld
syn keyword soloforthConversion hold
syn keyword soloforthConversion number?
syn keyword soloforthConversion sign
syn keyword soloforthDefine '
syn keyword soloforthDefine (user)
syn keyword soloforthDefine +field
syn keyword soloforthDefine ,
syn keyword soloforthDefine /user
syn keyword soloforthDefine 2constant
syn keyword soloforthDefine 2field:
syn keyword soloforthDefine 2lit
syn keyword soloforthDefine 2literal
syn keyword soloforthDefine 2to
syn keyword soloforthDefine 2value
syn keyword soloforthDefine 2variable
syn keyword soloforthDefine :noname
syn keyword soloforthDefine ;
syn keyword soloforthDefine ?defined
syn keyword soloforthDefine ]
syn keyword soloforthDefine ]2l
syn keyword soloforthDefine ]cl
syn keyword soloforthDefine ]l
syn keyword soloforthDefine ]xl
syn keyword soloforthDefine alias
syn keyword soloforthDefine begin-structure
syn keyword soloforthDefine cconstant
syn keyword soloforthDefine cfield:
syn keyword soloforthDefine clit
syn keyword soloforthDefine cliteral
syn keyword soloforthDefine code-field,
syn keyword soloforthDefine compile
syn keyword soloforthDefine compile,
syn keyword soloforthDefine compile-only
syn keyword soloforthDefine compile-only-mask
syn keyword soloforthDefine compile-only?
syn keyword soloforthDefine constant
syn keyword soloforthDefine create
syn keyword soloforthDefine cvariable
syn keyword soloforthDefine defer
syn keyword soloforthDefine defer!
syn keyword soloforthDefine defer@
syn keyword soloforthDefine defined
syn keyword soloforthDefine dliteral
syn keyword soloforthDefine docolon
syn keyword soloforthDefine does>
syn keyword soloforthDefine end-structure
syn keyword soloforthDefine field:
syn keyword soloforthDefine finish-code
syn keyword soloforthDefine header
syn keyword soloforthDefine header,
syn keyword soloforthDefine hidden
syn keyword soloforthDefine hide
syn keyword soloforthDefine immediate
syn keyword soloforthDefine immediate-mask
syn keyword soloforthDefine immediate?
syn keyword soloforthDefine input-stream-header
syn keyword soloforthDefine interpret
syn keyword soloforthDefine lit
syn keyword soloforthDefine literal
syn keyword soloforthDefine noname?
syn keyword soloforthDefine parse
syn keyword soloforthDefine parse-name
syn keyword soloforthDefine parse-string
syn keyword soloforthDefine parsed-name
syn keyword soloforthDefine postpone
syn keyword soloforthDefine reveal
syn keyword soloforthDefine revealed
syn keyword soloforthDefine slit
syn keyword soloforthDefine sliteral
syn keyword soloforthDefine smudge
syn keyword soloforthDefine smudge-mask
syn keyword soloforthDefine smudged
syn keyword soloforthDefine synonym
syn keyword soloforthDefine to
syn keyword soloforthDefine udp
syn keyword soloforthDefine undefined?
syn keyword soloforthDefine up
syn keyword soloforthDefine up0
syn keyword soloforthDefine value
syn keyword soloforthDefine variable
syn keyword soloforthDefine word
syn keyword soloforthDefine xliteral
syn keyword soloforthFiles (load)
syn keyword soloforthFiles +load
syn keyword soloforthFiles +thru
syn keyword soloforthFiles -->
syn keyword soloforthFiles .line
syn keyword soloforthFiles ?(
syn keyword soloforthFiles ?)
syn keyword soloforthFiles ?-->
syn keyword soloforthFiles ?\
syn keyword soloforthFiles b/buf
syn keyword soloforthFiles b/sector
syn keyword soloforthFiles blk
syn keyword soloforthFiles block
syn keyword soloforthFiles block-number
syn keyword soloforthFiles block>source
syn keyword soloforthFiles blocks
syn keyword soloforthFiles blocks/disk
syn keyword soloforthFiles buffer
syn keyword soloforthFiles buffer-block
syn keyword soloforthFiles buffer-data
syn keyword soloforthFiles buffer-id
syn keyword soloforthFiles c/l
syn keyword soloforthFiles disk-buffer
syn keyword soloforthFiles editor
syn keyword soloforthFiles empty-buffers
syn keyword soloforthFiles file-status
syn keyword soloforthFiles flush
syn keyword soloforthFiles free-buffer
syn keyword soloforthFiles from
syn keyword soloforthFiles include
syn keyword soloforthFiles included
syn keyword soloforthFiles index
syn keyword soloforthFiles l/scr
syn keyword soloforthFiles lastblk
syn keyword soloforthFiles list
syn keyword soloforthFiles load
syn keyword soloforthFiles locate
syn keyword soloforthFiles located
syn keyword soloforthFiles need
syn keyword soloforthFiles needed
syn keyword soloforthFiles read-block
syn keyword soloforthFiles read-mode
syn keyword soloforthFiles reload
syn keyword soloforthFiles reneed
syn keyword soloforthFiles reneeded
syn keyword soloforthFiles save-buffers
syn keyword soloforthFiles scr
syn keyword soloforthFiles sectors/block
syn keyword soloforthFiles thru
syn keyword soloforthFiles transfer-block
syn keyword soloforthFiles transfer-mode
syn keyword soloforthFiles transfer-sector
syn keyword soloforthFiles update
syn keyword soloforthFiles updated?
syn keyword soloforthFiles where
syn keyword soloforthFiles rename-file
syn keyword soloforthFiles delete-file
syn keyword soloforthFiles write-block
syn keyword soloforthFiles write-mode
syn keyword soloforthFlow +loop
syn keyword soloforthFlow -exit
syn keyword soloforthFlow -if
syn keyword soloforthFlow -until
syn keyword soloforthFlow -while
syn keyword soloforthFlow 0branch
syn keyword soloforthFlow 0leave
syn keyword soloforthFlow ?branch
syn keyword soloforthFlow ?do
syn keyword soloforthFlow ?exit
syn keyword soloforthFlow ?leave
syn keyword soloforthFlow ?throw
syn keyword soloforthFlow abort
syn keyword soloforthFlow again
syn keyword soloforthFlow ahead
syn keyword soloforthFlow begin
syn keyword soloforthFlow branch
syn keyword soloforthFlow case
syn keyword soloforthFlow catch
syn keyword soloforthFlow catcher
syn keyword soloforthFlow cold
syn keyword soloforthFlow do
syn keyword soloforthFlow don't
syn keyword soloforthFlow else
syn keyword soloforthFlow endcase
syn keyword soloforthFlow endof
syn keyword soloforthFlow error
syn keyword soloforthFlow evaluate
syn keyword soloforthFlow execute
syn keyword soloforthFlow exit
syn keyword soloforthFlow for
syn keyword soloforthFlow i
syn keyword soloforthFlow i'
syn keyword soloforthFlow if
syn keyword soloforthFlow j
syn keyword soloforthFlow j'
syn keyword soloforthFlow k
syn keyword soloforthFlow k'
syn keyword soloforthFlow leave
syn keyword soloforthFlow loop
syn keyword soloforthFlow mon
syn keyword soloforthFlow of
syn keyword soloforthFlow perform
syn keyword soloforthFlow quit
syn keyword soloforthFlow recurse
syn keyword soloforthFlow repeat
syn keyword soloforthFlow step
syn keyword soloforthFlow then
syn keyword soloforthFlow throw
syn keyword soloforthFlow unless
syn keyword soloforthFlow unloop
syn keyword soloforthFlow unnest
syn keyword soloforthFlow until
syn keyword soloforthFlow warm
syn keyword soloforthFlow while
syn keyword soloforthForth !csp
syn keyword soloforthForth (+loop)
syn keyword soloforthForth (.")
syn keyword soloforthForth (;code)
syn keyword soloforthForth (?do)
syn keyword soloforthForth (abort)
syn keyword soloforthForth (do)
syn keyword soloforthForth (find)
syn keyword soloforthForth (loop)
syn keyword soloforthForth (number)
syn keyword soloforthForth +origin
syn keyword soloforthForth .error-word
syn keyword soloforthForth ;code
syn keyword soloforthForth <mark
syn keyword soloforthForth <resolve
syn keyword soloforthForth >>link
syn keyword soloforthForth >>name
syn keyword soloforthForth >body
syn keyword soloforthForth >code
syn keyword soloforthForth >in
syn keyword soloforthForth >mark
syn keyword soloforthForth >name
syn keyword soloforthForth >resolve
syn keyword soloforthForth ?compiling
syn keyword soloforthForth ?csp
syn keyword soloforthForth ?executing
syn keyword soloforthForth ?loading
syn keyword soloforthForth ?pairs
syn keyword soloforthForth ?stack
syn keyword soloforthForth abort-message
syn keyword soloforthForth body>
syn keyword soloforthForth body>name
syn keyword soloforthForth boot
syn keyword soloforthForth compiling?
syn keyword soloforthForth csp
syn keyword soloforthForth enclose
syn keyword soloforthForth error#
syn keyword soloforthForth error-pos
syn keyword soloforthForth executing?
syn keyword soloforthForth farunused
syn keyword soloforthForth line>string
syn keyword soloforthForth link>name
syn keyword soloforthForth name<name
syn keyword soloforthForth name>
syn keyword soloforthForth name>>
syn keyword soloforthForth name>body
syn keyword soloforthForth name>immediate?
syn keyword soloforthForth name>link
syn keyword soloforthForth name>name
syn keyword soloforthForth name>str
syn keyword soloforthForth name>string
syn keyword soloforthForth nest-source
syn keyword soloforthForth nextname
syn keyword soloforthForth noop
syn keyword soloforthForth noop_
syn keyword soloforthForth pad
syn keyword soloforthForth refill
syn keyword soloforthForth source
syn keyword soloforthForth source-id
syn keyword soloforthForth state
syn keyword soloforthForth stream
syn keyword soloforthForth unnest-source
syn keyword soloforthForth unused
syn keyword soloforthForth warnings
syn keyword soloforthForth width
syn keyword soloforthForth word-length-mask
syn keyword soloforthFunction false
syn keyword soloforthFunction true
syn keyword soloforthGraphics at-xy
syn keyword soloforthGraphics attr-cls
syn keyword soloforthGraphics border
syn keyword soloforthGraphics bright.
syn keyword soloforthGraphics cls
syn keyword soloforthGraphics default-colors
syn keyword soloforthGraphics default-mode
syn keyword soloforthGraphics flash.
syn keyword soloforthGraphics ink.
syn keyword soloforthGraphics inverse
syn keyword soloforthGraphics overprint
syn keyword soloforthGraphics page
syn keyword soloforthGraphics paper.
syn keyword soloforthGraphics reset-pixel
syn keyword soloforthGraphics restore-mode
syn keyword soloforthGraphics save-mode
syn keyword soloforthGraphics set-pixel
syn keyword soloforthGraphics test-pixel
syn keyword soloforthGraphics terminal
syn keyword soloforthGraphics toggle-pixel
syn keyword soloforthGraphics xy
syn keyword soloforthHardware !p
syn keyword soloforthHardware @p
syn keyword soloforthHardware channel
syn keyword soloforthMath base
syn keyword soloforthMath decimal
syn keyword soloforthMath hex
syn keyword soloforthMemory !
syn keyword soloforthMemory !bank
syn keyword soloforthMemory !s
syn keyword soloforthMemory +stringer
syn keyword soloforthMemory -leading
syn keyword soloforthMemory -trailing
syn keyword soloforthMemory /string
syn keyword soloforthMemory /stringer
syn keyword soloforthMemory 1/string
syn keyword soloforthMemory 2!
syn keyword soloforthMemory 2,
syn keyword soloforthMemory 2@
syn keyword soloforthMemory >stringer
syn keyword soloforthMemory ?
syn keyword soloforthMemory ?csb
syn keyword soloforthMemory @
syn keyword soloforthMemory @bank
syn keyword soloforthMemory @s
syn keyword soloforthMemory align
syn keyword soloforthMemory aligned
syn keyword soloforthMemory allocate-string
syn keyword soloforthMemory allocate-stringer
syn keyword soloforthMemory allot
syn keyword soloforthMemory bank
syn keyword soloforthMemory bank-start
syn keyword soloforthMemory blank
syn keyword soloforthMemory dump
syn keyword soloforthMemory c!
syn keyword soloforthMemory c!bank
syn keyword soloforthMemory c!s
syn keyword soloforthMemory c,
syn keyword soloforthMemory c@
syn keyword soloforthMemory c@bank
syn keyword soloforthMemory c@s
syn keyword soloforthMemory cell
syn keyword soloforthMemory cell+
syn keyword soloforthMemory cell-
syn keyword soloforthMemory cells
syn keyword soloforthMemory char+
syn keyword soloforthMemory char-
syn keyword soloforthMemory chars
syn keyword soloforthMemory cmove
syn keyword soloforthMemory cmove>
syn keyword soloforthMemory count
syn keyword soloforthMemory csb0
syn keyword soloforthMemory default-bank
syn keyword soloforthMemory dp
syn keyword soloforthMemory empty-csb
syn keyword soloforthMemory empty-stack
syn keyword soloforthMemory empty-stringer
syn keyword soloforthMemory erase
syn keyword soloforthMemory far
syn keyword soloforthMemory far!
syn keyword soloforthMemory far-banks
syn keyword soloforthMemory far@
syn keyword soloforthMemory farc!
syn keyword soloforthMemory farc@
syn keyword soloforthMemory farcount
syn keyword soloforthMemory farlimit
syn keyword soloforthMemory farplace
syn keyword soloforthMemory fill
syn keyword soloforthMemory here
syn keyword soloforthMemory limit
syn keyword soloforthMemory move
syn keyword soloforthMemory np
syn keyword soloforthMemory np!
syn keyword soloforthMemory np0
syn keyword soloforthMemory np@
syn keyword soloforthMemory off
syn keyword soloforthMemory on
syn keyword soloforthMemory place
syn keyword soloforthMemory s,
syn keyword soloforthMemory smove
syn keyword soloforthMemory sp
syn keyword soloforthMemory sp!
syn keyword soloforthMemory sp0
syn keyword soloforthMemory sp@
syn keyword soloforthMemory stringer
syn keyword soloforthMemory system-bank
syn keyword soloforthMemory unused-csb
syn keyword soloforthMemory unused-stringer
syn keyword soloforthOperator *
syn keyword soloforthOperator */
syn keyword soloforthOperator */mod
syn keyword soloforthOperator +
syn keyword soloforthOperator +!
syn keyword soloforthOperator -
syn keyword soloforthOperator /
syn keyword soloforthOperator /mod
syn keyword soloforthOperator 0<
syn keyword soloforthOperator 0<>
syn keyword soloforthOperator 0=
syn keyword soloforthOperator 0>
syn keyword soloforthOperator 1+
syn keyword soloforthOperator 1-
syn keyword soloforthOperator 2*
syn keyword soloforthOperator 2+
syn keyword soloforthOperator 2-
syn keyword soloforthOperator 2/
syn keyword soloforthOperator <
syn keyword soloforthOperator <>
syn keyword soloforthOperator =
syn keyword soloforthOperator >
syn keyword soloforthOperator ?dnegate
syn keyword soloforthOperator ?negate
syn keyword soloforthOperator abs
syn keyword soloforthOperator and
syn keyword soloforthOperator between
syn keyword soloforthOperator c@and
syn keyword soloforthOperator c@and?
syn keyword soloforthOperator creset
syn keyword soloforthOperator cset
syn keyword soloforthOperator ctoggle
syn keyword soloforthOperator d+
syn keyword soloforthOperator d-
syn keyword soloforthOperator d2*
syn keyword soloforthOperator d2/
syn keyword soloforthOperator d<
syn keyword soloforthOperator d>s
syn keyword soloforthOperator dabs
syn keyword soloforthOperator dnegate
syn keyword soloforthOperator flip
syn keyword soloforthOperator fm/mod
syn keyword soloforthOperator invert
syn keyword soloforthOperator lshift
syn keyword soloforthOperator m*
syn keyword soloforthOperator m*/
syn keyword soloforthOperator m+
syn keyword soloforthOperator m/
syn keyword soloforthOperator max
syn keyword soloforthOperator min
syn keyword soloforthOperator mod
syn keyword soloforthOperator negate
syn keyword soloforthOperator or
syn keyword soloforthOperator rshift
syn keyword soloforthOperator s>d
syn keyword soloforthOperator sm/rem
syn keyword soloforthOperator u<
syn keyword soloforthOperator u>
syn keyword soloforthOperator ud/mod
syn keyword soloforthOperator um*
syn keyword soloforthOperator um/mod
syn keyword soloforthOperator umax
syn keyword soloforthOperator umin
syn keyword soloforthOperator within
syn keyword soloforthOperator xor
syn keyword soloforthReturnStack 2>r
syn keyword soloforthReturnStack 2r
syn keyword soloforthReturnStack 2r>
syn keyword soloforthReturnStack 2r@
syn keyword soloforthReturnStack 2rdrop
syn keyword soloforthReturnStack >r
syn keyword soloforthReturnStack r>
syn keyword soloforthReturnStack r@
syn keyword soloforthReturnStack rdrop
syn keyword soloforthReturnStack rp
syn keyword soloforthReturnStack rp!
syn keyword soloforthReturnStack rp0
syn keyword soloforthReturnStack rp0
syn keyword soloforthReturnStack rp@
syn keyword soloforthStack -rot
syn keyword soloforthStack 2drop
syn keyword soloforthStack 2dup
syn keyword soloforthStack 2nip
syn keyword soloforthStack 2over
syn keyword soloforthStack 2rot
syn keyword soloforthStack 2swap
syn keyword soloforthStack ?dup
syn keyword soloforthStack bounds
syn keyword soloforthStack cs-drop
syn keyword soloforthStack cs-dup
syn keyword soloforthStack cs-mark
syn keyword soloforthStack cs-pick
syn keyword soloforthStack cs-roll
syn keyword soloforthStack cs-swap
syn keyword soloforthStack cs-test
syn keyword soloforthStack depth
syn keyword soloforthStack drop
syn keyword soloforthStack dup
syn keyword soloforthStack nip
syn keyword soloforthStack over
syn keyword soloforthStack pick
syn keyword soloforthStack roll
syn keyword soloforthStack rot
syn keyword soloforthStack sp!
syn keyword soloforthStack sp0
syn keyword soloforthStack sp@
syn keyword soloforthStack swap
syn keyword soloforthStack tuck
syn keyword soloforthString compare
syn keyword soloforthString farlowers
syn keyword soloforthString faruppers
syn keyword soloforthString lower
syn keyword soloforthString lower_
syn keyword soloforthString lowers
syn keyword soloforthString replaces
syn keyword soloforthString scan
syn keyword soloforthString search
syn keyword soloforthString skip
syn keyword soloforthString str=
syn keyword soloforthString substitute
syn keyword soloforthString unescape
syn keyword soloforthString upper
syn keyword soloforthString uppers
syn keyword soloforthTool ms
syn keyword soloforthWordlists #words
syn keyword soloforthWordlists (find-name)
syn keyword soloforthWordlists -order
syn keyword soloforthWordlists .name
syn keyword soloforthWordlists >order
syn keyword soloforthWordlists also
syn keyword soloforthWordlists assembler
syn keyword soloforthWordlists assembler-wordlist
syn keyword soloforthWordlists context
syn keyword soloforthWordlists current
syn keyword soloforthWordlists current-latest
syn keyword soloforthWordlists definitions
syn keyword soloforthWordlists find
syn keyword soloforthWordlists find-name
syn keyword soloforthWordlists find-name-from
syn keyword soloforthWordlists forth
syn keyword soloforthWordlists forth-wordlist
syn keyword soloforthWordlists get-current
syn keyword soloforthWordlists get-order
syn keyword soloforthWordlists last
syn keyword soloforthWordlists lastxt
syn keyword soloforthWordlists latest
syn keyword soloforthWordlists latest-wordlist
syn keyword soloforthWordlists latestxt
syn keyword soloforthWordlists only
syn keyword soloforthWordlists previous
syn keyword soloforthWordlists root
syn keyword soloforthWordlists root-wordlist
syn keyword soloforthWordlists seal
syn keyword soloforthWordlists search-wordlist
syn keyword soloforthWordlists set-current
syn keyword soloforthWordlists set-order
syn keyword soloforthWordlists trail
syn keyword soloforthWordlists traverse
syn keyword soloforthWordlists vocabulary
syn keyword soloforthWordlists wordlist
syn keyword soloforthWordlists words

" Basic assembler words:

syn keyword soloforthAssembler code
syn keyword soloforthAssembler end-code

syn keyword soloforthAssembler asm
syn keyword soloforthAssembler end-asm

syn keyword soloforthAssembler macro
syn keyword soloforthAssembler endm

syn keyword soloforthAssembler jpnext,

syn keyword soloforthAssembler next
syn keyword soloforthAssembler pusha
syn keyword soloforthAssembler pushhl
syn keyword soloforthAssembler pushhlde
syn keyword soloforthAssembler fetchhl

" Assembler instructions:

syn keyword soloforthAssembler ?call,
syn keyword soloforthAssembler ?jp,
syn keyword soloforthAssembler ?jr,
syn keyword soloforthAssembler ?ret,
syn keyword soloforthAssembler adc#,
syn keyword soloforthAssembler adc,
syn keyword soloforthAssembler adcp,
syn keyword soloforthAssembler adcx,
syn keyword soloforthAssembler add#,
syn keyword soloforthAssembler add,
syn keyword soloforthAssembler addp,
syn keyword soloforthAssembler addx,
syn keyword soloforthAssembler and#,
syn keyword soloforthAssembler and,
syn keyword soloforthAssembler andx,
syn keyword soloforthAssembler bit,
syn keyword soloforthAssembler bitx,
syn keyword soloforthAssembler call,
syn keyword soloforthAssembler ccf,
syn keyword soloforthAssembler clr,
syn keyword soloforthAssembler cp#,
syn keyword soloforthAssembler cp,
syn keyword soloforthAssembler cpir,
syn keyword soloforthAssembler cpl,
syn keyword soloforthAssembler cpx,
syn keyword soloforthAssembler daa,
syn keyword soloforthAssembler dec,
syn keyword soloforthAssembler decp,
syn keyword soloforthAssembler decx,
syn keyword soloforthAssembler di,
syn keyword soloforthAssembler djnz,
syn keyword soloforthAssembler ei,
syn keyword soloforthAssembler exaf,
syn keyword soloforthAssembler exde,
syn keyword soloforthAssembler exsp,
syn keyword soloforthAssembler exx,
syn keyword soloforthAssembler fta,
syn keyword soloforthAssembler ftap,
syn keyword soloforthAssembler fthl,
syn keyword soloforthAssembler ftp,
syn keyword soloforthAssembler ftpx,
syn keyword soloforthAssembler ftx,
syn keyword soloforthAssembler ftx,
syn keyword soloforthAssembler ftx,
syn keyword soloforthAssembler halt,
syn keyword soloforthAssembler hook,
syn keyword soloforthAssembler in,
syn keyword soloforthAssembler inbc,
syn keyword soloforthAssembler inc,
syn keyword soloforthAssembler incp,
syn keyword soloforthAssembler incx,
syn keyword soloforthAssembler jp,
syn keyword soloforthAssembler jphl,
syn keyword soloforthAssembler jpix,
syn keyword soloforthAssembler jr,
syn keyword soloforthAssembler ld#,
syn keyword soloforthAssembler ld,
syn keyword soloforthAssembler ldai,
syn keyword soloforthAssembler ldd,
syn keyword soloforthAssembler lddr,
syn keyword soloforthAssembler ldi,
syn keyword soloforthAssembler ldia,
syn keyword soloforthAssembler ldir,
syn keyword soloforthAssembler ldp#,
syn keyword soloforthAssembler ldp,
syn keyword soloforthAssembler ldsp,
syn keyword soloforthAssembler neg,
syn keyword soloforthAssembler nop,
syn keyword soloforthAssembler or#,
syn keyword soloforthAssembler or,
syn keyword soloforthAssembler orx,
syn keyword soloforthAssembler out,
syn keyword soloforthAssembler outbc,
syn keyword soloforthAssembler pop,
syn keyword soloforthAssembler prt,
syn keyword soloforthAssembler push,
syn keyword soloforthAssembler res,
syn keyword soloforthAssembler resx,
syn keyword soloforthAssembler ret,
syn keyword soloforthAssembler rl,
syn keyword soloforthAssembler rla,
syn keyword soloforthAssembler rlc,
syn keyword soloforthAssembler rlca,
syn keyword soloforthAssembler rlcx,
syn keyword soloforthAssembler rld,
syn keyword soloforthAssembler rlx,
syn keyword soloforthAssembler rr,
syn keyword soloforthAssembler rra,
syn keyword soloforthAssembler rrc,
syn keyword soloforthAssembler rrca,
syn keyword soloforthAssembler rrcx,
syn keyword soloforthAssembler rrx,
syn keyword soloforthAssembler rst,
syn keyword soloforthAssembler sbc#,
syn keyword soloforthAssembler sbc,
syn keyword soloforthAssembler sbcp,
syn keyword soloforthAssembler sbcx,
syn keyword soloforthAssembler scf,
syn keyword soloforthAssembler set,
syn keyword soloforthAssembler setx,
syn keyword soloforthAssembler sla,
syn keyword soloforthAssembler slax,
syn keyword soloforthAssembler sll,
syn keyword soloforthAssembler sllx,
syn keyword soloforthAssembler sra,
syn keyword soloforthAssembler srax,
syn keyword soloforthAssembler srl,
syn keyword soloforthAssembler srlx,
syn keyword soloforthAssembler st#x,
syn keyword soloforthAssembler sta,
syn keyword soloforthAssembler stap,
syn keyword soloforthAssembler sthl,
syn keyword soloforthAssembler stp,
syn keyword soloforthAssembler stpx,
syn keyword soloforthAssembler stx,
syn keyword soloforthAssembler stx,
syn keyword soloforthAssembler stx,
syn keyword soloforthAssembler sub#,
syn keyword soloforthAssembler sub,
syn keyword soloforthAssembler subp,
syn keyword soloforthAssembler subx,
syn keyword soloforthAssembler tstp,
syn keyword soloforthAssembler xor#,
syn keyword soloforthAssembler xor,
syn keyword soloforthAssembler xorx,

" Assembler conditions:

syn keyword soloforthAssembler c?
syn keyword soloforthAssembler m?
syn keyword soloforthAssembler nc?
syn keyword soloforthAssembler nz?
syn keyword soloforthAssembler p?
syn keyword soloforthAssembler pe?
syn keyword soloforthAssembler po?
syn keyword soloforthAssembler z?

" Assembler control flow words:

syn keyword soloforthControlFlow aagain
syn keyword soloforthControlFlow abegin
syn keyword soloforthControlFlow aelse
syn keyword soloforthControlFlow aif
syn keyword soloforthControlFlow arepeat
syn keyword soloforthControlFlow athen
syn keyword soloforthControlFlow auntil
syn keyword soloforthControlFlow awhile
syn keyword soloforthControlFlow ragain
syn keyword soloforthControlFlow rbegin
syn keyword soloforthControlFlow relse
syn keyword soloforthControlFlow rif
syn keyword soloforthControlFlow rrepeat
syn keyword soloforthControlFlow rstep
syn keyword soloforthControlFlow rthen
syn keyword soloforthControlFlow runtil
syn keyword soloforthControlFlow rwhile

" Assembler labels:

syn keyword soloforthControlFlow l:
syn keyword soloforthControlFlow al#
syn keyword soloforthControlFlow rl#

" Quotations:

syn match soloforthFlow "\<\[:\>"
syn keyword soloforthFlow ;]

" Colon definitions:

syn match soloforthColonDef '\<:\s\+[^ \t]\+\>'

" Special cases because of the open bracket

"syn keyword soloforthDefine [
syn match soloforthDefine "\<\[\>"
"syn keyword soloforthDefine [']
syn match soloforthDefine "\<\[']\>"
"syn keyword soloforthDefine [COMPILE]
syn match soloforthDefine "\<\[compile]\>"

" Numbers

"syn match soloforthNumber '\<-\=[0-9.]*[0-9.]\+\>'


syn match soloforthNumber '\<-\=[0-9A-F]\{2}\.\=\>'
syn match soloforthNumber '\<-\=[0-9A-F]\{4}\.\=\>'
"syn match soloforthNumber '\<-\=[0-9A-F]\{8}\.\=\>'
syn match soloforthNumber '\<-\=[0-9]\+\.\=\>'
syn match soloforthNumber '\<\$-\=[0-9A-F]\+\.\=\>'
syn match soloforthNumber '\<#-\=[0-9]\+\.\=\>'
syn match soloforthNumber '\<%-\=[01]\+\.\=\>'
syn match soloforthNumber '\<\'.\'\>'

" Strings

syn region soloforthString start=+\<abort\"+ end=+"+ end=+$+
syn region soloforthString start=+\<\.\"+ end=+"+ end=+$+
syn region soloforthString start=+\<s\"+ end=+"+ end=+$+
syn region soloforthString start=+\<,\"+ end=+"+ end=+$+
syn region soloforthString start=+\<s\\\"+ end=+"+ end=+$+
syn region soloforthString start=+\<.\\\"+ end=+"+ end=+$+
"syn region soloforthCharOps start=+."\s+ skip=+\\"+ end=+"+

" Comments

syn match soloforthComment '\<(\s[^)]*)' contains=soloforthTodo
syn match soloforthComment '\<\\\>.*$' contains=soloforthTodo

" Comment regions have to come after the highlighting for numbers otherwise they have no effect:
syn region soloforthComment start='\<0 ?(\>' end='\<?)\>' contains=soloforthTodo
syn region soloforthComment start='\<0 \[if\]\>' end='\<\[then\]\>' contains=soloforthTodo

" Block Titles
" Adapted from fsb
" http://programandala.net/en.program.fsb.html

" syn match soloforthBlockTitle /^\.\?(\s.\+)\(\s\+.*\)\?$/
syn match soloforthBlockTitle /^\.\?(\s.\{-})/

syn match soloforthCharOps '\<char\s\+\S\+\>'
syn match soloforthCharOps '\<\[char\]\s\+\S\+\>'

" XXX OLD
" syn match soloforthInclude '\<require\s\+\k\+'

syn match soloforthComment '\<\.(\s[^)]*)'

syn match soloforthFiles "\<\[needed]\>"
syn match soloforthFiles "\<\[unneeded]\>"

syn match soloforthDefine "\<\[if]\>"
syn match soloforthDefine "\<\[defined]\>"
syn match soloforthDefine "\<\[undefined]\>"
syn match soloforthDefine "\<\[else]\>"
syn match soloforthDefine "\<\[then]\>"

syn match soloforthFunction "\<\[true]\>"
syn match soloforthFunction "\<\[false]\>"

" Define the highlighting.

hi def link soloforthAssembler Macro
hi def link soloforthBlockTitle Underlined
hi def link soloforthCharOps Character
"hi def link soloforthCharacterInput Character
hi def link soloforthCharacterInput Statement
"hi def link soloforthCharacterOutput Character
hi def link soloforthCharacterOutput Statement
hi def link soloforthColonDef Define
hi def link soloforthComment Comment
hi def link soloforthConversion String
hi def link soloforthDefine Define
hi def link soloforthFiles Statement
hi def link soloforthFlow Repeat
hi def link soloforthForth Statement
hi def link soloforthFunction Number
hi def link soloforthGraphics Statement
hi def link soloforthHardware Statement
hi def link soloforthInclude Include
hi def link soloforthMath Number
hi def link soloforthMemory Statement
hi def link soloforthNumber Number
hi def link soloforthOperator Operator
hi def link soloforthReturnStack Special
hi def link soloforthStack Special
hi def link soloforthString String
hi def link soloforthTodo Todo
hi def link soloforthTool Statement
hi def link soloforthWordlists Statement

let b:current_syntax = "soloforth"

" --------------------------------------------------------------
" Change log

" 2015-06-05: First version, based on <abersoftforth.vim> and
" <abersoftforthafera.vim>.
"
" 2015-06-06: Updated.
"
" 2015-06-07: Updated.
"
" 2015-06-20: Updated: `label`, `s"`, `sliteral`, `slit`, `s,`,
" `scr/disk`, `?-->`, `?\`, `cells`, `cell`, `cell+`, `cswap`.
"
" 2015-06-23: Updated.
"
" 2015-07-04: Updated: `postpone`.
"
" 2015-07-04: Updated: `0<>`.
"
" 2015-07-11: Updated.
"
" 2015-07-17: Updated: `np`, `np0`, `np@`, `cfap>lfa`, `!bank`,
" `c!bank`, `>mark`, `>resolve`...
"
" 2015-07-22: Updated: `exhaust`, `rdrop`, `2rdrop`.
"
" 2015-07-22: Updated: `l/scr'.
"
" 2015-07-23: Updated: `fetchhl'. 'fetchhl,', 'next,',
" 'pushhl,', 'pushde,'.
"
" 2015-07-24: Updated: `parse-name` instead of `parse-word`.
"
" 2015-08-11: Added `compile,`.
"
" 2015-08-12: Updated: `?terminal` -> `break-key?`. Added
" `nfa>string`, `upper`, `uppers`, `trail`. Removed `-find`,
" `forget`.  More changes.
"
" 2015-08-14: Updated: `pushde` -> pushhlde`, `pushde,` ->
" pushhlde,`
"
" 2015-08-15: Fixed: `blank`. Updated after the changes in the
" disk buffers.
"
" 2015-08-21: Updated with search order words defined in the
" kernel.
"
" 2015-08-23: Updated.
"
" 2015-08-30: Updated: `pusha`.
"
" 2015-08-30: Updated: `asm`, `end-asm`, circular string buffer.
"
" 2015-09-07: Added `don't` and `unnest`.
"
" 2015-09-11: Removed `offset` and `line`.
"
" 2015-09-11: Updated.
"
" 2015-09-12: Updated, user area words.
"
" 2015-09-13: Updated: `umin`, `umax`; `require` and family
" renamed to `need`.
"
" 2015-09-19: Updated.
"
" 2015-09-22: Updated.
"
" 2015-09-24: Updated: number prefixes "#", "$" and "%";
" `>number`, `number?`.
"
" 2015-09-26: Updated.
"
" 2015-10-05: Updated: `u>`, `-leading`, `chars`.
"
" 2015-10-06: Updated: `key?`, `place`, `source`, `stream`.
"
" 2015-10-07: Updated: `invert`.
"
" 2015-10-09: Updated: `parse-string`.
"
" 2015-10-14: Updated: `catch`, `throw`, `abort"`, `?throw`.
"
" 2015-10-16: Updated: `2,`, `smove`, `2r@`, `roll`... Fixed
" `@p`, `!p`.
"
" 2015-10-21: Upadted: `compile-only`.
"
" 2015-10-24: Updated.
"
" 2015-10-25: Updated.
"
" 2015-11-03: Updated.
"
" 2015-11-05: Updated.
"
" 2015-11-11: Updated.
"
" 2015-11-12: Updated.
"
" 2015-11-13: Updated.
"
" 2015-11-14: Fixed.
"
" 2015-11-16: Updated.
"
" 2015-11-18: Updated.
"
" 2015-11-23: Updated.
"
" 2015-11-24: Updated.
"
" 2015-12-04: Updated.
"
" 2015-12-14: Updated.
"
" 2015-12-17: Updated, after implementing the Forth-83 loops.
"
" 2015-12-19: Updated.
"
" 2015-12-23: Updated.
"
" 2015-12-25: New: library assembler (two versions)
"
" 2015-12-26: Updated: `step`, `for`, `>code`.
"
" 2016-01-01: Upadted: `.name`.
"
" 2016-02-15: Updated: `limit`.
"
" 2016-02-19: Removed `message`.
"
" 2016-02-26: Updated: `addix`, `addiy`.
"
" 2016-02-27: Updated: `macro`, `endm`.
"
" 2016-03-13: Updated: added `span`, removed `(home)`.
"
" 2016-03-15: Added `evaluate`.
"
" 2016-03-19: Added `there`, `system-bank`, `default-bank`,
" `bank`, `bank-start`.  Renamed the words related to the names
" bank.
"
" 2016-03-24: Added `:noname`.
"
" 2016-04-10: Added `display`, `default-mode`, `save-mode` and
" `restore-mode`.
"
" 2016-04-18: Removed `not`.
"
" 2016-04-21: Updated the kernel words related to user data
" space.
"
" 2016-04-24: Add `2literal`, `2lit`, `docolon`, `code-field,`,
" `noname?`, `last`, `lastxt`, `latestxt`.
"
" 2016-04-27: Remove `defined?`, add `?defined`. Add
" `.error-word`.
"
" 2016-04-29: Add `nest-source`, `unnest-source`,
" `block>source`.
"
" 2016-05-04: Add `channel`, `printing`, `/hold`; remove `out`.
"
" 2016-06-01: Remove `there`.
"
" 2016-10-16: Add `ms`.
"
" 2016-11-16: Update `hp` (formerly `np`) and related words.
" Add the far-memory words defined in the kernel.
"
" 2016-11-20: Add `blocks`.
"
" 2016-11-23: Add `align` and `aligned`. Update bit-manipulation
" words.
"
" 2016-11-24: Add `-exit`.
"
" 2016-11-25: Add `[:`, `;]`. Fix keywords matched with `syn
" match`.
"
" 2016-11-28: Add `lastblk`, Forth-2012 structures. Add
" `[needed]`, `[unneeded]`, `?)`.
"
" 2016-12-03: Add `within`, `between`.
"
" 2016-12-06: Add `source-id`, `refill`, `assembler-wordlist`,
" `root-wordlist`. Replace `next-name` with `nextname`.
"
" 2016-12-07: Fix `hided` to `hidden`. Add `farlimit`.
"
" 2016-12-08: Add `current-lastest`.
"
" 2016-12-16: Add `,"`.
"
" 2016-12-20: Rename `jppushhl` to `jppushhl,` and `jpnext` to
" `jpnext,`.
"
" 2016-12-22: Add `.\"`.
"
" 2016-12-23: Fix string regions to start at a word boundary.
"
" 2016-12-26: Update the order of some assembly words.
"
" 2016-12-27: Add `farunused`.
"
" 2017-01-05: Update the assembler words. The first version of
" the assembler has been removed from the system.
"
" 2017-01-06: Update `voc-link` to `latest-wordlist`.
"
" 2017-01-20: Add `word-length-mask`, `>>name`, `name>>`,
" `name<name`, `name>str`, `name>name`, `#words`.
"
" 2017-01-22: Add `1literal`.
"
" 2017-01-23: Add `replaces`, `unescape`, `substitute`,
" `1/string`.
"
" 2017-01-31: Add `attr-cls`, `]l`, `]1l`, `]2l`, `]cl`. Rename
" `paper` to `paper.`, etc.
"
" 2017-02-01: Add `lower`, `lowers`, `farlowers`,
" `lower-routine`, `noop-routine`.
"
" 2017-02-12: Add `file-status`.
"
" 2017-02-22: Add `str=`.
"
" 2017-02-26: Rename "hp" notation to "np", after the changes in
" the kernel.
"
" 2017-02-27: Add `ldi,` and `ldd,`.
"
" 2017-02-28: Update file header.
"
" 2017-03-04: Change "-routine" suffix to "_", aftert the new
" naming convention.
"
" 2017-03-12: Update the names of `stringer` words and add the
" missing ones.
"
" 2017-03-13: Update names including "rec" to "sector(s)";
" update names including "blk" to "block(s)". Add comment
" regions for `0 ?(` and `0 [if]`, adapted from <gforth.vim>.
"
" 2017-03-14: Remove `attr`.
"
" 2017-03-22: Add `sll,`, `sllx,`.
"
" 2017-03-25: Add `rename-file`, `delete-file`, `dump`, `l:`,
" `al#`, `rl#`. Remove old `;s`.
"
" 2017-04-04: Add `fm/mod`.
"
" 2017-04-16: Improve the patterns of numbers: only one optional
" decimal point at the end.
"
" 2017-05-05: Add `abort-message`.
"
" 2017-05-06: Add `m*/
"
" 2017-05-08: Update: Rename `display` to `terminal`.
"
" 2017-05-10: Update the assembler. Add `0.r` and `0d.r`.
"
" 2017-05-11: Add `catcher`.
"
" 2017-12-10: Add `finish-code`.
"
" 2017-12-11: Add `cs-dup`, `cs-mark`, `cs-test`.
"
" 2018-01-01: Add `empty-stack`.
"
" 2018-01-03: Update `1literal` to `xliteral`, `]1l` to `]xl`.
"
" 2018-03-28: Add `0leave`.

" --------------------------------------------------------------

" vim:ts=2:sts=2:sw=2:et:nocindent:smartindent:
