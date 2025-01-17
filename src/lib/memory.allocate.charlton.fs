  \ memory.allocate.charlton.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201806041305
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ A standard implementation of the memory-allocation word
  \ set.

  \ There are five broad areas that the program covers;

  \      1, General purpose extensions to the Forth system.

  \      2, Creation of the heap and associated use of the data
  \      space.

  \      3, Allocation of space from the heap.

  \      4, Releasing space back to the heap.

  \      5, Altering the size of allocated heap space.

  \ The ANS word set consists of three words, `allocate`,
  \ `free`, and `resize` which give the minimum functionality
  \ required to use the heap. These are given in areas 3, 4 and
  \ 5 respectively.

  \ The heap is maintained as a doubly linked ordered circular
  \ list of nodes with an additional field noting the size of
  \ each node and whether it is in use. The size of the heap is
  \ specified by the constant `/heap`. the constant
  \ `hysteresis` controls the amount of spare space that is
  \ added to an allocation, to reduce the need for block moves
  \ during resizing.

  \ Initially there is only one node, the size of the heap.
  \ Aditional nodes are created by dividing an existing node
  \ into two parts. Nodes are removed by marking as free, and
  \ merging with adjoining free nodes. Nodes are altered in
  \ size by merging with a following free node, if possible,
  \ and a node being created above the new size of the node, if
  \ needed, or by allocating a new node and block moving the
  \ data field if necessary.

  \ Finding an available node is done by sequential search and
  \ comparison. The first node to be found that is large enough
  \ is used for allocation. Each search starts from the node
  \ most recently allocated, making this a "nextfit" algorithm.
  \ The redundancy in the head fields is required to optimise
  \ the search loop, as is the use of a sentinel to terminate
  \ the search once every node has been looked at, by always
  \ succeeding. A final refinement is the use of the sign bit
  \ of the size field to mark "in-use" nodes so that they are
  \ disregarded without a separate test.

  \ ===========================================================
  \ Authors

  \ Copyright Gordon Charlton, 1994-09-12.

  \ Adapted to Solo Forth by Marcos Cruz (programandala.net),
  \ 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ Solo Forth version of the code:

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ Original code:

  \ This is freeware, copyright Gordon Charlton, 12th of
  \ September 1994.  Copy and distribute it. Use it. Don't mess
  \ with this file. Acknowledge its use. I make no guarentees
  \ as to its fitness for any purpose. Tell me about any bugs.
  \ Tell me how much you like it.  <gordon at charlton dot
  \ demon dot co dot uk>

  \ ===========================================================

( charlton-heap-wordlist )

  \ XXX REMARK (old): 1614 bytes used

get-order get-current only forth definitions

  \ --------------------------------------------
  \ **1** Requirements

need max-n need heap

wordlist dup constant charlton-heap-wordlist
         dup set-current >order

  \ doc{
  \
  \ charlton-heap-wordlist ( -- wid )
  \
  \ _wid_ is the word-list identifier of the word list that
  \ holds the words the memory `heap` implementation adapted
  \ from code written by Gordon Charlton (1994-09-12).
  \
  \ ``need charlton-heap-wordlist`` is used to load the memory
  \ heap implementation and configure `allocate`, `resize`,
  \ `free` and `empty-heap` accordingly.
  \
  \ An alternative, simpler and smaller implementation of the
  \ memory heap is provided by `gil-heap-wordlist`.
  \
  \ The actual heap must be created with `allot-heap`,
  \ `limit-heap`, `farlimit-heap` or `bank-heap`, which are
  \ independent from the heap implemention.
  \
  \ }doc

  \ --------------------------------------------
  \ **2** Heap Creation

  \ ............................
  \ Constants

4 cells 1- cconstant hysteresis

  \ Node lengths are rounded up according to the value of
  \ `hysteresis` to reduce the number of block moves during
  \ `resize` operations. The value of this constant must be one
  \ less than a power of two and at least equal to one less
  \ than the size of a cell.

