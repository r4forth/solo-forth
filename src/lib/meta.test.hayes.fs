  \ meta.test.hayes.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201804152333
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ This program tests the core words of an ANS Forth system.
  \
  \ The test assumes a two's complement implementation where
  \ the range of signed numbers is -2^(n-1) ... 2^(n-1)-1 and
  \ the range of unsigned numbers is 0 ... 2^(n)-1.
  \
  \ Some words are not tested: `key`, `quit`, `abort`, `abort"`
  \ `environment?`...

  \ ===========================================================
  \ Authors

  \ John Hayes S1I, 1995-11-27.

  \ Marcos Cruz (programandala.net) adapted it to Solo Forth,
  \ 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ Original version:

  \ (C) 1995 JOHNS HOPKINS UNIVERSITY / APPLIED PHYSICS
  \ LABORATORY MAY BE DISTRIBUTED FREELY AS LONG AS THIS
  \ COPYRIGHT NOTICE REMAINS.  VERSION 1.2

  \ This version:

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( hayes-test )

need hayes-tester need do  verbose on  hex

{ -> }  \ start with clean slate

  \ Test if any bits are set; answer in base 1.

{ : bitsset? if 0 0 else 0 then ; -> }
{  0 bitsset? -> 0 } {  1 bitsset? -> 0 0 }
  \ Zero is all bits clear; other number have at least one bit.

{ -1 bitsset? -> 0 0 }

testing( invert and or xor)

{ 0 0 and -> 0 } { 0 1 and -> 0 }
{ 1 0 and -> 0 } { 1 1 and -> 1 }

{ 0 invert 1 and -> 1 } { 1 invert 1 and -> 0 }

0  constant 0s  0 invert constant 1s

{ 0s invert -> 1s } { 1s invert -> 0s } { 0s 0s and -> 0s }
{ 0s 1s and -> 0s } { 1s 0s and -> 0s } { 1s 1s and -> 1s }
{ 0s 0s or -> 0s } { 0s 1s or -> 1s } { 1s 0s or -> 1s }
{ 1s 1s or -> 1s } { 0s 0s xor -> 0s } { 0s 1s xor -> 1s }
{ 1s 0s xor -> 1s } { 1s 1s xor -> 0s }  -->

( hayes-test )

testing( 2* 2/) need rshift need 2/

  \ We trust `1s`, `invert`, and `bitsset?`; we will confirm
  \ `rshift` later.

1s 1 rshift invert constant msb { msb bitsset? -> 0 0 }

{ 0s 2* -> 0s } { 1 2* -> 2 } { 4000 2* -> 8000 }
{ 1s 2* 1 xor -> 1s } { msb 2* -> 0s } { 0s 2/ -> 0s }
{ 1 2/ -> 0 } { 4000 2/ -> 2000 }
{ 1s 2/ -> 1s }  \ msb propagated
{ 1s 1 xor 2/ -> 1s } { msb 2/ msb and -> msb }  -->

( hayes-test )

testing( lshift rshift) need lshift need rshift

{ 1 0 lshift -> 1 } { 1 1 lshift -> 2 } { 1 2 lshift -> 4 }
{ 1 f lshift -> 8000 }  \ biggest guaranteed shift
{ 1s 1 lshift 1 xor -> 1s } { msb 1 lshift -> 0 }

{ 1 0 rshift -> 1 } { 1 1 rshift -> 0 } { 2 1 rshift -> 1 }
{ 4 2 rshift -> 1 }
{ 8000 f rshift -> 1 }  \ biggest
{ msb 1 rshift msb and -> 0 }  \ rshift zero fills msbs
{ msb 1 rshift 2* -> msb }  -->

( hayes-test )

testing( 0= = 0< < > u< min max)

0 invert  constant max-uint
0 invert 1 rshift  constant max-int
0 invert 1 rshift invert  constant min-int
0 invert 1 rshift  constant mid-uint
0 invert 1 rshift invert  constant mid-uint+1

0s constant <false> 1s constant <true>

{ 0 0= -> <true> } { 1 0= -> <false> } { 2 0= -> <false> }
{ -1 0= -> <false> } { max-uint 0= -> <false> }
{ min-int 0= -> <false> } { max-int 0= -> <false> }  -->

( hayes-test )

{ 0 0 = -> <true> } { 1 1 = -> <true> } { -1 -1 = -> <true> }
{ 1 0 = -> <false> } { -1 0 = -> <false> } { 0 1 = -> <false> }
{ 0 -1 = -> <false> } { 0 0< -> <false> } { -1 0< -> <true> }
{ min-int 0< -> <true> } { 1 0< -> <false> }
{ max-int 0< -> <false> } { 0 1 < -> <true> }
{ 1 2 < -> <true> } { -1 0 < -> <true> } { -1 1 < -> <true> }
{ min-int 0 < -> <true> } { min-int max-int < -> <true> }
{ 0 max-int < -> <true> } { 0 0 < -> <false> }
{ 1 1 < -> <false> } { 1 0 < -> <false> } { 2 1 < -> <false> }
{ 0 -1 < -> <false> } { 1 -1 < -> <false> }
{ 0 min-int < -> <false> } { max-int min-int < -> <false> }
{ max-int 0 < -> <false> } { 0 1 > -> <false> }
{ 1 2 > -> <false> } { -1 0 > -> <false> }
{ -1 1 > -> <false> } { min-int 0 > -> <false> }
{ min-int max-int > -> <false> } { 0 max-int > -> <false> } -->

