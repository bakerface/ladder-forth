( RUNTIME WORDS TO EVALUATE BOOLEAN LOGIC )
: R:  TRUE ;
: R;  DROP ;
: R[  FALSE OVER ;
: R]  OR AND ;
: R,  OR OVER ;

DEFER COIL,     ( DEFINED IN CUSTOM INSTRUCTION SET )
DEFER CONTACT,  ( DEFINED IN CUSTOM INSTRUCTION SET )
DEFER RUNG,     ( FORWARD DECLARATION FOR RECURSIVE WORD )

( CREATE DOUBLE LINKED LIST OF LINES FROM SOURCE UNTIL EMPTY LINE READ )
: $>LINE      ROT 2DUP C! 1+ SWAP 2DUP + OVER 2>R CMOVE 2R> 1+ OVER C! 1+ ;
: LINE@       + 1+ C@ ;
: LINE!       + 1+ C! ;
: 0LINE?      C@ 0= ;
: >LINE       DUP C@ + 2 + ;
: <LINE       1- DUP C@ - ;
: READLINE?   REFILL SOURCE NIP AND ;
: READLINES   BEGIN READLINE? WHILE SOURCE $>LINE REPEAT 0 SWAP C! ;

( ADVANCE FORWARD OR BACKWARD TO FIND WORDS OR CHARACTERS )
: >INS        OVER C@ SWAP DO DUP I LINE@ '-' <> IF I UNLOOP EXIT THEN LOOP
              ABORT" expected token" ;
: >SPACE      OVER C@ SWAP DO DUP I LINE@ BL = IF I UNLOOP EXIT THEN LOOP
              ABORT" expected space" ;
: <SPACE      OVER C@ SWAP DO DUP I LINE@ BL = IF I UNLOOP EXIT THEN 1 -LOOP
              ABORT" expected space" ;
: WORD@       <SPACE 1+ DUP >R >SPACE R@ - SWAP R> 1+ + SWAP ;
: WORD^       >R <LINE R> WORD@ ;

( LADDER COMPILER )
: FORK?       LINE@ DUP '+' = SWAP '|' = OR ;
: JOIN?       2DUP LINE@ '+' = >R 1+ LINE@ BL = R> AND ;
: vBRANCH     BEGIN 2DUP FORK? WHILE >R >LINE R> REPEAT >R <LINE R> ;
: >BRANCH     BEGIN 2DUP JOIN? 0= WHILE 1+ REPEAT ;
: >LEG        BEGIN >R >LINE R> 2DUP LINE@ '|' <> UNTIL ;
: <LEG        BEGIN >R <LINE R> 2DUP LINE@ '|' <> UNTIL ;
: LEG+        DUP IF POSTPONE R, THEN 1+ ;
: 3R@         2R> 2DUP 2R@ 2SWAP 2>R 2SWAP DROP ;
: LEG?        ROT 2>R 2DUP >= 2R> ROT >R -ROT R> ;
: LEG,        >R >R >R 2R@ LINE@ 2R@ 1+ LINE@ '|' 2R@ LINE! '|' 2R@ 1+ LINE!
              3R@ DROP SWAP 1+ RUNG, 2R@ 1+ LINE! 2R@ LINE! R> R> R> ;
: BRANCH,     OVER >R POSTPONE R[
              BEGIN LEG? WHILE R> R> R> LEG+ >R >R >R LEG, >LEG REPEAT
              POSTPONE R] 2DROP >R DROP 2R> 1+ ;
: FORK,       0 >R 2DUP vBRANCH >BRANCH 2SWAP BRANCH, RDROP ;
: AOI,        <LEG 2DUP >R >LINE R>
              BEGIN 1+ 2DUP LINE@ BL <> UNTIL 2DUP
              BEGIN 1+ 2DUP LINE@ BL = UNTIL 2>R ',' 2R@ LINE!
              R@ OVER - 1+ >R + 1+ R> SFIND BL 2R> LINE!
              0= IF ABORT" unknown AOI" THEN >R 2DUP
              BEGIN 1+ 2DUP LINE@ '-' <> UNTIL 1+
              BEGIN 2DUP LINE@ '-' <> WHILE >R >LINE R> REPEAT
              2SWAP R> EXECUTE ;
: |,          2DUP 1+ LINE@ '|' = IF TRUE EXIT THEN AOI, FALSE ;
: INS,        2DUP LINE@ CASE
              '|' OF |, ENDOF
              ']' OF 1+ 2DUP CONTACT, 2 + FALSE ENDOF
              '(' OF 1+ 2DUP COIL, 2 + FALSE ENDOF
              '+' OF FORK, FALSE ENDOF
              DUP OF TRUE ENDOF
              ENDCASE ;
: RUNG?       1+ DUP C@ '|' <> IF DROP FALSE EXIT THEN
              1+ DUP C@ '|' <> IF DROP FALSE EXIT THEN
              1+ C@ '-' = ;
: >RUNG       BEGIN
              DUP 0LINE? IF DROP FALSE EXIT THEN
              DUP RUNG? IF 3 TRUE EXIT THEN
              >LINE AGAIN ;
: (RUNG,)     BEGIN >INS INS, UNTIL 2DROP ;
: LADDER:     : PAD READLINES PAD
              BEGIN >RUNG
              WHILE OVER >R POSTPONE R: RUNG, POSTPONE R; R> >LINE
              REPEAT
              POSTPONE ; ;

' (RUNG,) IS RUNG,

( HELPERS FOR CUSTOM INSTRUCTION SET )
: TAG,        SFIND 0= IF ABORT" unknown tag" THEN COMPILE, ;
: IMM,        2DUP SNUMBER? IF POSTPONE LITERAL 2DROP EXIT THEN 
              TAG, POSTPONE @ ;
: TIME*       >R ROT R> * -ROT ;
: TIME+H      3600000 TIME* ;
: TIME+M      OVER C@ CASE
              's' OF 1 /STRING ENDOF
              DUP OF 60000 TIME* ENDOF
              ENDCASE ;
: TIME+S      1000 TIME* ;
: TIME+       2>R 0 0 2R> >NUMBER 2>R D>S 2R> OVER C@ CASE
              'h' OF 1 /STRING TIME+H ENDOF
              'm' OF 1 /STRING TIME+M ENDOF
              's' OF 1 /STRING TIME+S ENDOF
              DUP OF ABORT" unknown time units" ENDOF
              ENDCASE 2>R + 2R> ;
: TIME,       OVER C@ 'T' = >R OVER 1+ C@ '#' = R> AND
              0= IF IMM, EXIT THEN
              2 /STRING 0 -ROT BEGIN DUP WHILE TIME+ REPEAT
              2DROP POSTPONE LITERAL ;
: NAMED       2>R >R >LINE R> 2 +
              BEGIN >R >LINE R>
              2DUP LINE@ '-' = IF ABORT" cannot find argument" THEN
              2DUP WORD@ 2R@ COMPARE 0= UNTIL
              >SPACE BEGIN 1+ 2DUP LINE@ BL <> UNTIL
              WORD@ 2RDROP ;
