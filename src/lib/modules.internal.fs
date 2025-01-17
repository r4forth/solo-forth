  \ modules.internal.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803052149
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Simple and small implementation of unnamed modules.
  \
  \ Modules hide the internal implementation and leave visible
  \ the words of the outer interface.
  \
  \ This implementation uses the data stack for temporary
  \ values and does no error checking.

  \ ===========================================================
  \ Authors

  \ Deway Val Schorre wrote the original code for fig-Forth,
  \ which was published on the article _Structured programming
  \ by adding modules to FORTH_, on Forth Dimensions (volume 2,
  \ number 5, page 132, 1981-01).

  \ Marcos Cruz (programandala.net), adapted it to Solo Forth,
  \ 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( internal end-internal unlink-internal hide-internal )

unneeding internal unneeding end-internal and ?( need alias

' current-latest alias internal ( -- nt )

  \ doc{
  \
  \ internal ( -- nt )
  \
  \ Start internal (private) definitions.  Return the _nt_ of
  \ the latest word created in the compilation word list.
  \
  \ The end of the internal definitions is marked by
  \ `end-internal`. Then those definitions can be unlinked by
  \ `unlink-internal` or hidden by `hide-internal`.
  \
  \ See: `isolate`, `module`, `package`, `privatize`,
  \ `seclusion`.
  \
  \ }doc

' np@ alias end-internal ( -- a ) ?)

  \ doc{
  \
  \ end-internal ( -- a )
  \
  \ End internal (private) definitions.  Return the current
  \ value of the headers pointer, which is the _xtp_ (execution
  \ token pointer) of the next word defined.
  \
  \ The start of the internal definitions was marked by
  \ `internal`. The internal definitions can be unlinked by
  \ `unlink-internal` or hidden by `hide-internal`.
  \
  \ }doc

unneeding unlink-internal ?( need internal need >>link

: unlink-internal ( nt xtp -- ) >>link far! ; ?)

  \ XXX TODO -- `>>link far!` is used also in
  \ `forget-transient`; reuse it

  \ doc{
  \
  \ unlink-internal ( nt xtp -- )
  \
  \ Unlink all words defined between the latest pair `internal`
  \ and `end-external`, linking the first word after
  \ `end-internal` to the word before `internal`, thus making
  \ all the internal words skipped by the dictionary searches.
  \
  \ Usage example:

  \ ----
  \ internal
  \
  \ : hello ( -- ) ." hello" ;
  \
  \ end-internal
  \
  \ : salute ( -- ) hello ;
  \
  \ unlink-internal
  \
  \ salute  \ ok!
  \ hello   \ error!
  \ ----

  \ At least one word must be defined between `end-internal`
  \ and ``unlink-internal``.
  \
  \ The alternative word `hide-internal` can be used instead of
  \ ``unlink-internal`` in order to keep the internal words
  \ searchable.
  \
  \ }doc

  \ XXX TODO -- Add this when `traverse-wordlist` is
  \ implemented:
  \
  \ searchable, e.g. accessible for `traverse-wordlist`.

unneeding hide-internal ?(

need internal need name<name need >>name

: hide-internal ( nt xtp -- )
  >>name name<name ( nt1 nt2 ) begin   2dup swap u>
                               while   dup hidden name<name
                               repeat  2drop ; ?)

  \ doc{
  \
  \ hide-internal ( nt xtp -- )
  \
  \ Hide all words defined between the latest pair `internal`
  \ and `end-external`, setting the `smudge` bit of their
  \ headers.
  \
  \ Usage example:

  \ ----
  \ internal
  \
  \ : hello ( -- ) ." hello" ;
  \
  \ end-internal
  \
  \ : salute ( -- ) hello ;
  \
  \ hide-internal
  \
  \ salute  \ ok!
  \ hello   \ error!
  \ ----

  \ At least one word must be defined between `end-internal`
  \ and ``hide-internal``.
  \
  \ The alternative word `unlink-internal` uses a different,
  \ simpler method: it unlinks the internal words from the
  \ dictionary.
  \
  \ `privatize` uses a similar method, but it has error
  \ checking and does not use the stack.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-10-27: First version, as a simpler alternative to
  \ `privatize`, even with the same names at first.
  \
  \ 2016-04-26: Use `current-latest` (old fig-Forth `latest`)
  \ instead of current standard `latest`. Update the
  \ documentation.
  \
  \ 2016-05-06: Update the requirements: `current-lastest`
  \ moved to the kernel.
  \
  \ 2016-11-13: Rename `np@` to `hp@` after the changes in the
  \ kernel.
  \
  \ 2016-11-18: Adapt to far memory.
  \
  \ 2016-12-07: Rename `external` to `end-internal`, `module`
  \ to `unlink-internal`. Rename the module file accordingly.
  \ Add `hide-internal`.
  \
  \ 2017-01-05: Remove old system bank support from
  \ `unlink-internal`.
  \
  \ 2017-01-19: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-02-26: Update "hp" notation to "np", after the changes
  \ in the kernel.
  \
  \ 2017-03-14: Improve documentation.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.

  \ vim: filetype=soloforth