( hayes-test )

{ 0 0 > -> <false> } { 1 1 > -> <false> } { 1 0 > -> <true> }
{ 2 1 > -> <true> } { 0 -1 > -> <true> } { 1 -1 > -> <true> }
{ 0 min-int > -> <true> } { max-int min-int > -> <true> }
{ max-int 0 > -> <true> } { 0 1 u< -> <true> }
{ 1 2 u< -> <true> } { 0 mid-uint u< -> <true> }
{ 0 max-uint u< -> <true> } { mid-uint max-uint u< -> <true> }
{ 0 0 u< -> <false> } { 1 1 u< -> <false> }
{ 1 0 u< -> <false> } { 2 1 u< -> <false> }
{ mid-uint 0 u< -> <false> } { max-uint 0 u< -> <false> }
{ max-uint mid-uint u< -> <false> } { 0 1 min -> 0 }
{ 1 2 min -> 1 } { -1 0 min -> -1 } { -1 1 min -> -1 }
{ min-int 0 min -> min-int } { min-int max-int min -> min-int }
{ 0 max-int min -> 0 } { 0 0 min -> 0 } { 1 1 min -> 1 }
{ 1 0 min -> 0 } { 2 1 min -> 1 } -->

( hayes-test )

{ 0 -1 min -> -1 } { 1 -1 min -> -1 }
{ 0 min-int min -> min-int } { max-int min-int min -> min-int }
{ max-int 0 min -> 0 } { 0 1 max -> 1 } { 1 2 max -> 2 }
{ -1 0 max -> 0 } { -1 1 max -> 1 } { min-int 0 max -> 0 }
{ min-int max-int max -> max-int } { 0 max-int max -> max-int }
{ 0 0 max -> 0 } { 1 1 max -> 1 } { 1 0 max -> 1 }
{ 2 1 max -> 2 } { 0 -1 max -> 0 } { 1 -1 max -> 1 }
{ 0 min-int max -> 0 } { max-int min-int max -> max-int }
{ max-int 0 max -> max-int }  -->

( hayes-test )

testing( stack ops)

{ 1 2 2drop -> } { 1 2 2dup -> 1 2 1 2 }
{ 1 2 3 4 2over -> 1 2 3 4 1 2 } { 1 2 3 4 2swap -> 3 4 1 2 }
{ 0 ?dup -> 0 } { 1 ?dup -> 1 1 } { -1 ?dup -> -1 -1 }
{ depth -> 0 } { 0 depth -> 0 1 } { 0 1 depth -> 0 1 2 }
{ 0 drop -> } { 1 2 drop -> 1 } { 1 dup -> 1 1 }
{ 1 2 over -> 1 2 1 } { 1 2 3 rot -> 2 3 1 }
{ 1 2 swap -> 2 1 }

testing( >r r> r@)

{ : gr1 >r r> ; -> } { : gr2 >r r@ r> drop ; -> }
{ 123 gr1 -> 123 } { 123 gr2 -> 123 }
{ 1s gr1 -> 1s }  \ return stack holds cells

-->

( hayes-test )

testing( + - 1+ 1- abs negate)

{ 0 5 + -> 5 } { 5 0 + -> 5 } { 0 -5 + -> -5 } { -5 0 + -> -5 }
{ 1 2 + -> 3 } { 1 -2 + -> -1 } { -1 2 + -> 1 }
{ -1 -2 + -> -3 } { -1 1 + -> 0 }
{ mid-uint 1 + -> mid-uint+1 } { 0 5 - -> -5 } { 5 0 - -> 5 }
{ 0 -5 - -> 5 } { -5 0 - -> -5 } { 1 2 - -> -1 }
{ 1 -2 - -> 3 } { -1 2 - -> -3 } { -1 -2 - -> 1 }
{ 0 1 - -> -1 } { mid-uint+1 1 - -> mid-uint } { 0 1+ -> 1 }
{ -1 1+ -> 0 } { 1 1+ -> 2 } { mid-uint 1+ -> mid-uint+1 }
{ 2 1- -> 1 } { 1 1- -> 0 } { 0 1- -> -1 }
{ mid-uint+1 1- -> mid-uint } { 0 negate -> 0 }
{ 1 negate -> -1 } { -1 negate -> 1 } { 2 negate -> -2 }
{ -2 negate -> 2 } { 0 abs -> 0 } { 1 abs -> 1 }
{ -1 abs -> 1 } { min-int abs -> mid-uint+1 } -->

( hayes-test )

testing( s>d * m* um*)

{ 0 s>d -> 0 0 } { 1 s>d -> 1 0 } { 2 s>d -> 2 0 }
{ -1 s>d -> -1 -1 } { -2 s>d -> -2 -1 }
{ min-int s>d -> min-int -1 } { max-int s>d -> max-int 0 }
{ 0 0 m* -> 0 s>d } { 0 1 m* -> 0 s>d } { 1 0 m* -> 0 s>d }
{ 1 2 m* -> 2 s>d } { 2 1 m* -> 2 s>d } { 3 3 m* -> 9 s>d }
{ -3 3 m* -> -9 s>d } { 3 -3 m* -> -9 s>d }
{ -3 -3 m* -> 9 s>d } { 0 min-int m* -> 0 s>d }
{ 1 min-int m* -> min-int s>d } { 2 min-int m* -> 0 1s }
{ 0 max-int m* -> 0 s>d } { 1 max-int m* -> max-int s>d }
{ 2 max-int m* -> max-int 1 lshift 0 }
{ min-int min-int m* -> 0 msb 1 rshift }
{ max-int min-int m* -> msb msb 2/ }
{ max-int max-int m* -> 1 msb 2/ invert } -->

