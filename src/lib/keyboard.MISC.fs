  \ keyboard.MISC.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201711281730
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to the keyboard.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( at-accept clear-accept set-accept )

  \ XXX UNDER DEVELOPMENT
  \ Common code for several versions of `accept`
  \
  \ 2016-03-13: copied from the kernel, in
  \ order to make it optional in the future.

2variable accept-xy       \ coordinates of the edited string

  \
  \ accept-xy ( -- a )
  \
  \ A double variable that holds the cursor position at the
  \ start of the most recent `accept`.
  \

variable accept-buffer    \ address of the edited string

  \
  \ accept-buffer ( -- a )
  \
  \ A variable that holds the buffer address used by
  \ the latest execution of `accept`.
  \

variable /accept          \ max length of the edited string

  \
  \ /accept ( -- a )
  \
  \ A variable that holds the buffer max length used by
  \ the latest execution of `accept`.
  \

variable >accept          \ offset to the cursor position

  \
  \ >accept ( -- a )
  \
  \ A variable that holds the offset of the cursor in the
  \ string being edited by `accept`.
  \

: at-accept ( -- ) accept-xy 2@ at-xy ;

  \
  \ at-accept ( -- )
  \
  \ Set the cursor position at the start of the most recent
  \ `accept`.
  \

variable span

  \
  \ span ( -- a )
  \
  \ A variable. _a_ is the address of a cell containing the
  \ count of characters actually received and stored by the
  \ last execution of some words.  Originally ``span`` is used
  \ by ``expect``, which is not implemented in Solo Forth.
  \
  \ Origin: Forth-83 (Required Word Set), Forth-94 (CORE EXT,
  \ obsolescent).
  \

: clear-accept ( -- )
  at-accept span @ spaces at-accept  span off ;

  \
  \ clear-accept ( -- )
  \
  \ Clear the string currently edited by `accept`.
  \

