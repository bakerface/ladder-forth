include l5k.fs

VARIABLE IN
VARIABLE OUT
TIMER T

( HELPERS )
: OFF?          @ 0= ;
: ON?           @ 0<> ;
: EXPECTED      0= THROW ;
: RESET_VARS    T.EN OFF T.TT OFF T.DN OFF T.ACC OFF IN OFF OUT OFF ; 

LADDER: TEST_TON
||                                                                            ||
||                            +------------------+                            ||
||   IN                       |       TON        |                      OUT   ||
||---] [----------------------|                  |----------------------( )---||
||                            | TIMER          T |                            ||
||                            | PRE      T#250ms |                            ||
||                            |                  |                            ||
||                            +------------------+                            ||
||                                                                            ||

100 S:MS !
RESET_VARS
TEST_TON
OUT OFF? EXPECTED
T.EN OFF? EXPECTED
T.TT OFF? EXPECTED
T.DN OFF? EXPECTED
T.ACC @ 0= EXPECTED

100 S:MS !
IN ON
TEST_TON
OUT ON? EXPECTED
T.EN ON? EXPECTED
T.TT ON? EXPECTED
T.DN OFF? EXPECTED
T.ACC @ 0= EXPECTED

100 S:MS !
TEST_TON
OUT ON? EXPECTED
T.EN ON? EXPECTED
T.TT ON? EXPECTED
T.DN OFF? EXPECTED
T.ACC @ 100 = EXPECTED

100 S:MS !
IN OFF
TEST_TON
OUT OFF? EXPECTED
T.EN OFF? EXPECTED
T.TT OFF? EXPECTED
T.DN OFF? EXPECTED
T.ACC @ 0= EXPECTED

100 S:MS !
IN ON
TEST_TON
OUT ON? EXPECTED
T.EN ON? EXPECTED
T.TT ON? EXPECTED
T.DN OFF? EXPECTED
T.ACC @ 0= EXPECTED

100 S:MS !
TEST_TON
OUT ON? EXPECTED
T.EN ON? EXPECTED
T.TT ON? EXPECTED
T.DN OFF? EXPECTED
T.ACC @ 100 = EXPECTED

100 S:MS !
TEST_TON
OUT ON? EXPECTED
T.EN ON? EXPECTED
T.TT ON? EXPECTED
T.DN OFF? EXPECTED
T.ACC @ 200 = EXPECTED

100 S:MS !
TEST_TON
OUT ON? EXPECTED
T.EN ON? EXPECTED
T.TT OFF? EXPECTED
T.DN ON? EXPECTED
T.ACC @ 250 = EXPECTED

100 S:MS !
TEST_TON
OUT ON? EXPECTED
T.EN ON? EXPECTED
T.TT OFF? EXPECTED
T.DN ON? EXPECTED
T.ACC @ 250 = EXPECTED

LADDER: TEST_TOF
||                                                                            ||
||                            +------------------+                            ||
||   IN                       |       TOF        |                      OUT   ||
||---] [----------------------|                  |----------------------( )---||
||                            | TIMER          T |                            ||
||                            | PRE      T#250ms |                            ||
||                            |                  |                            ||
||                            +------------------+                            ||
||                                                                            ||

100 S:MS !
RESET_VARS
TEST_TOF
OUT OFF? EXPECTED
T.EN OFF? EXPECTED
T.TT OFF? EXPECTED
T.DN OFF? EXPECTED
T.ACC @ 0= EXPECTED

100 S:MS !
IN ON
TEST_TOF
OUT ON? EXPECTED
T.EN ON? EXPECTED
T.TT OFF? EXPECTED
T.DN ON? EXPECTED
T.ACC @ 0= EXPECTED

100 S:MS !
IN OFF
TEST_TOF
OUT OFF? EXPECTED
T.EN OFF? EXPECTED
T.TT ON? EXPECTED
T.DN ON? EXPECTED
T.ACC @ 0= EXPECTED

100 S:MS !
IN OFF
TEST_TOF
OUT OFF? EXPECTED
T.EN OFF? EXPECTED
T.TT ON? EXPECTED
T.DN ON? EXPECTED
T.ACC @ 100 = EXPECTED

100 S:MS !
IN ON
TEST_TOF
OUT ON? EXPECTED
T.EN ON? EXPECTED
T.TT OFF? EXPECTED
T.DN ON? EXPECTED
T.ACC @ 0= EXPECTED

100 S:MS !
IN OFF
TEST_TOF
OUT OFF? EXPECTED
T.EN OFF? EXPECTED
T.TT ON? EXPECTED
T.DN ON? EXPECTED
T.ACC @ 0= EXPECTED

100 S:MS !
TEST_TOF
OUT OFF? EXPECTED
T.EN OFF? EXPECTED
T.TT ON? EXPECTED
T.DN ON? EXPECTED
T.ACC @ 100 = EXPECTED

100 S:MS !
TEST_TOF
OUT OFF? EXPECTED
T.EN OFF? EXPECTED
T.TT ON? EXPECTED
T.DN ON? EXPECTED
T.ACC @ 200 = EXPECTED

100 S:MS !
TEST_TOF
OUT OFF? EXPECTED
T.EN OFF? EXPECTED
T.TT OFF? EXPECTED
T.DN OFF? EXPECTED
T.ACC @ 250 = EXPECTED

BYE
