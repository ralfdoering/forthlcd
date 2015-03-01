# forthlcd
Example code on how to connect a HD44780 or compatible LCD display to an amForth powered device

This is my first attempt on writing Forth code to interface an HD44780 based LCD to an Arduino running [AmForth](http://amforth.sourceforge.net/). For now, the pin setup is hardwired to the scheme shown below, but this will be changed in a future version. The code is running AmForth 5.7, the Forth kernel is build with the following `dict_appl.inc`:

```
; this dictionary contains optional words
; they may be moved to the core dictionary if needed

.include "dict/compiler2.inc" ; additional words for the compiler
.include "words/applturnkey.asm"

;; provide .s for interactive debugging
.include "words/dot-s.asm"

;; the following are needed to get the LCD code running
.include "words/edefer.asm"
.include "words/ms.asm"
```

# Origin

The base ideas for the forth code come from an [article](http://www.mikrocontroller.net/articles/AVR-Tutorial:_LCD) at http://www.mikrocontroller.net. I just adapted their assembler code to Forth.



# Hardware setup ##

The LCD will work in 4bit mode, with the following connections:

| Display Pin | Atmega Pin | Arduino Name |
|-------------|------------|--------------|
| D7          | PD7        | Digital 7    |
| D6          | PD6        | Digital 6    |
| D5          | PD5        | Digital 5    |
| D4          | PD4        | Digital 4    |
| Enable (E)  | PD3        | Digital 3    |
| RS          | PD2        | Digital 2    |
