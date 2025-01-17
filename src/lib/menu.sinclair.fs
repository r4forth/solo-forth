  \ menu.sinclair.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201807272035
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Configurable Sinclair-style menus.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ ===========================================================
  \ To-do

  \ XXX TODO -- Factor the Sinclair style to an upper layer, in
  \ order to make the menu configurable with any style.
  \
  \ XXX TODO -- Make it possible to configure key lists instead
  \ of single keys.
  \
  \ XXX TODO -- Use a module to hide private words.

( sinclair-stripes sinclair-stripes$ .sinclair-stripes )

unneeding sinclair-stripes ?(

create sinclair-stripes ( -- ca )

$01 c, $03 c, $07 c, $0F c, $1F c, $3F c, $7F c, $FF c,

  \ 0 0 0 0 0 0 0 1           X
  \ 0 0 0 0 0 0 1 1          XX
  \ 0 0 0 0 0 1 1 1         XXX
  \ 0 0 0 0 1 1 1 1        XXXX
  \ 0 0 0 1 1 1 1 1       XXXXX
  \ 0 0 1 1 1 1 1 1      XXXXXX
  \ 0 1 1 1 1 1 1 1     XXXXXXX
  \ 1 1 1 1 1 1 1 1    XXXXXXXX

$FE c, $FC c, $F8 c, $F0 c, $E0 c, $C0 c, $80 c, $00 c, ?)

  \ 1 1 1 1 1 1 1 0    XXXXXXX
  \ 1 1 1 1 1 1 0 0    XXXXXX
  \ 1 1 1 1 1 0 0 0    XXXXX
  \ 1 1 1 1 0 0 0 0    XXXX
  \ 1 1 1 0 0 0 0 0    XXX
  \ 1 1 0 0 0 0 0 0    XX
  \ 1 0 0 0 0 0 0 0    X
  \ 0 0 0 0 0 0 0 0

  \ doc{
  \
  \ sinclair-stripes ( -- ca )
  \
  \ Return address _ca_ where the following pair of UDG
  \ definitions, used to create Sinclair stripes, are stored:

  \ ....
  \ 0 0 0 0 0 0 0 1           X
  \ 0 0 0 0 0 0 1 1          XX
  \ 0 0 0 0 0 1 1 1         XXX
  \ 0 0 0 0 1 1 1 1        XXXX
  \ 0 0 0 1 1 1 1 1       XXXXX
  \ 0 0 1 1 1 1 1 1      XXXXXX
  \ 0 1 1 1 1 1 1 1     XXXXXXX
  \ 1 1 1 1 1 1 1 1    XXXXXXXX
  \
  \ 1 1 1 1 1 1 1 0    XXXXXXX
  \ 1 1 1 1 1 1 0 0    XXXXXX
  \ 1 1 1 1 1 0 0 0    XXXXX
  \ 1 1 1 1 0 0 0 0    XXXX
  \ 1 1 1 0 0 0 0 0    XXX
  \ 1 1 0 0 0 0 0 0    XX
  \ 1 0 0 0 0 0 0 0    X
  \ 0 0 0 0 0 0 0 0
  \ ....

  \ See: `.sinclair-stripes`,  `sinclair-stripes$`.
  \
  \ }doc

unneeding sinclair-stripes$ ?(

here dup $10 c, $02 c, $80 c, $11 c, $06 c, $81 c,
         $10 c, $04 c, $80 c, $11 c, $05 c, $81 c,
         $10 c, $00 c, $80 c,
here - abs 2constant sinclair-stripes$ ( -- ca len ) ?)

  \ doc{
  \
  \ sinclair-stripes$ ( -- ca len )
  \
  \ Return a string _ca len_ containing the following character
  \ codes:

  \ |===
  \ | Code(s) | Meaning
  \
  \ | $10 $02 | set ink 2 (red)
  \ | $80     | first stripe UDG
  \ | $11 $06 | set paper 6 (yellow)
  \ | $81     | second stripe UDG
  \ | $10 $04 | set ink 4 (green)
  \ | $80     | first stripe UDG
  \ | $11 $05 | set paper 5 (cyan)
  \ | $81     | second stripe UDG
  \ | $10 $00 | set ink 0 (black)
  \ | $80     | first stripe UDG
  \ |===

  \ Definitions for UDG codes $80 and $81 are provided
  \ optionally by `sinclair-stripes`.
  \
  \ See: `.sinclair-sripes`.
  \
  \ }doc