: set-accept ( ca1 len1 -- ca1' )
  clear-accept /accept @ min ( ca1 len1' )
  dup span ! 2dup fartype
  dup >r
  accept-buffer @ ( ca1 len1' ca2 )
  smove accept-buffer @ ( ca2 )
  r> + ( ca1' ) ;

  \
  \ set-accept ( ca1 len1 -- ca1' )
  \
  \ Set string _ca len_ as the string being edited by `accept`.
  \ Return the address _ca1'_ after its last character.
  \

( acceptx )

  \ XXX UNDER DEVELOPMENT
  \
  \ Alternative version of `accept` with more editing features
  \
  \ 2016-03-13: copied from the kernel, in
  \ order to make it optional in the future.

need at-accept need set-accept need toggle-capslock

: .acceptx ( -- )

  accept-buffer @ >accept @ at-accept type
    \ Display the start of the string, before the cursor.

  1 inverse  >accept @ span @ <
  if accept-buffer @ >accept @ + c@ emit  else  space  then
  0 inverse
    \ Display the cursor.

  accept-buffer @ span @ >accept @ 1+ min /string type ;
    \ Display the end of the string, after the cursor.

: accept-edit ( -- ) clear-accept init-accept ;
: accept-left ( -- ) ;
: accept-right ( -- ) ;
: accept-up ( -- ) ;
: accept-down ( -- ) ;
: accept-delete ( -- ) ;  -->

( acceptx )

create accept-commands ] noop noop noop noop noop noop
toogle-capslock accept-edit accept-left accept-right
accept-down accept-up accept-delete noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop [

: >accept-command ( c -- a ) cells accept-commands + ;
: accept-command ( c -- ) >accept-command perform ;

: init-acceptx ( ca len -- )
  /accept !  accept-buffer !  >accept off  xy accept-xy 2! ;

-->

( acceptx )

: (acceptx) ( ca len -- len' ) 2dup init-accept

  over + over ( bot eot cur )
  begin  key dup 13 <> \ not carriage return?
  while
    dup 12 =  \ delete?
    if    drop  >r over r@ < dup  \ any chars?
          if  8 dup emit  bl emit  emit  then  r> +
    else  \ printable
          >r  2dup <>  \ more?
          if r@ over c!  char+  r@ emit
          then r> drop
    then
  repeat  drop nip swap - ;

: acceptx ( ca len -- len' )
  span off  ?dup 0= if  drop 0  else  (acceptx)  then ;

  \ XXX TMP -- for debugging:

  \ : ax ( -- ) ['] acceptx ['] accept defer! ;
  \ : a0 ( -- ) ['] default-accept ['] accept defer! ;

( nuf? aborted? break? -keys new-key new-key- )

[unneeded] nuf? dup

?\ need aborted? need 'cr'
?\ : nuf? ( -- f ) 'cr' aborted? ;

  \ Credit:
  \
  \ Code adapted from lpForth and Forth Dimensions (volume 10,
  \ number 1, page 29, 1988-05).

  \ XXX OLD -- Classic definition:
  \
  \ : nuf? ( -- f ) key? dup if  key 2drop key 'cr' = then ;

  \ doc{
  \
  \ nuf? ( -- f )
  \
  \ If no key is pressed return _false_.  If a key is pressed,
  \ discard it and wait for a second key. Then return _true_ if
  \ it's a carriage return, else return _false_.
  \
  \ Usage example:
  \
  \ ----
  \ : listing ( -- )
  \   begin  ." bla " nuf?  until  ." Aborted" ;
  \ ----
  \
  \ See: `aborted?`.
  \
  \ }doc

[unneeded] aborted? ?(

: aborted? ( c -- f )
  key? dup if key 2drop key = else nip then ; ?)

  \ doc{
  \
  \ aborted? ( c -- f )
  \
  \ If no key is pressed return _false_.  If a key is pressed,
  \ discard it and wait for a second key. Then return _true_ if
  \ it's _c_, else return _false_.
  \
  \ ``aborted?`` is a useful factor of `nuf?`.
  \
  \ Usage example:

  \ ----
  \ : listing ( -- )
  \   begin  ." bla "  bl aborted?  until  ." Aborted" ;
  \ ----

  \ }doc

[unneeded] break? ?(

  \ XXX UNDER DEVELOPMENT
  \ XXX TODO try

: break? ( -- f ) key? dup if  key 2drop break-key?  then ; ?)

[unneeded] -keys ?(
code -keys ( -- )
  FD c, CB c, 01 c, 86 08 05 * + c, jpnext, end-code ?)
    \ 01 iy 5 resx, \ res 5,(iy+$01)
    \ Reset bit 5 of system variable FLAGS.

  \ Credit:
  \ Adapted from Galope.

  \ doc{
  \
  \ -keys ( -- )
  \
  \ Remove all keys from the keyboard buffer.
  \
  \ See: `key?`, `new-key`, `new-key-`, `key`, `xkey`.
  \
  \ }doc

[unneeded] new-key need -keys ?\ : new-key ( -- c ) -keys key ;

  \ doc{
  \
  \ new-key ( -- c )
  \
  \ Remove all keys from the keyboard buffer, then return
  \ character _c_ of the key struck, a member of the a member
  \ of the defined character set.
  \
  \ See: `new-key-`, `key`, `xkey`, `-keys`.
  \
  \ }doc

[unneeded] new-key- ?( need new-key need -keys
: new-key- ( -- ) new-key drop -keys ; ?)

  \ doc{
  \
  \ new-key- ( -- )
  \
  \ Remove all keys from the keyboard buffer, then wait for a
  \ key press and discard it. Finally remove all keys from the
  \ keyboard buffer.
  \
  \ See: `new-key`, `key`, `xkey`, `-keys`.
  \
  \ }doc

( /kk kk-ports kk, kk@ )

  \ Adapted from Afera.
  \ XXX UNDER DEVELOPMENT

  \ ===========================================================
  \ Description

  \ Some tools to manage key presses. An improved and detailed
  \ implementation can be found in the Tron 0xF game
  \ (http://programandala.net/en.program.tron_0xf.html).
  \
  \ "kk" stands for "keyboard key". This notation was chosen
  \ first in order to prevent future name clashes with standard
  \ words which uses the "k-" prefix, and second because these
  \ words manage only physical keys of the keyboard, not key
  \ combinations.
  \
  \ ===========================================================

[defined] /kk ?\ 4 cconstant /kk

  \ doc{
  \
  \ /kk ( -- n )
  \
  \ A constant that holds the bytes ocuppied by every key in
  \ the `kk-ports` table: 3 (smaller and slower table) or 4
  \ (bigger and faster table).
  \
  \ There are two versions of `kk,` and `kk@`. They depend on
  \ the value of `/kk`.
  \
  \ The application can define `/kk` before loading this block;
  \ else it will be defined as a cconstant with value 4.
  \
  \ }doc

  \ ............................................
  \ Method 1: smaller but slower

  \ Every key identifier occupies 3 bytes in the table (total
  \ size is 120 bytes)

/kk 3 <> dup

?\ : kk, ( bitmask port -- ) , c, ;
  \ Store a key definition into the keys table.

?\ : kk@ ( a -- bitmask port ) dup c@ swap 1+ @ ;  -->
  \ Fetch a key definition from the keys table.

  \ XXX TODO -- write this version of `kk@` in Z80

  \ ............................................
  \ Method 2: bigger but faster

  \ Every key identifier occupies 4 bytes in the table (total
  \ size is 160 bytes)

/kk 4 <> dup dup

?\ need alias

?\ ' 2, alias kk, ( bitmask port -- )
  \ Store a key definition into the keys table.

?\ ' 2@ alias kk@ ( a -- bitmask port ) -->
  \ Fetch a key definition from the keys table.

  \ doc{
  \
  \ kk, ( bitmask port -- )
  \
  \ Store a key definition into the keys table.
  \
  \ See: `kk@`, `/kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk@ ( a -- bitmask port )
  \
  \ Fetch a key definition from the keys table.
  \
  \ See: `kk,`, `/kk`, `kk-ports`.
  \
  \ }doc

( kk-ports )

  \ Key constants are defined with double constants this way:
  \ high part = bitmask
  \ low part = port of the keyboard row

  \ XXX TODO -- document these constants

$01 $F7FE 2constant kk-1  $02 $F7FE 2constant kk-2
$04 $F7FE 2constant kk-3  $08 $F7FE 2constant kk-4
$10 $F7FE 2constant kk-5

$01 $FBFE 2constant kk-q  $02 $FBFE 2constant kk-w
$04 $FBFE 2constant kk-e  $08 $FBFE 2constant kk-r
$10 $FBFE 2constant kk-t

$01 $FDFE 2constant kk-a  $02 $FDFE 2constant kk-s
$04 $FDFE 2constant kk-d  $08 $FDFE 2constant kk-f
$10 $FDFE 2constant kk-g

$01 $FDFE 2constant kk-cs  $02 $FDFE 2constant kk-z
$04 $FDFE 2constant kk-x   $08 $FDFE 2constant kk-c
$10 $FDFE 2constant kk-v

-->

( kk-ports )

$01 $EFFE 2constant kk-0  $02 $EFFE 2constant kk-9
$04 $EFFE 2constant kk-8  $08 $EFFE 2constant kk-7
$10 $EFFE 2constant kk-6

$01 $DFFE 2constant kk-p  $02 $DFFE 2constant kk-o
$04 $DFFE 2constant kk-i  $08 $DFFE 2constant kk-u
$10 $DFFE 2constant kk-y

$01 $BFFE 2constant kk-en  $02 $BFFE 2constant kk-l
$04 $BFFE 2constant kk-k   $08 $BFFE 2constant kk-j
$10 $BFFE 2constant kk-h

$01 $7FFE 2constant kk-sp $02 $7FFE 2constant kk-ss
$04 $7FFE 2constant kk-m  $08 $7FFE 2constant kk-n
$10 $7FFE 2constant kk-b

-->

( kk-ports )

need kk,

40 cconstant keys

  \ doc{
  \
  \ keys ( -- n )
  \
  \ A constant that holds the number of keys on the keyboard:
  \ 40.
  \
  \ }doc

create kk-ports

kk-1  kk,  kk-2  kk,  kk-3 kk,  kk-4 kk,  kk-5 kk,
kk-q  kk,  kk-w  kk,  kk-e kk,  kk-r kk,  kk-t kk,
kk-a  kk,  kk-s  kk,  kk-d kk,  kk-f kk,  kk-g kk,
kk-cs kk,  kk-z  kk,  kk-x kk,  kk-c kk,  kk-v kk,
kk-0  kk,  kk-9  kk,  kk-8 kk,  kk-7 kk,  kk-6 kk,
kk-p  kk,  kk-o  kk,  kk-i kk,  kk-u kk,  kk-y kk,
kk-en kk,  kk-l  kk,  kk-k kk,  kk-j kk,  kk-h kk,
kk-sp kk,  kk-ss kk,  kk-m kk,  kk-n kk,  kk-b kk,

  \ doc{
  \
  \ kk-ports ( -- a )
  \
  \ A table that contains the key definitions (bitmak and port)
  \ of all keys.
  \
  \ The table contains 40 items, one per physical key, and it's
  \ organized by keyboard rows.
  \
  \ Every item occupies 3 or 4 bytes, depending on the value of
  \ `/kk`.
  \
  \ See: `kk,`, `kk@`.
  \
  \ }doc

( kk-1# )

  \ Key number constants, to be used as indexes of the key
  \ tables.

need cenum

0
cenum kk-1#  cenum kk-2#  cenum kk-3# cenum kk-4# cenum kk-5#
cenum kk-q#  cenum kk-w#  cenum kk-e# cenum kk-r# cenum kk-t#
cenum kk-a#  cenum kk-s#  cenum kk-d# cenum kk-f# cenum kk-g#
cenum kk-cs# cenum kk-z#  cenum kk-x# cenum kk-c# cenum kk-v#
cenum kk-0#  cenum kk-9#  cenum kk-8# cenum kk-7# cenum kk-6#
cenum kk-p#  cenum kk-o#  cenum kk-i# cenum kk-u# cenum kk-y#
cenum kk-en# cenum kk-l#  cenum kk-k# cenum kk-j# cenum kk-h#
cenum kk-sp# cenum kk-ss# cenum kk-m# cenum kk-n# cenum kk-b#
drop

  \ XXX TODO -- document these constants

( kk-chars )

create kk-chars

'1' c,  '2' c,  '3' c,  '4' c,  '5' c,
'q' c,  'w' c,  'e' c,  'r' c,  't' c,
'a' c,  's' c,  'd' c,  'f' c,  'g' c,
128 c,  'z' c,  'x' c,  'c' c,  'v' c,
'0' c,  '9' c,  '8' c,  '7' c,  '6' c,
'p' c,  'o' c,  'i' c,  'u' c,  'y' c,
129 c,  'l' c,  'k' c,  'j' c,  'h' c,
130 c,  131 c,  'm' c,  'n' c,  'b' c,

  \ doc{
  \
  \ kk-chars ( -- a )
  \
  \ Address of a 40-byte table that contains the chars used as
  \ names of the keys.
  \
  \ The table contains one byte per physical key, and
  \ it's organized by keyboard rows.
  \
  \ By default, the first 4 UDG (chars 128..131) are used for
  \ keys whose names are not a printable char:
  \
  \ - 128 = Caps Shift
  \ - 129 = Enter
  \ - 130 = Space
  \ - 131 = Symbol Shift
  \
  \ }doc

( #>kk pressed pressed? )

[unneeded] #>kk ?( need kk-ports

: #>kk ( n -- d ) /kk * kk-ports + kk@ ; ?)

  \ doc{
  \
  \ #>kk ( n -- bitmask port )
  \
  \ Convert keyboard key number _n_ to its data: _bitmask_ is
  \ the key bitmask and _port_ is the keyboard row port.
  \
  \ }doc

[unneeded] pressed? ?( need @p

: pressed? ( bitmask port -- f ) @p and 0= ; ?)

  \ doc{
  \
  \ pressed? ( bitmask port -- f )
  \
  \ Is a key pressed?  _bitmask_ is the key bitmask and _port_
  \ is the keyboard row port.
  \
  \ See: `pressed`, `only-one-pressed`.
  \
  \ }doc

[unneeded] pressed ?( need pressed? need kk-ports

: pressed ( -- false | bitmask port true )
  false \ by default
  [ kk-ports keys /kk * bounds swap ] literal literal ?do
    i kk@ pressed? if  drop i kk@ 1 leave  then  /kk
  +loop ;

  \ doc{
  \
  \ pressed ( -- false | bitmask port true )
  \
  \ Return the key identifier of the first key from the keys
  \ table that happens to be pressed.  _bitmask_ is the key
  \ bitmask and _port_ is the keyboard row port.
  \
  \ See: `kk-ports`, `only-one-pressed`, `pressed?`.
  \
  \ }doc

?)

( only-one-pressed )

  \ XXX UNDER DEVELOPMENT

  \ The application must define the `/k` constant.

need kk-ports

0. 2variable kk-pressed

: only-one-pressed ( -- false | bitmask port true )

  \ XXX TODO finish

  0. kk-pressed 2! \ none by default
  [ kk-ports keys /kk * bounds swap ] literal literal
  ?do  i kk@ pressed?
  if  kk-pressed 2@ + if
  then
  /kk +loop
  kk-pressed 2@ 2dup + if  1  else  2drop 0  then ;

  \ doc{
  \
  \ only-one-pressed ( -- false | bitmask port true )
  \
  \ Return the key identifier of the key pressed, if there's
  \ only one key pressed.  _bitmask_ is the key bitmask and
  \ _port_ is the keyboard row port.
  \
  \ See: `kk-ports`, `pressed`, `pressed?`.
  \
  \ }doc

( key-edit key-left key-right key-down key-up key-delete )

  \ Words analogous to Forth-2012 constants, but with a
  \ different prefix and used to hold values returned by `key`.

[unneeded] key-edit   ?\  7 cconstant key-edit
[unneeded] key-left   ?\  8 cconstant key-left
[unneeded] key-right  ?\  9 cconstant key-right
[unneeded] key-down   ?\ 10 cconstant key-down
[unneeded] key-up     ?\ 11 cconstant key-up
[unneeded] key-delete ?\ 12 cconstant key-up

( key-enter )

  \ Words analogous to Forth-2012 constants, but with a
  \ different prefix and used to hold values returned by `key`.

[unneeded] key-enter  ?\ 13 cconstant key-enter

  \ ===========================================================
  \ Change log

  \ 2016-04-24: Remove `char`, which has been moved to the
  \ library.
  \
  \ 2016-05-07: Make block titles compatible with `indexer`.
  \
  \ 2016-11-17: Use `?(` instead of `[if]`.
  \
  \ 2016-12-04: Use `cenum` instead of `enum` for `kk-1#`
  \ constant and family. This change saves 40 bytes of data
  \ space and makes the access faster. Add `#>kk`. Document
  \ many words. Improve access by `need`. Define `/kk` by
  \ default. Compact the code, saving two blocks.
  \
  \ 2016-12-24: Add `key-edit`, `key-left`, `key-right`,
  \ `key-down`, `key-up`, `key-delete`, `key-enter`. Rename the
  \ module from <keyboard.fsb> to <keyboard.MISC.fsb>.
  \
  \ 2016-12-30: Compact the code, saving one block.
  \
  \ 2017-01-05: Convert `set-accept` to far memory.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-02-01: Move `span` from the kernel.
  \
  \ 2017-02-15: Update notation in the documentation of `span`.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-02-19: Replace `do`, which has been moved to the
  \ library, with `?do`.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-05-04: Improve documentation.
  \
  \ 2017-05-07: Improve documentation.
  \
  \ 2017-11-28: Add `-keys`, `new-key`, `new-key-`.

  \ vim: filetype=soloforth
