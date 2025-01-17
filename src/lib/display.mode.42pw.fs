  \ display.mode.42pw.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT

  \ Last modified: 201806041324
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ A 42 CPL display mode that adapts the current 32-CPL font
  \ at real time.

  \ ===========================================================
  \ Authors

  \ P. Wardle wrote the original routine.  It was published on
  \ Your Sinclair #78 (June 1992).  It was found as part of the
  \ VU-R Browser utility, written by  Jim Grimwood:
  \
  \ http://www.users.globalnet.co.uk/~jg27paw4/pourri/pourri.htm

  \ Marcos Cruz (programandala.net) integrated it into Solo
  \ Forth, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( mode-42pw )

create mode-42pw-coordinates 0 c, 0 c,

create mode-42pw-workspace 8 allot

create mode-42pw-removable-columns

  \ Data showing which columns to remove from each character,
  \ for characters in range $20..$7F.
  \
  \ A datum in range $00..$20 indicates an alternative
  \ definition is used insted, and the datum is used as index
  \ in the table `mode-42pw-redefined-chars`.

  $FE c, $FE c, $80 c, $E0 c, $80 c, $00 c, $01 c, $80 c,
  $80 c, $80 c, $80 c, $80 c, $80 c, $80 c, $80 c, $80 c,
  $02 c, $80 c, $E0 c, $E0 c, $FC c, $E0 c, $E0 c, $C0 c,
  $F0 c, $F0 c, $F0 c, $F0 c, $C0 c, $F0 c, $C0 c, $C0 c,
  $F8 c, $F0 c, $F0 c, $F0 c, $F0 c, $F0 c, $F0 c, $F0 c,
  $F0 c, $80 c, $F0 c, $C0 c, $F0 c, $F0 c, $F8 c, $F0 c,
  $F0 c, $F8 c, $F0 c, $F0 c, $03 c, $F0 c, $F0 c, $F0 c,
  $F0 c, $04 c, $FC c, $E0 c, $FC c, $F0 c, $FC c, $F0 c,
  $F0 c, $FF c, $80 c, $FF c, $FF c, $FF c, $FF c, $FF c,
  $FF c, $FF c, $FF c, $FF c, $C0 c, $FF c, $FF c, $FF c,
  $FF c, $FF c, $FF c, $FF c, $FF c, $FF c, $FF c, $FF c,
  $FF c, $FF c, $FF c, $80 c, $80 c, $FF c, $80 c, $05 c, -->

( mode-42pw-output )

create mode-42pw-redefined-chars

  \ Completely redefined characters: %, &, 0, T, Y, (c)
  \
  \ These characters are pointed by values $00..$05 in the
  \ `mode-42pw-removable-columns` table.

  $00 c, $00 c, $64 c, $68 c, $10 c, $2C c, $4C c, $00 c,
  $00 c, $20 c, $50 c, $20 c, $54 c, $48 c, $34 c, $00 c,
  $00 c, $38 c, $4C c, $54 c, $54 c, $64 c, $38 c, $00 c,
  $00 c, $7E c, $10 c, $10 c, $10 c, $10 c, $10 c, $00 c,
  $00 c, $44 c, $44 c, $28 c, $10 c, $10 c, $10 c, $00 c,
  $00 c, $78 c, $84 c, $B4 c, $A4 c, $B4 c, $84 c, $78 c,

need where need ?depth
  \ XXX TMP -- debugging tools

-->

( mode-42pw )

need assembler need l: need os-chars need os-attr-p

also assembler max-labels c@ 9 max-labels c! previous