unneeding .sinclair-stripes ?( need sinclair-stripes
                               need sinclair-stripes$

: .sinclair-stripes ( -- )
  get-udg [ sinclair-stripes 128 8 * - ] literal set-udg
  sinclair-stripes$ type set-udg ; ?)

  \ doc{
  \
  \ .sinclair-stripes ( -- ) "dot-sinclair-stripes"
  \
  \ Display the Sinclair stripes by using `sinclair-stripes` as
  \ UDG font and typing `sinclair-stripes$`. The current UDG
  \ font is preserved.
  \
  \ See: `set-udg`, `get-udg`.
  \
  \ }doc

5 cconstant /sinclair-stripes

  \ doc{
  \
  \ /sinclair-stripes ( -- len )
  \
  \ A `cconstant`. _len_ is the size of `sinclair-stripes$` in
  \ graphic characters, i.e. the visible length of the string
  \ when displayed.
  \
  \ ``/sinclair-stripes`` is used by `set-menu` and other menu
  \ words.
  \
  \ }doc

( menu )

need attr! need xy>attra need get-udg need set-udg
need type-left-field need case need array>
need white need black need cyan need papery need brighty
need overprint-off need inverse-off
need xy>gxy need ortholine need 8*
need under+ need within need polarity
need .sinclair-stripes

-->

( menu )

2variable menu-xy

2variable menu-title

variable actions-table

variable options-table

create menu-width 0 c, create menu-options 0 c,

create menu-banner-attr black papery white + brighty c,

create menu-body-attr white papery brighty c,

create menu-key-down '6' c,

create menu-key-up   '7' c,

create menu-key-choose 13 c,

create menu-highlight-attr cyan papery brighty c,

variable menu-rounding  menu-rounding on

-->

( menu )

: .banner ( -- )
  menu-banner-attr c@ attr! overprint-off inverse-off
  menu-xy 2@
  2dup at-xy menu-title 2@ menu-width c@ type-left-field
       swap menu-width c@ +
       [ /sinclair-stripes 1+ ] cliteral - swap
       at-xy .sinclair-stripes ;
  \ Display the banner of the current menu.

