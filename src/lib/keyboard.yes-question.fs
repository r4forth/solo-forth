  \ keyboard.yes-question.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201807221229
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words for "yes/no" questions.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( "y" "n" y/n? y/n no? yes? )

  \ Credit:
  \
  \ Code adapted from Afera.

unneeding "y" unneeding "n" and ?(

'y' cconstant "y"

  \ doc{
  \
  \ "y" ( -- c ) "quote-y-quote"
  \
  \ A character constant containing the (lowercase) character
  \ used by `y/n`, `y/n?` and `yes?`, to represent an
  \ affirmative answer. By default it's "y".  For localization,
  \ the value can be changed with `c!>`.
  \
  \ See: `"n"`.
  \
  \ }doc

'n' cconstant "n" ?)

  \ doc{
  \
  \ "n" ( -- c ) "quote-n-quote"
  \
  \ A character constant containing the (lowercase) character
  \ used by `y/n`, `y/n?` and `no?` to represent a negative
  \ answer. By default it's "n".  For localization, the value
  \ can be changed with `c!>`.
  \
  \ See: `"y"`.
  \
  \ }doc

unneeding y/n? ?( need "y" need "n"

: y/n? ( c -- f ) lower dup "y" = swap "n" = or ; ?)

  \ doc{
  \
  \ y/n? ( c -- f ) "y-slash-n-question"
  \
  \ Is character _c_, converted to lowercase, a valid answer
  \ for a "y/n" question? I.e., is _c_ the current value of
  \ `"y"` or `"n"`?
  \
  \ See: `yes?`, `no?`, `y/n`.
  \
  \ }doc

unneeding y/n ?( need y/n?

: y/n ( -- c ) begin key dup y/n? 0= while drop repeat ; ?)

  \ doc{
  \
  \ y/n ( -- c ) "y-slash-n"
  \
  \ Wait for a valid `key` press for a "yes/no" question and
  \ return its code _c_, which is `"y"` or `"n"`.
  \
  \ See: `y/n?`.
  \
  \ }doc

unneeding no? ?\ need y/n need "n" : no? ( -- f ) y/n "n" = ;

  \ doc{
  \
  \ no? ( -- f ) "no-question"
  \
  \ Wait for a valid `key` press for a `y/n` question
  \ and return _true_ if it's the current value of `"n"`,
  \ else return _false_.
  \
  \ See: `yes?`, `y/n?`.
  \
  \ }doc

unneeding yes?

?\ need y/n need "y" : yes? ( -- f ) y/n "y" = ;

  \ doc{
  \
  \ yes? ( -- f ) "yes-question"
  \
  \ Wait for a valid `key` press for a `y/n` question and
  \ return _true_ if it's the current value of `"y"`, else
  \ return _false_.
  \
  \ See: `no?`, `y/n?`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-04-24: Remove `char`, which has been moved to the
  \ library.
  \
  \ 2017-02-01: Replace `upper` with `lower`, because `upper`
  \ has been moved to the library.
  \
  \ 2018-02-17: Replace `value` with `cconstant`. Fully
  \ document, including pronunciation. Make all words
  \ individually accessible to `need`.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-07-22: Fix pronunciation of `"y"` and `"n"`.

  \ vim: filetype=soloforth