3 cells cconstant headsize

  \ A node on the heap consists of a three cell head followed
  \ by a variable length data space. The first cell in the head
  \ points to the next node in the heap. The second cell
  \ indicates the size of the node, and the third points to the
  \ previous node. The second cell is negated to indicate the
  \ node is in use. The heap consists of a doubly linked
  \ circular list. There is no special notation to indicate an
  \ empty list, as this situation cannot occur.

: adjustsize ( n1 -- n2 ) headsize + hysteresis or 1+ ;

  \ The amount of space that is requested for a node needs
  \ adjusting to include the length of the head, and to
  \ incorporate the hysteresis.

0 adjustsize constant overhead

  \ The size of the smallest possible node.

  \ ............................
  \ Structure

create sentinel  here cell+ ,  max-n ,  0 ,  0 ,

  \ A dummy node used to speed up searching the heap. The
  \ search, which is for a node larger than or equal to the
  \ specified size will always succeed.  The cell that points
  \ to the next node is set up so that the there is a zero
  \ three cells ahead of where it points, where the pointer to
  \ the previous node (ie the sentinel) should be. This is a
  \ special value that indicates the search has failed.

variable nextnode

  \ Searching is done using a "nextfit" algorithm. `nextnode`
  \ points to the most recently allocated node to indicate
  \ where the next search is to start from.

: >size ( a1 -- a2 ) cell+ ;

  \ Move from the "next" cell in the node head to the "size"
  \ cell. Within the word set nodes are referred to by the
  \ address of the "next" cell.  Externally they are referred
  \ to by the address of the start of the data field.

: >prev ( a1 -- a2 ) cell+ cell+ ;  -->

  \ Move from the "next" cell to the "previous" cell.

( charlton-heap-wordlist )

: charlton-empty-heap ( -- )
  heap dup nextnode !  dup dup !  dup /heap  over >size !
  >prev ! ;

  \ doc{
  \
  \ charlton-empty-heap ( -- )
  \
  \ Empty the current `heap`, which was created by
  \ `allot-heap`, `limit-heap`, `bank-heap` or `farlimit-heap`.
  \
  \ ``charlton-empty-heap`` is the action of `empty-heap` in
  \ the memory `heap` implementation adapted from code written
  \ by Gordon Charlton, whose words are defined in
  \ `charlton-heap-wordlist`.
  \
  \ See: `charlton-allocate`, `charlton-resize`,
  \ `charlton-free`.
  \
  \ }doc

  \ Initially the heap contains only one node, which is the
  \ same size as the heap. Both the "next" cell and the
  \ "previous" cell point to the "next" cell, as does
  \ `nextnode`.

  \ --------------------------------------------
  \ **3** Heap Allocation

  \ ............................
  \ List Searching

: attach ( a -- )
  >prev @  dup sentinel rot !  sentinel >prev ! ;

  \ The sentinel is joined into the nodelist. The "next" field
  \ of the node preceding the one specified (_a_) is set to
  \ point to the sentinel, and the "prev" field of the sentinel
  \ to point to the node that points to the sentinel.

: search ( a size -- a|0 )
  >r begin 2@ swap R@ < invert until  r> drop  >prev @ ;

  \ Search the nodelist, starting at the node specified
  \ (_a_), for a free node larger than or equal to the
  \ specified _size_.  Return the address of the first node
  \ that matches, or zero for no match. The heap structure is
  \ set up to make this a near optimal search loop. The "size"
  \ field is next to the "next" field so that both can be
  \ collected in a single operation (2@). Nodes in use have
  \ negated sizes so they never match the search. The
  \ "previous" field is included to allow the search to
  \ overshoot the match by one node and then link back outside
  \ the loop, rather than remembering the address of the node
  \ just examined. The sentinel removes the need for a separate
  \ test for failure. `search` assumes the sentinel is in
  \ place.

: detach ( a -- ) dup >prev @ ! ;

  \ Remake the link from the node prior to the one specified to
  \ the one specified. This will remove the sentinel if it is
  \ attached here. (It will be.)