( hayes-test )

  \ testing( Identities)

{ 0 0 * -> 0 } { 0 1 * -> 0 } { 1 0 * -> 0 } { 1 2 * -> 2 }
{ 2 1 * -> 2 } { 3 3 * -> 9 } { -3 3 * -> -9 } { 3 -3 * -> -9 }
{ -3 -3 * -> 9 }

{ mid-uint+1 1 rshift 2 * -> mid-uint+1 }
{ mid-uint+1 2 rshift 4 * -> mid-uint+1 }
{ mid-uint+1 1 rshift mid-uint+1 or 2 * -> mid-uint+1 }

{ 0 0 um* -> 0 0 } { 0 1 um* -> 0 0 } { 1 0 um* -> 0 0 }
{ 1 2 um* -> 2 0 } { 2 1 um* -> 2 0 } { 3 3 um* -> 9 0 }

{ mid-uint+1 1 rshift 2 um* -> mid-uint+1 0 }
{ mid-uint+1 2 um* -> 0 1 } { mid-uint+1 4 um* -> 0 2 }
{ 1s 2 um* -> 1s 1 lshift 1 }
{ max-uint max-uint um* -> 1 1 invert } -->

( hayes-test )

testing( fm/mod sm/rem um/mod */ */mod / /mod mod)

need fm/mod need */

{ 0 s>d 1 fm/mod -> 0 0 } { 1 s>d 1 fm/mod -> 0 1 }
{ 2 s>d 1 fm/mod -> 0 2 } { -1 s>d 1 fm/mod -> 0 -1 }
{ -2 s>d 1 fm/mod -> 0 -2 } { 0 s>d -1 fm/mod -> 0 0 }
{ 1 s>d -1 fm/mod -> 0 -1 } { 2 s>d -1 fm/mod -> 0 -2 }
{ -1 s>d -1 fm/mod -> 0 1 } { -2 s>d -1 fm/mod -> 0 2 }
{ 2 s>d 2 fm/mod -> 0 1 } { -1 s>d -1 fm/mod -> 0 1 }
{ -2 s>d -2 fm/mod -> 0 1 } {  7 s>d  3 fm/mod -> 1 2 }  -->

( hayes-test )

{  7 s>d -3 fm/mod -> -2 -3 } { -7 s>d  3 fm/mod -> 2 -3 }
{ -7 s>d -3 fm/mod -> -1 2 }
{ max-int s>d 1 fm/mod -> 0 max-int }
{ min-int s>d 1 fm/mod -> 0 min-int }
{ max-int s>d max-int fm/mod -> 0 1 }
{ min-int s>d min-int fm/mod -> 0 1 }
{ 1s 1 4 fm/mod -> 3 max-int }
{ 1 min-int m* 1 fm/mod -> 0 min-int }
{ 1 min-int m* min-int fm/mod -> 0 1 }
{ 2 min-int m* 2 fm/mod -> 0 min-int }
{ 2 min-int m* min-int fm/mod -> 0 2 }
{ 1 max-int m* 1 fm/mod -> 0 max-int }
{ 1 max-int m* max-int fm/mod -> 0 1 }
{ 2 max-int m* 2 fm/mod -> 0 max-int }
{ 2 max-int m* max-int fm/mod -> 0 2 } -->

( hayes-test )

{ min-int min-int m* min-int fm/mod -> 0 min-int }
{ min-int max-int m* min-int fm/mod -> 0 max-int }
{ min-int max-int m* max-int fm/mod -> 0 min-int }
{ max-int max-int m* max-int fm/mod -> 0 max-int }

{ 0 s>d 1 sm/rem -> 0 0 } { 1 s>d 1 sm/rem -> 0 1 }
{ 2 s>d 1 sm/rem -> 0 2 } { -1 s>d 1 sm/rem -> 0 -1 }
{ -2 s>d 1 sm/rem -> 0 -2 } { 0 s>d -1 sm/rem -> 0 0 }
{ 1 s>d -1 sm/rem -> 0 -1 } { 2 s>d -1 sm/rem -> 0 -2 }
{ -1 s>d -1 sm/rem -> 0 1 } { -2 s>d -1 sm/rem -> 0 2 }
{ 2 s>d 2 sm/rem -> 0 1 } { -1 s>d -1 sm/rem -> 0 1 }
{ -2 s>d -2 sm/rem -> 0 1 } {  7 s>d  3 sm/rem -> 1 2 }
{  7 s>d -3 sm/rem -> 1 -2 } { -7 s>d  3 sm/rem -> -1 -2 }
{ -7 s>d -3 sm/rem -> -1 2 }
{ max-int s>d 1 sm/rem -> 0 max-int }
{ min-int s>d 1 sm/rem -> 0 min-int } -->

( hayes-test )