create mode-42pw-output_ ( -- a ) asm

  \ _a_ is the address of a Z80 routine that displays the
  \ character in register A in `mode-42pw`.
  \
  \ Recognized:
  \
  \ Character #13 (move cursor to next line, column 0).
  \
  \ XXX TODO -- Not recognized:
  \
  \ Character #22 + y-pos + x-pos.

  \ Credit:
  \
  \ Original routine "PRINT42" from Your Sinclair #78 (1992-06)
  \ by P Wardle.
  \
  \ Modified by Marcos Cruz (programandala.net) to adapt it to
  \ Solo Forth, 2017-04.

  \ ============================================================

  \ 16 cp#, #00 rl# nz? ?jr,
  \   cp   $16
  \   jr   nz,check_return_character
  \
  \   ; A = $16 (AT control character)

  \ XXX TODO -- Rewrite the AT control, because the original
  \ works in a string and can get the coordinates in advance:

  \ exde, a and, $0002 h ldp#, b sbcp, exde, nc? ?ret,
  \   ex   de,hl ; preserve HL ; XXX OLD
  \   and  a
  \   ld   hl,$0002
  \   sbc  hl,bc ; are there at least 2 characters left?
  \   ex   de,hl ; restore HL
  \   ret  nc ; if not, return

  \ ret, \ XXX TMP --

  \ h incp,  m d ld, b decp, m inc, m e ld,
  \ #07 call, al#  #01 rl# jr,
  \   inc  hl
  \   ld   d,(hl) ; get row
  \   dec  bc
  \   inc  hl
  \   ld   e,(hl) ; get column
  \   dec  bc
  \   call check_coordinates_in_DE
  \   jr   update_coordinates_and_return

  #00 l: 0D cp#, z? rif
    mode-42pw-coordinates d ftp, #08 call, al#
  \ check_return_character:
  \   cp   $0D
  \   jr   nz,check_printable_character
  \   ld   de,(coordinates)
  \   call next_row

    #01 l: mode-42pw-coordinates d stp, ret, rthen
  \ update_coordinates_and_return:
  \   ld   (coordinates),de
  \   ret

  bl cp#, c? ?ret, a c ld, 00 h ld#, a l ld,
  \ check_printable_character:
  \   cp   32 ; printable character (space or higher)?
  \   ret  c  ; if not, return
  \
  \ printable_character:
  \   ; Form the new 6-bit wide characters and alter colors to match
  \   ld   c,a
  \   ld   h,$00
  \   ld   l,a

  mode-42pw-removable-columns bl - d ldp#, d addp, m a ld,
  \   ld   de,removable_columns-32
  \   add  hl,de
  \   ld   a,(hl)
  20 cp#, #02 rl# nc? ?jr,
  \   cp   $20 ; redefined character instead?
  \   jr   nc,convert_rom_character_to_42cpl ; if not, jump
  mode-42pw-redefined-chars d ldp#, a l ld,
  \   ld   de,redefined_characters
  \   ld   l,a
  #05 call, al# h b ld, l c ld, #03 rl# jr,
  \   call character_bitmap
  \   ld   b,h ; XXX TODO -- why this?
  \   ld   c,l ; XXX TODO -- why this?
  \   jr   display_character_bitmap

  #02 l: os-chars d ftp, c l ld, #05 call, al# -->
  \ convert_rom_character_to_42cpl:
  \   ; C = character code
  \   ; A = bitmask of the removable columns
  \   ld   de,$3C00 ; ROM font
  \   ld   l,c
  \   call character_bitmap

