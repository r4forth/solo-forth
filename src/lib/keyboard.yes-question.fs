  \ keyboard.yes-question.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words for "yes/no" questions.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( y/n? y/n no? yes? )

  \ Credit:
  \
  \ Code adapted from Afera.

need value

'y' value "y"  'n' value "n"
  \ Default (uppercase) letters for "yes" and "no".

: y/n? ( c -- f ) dup "y" = swap "n" = or ;
  \ Is the given (uppercase) char _c_
  \ a valid answer for a "y/n" question?

: y/n ( -- c )
  begin  key lower dup y/n? 0=  while  drop  repeat ;
  \ Wait for a valid key press for a "y/n" question
  \ and return its code.

: no? ( -- f ) y/n "n" = ;
  \ Wait for a valid key press for a "y/n" question
  \ and return _true_ if it's the current letter for "no",
  \ else return _false_.

: yes? ( -- f ) y/n "y" = ;
  \ Wait for a valid key press for a "y/n" question
  \ and return _true_ if it's the current letter for "yes",
  \ else return _false_.

  \ ===========================================================
  \ Change log

  \ 2016-04-24: Remove `char`, which has been moved to the
  \ library.
  \
  \ 2017-02-01: Replace `upper` with `lower`, because `upper`
  \ has been moved to the library.

  \ vim: filetype=soloforth
