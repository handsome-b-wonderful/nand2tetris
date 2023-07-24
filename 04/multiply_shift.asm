// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/04/Mult.asm

// Multiplies R0 and R1 and stores the result in R2.
// (R0, R1, R2 refer to RAM[0], RAM[1], and RAM[2], respectively.)
//
// This program only needs to handle arguments that satisfy
// R0 >= 0, R1 >= 0, and R0*R1 < 32768.

// bit-shifting version
//    R2 = 0;
//    while (R1 != 0)               // Iterate the loop till R1 == 0
//    {
//        if (R1 & 1)               // Bitwise AND of R1 with 1 which determines if R1is odd
//        {
//            R2 = R2 + R0;  // Add R0 to result if R1 is odd.
//        }
//        R0 <<= 1;                    // Left shifting the value contained in R0. This multiplies R0 by 2 for each loop.
//        R1 >>= 1;                    // Right shifting the value contained in R1 by 1 position.
//    }

// NOTES
// L-SHIFT: just add the value to itself (multiply by 2)
// R-SHIFT: L-SHIFT with ROTATE 15 times then discard the MSB

(SETUP)
	@R2 // product
	M=0
(LOOP)
	@R1
	D=M
	@END
	D;JEQ
	// if R1 AND 1 then product = product + R0
	@R1
	D=M
	@1
	D=D&A
	@SKIP
	D;JEQ
	@R0
	D=M
	@R2
	M=D+M
(SKIP)
	// L-shift R0 by 1
	@R0
	D=M
	M=D+M
	// R-shift R1 by 1 (L=shift with rotate by 15?)
	@RSHIFT
	0;JMP
(FROM)
	@LOOP
	0;JMP
(END)
	@END
	0;JMP

// L-Shift with rotate 15 times
(RSHIFT)
	@15
	D=A
	@count
	M=D
	@R1 // value in R1
	D=M
	@val
	M=D
(LOOP2)
	@count
	D=M
	@RSHIFT_DONE
	D;JLE
	@val
	D=M
	@msb
	M=1
	@NEG
	D;JLT
	@msb
	M=M-1
(NEG)
	@val
	D=M
	M=D+M
	@msb
	D=M
	@val
	M=D+M
	@count
	M=M-1
	@LOOP2
	0;JMP
(RSHIFT_DONE)
	@val // transfer val back to R1
	D=M
	@32767 // drop MSB
	D=D&A
	@R1
	M=D
	@FROM // done - return
	0;JMP





	
