include ladder.fs

( VARIABLES )
VARIABLE S:MS

( HELPERS )
: EN1           SWAP IF EXIT THEN DROP RDROP FALSE ;
: ADVANCE       2DUP @ S:MS @ + MIN DUP ROT ! = ;
: HOLDS         BEGIN DUP WHILE 1- 2DUP + C@ HOLD REPEAT 2DROP ;
: SUFFIX        <# HOLDS HOLDS 0 0 #> ;
: .CREATE       SUFFIX NEXTNAME CREATE ;

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
: TON           5 PICK 0= IF OFF OFF OFF OFF DROP EXIT THEN
                DUP @ IF DROP ON OFF 2DROP EXIT THEN
                OVER @ 0= IF DROP ON ON 2DROP EXIT THEN
                >R ON >R ADVANCE IF R> OFF R> ON EXIT THEN
                R> ON RDROP ;

: TOF           5 PICK IF ON ON OFF OFF DROP EXIT THEN
                DUP @ 0= IF DROP OFF OFF 2DROP EXIT THEN
                OVER @ IF DROP OFF ON 2DROP EXIT THEN
                >R OFF >R ADVANCE IF R> OFF R> OFF EXIT THEN
                R> ON RDROP ;

: TIMER,        2DUP S" PRE" NAMED TIME,
                S" TIMER" NAMED
                2DUP S" .ACC" SUFFIX TAG,
                2DUP S" .TT" SUFFIX TAG,
                2DUP S" .EN" SUFFIX TAG,
                S" .DN" SUFFIX TAG, ;

: TON,          TIMER, POSTPONE TON ;
: TOF,          TIMER, POSTPONE TOF ;

: TIMER         PARSE-NAME
                2DUP S" .EN"  .CREATE 
                2DUP S" .TT"  .CREATE 
                2DUP S" .DN"  .CREATE 
                     S" .ACC" .CREATE ;