( mode-42pw )

  mode-42pw-workspace d ldp#, d push, exx, a c ld, cpl, a b ld,
  \   ld   de,workspace
  \   push de
  \   exx
  \   ld   c,a
  \   cpl
  \   ld   b,a

  exx, 08 b ld#,
  \   exx
  \   ld   b,$08

  rbegin m a ld, h incp, exx, a e ld, c and, a d ld, e a ld,
  \ label_eaea:
  \   ld   a,(hl)
  \   inc  hl
  \   exx
  \   ld   e,a
  \   and  c
  \   ld   d,a
  \   ld   a,e

  rla, b and, d or, exx, d ftap, d incp, rstep b pop,
  \   rla
  \   and  b
  \   or   d
  \   exx
  \   ld   (de),a
  \   inc  de
  \   djnz label_eaea
  \
  \   pop  bc

  #03 l: #06 call, al# e inc,
  \ display_character_bitmap:
  \
  \   ; HL = address of the character bitmap
  \   ; BC = address of the character bitmap?
  \
  \   call check_coordinates
  \   inc  e

  mode-42pw-coordinates d stp, e dec, e a ld, a sla, a l ld,
  \   ld   (coordinates),de
  \   dec  e
  \   ld   a,e
  \   sla  a
  \   ld   l,a

  a sla, l add, a l ld, a srl, a srl, a srl, a e ld, l a ld,
  \   sla  a
  \   add  a,l
  \   ld   l,a
  \   srl  a
  \   srl  a
  \   srl  a
  \   ld   e,a
  \   ld   a,l

  07 and#, a push, exaf, d a ld, a sra, a sra, a sra, 58 add#,
  \   and  $07
  \   push af
  \   ex   af,af'
  \   ld   a,d
  \   sra  a
  \   sra  a
  \   sra  a
  \   add  a,$58

  a h ld, d a ld, 07 and#, rrca, rrca, rrca, e add, a l ld,
  \   ld   h,a
  \   ld   a,d
  \   and  $07
  \   rrca
  \   rrca
  \   rrca
  \   add  a,e
  \   ld   l,a

  os-attr-p fta, a e ld, e m ld, h incp, a pop, 03 cp#,
  \   ld   a,(attr_p)
  \   ld   e,a
  \   ld   (hl),e
  \   inc  hl
  \   pop  af
  \   cp   $03

  nc? rif e m ld, rthen
  \   jr   c,label_eb35
  \   ld   (hl),e
  \ label_eb35:

  h decp, d a ld, F8 and#, 40 add#, a h ld, h push, exx, h pop,
  \   dec  hl
  \   ld   a,d
  \   and  $F8
  \   add  a,$40
  \   ld   h,a
  \   push hl
  \   exx
  \   pop  hl

  exx, 08 a ld#, -->
  \   exx
  \   ld   a,$08

( mode-42pw )

  rbegin a push, b ftap, exx, h push, 00 c ld#, 03FF d ldp#,
  \ label_eb42:
  \   push af
  \   ld   a,(bc)
  \   exx
  \   push hl
  \   ld   c,$00
  \   ld   de,$03FF

  exaf, a and, #04 rl# z? ?jr, a b ld, exaf,
  \   ex   af,af'
  \   and  a
  \   jr   z,label_eb5d
  \   ld   b,a
  \   ex   af,af'

  rbegin a and, rra, c rr, d rr, e rr, rstep exaf,
  \ label_eb51:
  \   and  a
  \   rra
  \   rr   c
  \   scf
  \   rr   d
  \   rr   e
  \   djnz label_eb51
  \   ex   af,af'

  #04 l: exaf, a b ld, m a ld, d and, b or, a m ld,
  \ label_eb5d:
  \   ex   af,af'
  \   ld   b,a
  \   ld   a,(hl)
  \   and  d
  \   or   b
  \   ld   (hl),a

  h incp, m a ld, e and, c or, a m ld, h pop, h inc, exx,
  \   inc  hl
  \   ld   a,(hl)
  \   and  e
  \   or   c
  \   ld   (hl),a
  \   pop  hl
  \   inc  h
  \   exx

  b incp, a pop, a dec, z? runtil ret,
  \   inc  bc
  \   pop  af
  \   dec  a
  \   jr   nz,label_eb42
  \   ret

  #05 l: 0 h ld#, h addp, h addp, h addp, d addp, ret,
  \ character_bitmap:
  \
  \   ; In:
  \   ;   DE = address of a font
  \   ;   L = character code
  \   ; Out:
  \   ;   HL = address of the character bitmap
  \
  \   ld   h,$00
  \   add  hl,hl
  \   add  hl,hl
  \   add  hl,hl
  \   add  hl,de
  \   ret

  #06 l: mode-42pw-coordinates d ftp,
  \ check_coordinates:
  \   ld   de,(coordinates) ; XXX TODO -- hardcode the coordinates

  #07 l: e a ld, 2A cp#, nc? rif #08 l: d inc, 00 e ld#, rthen
  \ check_coordinates_in_DE:
  \   ld   a,e    ; column
  \   cp   $2A    ; end of row?
  \   jr   c,check_end_of_screen ; if so, jump
  \ next_row:
  \   inc  d      ; next row
  \   ld   e,$00  ; first column
  \ check_end_of_screen:

  d a ld, 18 cp#, c? ?ret, 00 d ld#, ret,
  \   ld   a,d    ; row
  \   cp   $18    ; end of screen?
  \   ret  c      ; if not, return
  \   ; XXX TODO -- scroll
  \   ld   d,$00  ; if so, go to the first row
  \   ret

