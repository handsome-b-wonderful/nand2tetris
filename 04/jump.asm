// increment by X loop
@3 // inc by
D=A
@INC
M=D
@SUM
M=0
(LOOP)
	@INC
	D=M
	@SUM
	M=D+M
	@LOOP
	0;JMP