\ simple LCD HD44780 interface

\ needs the following includes:
\ ./lib/ans94/core-ext/marker.frt
\ lib/bitnames.frt


marker lcd:forth:ifs

PORTD 3 portpin: pin:lcd:enable
PORTD 2 portpin: pin:lcd:rs

: init:lcd:pins
    pin:lcd:enable pin_output
    pin:lcd:enable low
    pin:lcd:rs pin_output
    pin:lcd:rs low
    DDRD c@ %11110000 OR DDRD c!
;

: nop  \ wait a short delay...
;

: lcd:enable \ pulse the Enable pin
    pin:lcd:enable high
    \ 10 0 do nop loop \ wait a short time...
    1ms
    pin:lcd:enable low
;


: delay5ms
    5 0 ?do 1ms loop
;


: write-hnibble ( n -- )
    \ set lower nibble to 0
    $F0 AND 
    PORTD c@ $F AND  \ get current PORTD setting and set high nibble to 0
    \ combine high and low nibble and write it to port
    OR  PORTD C!
    lcd:enable
;
    


: write-byte ( n -- )
    dup write-hnibble
    1ms
    4 LSHIFT
    write-hnibble
    1ms
;

: lcd:command ( n -- )
    pin:lcd:rs low
    write-byte

;


: lcd:data ( n -- )
    pin:lcd:rs high
    write-byte
;

: lcd:init
    
    init:lcd:pins
    delay5ms
    \ write 0011 to the High nible of PORTD 3 times
    3 0 ?do
	PORTD c@ %00001111 AND \ fetch PORTD and set high nibble to 0000
	%00110000 OR PORTD c!  \ set high nibble to 0010 and leave low nibble untouched
	lcd:enable
	delay5ms
    loop
    \ 4 Bit Mode --> set high nibble to 0010
    PORTD c@ %00001111 AND \ fetch PORTD and set high nibble to 0000
    %00100000 OR PORTD c!  \ set high nibble to 0010 and leave low nibble untouched
    lcd:enable
    delay5ms
    \ 4 Bit, zwei Zeilen, 5x8
    %00101000 lcd:command
    delay5ms
    %00001100 lcd:command \ Display ein / Cursor aus / kein Blinken
    delay5ms
    %00000100 lcd:command \ kein increment, kein scrollen.
    delay5ms
;


: lcd:string ( caddr u  -- )
    0 ?do
	dup I + c@ lcd:data
    loop
    drop
;

: lcd:home
    %10 lcd:command
;

: lcd:clear
    1 lcd:command
;

\ display string from flash
: lcd:istring ( caddr u -- )
    0 ?do
	dup I + @I lcd:data
    loop
    drop
;
    