end-asm

also assembler max-labels c! previous \ restore default

  \ ============================================================
  \ Original code

  \ ; PRINT42.ASM
  \ ; a routine from Your Sinclair #78 (Jun.1992) by P Wardle
  \ ; put string to print in Z$
  \ ;
  \ ; These special characters are recognised in z$:
  \ ; CHR$ 22 + y-pos + x-pos: "PRINT AT" control (line+column)
  \ ; CHR$ 13: "ENTER" (positions to next line, column 0)
  \ ;
  \ ; EA60-EABC  Main handling routine, checks for controls and prints string.
  \ ; EABD-EB97  Forms the new 6-bit wide characters & alters colours to match,
  \ ;    followed by y,x coords and eight bytes of workspace.
  \ ; EB98-EBF7  Data showing which columns to remove from each character.
  \ ; EBF8-EC27  Completely redefined characters: %, &, 0, T, Y, (c)
  \ ;
  \ EA60 2A5D5C   LD   HL,(CH_ADD)
  \ EA63 E5       PUSH HL
  \ EA64 21BBEA   LD   HL,$EABB
  \ EA67 225D5C   LD   (CH_ADD),HL
  \ EA6A CDB228   CALL LOOK_VARS
  \ EA6D D1       POP  DE
  \ EA6E ED535D5C LD   (CH_ADD),DE
  \ EA72 D8       RET  C
  \ EA73 23       INC  HL
  \ EA74 4E       LD   C,(HL)
  \ EA75 23       INC  HL
  \ EA76 46       LD   B,(HL)
  \ EA77 23       INC  HL
  \ EA78 79       LD   A,C
  \ EA79 B0       OR   B
  \ EA7A C8       RET  Z
  \ EA7B 7E       LD   A,(HL)
  \ EA7C FE80     CP   $80
  \ EA7E 3034     JR   NC,$EAB4
  \ EA80 FE16     CP   $16
  \ EA82 2014     JR   NZ,$EA98
  \ EA84 EB       EX   DE,HL
  \ EA85 A7       AND  A
  \ EA86 210200   LD   HL,$0002
  \ EA89 ED42     SBC  HL,BC
  \ EA8B EB       EX   DE,HL
  \ EA8C D0       RET  NC
  \ EA8D 23       INC  HL
  \ EA8E 56       LD   D,(HL)
  \ EA8F 0B       DEC  BC
  \ EA90 23       INC  HL
  \ EA91 5E       LD   E,(HL)
  \ EA92 0B       DEC  BC
  \ EA93 CD7FEB   CALL $EB7F
  \ EA96 180B     JR   $EAA3
  \ EA98 FE0D     CP   $0D
  \ EA9A 200D     JR   NZ,$EAA9
  \ EA9C ED5B8EEB LD   DE,($EB8E)
  \ EAA0 CD84EB   CALL $EB84
  \ EAA3 ED538EEB LD   ($EB8E),DE
  \ EAA7 180B     JR   $EAB4
  \ EAA9 FE1F     CP   $1F
  \ EAAB 3807     JR   C,$EAB4
  \ EAAD E5       PUSH HL
  \ EAAE C5       PUSH BC
  \ EAAF CDBDEA   CALL $EABD
  \ EAB2 C1       POP  BC
  \ EAB3 E1       POP  HL
  \ EAB4 23       INC  HL
  \ EAB5 0B       DEC  BC
  \ EAB6 78       LD   A,B
  \ EAB7 B1       OR   C
  \ EAB8 20C1     JR   NZ,$EA7B
  \ EABA C9       RET

  \ EABB 7A24     DEFS "z$"      ; string to search for

  \ EABD D9       EXX
  \ EABE E5       PUSH HL
  \ EABF D9       EXX
  \ EAC0 4F       LD   C,A
  \ EAC1 2600     LD   H,$00
  \ EAC3 6F       LD   L,A
  \ EAC4 1178EB   LD   DE,$EB78
  \ EAC7 19       ADD  HL,DE
  \ EAC8 7E       LD   A,(HL)
  \ EAC9 FE20     CP   $20
  \ EACB 300B     JR   NC,$EAD8
  \ EACD 11F8EB   LD   DE,$EBF8
  \ EAD0 6F       LD   L,A
  \ EAD1 CD74EB   CALL $EB74
  \ EAD4 44       LD   B,H
  \ EAD5 4D       LD   C,L
  \ EAD6 1822     JR   $EAFA
  \ EAD8 11003C   LD   DE,$3C00
  \ EADB 69       LD   L,C
  \ EADC CD74EB   CALL $EB74
  \ EADF 1190EB   LD   DE,$EB90
  \ EAE2 D5       PUSH DE
  \ EAE3 D9       EXX
  \ EAE4 4F       LD   C,A
  \ EAE5 2F       CPL
  \ EAE6 47       LD   B,A
  \ EAE7 D9       EXX
  \ EAE8 0608     LD   B,$08
  \ EAEA 7E       LD   A,(HL)
  \ EAEB 23       INC  HL
  \ EAEC D9       EXX
  \ EAED 5F       LD   E,A
  \ EAEE A1       AND  C
  \ EAEF 57       LD   D,A
  \ EAF0 7B       LD   A,E
  \ EAF1 17       RLA
  \ EAF2 A0       AND  B
  \ EAF3 B2       OR   D
  \ EAF4 D9       EXX
  \ EAF5 12       LD   (DE),A
  \ EAF6 13       INC  DE
  \ EAF7 10F1     DJNZ $EAEA
  \ EAF9 C1       POP  BC
  \ EAFA CD7BEB   CALL $EB7B
  \ EAFD 1C       INC  E
  \ EAFE ED538EEB LD   ($EB8E),DE
  \ EB02 1D       DEC  E
  \ EB03 7B       LD   A,E
  \ EB04 CB27     SLA  A
  \ EB06 6F       LD   L,A
  \ EB07 CB27     SLA  A
  \ EB09 85       ADD  A,L
  \ EB0A 6F       LD   L,A
  \ EB0B CB3F     SRL  A
  \ EB0D CB3F     SRL  A
  \ EB0F CB3F     SRL  A
  \ EB11 5F       LD   E,A
  \ EB12 7D       LD   A,L
  \ EB13 E607     AND  $07
  \ EB15 F5       PUSH AF
  \ EB16 08       EX   AF,AF'
  \ EB17 7A       LD   A,D
  \ EB18 CB2F     SRA  A
  \ EB1A CB2F     SRA  A
  \ EB1C CB2F     SRA  A
  \ EB1E C658     ADD  A,$58
  \ EB20 67       LD   H,A
  \ EB21 7A       LD   A,D
  \ EB22 E607     AND  $07
  \ EB24 0F       RRCA
  \ EB25 0F       RRCA
  \ EB26 0F       RRCA
  \ EB27 83       ADD  A,E
  \ EB28 6F       LD   L,A
  \ EB29 3A8D5C   LD   A,(ATTR_P)
  \ EB2C 5F       LD   E,A
  \ EB2D 73       LD   (HL),E
  \ EB2E 23       INC  HL
  \ EB2F F1       POP  AF
  \ EB30 FE03     CP   $03
  \ EB32 3801     JR   C,$EB35
  \ EB34 73       LD   (HL),E
  \ EB35 2B       DEC  HL
  \ EB36 7A       LD   A,D
  \ EB37 E6F8     AND  $F8
  \ EB39 C640     ADD  A,$40
  \ EB3B 67       LD   H,A
  \ EB3C E5       PUSH HL
  \ EB3D D9       EXX
  \ EB3E E1       POP  HL
  \ EB3F D9       EXX
  \ EB40 3E08     LD   A,$08
  \ EB42 F5       PUSH AF
  \ EB43 0A       LD   A,(BC)
  \ EB44 D9       EXX
  \ EB45 E5       PUSH HL
  \ EB46 0E00     LD   C,$00
  \ EB48 11FF03   LD   DE,$03FF
  \ EB4B 08       EX   AF,AF'
  \ EB4C A7       AND  A
  \ EB4D 280E     JR   Z,$EB5D
  \ EB4F 47       LD   B,A
  \ EB50 08       EX   AF,AF'
  \ EB51 A7       AND  A
  \ EB52 1F       RRA
  \ EB53 CB19     RR   C
  \ EB55 37       SCF
  \ EB56 CB1A     RR   D
  \ EB58 CB1B     RR   E
  \ EB5A 10F5     DJNZ $EB51
  \ EB5C 08       EX   AF,AF'
  \ EB5D 08       EX   AF,AF'
  \ EB5E 47       LD   B,A
  \ EB5F 7E       LD   A,(HL)
  \ EB60 A2       AND  D
  \ EB61 B0       OR   B
  \ EB62 77       LD   (HL),A
  \ EB63 23       INC  HL
  \ EB64 7E       LD   A,(HL)
  \ EB65 A3       AND  E
  \ EB66 B1       OR   C
  \ EB67 77       LD   (HL),A
  \ EB68 E1       POP  HL
  \ EB69 24       INC  H
  \ EB6A D9       EXX
  \ EB6B 03       INC  BC
  \ EB6C F1       POP  AF
  \ EB6D 3D       DEC  A
  \ EB6E 20D2     JR   NZ,$EB42
  \ EB70 D9       EXX
  \ EB71 E1       POP  HL
  \ EB72 D9       EXX
  \ EB73 C9       RET

  \ EB74 2600     LD   H,$00
  \ EB76 29       ADD  HL,HL
  \ EB77 29       ADD  HL,HL
  \ EB78 29       ADD  HL,HL
  \ EB79 19       ADD  HL,DE
  \ EB7A C9       RET

  \ EB7B ED5B8EEB LD   DE,($EB8E)

  \ EB7F 7B       LD   A,E
  \ EB80 FE2A     CP   $2A
  \ EB82 3803     JR   C,$EB87
  \ EB84 14       INC  D
  \ EB85 1E00     LD   E,$00
  \ EB87 7A       LD   A,D
  \ EB88 FE18     CP   $18
  \ EB8A D8       RET  C
  \ EB8B 1600     LD   D,$00
  \ EB8D C9       RET

  \ EB8E 2A17     DEFB +2A,+17 ; y & x coords
  \ EB90 00181818              ; 8 bytes of workspace
  \ EB94 18001800

  \ EB98 FE FE 80 E0 80 00 01 80
  \ EBA0 80 80 80 80 80 80 80 80
  \ EBA8 02 80 E0 E0 FC E0 E0 C0
  \ EBB0 F0 F0 F0 F0 C0 F0 C0 C0
  \ EBB8 F8 F0 F0 F0 F0 F0 F0 F0
  \ EBC0 F0 80 F0 C0 F0 F0 F8 F0
  \ EBC8 F0 F8 F0 F0 03 F0 F0 F0
  \ EBD0 F0 04 FC E0 FC F0 FC F0
  \ EBD8 F0 FF 80 FF FF FF FF FF
  \ EBE0 FF FF FF FF C0 FF FF FF
  \ EBE8 FF FF FF FF FF FF FF FF
  \ EBF0 FF FF FF 80 80 FF 80 05

  \ EBF8 00 00 64 68 10 2C 4C 00
  \ EC00 00 20 50 20 54 48 34 00
  \ EC08 00 38 4C 54 54 64 38 00
  \ EC10 00 7E 10 10 10 10 10 00
  \ EC18 00 44 44 28 10 10 10 00
  \ EC20 00 78 84 B4 A4 B4 84 78

