// R2 = MAX(R0,R1)
// IF (R0>R1) THEN R0 ELSE R1
	@R1 // second
	D=M
	@R2
	M=D // put R1 in result (assume first <= second)
	@R0 // first
	D=D-M
	@FIRST
	D;JLT
	@END
	0;JMP
(FIRST)
	@R0
	D=M
	@R2
	M=D
(END)
	@END
	0;JMP
