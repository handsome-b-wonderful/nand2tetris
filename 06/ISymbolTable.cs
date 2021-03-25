using System;
using System.Collections.Generic;
using System.Text;

namespace HackAssembler
{
    public interface ISymbolTable
    {
        void Add(string symbol);
        void Add(string symbol, int address);
        bool Contains(string symbol);
        int Get(string symbol);
    }
}