{ max-int s>d max-int sm/rem -> 0 1 }
{ min-int s>d min-int sm/rem -> 0 1 }
{ 1s 1 4 sm/rem -> 3 max-int }
{ 2 min-int m* 2 sm/rem -> 0 min-int }
{ 2 min-int m* min-int sm/rem -> 0 2 }
{ 2 max-int m* 2 sm/rem -> 0 max-int }
{ 2 max-int m* max-int sm/rem -> 0 2 }
{ min-int min-int m* min-int sm/rem -> 0 min-int }
{ min-int max-int m* min-int sm/rem -> 0 max-int }
{ min-int max-int m* max-int sm/rem -> 0 min-int }
{ max-int max-int m* max-int sm/rem -> 0 max-int }
{ 0 0 1 um/mod -> 0 0 } { 1 0 1 um/mod -> 0 1 }
{ 1 0 2 um/mod -> 1 0 } { 3 0 2 um/mod -> 1 1 }
{ max-uint 2 um* 2 um/mod -> 0 max-uint }
{ max-uint 2 um* max-uint um/mod -> 0 2 } -->

( hayes-test )

{ max-uint max-uint um* max-uint um/mod -> 0 max-uint }
: iffloored
  [ -3 2 / -2 = invert ] literal if postpone \ then ;
: ifsym
  [ -3 2 / -1 = invert ] literal if postpone \ then ;

  \ The system might do either floored or symmetric division.
  \ since we have already tested `m*`, `fm/mod`, and `sm/rem`
  \ we can use them in test.

iffloored : t/mod  >r s>d r> fm/mod ;
iffloored : t/  t/mod swap drop ;
iffloored : tmod  t/mod drop ;
iffloored : t*/mod >r m* r> fm/mod ;
iffloored : t*/  t*/mod swap drop ;
ifsym  : t/mod  >r s>d r> sm/rem ;
ifsym  : t/  t/mod swap drop ;
ifsym  : tmod  t/mod drop ;
ifsym  : t*/mod >r m* r> sm/rem ;
ifsym  : t*/  t*/mod swap drop ; -->

( hayes-test )

{ 0 1 /mod -> 0 1 t/mod } { 1 1 /mod -> 1 1 t/mod }
{ 2 1 /mod -> 2 1 t/mod } { -1 1 /mod -> -1 1 t/mod }
{ -2 1 /mod -> -2 1 t/mod } { 0 -1 /mod -> 0 -1 t/mod }
{ 1 -1 /mod -> 1 -1 t/mod } { 2 -1 /mod -> 2 -1 t/mod }
{ -1 -1 /mod -> -1 -1 t/mod } { -2 -1 /mod -> -2 -1 t/mod }
{ 2 2 /mod -> 2 2 t/mod } { -1 -1 /mod -> -1 -1 t/mod }
{ -2 -2 /mod -> -2 -2 t/mod } { 7 3 /mod -> 7 3 t/mod }
{ 7 -3 /mod -> 7 -3 t/mod } { -7 3 /mod -> -7 3 t/mod }
{ -7 -3 /mod -> -7 -3 t/mod }
{ max-int 1 /mod -> max-int 1 t/mod }
{ min-int 1 /mod -> min-int 1 t/mod }
{ max-int max-int /mod -> max-int max-int t/mod }
{ min-int min-int /mod -> min-int min-int t/mod } -->

( hayes-test )

{ 0 1 / -> 0 1 t/ } { 1 1 / -> 1 1 t/ } { 2 1 / -> 2 1 t/ }
{ -1 1 / -> -1 1 t/ } { -2 1 / -> -2 1 t/ }
{ 0 -1 / -> 0 -1 t/ } { 1 -1 / -> 1 -1 t/ }
{ 2 -1 / -> 2 -1 t/ } { -1 -1 / -> -1 -1 t/ }
{ -2 -1 / -> -2 -1 t/ } { 2 2 / -> 2 2 t/ }
{ -1 -1 / -> -1 -1 t/ } { -2 -2 / -> -2 -2 t/ }
{ 7 3 / -> 7 3 t/ } { 7 -3 / -> 7 -3 t/ }
{ -7 3 / -> -7 3 t/ } { -7 -3 / -> -7 -3 t/ }
{ max-int 1 / -> max-int 1 t/ }
{ min-int 1 / -> min-int 1 t/ }
{ max-int max-int / -> max-int max-int t/ }
{ min-int min-int / -> min-int min-int t/ }

{ 0 1 mod -> 0 1 tmod } { 1 1 mod -> 1 1 tmod }
{ 2 1 mod -> 2 1 tmod } { -1 1 mod -> -1 1 tmod }
{ -2 1 mod -> -2 1 tmod } { 0 -1 mod -> 0 -1 tmod }  -->

( hayes-test )

{ 1 -1 mod -> 1 -1 tmod } { 2 -1 mod -> 2 -1 tmod }
{ -1 -1 mod -> -1 -1 tmod } { -2 -1 mod -> -2 -1 tmod }
{ 2 2 mod -> 2 2 tmod } { -1 -1 mod -> -1 -1 tmod }
{ -2 -2 mod -> -2 -2 tmod } { 7 3 mod -> 7 3 tmod }
{ 7 -3 mod -> 7 -3 tmod } { -7 3 mod -> -7 3 tmod }
{ -7 -3 mod -> -7 -3 tmod } { max-int 1 mod -> max-int 1 tmod }
{ min-int 1 mod -> min-int 1 tmod }

{ max-int max-int mod -> max-int max-int tmod }
{ min-int min-int mod -> min-int min-int tmod }