code mode-42pw-emit ( c -- )
  d pop, b push, e a ld, mode-42pw-output_ call,
         b pop, jpnext, end-code -->

  \ doc{
  \
  \ mode-42pw-emit ( c -- ) "mode-42-p-w-emit"
  \
  \ Display character _c_ in `mode-42pw`, by calling
  \ `mode-64ao-output_`.
  \
  \ ``mode-42pw-emit`` is configured by `mode-42pw` as the
  \ action of `emit`.
  \
  \ }doc

( mode-42pw )

need mode-32 need (at-xy need set-mode-output need >form

: mode-42pw-xy ( -- col row )
  mode-42pw-coordinates c@
  [ mode-42pw-coordinates 1+ ] literal c@ ;

  \ doc{
  \
  \ mode-42pw-xy ( -- col row ) "mode-42-p-w-x-y"
  \
  \ Return the current cursor coordinates _col row_ in
  \ `mode-42pw`. ``mode-64ao-xy`` is the action of `xy` when
  \ `mode-42pw` is active.
  \
  \ }doc

variable mode-42pw-font  rom-font bl 8 * + mode-42pw-font !
  \ XXX TODO -- Not used.

  \ doc{
  \
  \ mode-42pw-font ( -- a ) "mode-42-p-w-font"
  \
  \ A `variable`. _a_ is the address of a cell containing the
  \ address of the font used by `mode-42pw`. The font is a
  \ standard ZX Spectrum font (8x8-pixel characters, 32
  \ characters per line), which is converted to 42 characters
  \ per line at real time.  Note the address of the font must
  \ be the address of its character 32 (space).
  \
  \ The default value of ``mode-42pw-font`` is `rom-font` plus
  \ 256 (the address of the space character in the ROM font).
  \
  \ }doc

: mode-42pw ( -- )
  [ latestxt ] literal current-mode !
  mode-42pw-font @ 256 - set-font
  mode-42pw-output_ set-mode-output
  ['] mode-42pw-emit ['] emit  defer!
  ['] (at-xy         ['] at-xy defer! 42 24 >form
  ['] mode-42pw-xy   ['] xy    defer! ;

  \ doc{
  \
  \ mode-42pw ( -- ) "mode-42-p-w"
  \
  \ Start the 42 CPL display mode based on:

  \ ....
  \ PRINT42.ASM
  \ a routine from Your Sinclair #78 (Jun.1992) by P Wardle
  \
  \ Part of the VU-R Browser utility, written by  Jim Grimwood:
  \
  \ http://www.users.globalnet.co.uk/~jg27paw4/pourri/pourri.htm
  \ ....

  \ The control characters recognized are 13 (carriage return)
  \ and 22 (at).
  \
  \ WARNING: the "at" control character is followed by row and
  \ column, i.e. the order used in Sinclair BASIC strings. This
  \ is the order used also in `mode-32` and `mode-32iso`, but
  \ not in `mode-64ao`.
  \
  \ See: `current-mode`, `set-font`, `set-mode-output`,
  \ `columns`, `rows`, `mode-42pw-emit`, `mode-42pw-xy`,
  \ `mode-42pw-font`, `>form`, `mode-42pw-output_`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2017-04-19: Start the module. Start modifying the source of
  \ the original routine: Change the layout of the code;
  \ document the first labels; remove the channels stuff.
  \
  \ 2017-04-20: Identify and label more routines.
  \
  \ 2017-04-21: Rename module and words after the new
  \ convention for display modes. Need `(at-xy`, which has been
  \ moved to the common module.
  \
  \ 2017-04-23: Review.
  \
  \ 2017-05-15: Start adapting the code to the Forth assembler.
  \
  \ 2017-12-05: Advance the conversion of the original code.
  \
  \ 2017-12-06: Convert the labels. Compact the code, saving
  \ one block. Need `assembler` and `l:`. Fix assembly errors.
  \ First successful compilation.
  \
  \ 2017-12-10: Update to `a push,` and `a pop,`, after the
  \ change in the assembler.
  \
  \ 2017-12-12: Improve documentation. Remove saving/restoring
  \ of register HL', used by BASIC.
  \
  \ 2018-01-24: Update after the renaming of all display modes
  \ files and words: "42rt" (real time) ->  "42pw" (P. Wardle).
  \
  \ 2018-03-09: Add words' pronunciaton.
  \
  \ 2018-06-04: Link `variable` in documentation.

  \ vim: filetype=soloforth
