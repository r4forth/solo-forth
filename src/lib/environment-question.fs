  \ environment-question.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ The environmental queries of Forth-2012.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( environment? )

need search-wordlist need alias

wordlist constant environment-wordlist ( -- wid )

  \ doc{
  \
  \ environment-wordlist ( -- wid )
  \
  \ A constant that holds the word list identifier _wid_ where
  \ the environmental queries are defined.
  \
  \ See also: `environment?`.
  \
  \ }doc

: environment? ( ca len -- false | i*x true )
  environment-wordlist search-wordlist
  if  execute true  else  false  then ;

  \ doc{
  \
  \ environment? ( ca len -- false | i*x true )
  \
  \ The string _ca len_ is the identifier of an environmental
  \ query. If the string is not recognized, return a false
  \ flag. Otherwise return a true flag and some information
  \ about the query.
  \

  \ [caption="Environmental Query Strings"]
  \ |===
  \ | String   | Value data type  | Constant? | Meaning

  \ | /COUNTED-STRING    | n      | yes       |  maximum size of a counted string, in characters
  \ | /HOLD              | n      | yes       |  size of the pictured numeric output string buffer, in characters
  \ | /PAD               | n      | yes       |  size of the scratch area pointed to by PAD, in characters
  \ | ADDRESS-UNIT-BITS  | n      | yes       |  size of one address unit, in bits
  \ | FLOORED            | flag   | yes       |  true if floored division is the default
  \ | MAX-CHAR           | u      | yes       |  maximum value of any character in the implementation-defined character set
  \ | MAX-D              | d      | yes       |  largest usable signed double number
  \ | MAX-N              | n      | yes       |  largest usable signed integer
  \ | MAX-U              | u      | yes       |  largest usable unsigned integer
  \ | MAX-UD             | ud     | yes       |  largest usable unsigned double number
  \ | RETURN-STACK-CELLS | n      | yes       |  maximum size of the return stack, in cells
  \ | STACK-CELLS        | n      | yes       |  maximum size of the data stack, in cells
  \ |===

  \ Note: Forth-2012 designates the Forth-94 practice of using
  \ `environment?` to inquire whether a given word set is
  \ present as obsolescent.  The Forth-94 environmental
  \ strings are not supported in Solo Forth.
  \
  \ Origin: Forth-2012 (CORE).

  \ See also: `environment-wordlist`.
  \
  \ }doc

get-current  environment-wordlist dup >order set-current

  \ XXX TODO -- document?

8 constant address-unit-bits ( -- n )
  \ Size of one address unit, in bits.

255 constant max-char ( -- u )
  \ Maximum value of any character in the character set.

255 constant /counted-string ( -- n )
  \ Maximum size of a counted string, in characters.

' /hold alias /hold ( -- n )
  \ Size of the pictured numeric string output buffer, in
  \ characters.

84 constant /pad ( -- n )
  \ Size of the scratch area pointed to by `pad`, in
  \ characters.
  \
  \ XXX TODO -- A more useful definition, but non-constant,
  \ therefore non-standard:
  \
  \ : /pad ( -- u ) limit @ pad - ;

false constant floored ( -- f )
  \ True if floored division is the default.
  \
  \ XXX OLD -- This returned the current behaviour when
  \ `environment?` is being loaded:
  \       1 -3 mod 0< constant floored ( -- f )

-->

( environment? )

32767 constant max-n ( -- n )
  \ Largest usable signed integer.

-1 constant max-u ( -- u )
  \ Largest usable unsigned integer.

-1 max-n 2constant max-d ( -- d )
  \ Largest usable signed double.

-1. 2constant max-ud ( -- ud )
  \ Largest usable unsigned double.

$2C +origin @ constant return-stack-cells ( -- n )
    \ Maximum size of the return stack, in cells.

$2A +origin @ constant stack-cells ( -- n )
    \ Maximum size of the data stack, in cells.

  \ XXX TODO -- add "#locals" when needed

set-current previous

  \ ===========================================================
  \ Change log

  \ 2015-11-13: Start: only the Forth-2012 queries, not the
  \ obsolescent word set queries of Forth-94.
  \
  \ 2016-05-18: Update: use `wordlist` instead of `vocabulary`,
  \ which has been moved to the library.
  \
  \ 2016-11-26: Need `search-wordlist`, which has been moved to
  \ the library.
  \
  \ 2016-12-08: Fix `/hold`, `floored`. Rename the module
  \ filename to <environment-question.fsb>.  Add
  \ `return-stack-cells` and `data-stack-cells`.  Fix restoring
  \ of the search order. Document `environment?` and
  \ `environment-wordlist`.
  \
  \ 2017-02-17: Update cross references.

  \ vim: filetype=soloforth