: (.option ( ca len -- ) menu-width c@ 1- type-left-field ;
  \ Display menu option _ca len_ at the current cursor
  \ coordinates.

: option>xy ( n -- col row ) menu-xy 2@ rot + 1+ ;
  \ Convert menu option _n_ to its cursor coordinates _col row_.

: at-option ( n -- ) option>xy at-xy ;
  \ Set the cursor at option _n_.

-->

( menu )

: vertical-line ( gx gy -- )
  0 -1 menu-options c@ 1+ 8* ortholine ;
  \ Draw a vertical 1-pixel border from _gx gy_ down to
  \ the bottom of the menu.

: menu-x-pixels ( -- n ) menu-width c@ 8* ;

: .border ( -- )
  menu-xy 2@ 1+
  2dup xy>gxy 2dup menu-x-pixels 1- under+ vertical-line
                                           vertical-line
       menu-options c@ + 1+ xy>gxy 1+ 1 0 menu-x-pixels
       ortholine ;
  \ Draw a 1-pixel border around the menu options, preserving
  \ the attributes.

  \ XXX TODO -- Reuse the result of the first `xy>gyx` for the
  \ horizontal line, the calculation will be faster.

-->

( menu )

: .option ( n -- )
  dup at-option space options-table @ array> @ count (.option ;
  \ Display menu option _n_ of the current menu.

: .options ( -- )
  menu-body-attr c@ attr!
  menu-options c@ dup 0 ?do i .option loop
                      at-option menu-width c@ spaces ;
  \ Display the options of the current menu, from texts table
  \ _a_.

: option>attrs ( n -- ca len )
  option>xy xy>attra menu-width c@ ;
  \ Convert menu option _n_ to its attributes zone _ca len_.

create current-option 0 c,

: -option ( -- ) current-option c@
                 option>attrs menu-body-attr c@ fill ;
  \ Remove the highlighting of the current option.

: +option ( n -- ) dup current-option c!
                   option>attrs menu-highlight-attr c@ fill ;
  \ Set _n_ as the current option and highlight it.

-->

( menu )

: round-option ( n -- n' )
  dup 0 menu-options c@ within ?exit
      polarity ( -1|1) 0< ( -1|0) menu-options c@ 1- and ;

: limit-option ( n -- n' ) 0 max menu-options c@ 1- min ;

: adjust-option ( n -- n' )
  menu-rounding @ if   round-option exit
                  then limit-option ;

: option+ ( n -- ) current-option c@ + adjust-option +option ;
  \ Add _n_ to the current option, make the result fit the
  \ valid range and make it the current option.

: previous-option ( -- ) -option -1 option+ ;

: next-option     ( -- ) -option  1 option+ ;

: choose-option ( n1 -- )
  current-option c@ actions-table @ array> perform ; -->

( menu )

: menu ( -- )
  0 +option
  begin key case
          menu-key-up     c@ of previous-option   endof
          menu-key-down   c@ of next-option       endof
          menu-key-choose c@ of choose-option     endof
        endcase again ;

  \ doc{
  \
  \ menu  ( -- )
  \
  \ Activate the current menu, which has been set by `set-menu`
  \ and displayed by `.menu`.
  \
  \ See: `new-menu`.
  \
  \ }doc

: .menu ( -- ) .banner .options .border ;

  \ doc{
  \
  \ .menu  ( -- ) "dot-menu"
  \
  \ Display the current menu, which has been set by `set-menu`
  \ and can be activated by `menu`.
  \
  \ See: `new-menu`.
  \
  \ }doc

: set-menu ( a1 a2 ca len col row n1 n2 -- )
  menu-options c!
  [ /sinclair-stripes 2+ ] cliteral max menu-width c!
  menu-xy 2! menu-title 2!  options-table ! actions-table ! ;

  \ doc{
  \
  \ set-menu ( a1 a2 ca len col row n1 n2 -- )
  \
  \ Set the current menu to cursor coordinates _col row_,
  \ _n2_ options, _n1_ characters width, title _ca len_,
  \ actions table _a1_ (a cell array of _n2_ execution tokens)
  \ and option texts table _a2_ (a cell array of _n2_ addresses
  \ of counted strings).
  \
  \ See: `new-menu`, `.menu`, `menu`.
  \
  \ }doc

: new-menu ( a1 a2 ca len col row n1 n2 -- )
  set-menu .menu menu ;

  \ doc{
  \
  \ new-menu ( a1 a2 ca len col row n1 n2 -- )
  \
  \ Set, display an activate a new menu at cursor coordinates
  \ _col row_, with _n2_ options, _n1_ characters width, title
  \ _ca len_, actions table _a1_ (a cell array of _n2_
  \ execution tokens) and option texts table _a2_ (a cell array
  \ of _n2_ addresses of counted strings).
  \
  \ Usage example:

  \ ----

  \ need menu need :noname
  \
  \ :noname ( -- ) unnest unnest ;
  \ :noname ( -- ) 2 border ;
  \ :noname ( -- ) 1 border ;
  \ :noname ( -- ) 0 border ;
  \
  \ create actions> , , , ,
  \
  \ here s" EXIT"  s,
  \ here s" Red"   s,
  \ here s" Blue"  s,
  \ here s" Black" s,
  \
  \ create texts> , , , ,
  \
  \ : menu-pars ( -- a1 a2 ca len col row n1 n2 )
  \   actions> texts> s" Border" 7 7 14 4 ;
  \
  \ menu-pars new-menu

  \ ----

  \ See: `set-menu`, `.menu`, `menu`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2017-03-28: Start. First working version.
  \
  \ 2017-03-29: Add the 1-pixel border. Add `menu-rounding` to
  \ configure the behaviour of the option selector.
  \
  \ 2017-11-25: Update stack comments.
  \
  \ 2018-03-07: Add words' pronunciaton. Improve documentation.
  \
  \ 2018-03-08: Rename `sinclair-stripes-bitmaps`
  \ `sinclair-stripes`. Make it, `sinclair-stripes$` and
  \ `.sinclair-stripes` independent, reusable. Fix
  \ documentation.
  \
  \ 2018-03-09: Update stack notation "x y" to "col row".
  \
  \ 2018-07-25: Rename `/stripes` to `/sinclair-stripes` and
  \ document it.
  \
  \ 2018-07-27: Remove useless code from `menu`. Improve
  \ documentation of `new-menu`.

  \ vim: filetype=soloforth
