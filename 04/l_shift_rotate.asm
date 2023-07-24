// L-shift with rotate used to implement R-shift
// performs a L-shift with rotate 15 times to implement a R-shift
// NOTE: Drops MSB at end. Remove this code if you want a R-Shift with rotate

	@15 // loop counter
	D=A
	@counter
	M=D
	@24579 // 0110 0000 0000 0011
	D=A
	@result
	M=D
(LOOP)
	@counter
	D=M
	@END
	D;JLE
	@result
	D=M
	@msb // save the MSB
	M=1 // assume 1
	@SKIP
	D;JLT // negative?
	@msb // no - set to 0
	M=M-1
(SKIP)
	@result
	D=M
	M=D+M // double for L-shift
	@msb
	D=M
	@result
	M=D+M // add MSB as LSB for rotate
	@counter
	M=M-1 // decrement counter
	@LOOP
	0;JMP
(END)
	@32767 // mask of 0111 1111 1111 1111 to drop MSB
	D=A
	@result
	M=D&M // apply mask to result
	@END
	0;JMP
