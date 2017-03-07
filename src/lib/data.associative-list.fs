  \ data.associative-list.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702221550

  \ -----------------------------------------------------------
  \ Description

  \ An associative list implemented with standard word lists.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ -----------------------------------------------------------
  \ Credit

  \ Based on code written by Wil Baden, published in Forth
  \ Dimensions (volume 17, number 4, page 36, 1995-11).

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

  \ 2015-11-06: Start, adapted from Wil Baden's code.
  \
  \ 2016-03-24: Comments.
  \
  \ 2016-04-15: Improved with different types of items.
  \ Factored. An obscure bug was discovered during the changes.
  \ Finally its origin was found in `(;code)`, in the kernel,
  \ and fixed.
  \
  \ 2016-11-26: Need `search-wordlist`, which has been moved to
  \ the library.
  \
  \ 2016-12-15: Improve documentation. Rename `entry` and
  \ friends after `field:` and friends, and make them
  \ individually accessible by `need`. Compact the code to save
  \ one block. Move the test to the tests module.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation.

( associative-list item? item create-entry )

need search-wordlist

: associative-list ( "name" -- ) wordlist constant ;

  \ doc{
  \
  \ associative-list ( "name" -- )
  \
  \ Create a new associative list "name".
  \
  \ }doc

: item? ( ca len wid -- false | xt true ) search-wordlist 0<> ;

  \ doc{
  \
  \ item? ( ca len wid -- false | xt true )
  \
  \ Is _ca len_ an item of associative list _wid_?
  \ If so return its _xt_ and _true_, else return _false_.
  \
  \ }doc

: item ( ca len wid -- i*x ) item? 0= #-13 ?throw execute ;

  \ doc{
  \
  \ item ( ca len wid -- i*x )
  \
  \ If _ca len_ is an item of associative list _wid_, return
  \ its value _i*x_; else throw exception -13, "undefined
  \ word".
  \
  \ }doc


: create-entry ( i*x wid xt "name" -- )
  get-current >r swap set-current  create execute
  r> set-current ;

  \ doc{
  \
  \ create-entry ( i*x wid xt "name" -- )
  \
  \ Create an entry "name" in associative list _wid_,
  \ using _xt_ to store its value _i*x_.
  \
  \ }doc

-->

( entry: centry: 2entry: sentry: items )

need create-entry  [unneeded] entry: ?(
: entry: ( x wid "name" -- )
  ['] , create-entry does> ( -- x ) ( pfa ) @ ; ?)

  \ doc{
  \
  \ entry: ( x wid "name" -- )
  \
  \ Create a cell entry "name" in associative list
  \ _wid_, with value _x_.
  \
  \ }doc

[unneeded] centry: ?(
: centry: ( c wid "name" -- )
  ['] c, create-entry does> ( -- c ) ( pfa ) c@ ; ?)

  \ doc{
  \
  \ centry: ( c wid "name" -- )
  \
  \ Create a character entry "name" in associative list
  \ _wid_, with value _c_.
  \
  \ }doc

[unneeded] 2entry: ?(
: 2entry: ( dx wid "name" -- )
  ['] 2, create-entry does> ( -- dx ) ( pfa ) 2@ ; ?)

  \ doc{
  \
  \ 2entry: ( dx wid "name" -- )
  \
  \ Create a double-cell entry "name" in associative list
  \ _wid_, with value _dx_.
  \
  \ }doc

[unneeded] sentry: ?(
: sentry: ( ca len wid "name" -- )
  ['] s, create-entry does> ( -- ca len ) ( pfa ) count ; ?)

  \ doc{
  \
  \ sentry: ( ca len wid "name" -- )
  \
  \ Create a string entry "name" in associative list
  \ _wid_, with value _ca len_.
  \
  \ }doc

[unneeded] items ?exit need alias need wordlist-words

' wordlist-words alias items ( wid -- )

  \ doc{
  \
  \ items ( wid -- )
  \
  \ List items of associative list _wid_.
  \
  \ }doc

  \ vim: filetype=soloforth