: findspace ( size -- a|0 )
  nextnode @  dup attach  dup rot search  swap detach ;

  \ Search the nodelist for a node larger or equal to that
  \ specified. Return the address of a suitable node, or zero
  \ if none found. The search starts at the node pointed to by
  \ `nextnode`, the sentinal temporarily attached, the search
  \ proceeded with and the sentinel detached.

  \ ............................
  \ Head Creation

: fits ( size a -- f ) >size @ swap -  overhead  < ;

  \ Returns _true_ if the size of the node specified is the
  \ same as the specified size, or larger than it by less than
  \ the size of the smallest possible node. Returns _false_
  \ otherwise.

: togglesize ( a -- ) >size dup @  negate swap ! ;

  \ Negate the contents of the "size" field of the specified
  \ node. If the node was available it is marked as in use, and
  \ vice versa.

: next! ( a -- ) nextnode ! ;  -->

( charlton-heap-wordlist )

  \ Make the specified node the starting node for future
  \ searches of the node list.

: sizes! ( size a -- a ) 2dup + >r  >size 2dup @ swap -
                         r@ >size !   swap negate swap !  r> ;

  \ Given a free node (_a_), reduce its size to that
  \ specified and mark it as in use. Start to construct a new
  \ node within the specified node beyond its new length, by
  \ storing the length of the remainder of the node in the size
  \ field of the new node. Return the address of the partially
  \ constructed node.

: links! ( a1 a2 -- )
  2dup swap @  2dup  swap !  >prev !  2dup >prev !   swap ! ;

  \ _a1_ is an existing node. _a2_ is the address of a
  \ new node just above the existing node. Break the links from
  \ the existing node to the next node and from the next node
  \ to the existing node and join the new node to them.


  \ ANSI heap  --  Node Construction  ALLOCATE

: newnode ( size a -- ) tuck sizes!  links! ;

  \ Given a free node at _a_ split it into an in-use node of
  \ the specified size and a new free node above the in-use
  \ node.

: makenode ( size a -- )
  2dup fits if  togglesize drop  else  newnode  then ;

  \ Given a free node at a make an in-use node of the
  \ specified size and free the remainder, if there is any
  \ usable space left.

: charlton-allocate ( u -- a ior ) heap-in dup 0<
  if    #-59
  else  adjustsize dup findspace dup
        if    dup next! tuck makenode headsize + 0
        else  drop #-59 then
  then  heap-out ; -->

  \ Note: #-59 = `allocate` error code

  \ doc{
  \
  \ charlton-allocate ( u -- a ior )
  \
  \ Allocate _u_ bytes of contiguous data space. The data-space
  \ pointer is unaffected by this operation. The initial
  \ content of the allocated space is undefined.
  \
  \ If the allocation succeeds, _a_ is the starting
  \ address of the allocated space and _ior_ is zero.
  \
  \ If the operation fails, _a_ does not represent a valid
  \ address and the I/O resul code _ior_ is #-59, the `throw`
  \ code for `allocate`.
  \
  \ ``charlton-allocate`` is the action of `allocate` in the
  \ memory `heap` implementation adapted from code written by
  \ Gordon Charlton, whose words are defined in
  \ `charlton-heap-wordlist`.
  \
  \ See: `charlton-resize`, `charlton-free`.
  \
  \ }doc

  \ Original description:

  \ Make an in-use node with a data field at least _u_ bytes
  \ long.  Return the address of the data field and an ior of 0
  \ to indicate success.  If the space is not available return
  \ any old number and the standard ior.  The standard
  \ specifies that the argument to `allocate` is unsigned. As
  \ the implementation uses the sign bit of the size field for
  \ its own purposes any request for an amount of space greater
  \ than `max-n` must fail. As this would be a request for half
  \ the addressable memory or more this is not unreasonable.

( charlton-heap-wordlist )

  \ --------------------------------------------
  \ **4** Releasing Space

  \ ANSI heap  --  Head Destruction

: mergesizes ( a1 a2 -- ) >size @ swap >size +! ;

  \ Make the size field of the node at _a1_ equal to the sum
  \ of the sizes of the two specified nodes. In usage the node
  \ at _a2_ will be the one immediately above _a1_.

