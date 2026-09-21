# LADDER: Forth
**A PLC Ladder Logic compiler and runtime in Forth**

### Overview
The purpose of this project is to allow a developer to inline an ASCII version
of a PLC Ladder Diagram in their source code and use it like they would any
other word in forth. Below is a simple example. A more advanced version of this
example can be seen in `main.fs`.

``` forth
include l5k.fs

( INPUTS )
VARIABLE START
VARIABLE STOP

( OUTPUTS )
VARIABLE MOTOR
VARIABLE FAN

( LOCALS )
VARIABLE RUN
TIMER FAN_TIMER

LADDER: MOTOR_ROUTINE
||                                                                    ||
||   STOP           START          +------------------+         RUN   ||
||---]/[---------+---] [---+-------|       TOF        |---------( )---||
||               |         |       |                  |               ||
||               |   RUN   |       | TIMER  FAN_TIMER |               ||
||               +---] [---+       | PRE         T#3s |               ||
||                                 |                  |               ||
||                                 +------------------+               ||
||                                                                    ||
||   RUN                                                       MOTOR  ||
||---] [--------------------------------------------------------( )---||
||                                                                    ||
||                                                                    ||
|| FAN_TIMER.DN                                                 FAN   ||
||---] [--------------------------------------------------------( )---||
||                                                                    ||

SEE MOTOR_ROUTINE
\ : MOTOR_ROUTINE
\   R: STOP XIO
\      R[ START XIC R, RUN XIC R]
\      3000 FAN_TIMER.ACC FAN_TIMER.TT FAN_TIMER.EN FAN_TIMER.DN TOF
\      RUN OTE
\   R;
\   R: RUN XIC MOTOR OTE R;
\   R: FAN_TIMER.DN XIC FAN OTE R;
\ ;

MOTOR_ROUTINE
MOTOR ? ( 0 )

START ON
MOTOR_ROUTINE
MOTOR ? ( -1 )

START OFF
MOTOR_ROUTINE
MOTOR ? ( -1 )

STOP ON
MOTOR_ROUTINE
MOTOR ? ( 0 )
```

As you can tell from the `SEE` output, the ladder diagram is compiled into a
human-readable definition that looks very similar to the standard L5K format.
This keeps the execution fast, as the ladder diagram is compiled rather than
interpreted.

The ladder compiler is also vectorized, `DEFER`ing the compiler for each
instruction. This allows the user to bring their own instruction set. If you
prefer the Siemens instruction set, simply `include ladder.fs` and implement the
desired instructions.

### Running the example

In order to run the example, you will need to install GForth. Once installed,
you can run the example program with `gforth main.fs`.

### Source files

A brief description of each source file:
- `ladder.fs` is the ladder logic compiler and runtime.
- `l5k.fs` is the extension to add support for L5K instructions.
- `test.fs` contains unit tests to verify functionality.
- `main.fs` is an example program to use as a reference.
