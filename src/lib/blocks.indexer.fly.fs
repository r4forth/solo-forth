  \ blocks.indexer.fly.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT -- not usable yet

  \ Last modified: 201806041144
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Blocks Fly Indexer
  \
  \ A blocks indexer that changes the default action of
  \ `need` and related words: The disk is searched for the
  \ needed word only when it's not found in the blocks index,
  \ and the searched blocks are indexed on the fly.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( use-fly-index )

only forth definitions

need common-indexer need get-order need set-order
need bit-array need catch need use-default-need

blocks/disk bit-array indexed-blocks

  \ doc{
  \
  \ indexed-blocks ( -- ca )
  \
  \ Bit array to mark the indexed blocks
  \
  \ See: `use-fly-index`.
  \
  \ }doc

: indexed-block? ( block -- f ) indexed-blocks @bit ;

  \ doc{
  \
  \ indexed-block? ( block -- f ) "indexed-block-question"
  \
  \ Is block _block_ indexed?
  \
  \ See: `use-fly-index`.
  \
  \ }doc

: block-indexed ( block -- ) indexed-blocks !bit ;

  \ doc{
  \
  \ block-indexed ( block -- )
  \
  \ Mark block _block_ as indexed.
  \
  \ See: `use-fly-index`.
  \
  \ }doc

-->

( use-fly-index )

: index-block ( block -- )
  get-current get-order set-index-order
  ['] (index-block catch  dup #-278 <> swap ?throw
  set-order set-current block-indexed ;
  \ Index block _block_.

  \ XXX TODO -- #-278 \ empty block found: quit indexing

  \ doc{
  \
  \ index-block ( block -- )
  \
  \ Index block _block_.
  \
  \ See: `use-fly-index`.
  \
  \ }doc

: ?index-block ( block -- ) ~~
  dup indexed-block? if  drop exit  then  index-block ;

  \ doc{
  \
  \ ?index-block ( block -- ) "question-index-block"
  \
  \ Index block _block_, if not done before.
  \
  \ See: `use-fly-index`.
  \
  \ }doc

: fly-located ( ca len -- block | false ) ~~
  2dup indexed-name? if  ~~ nip nip load exit
  then  ~~ (located ;

  \ doc{
  \
  \ fly-located ( ca len -- block | 0 )
  \
  \ Locate the first block whose header contains the string _ca
  \ len_ (surrounded by spaces), and return its number. If not
  \ found, return zero.  The search is case-sensitive.
  \ Index all searched blocks on the fly.
  \
  \ See: `use-fly-index`.
  \
  \ }doc

: use-fly-index ( -- )
  use-default-need
  ['] fly-located     ['] located   defer!
  ['] ?index-block    ['] unlocated defer! ;

  \ doc{
  \
  \ use-fly-index ( -- )
  \
  \ Set the alternative action of `need`, `needed`, `reneed`,
  \ `reneeded`, `located` and `unlocated` in order to use the
  \ blocks index and index the searched blocks on the fly.
  \
  \ The default action of all said words can be restored by
  \ `use-no-index`.
  \
  \ See: `use-thru-index`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-04-19: Start, based on the code of `indexer`.
  \
  \ 2016-11-24: Rename the module and some words, to be
  \ consistent with the previous version Thru Indexer.
  \
  \ 2016-11-25: Factor `set-index-order`. Improve documentation
  \ and names.
  \
  \ 2016-11-26: Need `catch`, which has been moved to the
  \ library.  Rename `blocks` to `blk/disk` after the fix in
  \ the kernel.
  \
  \ 2017-02-17: Update notation "behaviour" to "action".
  \ Update cross references.
  \
  \ 2017-02-21: Need `use-default-need`, which now is optional.
  \
  \ 2017-03-13: Update names including "rec" to "sector(s)";
  \ update names including "blk" to "block(s)".
  \
  \ 2018-02-05: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-06-04: Update: remove trailing closing paren from
  \ word names.

  \ vim: filetype=soloforth
