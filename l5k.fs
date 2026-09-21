include ladder.fs

( VARIABLES )
VARIABLE S:MS

( HELPERS )
: EN1           SWAP IF EXIT THEN DROP RDROP FALSE ;
: ADVANCE       2DUP @ S:MS @ + MIN DUP ROT ! = ;
: HOLDS         BEGIN DUP WHILE 1- 2DUP + C@ HOLD REPEAT 2DROP ;
: SUFFIX        <# HOLDS HOLDS 0 0 #> ;
: 2CELL+        CELL+ CELL+ ;
: $>XT          SFIND 0= IF ABORT" unknown word" THEN ;

( CONTACTS )
: XIC           EN1 @ 0<> ;
: XIO           EN1 @ 0= ;

: (CONTACT,)    2DUP WORD^ TAG, LINE@ CASE
                BL  OF POSTPONE XIC ENDOF
                '/' OF POSTPONE XIO ENDOF
                DUP OF ABORT" unknown contact type" ENDOF
                ENDCASE ;

' (CONTACT,) IS CONTACT,

( COILS )
: OTE           OVER SWAP ! ;
: OTD           OVER 0= SWAP ! ;
: OTL           EN1 ON TRUE ;
: OTU           EN1 OFF TRUE ;

: (COIL,)       2DUP WORD^ TAG, LINE@ CASE
                BL  OF POSTPONE OTE ENDOF
                '/' OF POSTPONE OTD ENDOF
                'L' OF POSTPONE OTL ENDOF
                'U' OF POSTPONE OTU ENDOF
                DUP OF ABORT" unknown coil type" ENDOF
                ENDCASE ;

' (COIL,) IS COIL, 

( TIMERS )
: TIMER:        CREATE 4 CELLS ALLOT ;
: TIMER.EN      ;
: TIMER.TT      CELL+ ;
: TIMER.DN      2CELL+ ;
: TIMER.ACC     2CELL+ CELL+ ;

: TIMER.EN,     2DUP S" .EN" SUFFIX NEXTNAME $>XT
                CREATE , DOES> @ EXECUTE TIMER.EN ;

: TIMER.TT,     2DUP S" .TT" SUFFIX NEXTNAME $>XT
                CREATE , DOES> @ EXECUTE TIMER.TT ;

: TIMER.DN,     2DUP S" .DN" SUFFIX NEXTNAME $>XT
                CREATE , DOES> @ EXECUTE TIMER.DN ;

: TIMER.ACC,    2DUP S" .ACC" SUFFIX NEXTNAME $>XT
                CREATE , DOES> @ EXECUTE TIMER.ACC ;

: TIMER         PARSE-NAME
                2DUP NEXTNAME TIMER:
                2DUP TIMER.EN,
                2DUP TIMER.TT,
                2DUP TIMER.DN,
                TIMER.ACC,
                ;

: TIMER,        2DUP S" PRE" NAMED TIME, S" TIMER" NAMED TAG, ;

: TON.RESET     DUP TIMER.EN OFF
                DUP TIMER.TT OFF
                DUP TIMER.DN OFF
                TIMER.ACC OFF ;

: TON           2>R DUP 0= IF R> TON.RESET RDROP EXIT THEN
                R@ TIMER.TT ON
                R@ TIMER.DN @ IF R@ TIMER.TT OFF R> TIMER.EN ON RDROP EXIT THEN
                R@ TIMER.EN @ 0= IF R> TIMER.EN ON RDROP EXIT THEN
                R@ TIMER.EN ON 2R@ TIMER.ACC ADVANCE
                IF R@ TIMER.TT OFF R> TIMER.DN ON RDROP EXIT THEN
                2RDROP ;

: TOF.RESET     DUP TIMER.EN ON
                DUP TIMER.TT OFF
                DUP TIMER.DN ON
                TIMER.ACC OFF ;

: TOF           2>R DUP IF R> TOF.RESET RDROP EXIT THEN
                R@ TIMER.TT ON
                R@ TIMER.DN @ 0= IF R@ TIMER.TT OFF R> TIMER.EN OFF RDROP EXIT THEN
                R@ TIMER.EN @ IF R> TIMER.EN OFF RDROP EXIT THEN
                R@ TIMER.EN OFF 2R@ TIMER.ACC ADVANCE
                IF R@ TIMER.TT OFF R> TIMER.DN OFF RDROP EXIT THEN
                2RDROP ;

: TON,          TIMER, POSTPONE TON ;
: TOF,          TIMER, POSTPONE TOF ;
