// fibonnaci
	@FIRST
	M=0
	@SECOND
	M=1
	// counter
	@10
	D=A
	@COUNT
	M=D
(LOOP)
	@FIRST
	D=M
	@SECOND
	D=D+M
	@SUM
	M=D
	// transfer
	@SECOND
	D=M
	@FIRST
	M=D
	@SUM
	D=M
	@SECOND
	M=D
	@COUNT
	MD=M-1 	// dec loop counter
	@LOOP
	D;JGT // repeat until COUNT == 0
(END)
	@END
	0;JMP // done
