include l5k.fs

( INPUTS )
VARIABLE START
VARIABLE STOP
VARIABLE ESTOP

( OUTPUTS )
VARIABLE MOTOR
VARIABLE FAN
VARIABLE GREEN
VARIABLE RED

( LOCALS )
VARIABLE RUN
TIMER FAN_ON
TIMER RED_ON
TIMER RED_OFF

LADDER: MAIN_ROUTINE
||                                                                            ||
||   STOP    ESTOP        START                                    RUN        ||
||---]/[------]/[------+---] [---+---------------------------------( )--------||
||                     |         |                                            ||
||                     |   RUN   |                                            ||
||                     +---] [---+                                            ||
||                                                                            ||
||                                          +------------------+              ||
||   RUN                                    |       TOF        |              ||
||---] [------------------------------------|                  |--------------||
||                                          | TIMER     FAN_ON |              ||
||                                          | PRE         T#3s |              ||
||                                          |                  |              ||
||                                          +------------------+              ||
||                                                                            ||
||                                          +------------------+              ||
||   RUN                  RED_OFF.TT        |       TON        |              ||
||---]/[--------------+------]/[------------|                  |------+-------||
||                    |                     | TIMER     RED_ON |      |       ||
||                    |                     | PRE      T#750ms |      |       ||
||                    |                     |                  |      |       ||
||                    |                     +------------------+      |       ||
||                    |                                               |       ||
||                    |                     +------------------+      |       ||
||                    |   RED_ON.TT         |       TON        |      |       ||
||                    +------]/[------------|                  |------+       ||
||                                          | TIMER    RED_OFF |              ||
||                                          | PRE      T#250ms |              ||
||                                          |                  |              ||
||                                          +------------------+              ||
||                                                                            ||
|| FAN_ON.DN                                                            FAN   ||
||---] [----------------------------------------------------------------( )---||
||                                                                            ||
|| RED_ON.TT                                                            RED   ||
||---] [----------------------------------------------------------------( )---||
||                                                                            ||
||   RUN                                                          MOTOR       ||
||---] [-------------------------------------------------------+---( )---+----||
||                                                             |         |    ||
||                                                             |  GREEN  |    ||
||                                                             +---( )---+    ||
||                                                                            ||

( UI )
: D>$         TUCK DABS <# #S ROT SIGN #> ;
: S>$         S>D D>$ ;
: ESC[K       ESC[ S>$ TYPE 75 EMIT 13 EMIT ;
: ESC[M       ESC[ S>$ TYPE 109 EMIT ;
: ESC[EMIT    IF 30 ESC[M 42 ESC[M THEN EMIT 0 ESC[M ;
: .CONTACT    ']' EMIT ESC[EMIT '[' EMIT ;
: .COIL       '(' EMIT BL SWAP ESC[EMIT ')' EMIT ;
: TOGGLE      DUP @ 0= SWAP ! ;
: PRESCAN     DUP MS S:MS !
              KEY? 0= IF TRUE EXIT THEN
              KEY CASE
              'q' OF FALSE ENDOF
              '1' OF TRUE START TOGGLE ENDOF
              '2' OF TRUE STOP  TOGGLE ENDOF
              '3' OF TRUE ESTOP TOGGLE ENDOF
              DUP OF TRUE ENDOF
              ENDCASE ;
: POSTSCAN    2 ESC[K SPACE
              '1' START @ .CONTACT 3 SPACES
              '2' STOP  @ .CONTACT 4 SPACES
              '3' ESTOP @ .CONTACT 4 SPACES
              MOTOR @ .COIL 3 SPACES
              FAN   @ .COIL 3 SPACES
              GREEN @ .COIL 3 SPACES
              RED   @ .COIL SPACE ;
: UI          BEGIN 10 PRESCAN WHILE MAIN_ROUTINE POSTSCAN REPEAT ;
: HELP        CR ." Press a number to toggle a contact, or press 'q' to quit."
              CR ." START  STOP  ESTOP  MOTOR  FAN  GREEN  RED" ;

SEE MAIN_ROUTINE
CR HELP
CR UI
CR BYE
