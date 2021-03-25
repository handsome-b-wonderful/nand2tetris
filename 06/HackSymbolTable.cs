using System;
using System.Collections.Generic;
using System.Text;

namespace HackAssembler
{
    public class HackSymbolTable : ISymbolTable
    {
        public enum Label { SP, LCL, ARG, THIS, THAT, R0, R1, R2, R3, R4, R5, R6, R7, R8, R9, R10, R11, R12, R13, R14, R15, SCREEN, KBD };

        public const int StartOfRam = 16;

        private Dictionary<string,int> _table;
        private int freeRam = StartOfRam;
        public HackSymbolTable()
        {
            _table = new Dictionary<string, int>();
            
            // constant values
            foreach (string label in Enum.GetNames(typeof(Label)))
                _table.Add(label, Ram((Label)(Enum.Parse(typeof(Label), label))));
        }

        public static int Ram(Label label)
        {
            switch (label)
            {
                case Label.SP:
                case Label.R0:
                    return 0;
                case Label.LCL:
                case Label.R1:
                    return 1;
                case Label.ARG:
                case Label.R2:
                    return 2;
                case Label.THIS:
                case Label.R3:
                    return 3;
                case Label.THAT:
                case Label.R4:
                    return 4;
                case Label.R5:
                    return 5;
                case Label.R6:
                    return 6;
                case Label.R7:
                    return 7;
                case Label.R8:
                    return 8;
                case Label.R9:
                    return 9;
                case Label.R10:
                    return 10;
                case Label.R11:
                    return 11;
                case Label.R12:
                    return 12;
                case Label.R13:
                    return 13;
                case Label.R14:
                    return 14;
                case Label.R15:
                    return 15;
                case Label.SCREEN:
                    return 16384;
                case Label.KBD:
                    return 24576;
            }
            throw new Exception($"Invalid predefined symbol \"{label}\"");
        }

        public void Add(string symbol)
        {
            Add(symbol, freeRam);
            freeRam++;
        }

        public void Add(string symbol, int address)
        {
            if (Contains(symbol))
                _table[symbol] = address;
            else
                _table.Add(symbol, address);
        }

        public bool Contains(string symbol)
        {
            return (_table.ContainsKey(symbol));
        }

        public int Get(string symbol)
        {
            if (Contains(symbol))
                return _table[symbol];

            throw new KeyNotFoundException($"No entry for symbol \"{symbol}\" found.");
        }
    }
}