{ 0 2 1 */ -> 0 2 1 t*/ } { 1 2 1 */ -> 1 2 1 t*/ }
{ 2 2 1 */ -> 2 2 1 t*/ } { -1 2 1 */ -> -1 2 1 t*/ }
{ -2 2 1 */ -> -2 2 1 t*/ } { 0 2 -1 */ -> 0 2 -1 t*/ }
{ 1 2 -1 */ -> 1 2 -1 t*/ } { 2 2 -1 */ -> 2 2 -1 t*/ }
{ -1 2 -1 */ -> -1 2 -1 t*/ } { -2 2 -1 */ -> -2 2 -1 t*/ }
{ 2 2 2 */ -> 2 2 2 t*/ }  -->

( hayes-test )

{ -1 2 -1 */ -> -1 2 -1 t*/ } { -2 2 -2 */ -> -2 2 -2 t*/ }
{ 7 2 3 */ -> 7 2 3 t*/ } { 7 2 -3 */ -> 7 2 -3 t*/ }
{ -7 2 3 */ -> -7 2 3 t*/ } { -7 2 -3 */ -> -7 2 -3 t*/ }
{ max-int 2 max-int */ -> max-int 2 max-int t*/ }
{ min-int 2 min-int */ -> min-int 2 min-int t*/ }

{ 0 2 1 */mod -> 0 2 1 t*/mod } { 1 2 1 */mod -> 1 2 1 t*/mod }
{ 2 2 1 */mod -> 2 2 1 t*/mod }
{ -1 2 1 */mod -> -1 2 1 t*/mod }
{ -2 2 1 */mod -> -2 2 1 t*/mod }
{ 0 2 -1 */mod -> 0 2 -1 t*/mod }
{ 1 2 -1 */mod -> 1 2 -1 t*/mod }
{ 2 2 -1 */mod -> 2 2 -1 t*/mod }
{ -1 2 -1 */mod -> -1 2 -1 t*/mod }
{ -2 2 -1 */mod -> -2 2 -1 t*/mod }
{ 2 2 2 */mod -> 2 2 2 t*/mod } -->

( hayes-test )

{ -1 2 -1 */mod -> -1 2 -1 t*/mod }
{ -2 2 -2 */mod -> -2 2 -2 t*/mod }
{ 7 2 3 */mod -> 7 2 3 t*/mod }
{ 7 2 -3 */mod -> 7 2 -3 t*/mod }
{ -7 2 3 */mod -> -7 2 3 t*/mod }
{ -7 2 -3 */mod -> -7 2 -3 t*/mod }
{ max-int 2 max-int */mod -> max-int 2 max-int t*/mod }
{ min-int 2 min-int */mod -> min-int 2 min-int t*/mod }

testing( here , @ ! cell+ cells c, c@ c! chars 2@ 2! +! aligned
align allot)

here 1 allot here constant 2nda constant 1sta
{ 1sta 2nda u< -> <true> } { 1sta 1+ -> 2nda }
  \ `here` must grow with allot by one byte

  \ missing test: negative allot

-->

( hayes-test )

here 1 , here 2 , constant 2nd constant 1st
{ 1st 2nd u< -> <true> } { 1st cell+ -> 2nd }
  \ `here` must grow with allot by one cell.

{ 1st 1 cells + -> 2nd } { 1st @ 2nd @ -> 1 2 } { 5 1st ! -> }
{ 1st @ 2nd @ -> 5 2 } { 6 2nd ! -> } { 1st @ 2nd @ -> 5 6 }
{ 1st 2@ -> 6 5 } { 2 1 1st 2! -> } { 1st 2@ -> 2 1 }
{ 1s 1st !  1st @ -> 1s }  \ can store cell-wide value

here 1 c, here 2 c, constant 2ndc constant 1stc
{ 1stc 2ndc u< -> <true> } { 1stc char+ -> 2ndc }
  \ `here` must grow with allot by one char.

{ 1stc 1 chars + -> 2ndc } { 1stc c@ 2ndc c@ -> 1 2 }
{ 3 1stc c! -> } { 1stc c@ 2ndc c@ -> 3 2 }
{ 4 2ndc c! -> } { 1stc c@ 2ndc c@ -> 3 4 }

-->

( hayes-test )

need align need aligned

align 1 allot here align here 3 cells allot
constant a-addr  constant ua-addr
{ ua-addr aligned -> a-addr }
{  1 a-addr c!  a-addr c@ ->  1 }
{ 1234 a-addr  !  a-addr  @ -> 1234 }
{ 123 456 a-addr 2!  a-addr 2@ -> 123 456 }
{ 2 a-addr char+ c!  a-addr char+ c@ -> 2 }
{ 3 a-addr cell+ c!  a-addr cell+ c@ -> 3 }
{ 1234 a-addr cell+ !  a-addr cell+ @ -> 1234 }
{ 123 456 a-addr cell+ 2!  a-addr cell+ 2@ -> 123 456 }

-->

( hayes-test )

: bits ( x -- u )
  0 swap begin   dup
         while   dup msb and if  >r 1+ r>  then  2*
         repeat  drop ;

{ 1 chars 1 < -> <false> } { 1 chars 1 cells > -> <false> }
  \ Characters >= 1 au, <= size of cell, >= 8 bits.
  \ XXX TODO -- How to find number of bits?

{ 1 cells 1 < -> <false> } { 1 cells 1 chars mod -> 0 }
{ 1s bits 10 < -> <false> }
  \ Cells >= 1 au, integral multiple of char size, >= 16 bits.

{ 0 1st ! -> } { 1 1st +! -> } { 1st @ -> 1 }
{ -1 1st +! 1st @ -> 0 }

