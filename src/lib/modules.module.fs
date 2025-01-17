  \ modules.module.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703142257
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Implementation of VFX Forth's `module`.

  \ ===========================================================
  \ Authors

  \ Ulrich Hoffmann wrote the original version, "Modules", for
  \ Forth-94 and Forth-2012, 2015, 2016.

  \ Marcos Cruz (programandala.net) integrated the code into
  \ Solo Forth and improved it using the specific features of
  \ the system, 2016.

  \ ===========================================================
  \ License

  \ The MIT License (MIT)

  \ Copyright (c) 2015,2016 Ulrich Hoffmann
  \ Copyright (c) 2016 Marcos Cruz (programandala.net)

  \ Permission is hereby granted, free of charge, to any person
  \ obtaining a copy of this software and associated
  \ documentation files (the "Software"), to deal in the
  \ Software without restriction, including without limitation
  \ the rights to use, copy, modify, merge, publish,
  \ distribute, sublicense, and/or sell copies of the Software,
  \ and to permit persons to whom the Software is furnished to
  \ do so, subject to the following conditions:

  \ The above copyright notice and this permission notice shall
  \ be included in all copies or substantial portions of the
  \ Software.

  \ THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY
  \ KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
  \ WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
  \ PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
  \ OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
  \ OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
  \ OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
  \ SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

  \ ===========================================================
  \ References

  \ http://theforth.net/package/modules
  \ http://www.mpeforth.com/vfxcom.htm

( module end-module export )

need alias need nextname

  \ XXX REMARK -- 2016-12-07:
  \     Data space used:
  \       Code           59 B
  \       Requirements  173 B  (`alias` and `nextname`)

: module ( "name" -- parent-wid )
  get-current  wordlist dup >r constant
  r@ >order  r> set-current ;

  \ doc{
  \
  \ module ( "name" -- parent-wid )
  \
  \ Start the definition of a new module named _name_.
  \ `end-module` ends the module and `export` exports a word.
  \
  \ Usage example:

  \ ----
  \ module greet
  \
  \   : hello ( -- ) ." Hello" ;
  \   : mods ( -- ) ." Modules" ;
  \
  \   : hi ( -- ) hello ." , " mods ." !" cr ;
  \
  \ export hi
  \
  \ end-module
  \ ----

  \ Now only the exported definitions of the module are
  \ available.

  \ ----
  \ hi      \ displays "Hello, Modules!"
  \ hello   \ error, not found
  \ ----

  \ The module name is defined as a constant that holds the
  \ word list identifier the module words are defined into.
  \ Therefore, to expose the internal words of a module, you
  \ can use `name >order`, where _name_ is the name of the
  \ module.
  \
  \ See: `internal`, `isolate`, `package`, `privatize`,
  \ `seclusion`.
  \
  \ }doc

: export ( parent-wid "name" -- parent-wid )
  dup get-current  defined name>  parsed-name 2@ nextname
  rot set-current alias  set-current ;

  \ doc{
  \
  \ export ( parent-wid "name" -- parent-wid )
  \
  \ Make the word named _name_ accessible outside the `module`
  \ currently defined.  _name_ will be still available after
  \ `end-module`.
  \
  \ }doc

: end-module ( parent-wid -- ) set-current previous ;

  \ doc{
  \
  \ end-module ( parent-wid -- )
  \
  \ End a `module` definition. All module internal words are no
  \ longer accessible.  Only words that have been exported with
  \ `export` are still available.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-12-07: Start. Copy the code and documentation of
  \ Modules 1.0.2 (http://theforth.net/package/modules) and
  \ modify the code style after the conventions used in Solo
  \ Forth.  Improve `end-module` with `perform`; `export` with
  \ `previous`; `module` with `constant`; `expose-module` with
  \ `execute >order`. Rewrite `export` to improve it with
  \ `alias`. Document with example from the Modules'
  \ documentation. Remove `expose-module` (it's just parsing
  \ syntactic sugar); explain its simple alternative in the
  \ documentation.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-03-14: Improve documentation.

  \ vim: filetype=soloforth
