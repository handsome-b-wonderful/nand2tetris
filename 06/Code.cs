using System;
using System.Collections.Generic;
using System.Text;

namespace HackAssembler
{
    public class Code
    {
        public ISymbolTable Symbols { get; private set; }
        public Code(ISymbolTable symbols) // TODO: pass in Symbol table
        {
            Symbols = symbols;
        }

        public string Resolve(string address)
        {
            if (int.TryParse(address, out var val))
            {
                // address was location
                var ram = "000000000000000" + ToBinary(val);
                return ram.Substring(ram.Length - 15);
            }

            // address is a variable
            if (!Symbols.Contains(address)) // new; add it
                Symbols.Add(address);

            // return value from symbol table
            var location = "000000000000000" + ToBinary(Symbols.Get(address));
            return location.Substring(location.Length - 15);
        }

        public string Instruction(string code, string destination, string jump)
        {
            return "111" + Compute(code) + Destination(destination) + Jump(jump);
        }

        public string Destination(string destination)
        {
            switch (destination)
            {
                case "":
                case null:
                    return "000";
                case "M":
                    return "001";
                case "D":
                    return "010";
                case "MD":
                    return "011";
                case "A":
                    return "100";
                case "AM":
                    return "101";
                case "AD":
                    return "110";
                case "AMD":
                    return "111";
                default:
                    throw new NotSupportedException($"Unrecognized destination: \"{destination}\".");
            }
        }

        public string Compute(string comp)
        {
            switch (comp)
            {
                case "0":
                    return "0101010";
                case "1":
                    return "0111111";
                case "-1":
                    return "0111010";
                case "D":
                    return "0001100";
                case "A":
                    return "0110000";
                case "M":
                    return "1110000";
                case "!D":
                    return "0001101";
                case "!A":
                    return "0110001";
                case "!M":
                    return "1110001";
                case "-D":
                    return "0001111";
                case "-A":
                    return "0110011";
                case "-M":
                    return "1110011";
                case "D+1":
                    return "0011111";
                case "A+1":
                    return "0110111";
                case "M+1":
                    return "1110111";
                case "D-1":
                    return "0001110";
                case "A-1":
                    return "0110010";
                case "M-1":
                    return "1110010";
                case "D+A":
                    return "0000010";
                case "D+M":
                    return "1000010";
                case "D-A":
                    return "0010011";
                case "D-M":
                    return "1010011";
                case "A-D":
                    return "0000111";
                case "M-D":
                    return "1000111";
                case "D&A":
                    return "0000000";
                case "D&M":
                    return "1000000";
                case "D|A":
                    return "0010101";
                case "D|M":
                    return "1010101";
                default:
                    throw new NotSupportedException($"Unrecognized code: \"{comp}\".");
            }
        }

        public string Jump(string jump)
        {
            switch (jump)
            {
                case "":
                case null:
                    return "000";
                case "JGT":
                    return "001";
                case "JEQ":
                    return "010";
                case "JGE":
                    return "011";
                case "JLT":
                    return "100";
                case "JNE":
                    return "101";
                case "JLE":
                    return "110";
                default: // unconditional
                    // JMP
                    return "111";
            }

        }


        public static string ToBinary(int val)
        {
            return Convert.ToString(val, 2);
        }

        public static int FromBinary(string binary)
        {
            return Convert.ToInt32(binary, 2);
        }

        public static string ToHex(int val)
        {
            return val.ToString("X");
        }

        public static int FromHex(string hex)
        {
            return int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
        }
    }
}