: mergelinks ( a1 a2 -- ) @ 2dup swap !  >prev ! ;

  \ The node at _a2_ is removed from the node list. As with
  \ `mergesizes` the node at _a2_ will be immediately above
  \ that at _a1_. Destroy the link from node1 to node2 and
  \ relink node1 to the node above node2. Destroy the backward
  \ link from the node above node2 and relink it to node1.

: jiggle ( -- ) nextnode @ @  >prev @  next! ;

  \ There is a possibility when a node is removed from the node
  \ list that `nextnode` may point to it. This is cured by
  \ making it point to the node prior to the one removed. We do
  \ not want to alter the pointer if it does not point to the
  \ removed node as that could be detrimental to the efficiency
  \ of the nextfit search algorithm. Rather than testing for
  \ this condition we jiggle the pointer about a bit to settle
  \ it into a linked node. This is done for reasons of
  \ programmer amusement. Specifically `nextnode` is set to
  \ point to the node pointed to by the "previous" field of the
  \ node pointed to in the "next" field of the node pointed to
  \ by `nextnode`. Ordinarily this is a no-op (ie I am my
  \ father's son) but when the node has had its links merged it
  \ sets `nextnode` to point to the node prior to the node it
  \ pointed to (ie when I died my father adopted my son, so now
  \ my son is my father's son).

: merge ( a -- ) dup @ 2dup mergesizes  mergelinks  jiggle ;

  \ Combine the node specified with the node above it. Merge
  \ the sizes, merge the lengths and jiggle.

  \ ............................
  \ Node Removal

: ?merge ( a1 a2 -- )
  >size @ 0> if dup dup @ u< if dup merge then then drop ;

  \ Merge the node at _a1_ with the one above it on two
  \ conditions, firstly that the node at _a2_ is free, and
  \ secondly that the node pointed to by the next field in
  \ _a1_ is actually above _a1_ (ie that it does not wrap
  \ around because it is the topmost node). In usage _a2_
  \ will be either _a1_ or the node above it. In each
  \ instance the other affected node (either the node above
  \ _a1_ or _a1_) is known to be free, so no test is
  \ needed for this.

: ?mergenext ( a -- ) dup @ ?merge ;

  \ Merge the node following the specified node with the
  \ specified node, if following node is free.

: ?mergeprev ( a -- ) >prev @ dup ?merge ;

  \ Merge the specified node with the one preceding it, if the
  \ preceding node is free.

: charlton-free ( a -- ior )
  heap-in headsize - dup togglesize dup ?mergenext ?mergeprev 0
  heap-out ;

  \ doc{
  \
  \ charlton-free ( a -- ior )
  \
  \ Return the contiguous region of data space indicated by _a_
  \ to the system for later allocation. _a_ shall indicate a
  \ region of data space that was previously obtained by
  \ `charlton-allocate` or `charlton-resize`.
  \
  \ As there is no compelling reason for this to fail, _ior_ is
  \ zero.
  \
  \ ``charlton-free`` is the action of `free` in the memory
  \ `heap` implementation adapted from code written by Gordon
  \ Charlton, whose words are defined in
  \ `charlton-heap-wordlist`.
  \
  \ }doc

  \ Original description:
  \
  \ Mark the specified in-use word as free, and merge with any
  \ adjacent free space. As this is a standard word _a_ is the
  \ address of the data field rather than the "next" field. As
  \ there is no compelling reason for this to fail the ior is
  \ zero.

  \ --------------------------------------------
  \ **5** Resizing Allocated Space

  \ ............................
  \ Node Repairing

variable stash

  \ The `resize` algorithm is simplified and made faster by
  \ assuming that it will always succeed. `stash` holds the
  \ minimum information required to make good when it fails.

: savelink ( a -- ) @ stash ! ;

  \ Save the contents of the `>next` field of the node being
  \ `resize`d in `stash` (above).

: restorelink ( a -- ) stash @ swap ! ;

  \ Converse operation to `savelink` (above).

: fixprev ( a -- ) dup >prev @ ! ; -->

  \ The `>next` field of the node prior to the node being
  \ `resize`d should point to the node being `resize`d. it may
  \ very well do already, but this makes sure.

( charlton-heap-wordlist )

: fixnext ( a -- ) dup @ >prev ! ;

  \ The `>prev` field of the node after the node resized may
  \ need correcting.  This corrects it whether it needs it or
  \ not.  (Its quicker just to do it than to check first.)

: fixlinks ( a -- ) dup fixprev  dup fixnext  @ fixnext ;

  \ `resize` may very well merge its argument node with the
  \ previous one. It may very well merge that with the next
  \ one. This means we need to fix the previous one, the next
  \ one and the one after next. To extend the metaphor started
  \ in the description of `jiggle` (above), not only did I die,
  \ but my father did too. This brings my grandfather into the
  \ picture as guardian of my son. Now to confound things we
  \ have all come back to life. I still remember who my son is,
  \ and my father remembers who his father is. Once I know who
  \ my father is I can tell my son that I am his father, I can
  \ tell my father that I am his son and my grandfather who his
  \ son is. Thankfully we are only concerned about the male
  \ lineage here! (In fact nodes reproduce by division, like
  \ amoebae, which is where the metaphor breaks down -- (1)
  \ they are sexless and (2) which half is parent and which
  \ child?)

: fixsize ( a -- )
  dup >size @ 0> if   dup @ 2dup <
                      if over - swap >size ! else 2drop then
                 else drop then ;

  \ Reconstruct the size field of a node from the address of
  \ the head and the contents of the `>next` field provided
  \ that the node is free and it is not the topmost node in the
  \ heap (ie there is no wraparound). Both these conditions
  \ need to be true for the node to have been merged with its
  \ successor.

: fixsizes ( a -- ) dup fixsize  >prev @ fixsize ;

  \ The two nodes whose size fields may need repairing are the
  \ one passed as an argument to `resize` (damaged by
  \ `?mergenext`) and its predecessor (damaged by `?mergeprev`).

: repair ( a -- )
  dup restorelink dup fixlinks dup fixsizes togglesize ;

  \ Make good the damage done by `resize`. Restore the `>next`
  \ field, fix the links, fix the size fields and mark the node
  \ as in-use. Note that this may not restore the system to
  \ exactly how it was. In particular the pointer `nextnode`
  \ may have moved back one or two nodes by virtue of having
  \ been `jiggle`d about if it happened to be pointing to the
  \ wrong node. This is not serious, so I have chosen to ignore
  \ it.

  \ ............................
  \ Node Movement

: toobig? ( a size -- f ) swap  >size @  > ;

  \ _f_ is true if the node at _a_ is smaller than the
  \ specified size.

: copynode ( a1 a2 -- )
  over >size @  headsize -  rot  headsize + rot rot move ;

  \ Move the contents of the data field of the node at _a1_
  \ to the data field at _a2_. Assumes _a2_ is large
  \ enough. It will be.

: enlarge ( a1 size -- a2 ior )
  over ?mergeprev  allocate dup >r
  if  swap repair  else  tuck copynode  then  r> ; -->

  \ Make a new node of the size specified. Copy the data field
  \ of _a1_ to the new node. Merge the node at a1 with
  \ the one preceding it, if possible. This last behaviour is
  \ to finish off removing the node at _a1_. The word
  \ `adjust` (below) starts removing the node. The node is
  \ removed before allocation to increase the probability of
  \ `allocate` succeeding. The address returned by `enlarge` is
  \ that returned by `allocate`, which is that of the data
  \ field, not the head. If the allocation fails repair the
  \ damage done by removing the node at _a1_.

( charlton-heap-wordlist )

  \ ............................
  \ Node Restructuring

: adjust ( a1 size1 -- a2 size2 )
  adjustsize >r  headsize -  dup savelink  dup togglesize
  dup ?mergenext r> ;

  \ _a1_ points to the data field of a node, not the "next"
  \ field. This needs correcting. _Size1_ also needs adjusting
  \ as per `adjustsize`. In addition it is easier to work with
  \ free nodes than live ones as the size field is correct,
  \ and, as we intend to change the nodes size we will
  \ inevitably want to muck about with the next node, if its
  \ free, so lets merge with it straight away. Sufficient
  \ information is first saved to put the heap back as it was,
  \ if necessary.  Now we are ready to get down to business.

: charlton-resize ( a1 u -- a2 ior )
  heap-in dup 0< if    drop #-61  \ `resize` error code
                 else  adjust  2dup toobig?
                       if enlarge
                       else over makenode headsize + 0 then
                 then  heap-out ;

  \ doc{
  \
  \ charlton-resize ( a1 u -- a2 ior )
  \
  \ Change the allocation of the contiguous data space starting
  \ at the address _a1_, previously allocated  by
  \ `charlton-allocate` or ``charlton-resize``, to _u_ bytes.
  \ _u_ may be either larger or smaller than the current size
  \ of the region. The data-space pointer is unaffected by this
  \ operation.
  \
  \ If the operation succeeds, _a2_ is  the starting
  \ address of  _u_ bytes of allocated  memory and _ior_ is
  \ zero.  _a2_ may be,  but need not be,  the same as _a1_.
  \ If they are  not the same,  the values contained in the
  \ region at _a1_ are copied to _a2_, up to the minimum size
  \ of either of  the two regions. If they are the same, the
  \ values contained in the region are preserved to the minimum
  \ of _u_ or the original size. If _a2_ is not the same as
  \ _a1_, the region of memory at _a1_ is returned to the
  \ system according to the operation of `free`.
  \
  \ If the operation fails, _a2_ equals _a1_, the region of
  \ memory at _a1_ is unaffected, and  the I/O result code
  \ _ior_ is #-61, the `throw` code for `resize`.
  \
  \ ``charlton-resize`` is the action of `resize` in the memory
  \ `heap` implementation adapted from code written by Gordon
  \ Charlton, whose words are defined in
  \ `charlton-heap-wordlist`.
  \
  \ }doc

  \ Original description:
  \
  \ Resize the node at _a1_ to the specified size _u_. Return
  \ the address of the resized node _a2_ along with an _ior_ of
  \ zero if successful and #-61 if not. _a2_ may be the same
  \ as, or different to, _a1_.  If _ior_ is non-zero then _a2_
  \ is not meaningful. Being a standard word the arguments need
  \ adjusting to the internal representation on entry, and back
  \ again on exit. If after the first merge the requested size
  \ is still too large to reuse the specified node then it is
  \ moved to a larger node and the specified node released. If,
  \ on the other hand the request is not too big for the node,
  \ then we remake the node at the right length, and free any
  \ space at the top using `makenode`, which has just the right
  \ functionality.  In this case the ior is zero. As this is a
  \ standard word it takes an unsigned size argument, but
  \ excessive requests fail automatically, as with
  \ `charlton-allocate`.

forth-wordlist set-current

need empty-heap ' charlton-empty-heap ' empty-heap defer!
need allocate   ' charlton-allocate   ' allocate   defer!
need resize     ' charlton-resize     ' resize     defer!
need free       ' charlton-free       ' free       defer!

set-current set-order

  \ ===========================================================
  \ Change log

  \ 2016-05-18: Need `vocabulary`, which has been moved to the
  \ library.
  \
  \ 2016-11-26: Improve `>prev`.
  \
  \ 2016-12-30: Compact the code, saving three blocks. Change
  \ the stack notation after the convention used in Solo Forth.
  \
  \ 2017-03-30: Improve documentation.
  \
  \ 2017-04-01: Use `wordlist` instead of `vocabulary`. Use the
  \ `max-n` constant instead of calculating `maxpos`.
  \
  \ 2017-04-09: Improve documentation. Use `heap-in` and
  \ `heap-out`.  Add the "charlton-" prefix to the interface
  \ words, which are the actions associated to the standard
  \ words.
  \
  \ 2017-04-16: Improve documentation.
  \
  \ 2018-03-09: Update notation "address units" to "bytes".
  \
  \ 2018-04-16: Improve description of _ior_ notation.
  \
  \ 2018-06-04: Update documentation: remove mentions of
  \ aligned addresses.

  \ vim: filetype=soloforth