testing( char [char] [ ] bl s") need char need [char]

{ bl -> 20 } { char X -> 58 } { char HELLO -> 48 }
{ : gc1 [char] X ; -> } { : gc2 [char] HELLO ; -> }
{ gc1 -> 58 } { gc2 -> 48 } { : gc3 [ gc1 ] literal ; -> }
{ gc3 -> 58 } { : gc4 s" XY" ; -> } { gc4 swap drop -> 2 }
{ gc4 drop dup c@ swap char+ c@ -> 58 59 } -->

( hayes-test )

testing( ' ['] find execute immediate count literal postpone
state)

need find

{ : gt1 123 ; -> } { ' gt1 execute -> 123 }
{ : gt2 ['] gt1 ; immediate -> } { gt2 execute -> 123 }
here 3 c, char G c, char T c, char 1 c, constant gt1string
here 3 c, char G c, char T c, char 2 c, constant gt2string
{ gt1string find -> ' gt1 -1 } { gt2string find -> ' gt2 1 }
  \ how to search for non-existent word?
{ : gt3 gt2 literal ; -> } { gt3 -> ' gt1 }
{ gt1string count -> gt1string char+ 3 }
{ : gt4 postpone gt1 ; immediate -> } { : gt5 gt4 ; -> }
{ gt5 -> 123 } { : gt6 345 ; immediate -> }
{ : gt7 postpone gt6 ; -> } { gt7 -> 345 }
{ : gt8 state @ ; immediate -> } { gt8 -> 0 }
{ : gt9 gt8 literal ; -> } { gt9 0= -> <false> } -->

( hayes-test )

testing( if else then)

{ : gi1 if 123 then ; -> } { : gi2 if 123 else 234 then ; -> }
{ 0 gi1 -> } { 1 gi1 -> 123 } { -1 gi1 -> 123 }
{ 0 gi2 -> 234 } { 1 gi2 -> 123 } { -1 gi1 -> 123 }

testing( begin while repeat until recurse)

{ : gi3 begin dup 5 < while dup 1+ repeat ; -> }
{ 0 gi3 -> 0 1 2 3 4 5 }
{ 4 gi3 -> 4 5 } { 5 gi3 -> 5 } { 6 gi3 -> 6 }

{ : gi4 begin dup 1+ dup 5 > until ; -> }
{ 3 gi4 -> 3 4 5 6 } { 5 gi4 -> 5 6 } { 6 gi4 -> 6 7 }

{ : gi5 begin dup 2 >
    while dup 5 < while dup 1+ repeat 123 else 345 then ; -> }

{ 1 gi5 -> 1 345 }      { 2 gi5 -> 2 345 }
{ 3 gi5 -> 3 4 5 123 }  { 4 gi5 -> 4 5 123 }
{ 5 gi5 -> 5 123 }  -->

( hayes-test )

testing( recurse) need recurse

{ : gi6 ( n -- 0 1 ... n ) dup if dup >r 1- recurse r> then ;
-> }

{ 0 gi6 -> 0 } { 1 gi6 -> 0 1 } { 2 gi6 -> 0 1 2 }
{ 3 gi6 -> 0 1 2 3 } { 4 gi6 -> 0 1 2 3 4 }

testing( do loop +loop i j unloop leave exit) need j

{ : gd1 do i loop ; -> } { 4 1 gd1 -> 1 2 3 }
{ 2 -1 gd1 -> -1 0 1 } { mid-uint+1 mid-uint gd1 -> mid-uint }

{ : gd2 do i -1 +loop ; -> } { 1 4 gd2 -> 4 3 2 1 }
{ -1 2 gd2 -> 2 1 0 -1 }
{ mid-uint mid-uint+1 gd2 -> mid-uint+1 mid-uint }

{ : gd3 do 1 0 do j loop loop ; -> }
{ 4 1 gd3 -> 1 2 3 } { 2 -1 gd3 -> -1 0 1 }
{ mid-uint+1 mid-uint gd3 -> mid-uint } -->

( hayes-test )

{ : gd4 do 1 0 do j loop -1 +loop ; -> }
{ 1 4 gd4 -> 4 3 2 1 }
{ -1 2 gd4 -> 2 1 0 -1 }
{ mid-uint mid-uint+1 gd4 -> mid-uint+1 mid-uint }

{ : gd5 123 swap 0 do i 4 > if drop 234 leave then loop ; -> }
{ 1 gd5 -> 123 } { 5 gd5 -> 123 } { 6 gd5 -> 234 }

{ : gd6
    0 swap 0 do
    i 1+ 0 do i j + 3 = if i unloop i unloop exit then 1+ loop
    loop ; -> }
  \ Pat: {0 0},{0 0}{1 0}{1 1},{0 0}{1 0}{1 1}{2 0}{2 1}{2 2}.

{ 1 gd6 -> 1 }  { 2 gd6 -> 3 }  { 3 gd6 -> 4 1 2 } -->

( hayes-test )

testing( : ; constant variable create does> >body)

need >body

{ 123 constant x123 -> } { x123 -> 123 }
{ : equ constant ; -> } { x123 equ y123 -> } { y123 -> 123 }

{ variable v1 -> } { 123 v1 ! -> } { v1 @ -> 123 }

{ : nop : postpone ; ; -> }
{ nop nop1 nop nop2 -> } { nop1 -> } { nop2 -> }

{ : does1 does> @ 1 + ; -> } { : does2 does> @ 2 + ; -> }
{ create cr1 -> } { cr1 -> here }
{ ' cr1 >body -> here }
{ 1 , -> } { cr1 @ -> 1 } { does1 -> }
{ cr1 -> 2 } { does2 -> } { cr1 -> 3 }

{ : weird: create does> 1 + does> 2 + ; -> }
{ weird: w1 -> } { ' w1 >body -> here }
{ w1 -> here 1 + } { w1 -> here 2 + }  -->

( hayes-test )

testing( evaluate)

need evaluate

: ge1 s" 123" ; immediate   : ge2 s" 123 1+" ; immediate
: ge3 s" : ge4 345 ;" ;     : ge5 evaluate ; immediate

{ ge1 evaluate -> 123 } { ge2 evaluate -> 124 }
{ ge3 evaluate -> } { ge4 -> 345 }
  \ test `evaluate` in interpretation state

{ : ge6 ge1 ge5 ; -> } { ge6 -> 123 }
{ : ge7 ge2 ge5 ; -> } { ge7 -> 124 }
  \ test `evaluate` in compilation state

testing( source >in word)

: gs1 s" SOURCE" 2dup evaluate >r swap >r = r> r> = ;
{ gs1 -> <true> <true> }

variable scans  2 scans !
: rescan?  -1 scans +! scans @ if 0 >in ! then ;

-->

( hayes-test )

{ 345 rescan?  -> 345 345 }
  \ Note: this test must be the first code in its block.

: gs2  5 scans ! s" 123 RESCAN?" evaluate ;

-->

( hayes-test )

{ gs2 -> 123 123 123 123 123 }
  \ XXX REMARK: this test must be the first code in its block.

need word

: gs3 word count swap c@ ;
{ bl gs3 HELLO -> 5 char H }
{ char " gs3 GOODBYE" -> 7 char G }

need +thru need continued  1 2 +thru  blk @ 3 + continued
  \ Load the two following blocks and then continue loading
  \ after them.  This is needed, because `-->` can not be used
  \ in those blocks.

( hayes-test )

\ line 1
\ line 2
\ line 3
\ line 4
\ line 5
\ line 6
\ line 7
\ line 8
\ line 9
\ line 10
\ line 11
\ line 12
\ line 13
\ line 14

  \ Note: The following test must start at the end of the last
  \ line of the block (line 15):

                                                       { bl gs3

( hayes-test )

drop -> 0 }  \ blank line return zero-length string
  \ Note: This line must be the first code of the block,
  \ because the first part of the test must be at the end of
  \ the last line of a block.

: gs4 source >in ! drop ;

{ gs4 123 456
  \ Note: This line must be the last code of the block.

( hayes-test )

-> }
  \ Note: This line must be the first code of the block,
  \ because the first part of the test must be at the end of a
  \ block.

testing( <# # #s #> hold sign base >number hex decimal)

: s=  \ ( addr1 c1 addr2 c2 -- t/f ) compare two strings.
  >r swap r@ = if  \ make sure strings have same length
  r> ?dup if  \ if non-empty strings
    0 do  over c@ over c@ - if  2drop <false> unloop exit  then
          swap char+ swap char+  loop
  then  2drop <true>  \ if we get here, strings match
  else  r> drop 2drop <false>  then ; \ lengths mismatch

: gp1  <# 41 hold 42 hold 0 0 #> s" BA" s= ; { gp1 -> <true> }

: gp2  <# -1 sign 0 sign -1 sign 0 0 #> s" --" s= ;
{ gp2 -> <true> }

: gp3  <# 1 0 # # #> s" 01" s= ; { gp3 -> <true> }

: gp4  <# 1 0 #s #> s" 1" s= ; { gp4 -> <true> } -->

( hayes-test )

24 constant max-base  \ base 2 ... 36
: count-bits
  0 0 invert begin dup while >r 1+ r> 2* repeat drop ;

count-bits 2* constant #bits-ud  \ number of bits in ud

: gp5
  base @ <true>
  max-base 1+ 2 do  \ for each possible base
    i base !  \ tbd: assumes base works
    i 0 <# #s #> s" 10" s= and
  loop  swap base ! ;  { gp5 -> <true> } -->

( hayes-test )

: gp6
  base @ >r  2 base !
  max-uint max-uint <# #s #>  \ maximum ud to binary
  r> base !  \ s: c-addr u
  dup #bits-ud = swap 0 do  \ s: c-addr flag
                          over c@ [char] 1 = and  \ all ones
                          >r char+ r>
                        loop swap drop ;  { gp6 -> <true> }

: gp7
  base @ >r  max-base base !  <true>
  a 0 do  i 0 <# #s #>  1 = swap c@ i 30 + = and and  loop
  max-base a do
    i 0 <# #s #>  1 = swap c@ 41 i a - + = and and
  loop  r> base ! ;  { gp7 -> <true> }  -->

( hayes-test )

  \ `>number` tests

create gn-buf 0 c,
: gn-string  gn-buf 1 ;
: gn-consumed  gn-buf char+ 0 ;
: gn'  [char] ' word char+ c@ gn-buf c!  gn-string ;

{ 0 0 gn' 0' >number -> 0 0 gn-consumed }
{ 0 0 gn' 1' >number -> 1 0 gn-consumed }
{ 1 0 gn' 1' >number -> base @ 1+ 0 gn-consumed }
{ 0 0 gn' -' >number -> 0 0 gn-string }
  \ Should fail to convert these.

{ 0 0 gn' +' >number -> 0 0 gn-string }
{ 0 0 gn' .' >number -> 0 0 gn-string }

-->

( hayes-test )

: >number-based
  base @ >r base ! >number r> base ! ;

{ 0 0 gn' 2' 10 >number-based -> 2 0 gn-consumed }
{ 0 0 gn' 2'  2 >number-based -> 0 0 gn-string }
{ 0 0 gn' f' 10 >number-based -> f 0 gn-consumed }
{ 0 0 gn' g' 10 >number-based -> 0 0 gn-string }
{ 0 0 gn' g' max-base >number-based -> 10 0 gn-consumed }
{ 0 0 gn' z' max-base >number-based -> 23 0 gn-consumed }

-->

( hayes-test )

: gn1 ( ud base -- ud' len )
  base @ >r base !
  <# #s #>
  0 0 2swap >number swap drop  \ return length only
  r> base ! ;
  \ ud should equal ud' and len should be zero.
{ 0 0 2 gn1 -> 0 0 0 }
{ max-uint 0 2 gn1 -> max-uint 0 0 }
{ max-uint dup 2 gn1 -> max-uint dup 0 }
{ 0 0 max-base gn1 -> 0 0 0 }
{ max-uint 0 max-base gn1 -> max-uint 0 0 }
{ max-uint dup max-base gn1 -> max-uint dup 0 }

: gn2  \ ( -- 16 10 )
  base @ >r  hex base @  decimal base @  r> base ! ;
{ gn2 -> 10 a } -->

( hayes-test )

testing( fill move)

create fbuf 00 c, 00 c, 00 c,
create sbuf 12 c, 34 c, 56 c,
: seebuf fbuf c@  fbuf char+ c@  fbuf char+ char+ c@ ;

{ fbuf 0 20 fill -> } { seebuf -> 00 00 00 }

{ fbuf 1 20 fill -> } { seebuf -> 20 00 00 }

{ fbuf 3 20 fill -> } { seebuf -> 20 20 20 }

{ fbuf fbuf 3 chars move -> } { seebuf -> 20 20 20 }
  \ Bizarre special case.

{ sbuf fbuf 0 chars move -> } { seebuf -> 20 20 20 }

{ sbuf fbuf 1 chars move -> } { seebuf -> 12 20 20 }

{ sbuf fbuf 3 chars move -> } { seebuf -> 12 34 56 }

{ fbuf fbuf char+ 2 chars move -> } { seebuf -> 12 12 34 }

{ fbuf char+ fbuf 2 chars move -> } { seebuf -> 12 34 34 } -->

( hayes-test )

testing( . ." cr emit space spaces type u.)

: output-test
  ." You should see the standard graphic characters:" cr
  41 bl do i emit loop cr 61 41 do i emit loop cr
  7F 61 do i emit loop cr
  ." You should see 0-9 separated by a space:" cr
  9 1+ 0 do i . loop cr
  ." You should see 0-9 (with no spaces):" cr
  [char] 9 1+ [char] 0 do i 0 spaces emit loop cr
  ." You should see A-G separated by a space:" cr
  [char] G 1+ [char] A do i emit space loop cr
  ." You should see 0-5 separated by two spaces:" cr
  5 1+ 0 do i [char] 0 + emit 2 spaces loop cr

-->

( hayes-test )

  ." You should see two separate lines:" cr
  s" line 1" type cr s" line 2" type cr
  ." You should see the number ranges" cr
  ." of signed and unsigned numbers:" cr
  ."   signed: " min-int . max-int . cr
  ." unsigned: " 0 u. max-uint u. cr ;  { output-test -> }

testing( accept)

create abuf 80 chars allot

: accept-test
  cr ." Please type up to 80 characters:" cr  abuf 80 accept
  cr ." Received: " [char] " emit
  abuf swap type [char] " emit cr ;  { accept-test -> }

testing( dictionary search rules)

{ : gdx  123 ; : gdx  gdx 234 ; -> } { gdx -> 123 234 }

decimal cr .( Test passed) cr

  \ doc{
  \
  \ hayes-test ( -- )
  \
  \ A non-existant word. ``hayes-test`` is used just for doing
  \ ``need hayes-test`` in order to run the Hayes test, which
  \ tests the core words of an ANS Forth system.
  \
  \ The test assumes a two's complement implementation where
  \ the range of signed numbers is -2^(n-1) ... 2^(n-1)-1 and
  \ the range of unsigned numbers is 0 ... 2^(n)-1.
  \
  \ Some words are not tested: `key`, `quit`, `abort`, `abort"`
  \ `environment?`...
  \
  \ See: `hayes-tester`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-05-09: Start.
  \
  \ 2016-05-10: First working version.
  \
  \ 2016-05-14: Update: `evaluate` has been moved to the
  \ library.
  \
  \ 2016-05-17: Need `>body`, which has been moved to the
  \ library.
  \
  \ 2016-11-26: Remove `warnings off`, because now warnings are
  \ deactivated by default.
  \
  \ 2017-02-19: Need `do`, which has been moved to the library.
  \
  \ 2017-12-09: Update with `need */`, since `*/` was moved to
  \ the library.
  \
  \ 2018-03-09: Update notation "address units" to "bytes".
  \ Improve documentation.
  \
  \ 2018-04-15: Update notation ".." to "...".

  \ vim: filetype=soloforth
