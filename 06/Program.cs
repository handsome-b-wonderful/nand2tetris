using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HackAssembler
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("ERROR: pass the input file as an argument to the assembler");
                return;
            }

            var raw = args[0];
            var info = new FileInfo(raw);

            if (!info.Exists)
            {
                Console.WriteLine($"ERROR: file \"{info.FullName}\" not found.");
                return;
            }

            Console.WriteLine($"assembling `\"{info.FullName}\"...");

            var symbols = new HackSymbolTable();
            var code = new Code(symbols);
            var target = new List<string>();

            using (var parser = new Parser(info.FullName))
            {
                // first-pass
                //current ROM location
                var romLocation = 0;
                while (parser.HasMoreCommands)
                {
                    parser.Advance();
                    switch (parser.CommandType)
                    {
                        case Parser.CommandTypes.NO_COMMAND:
                            // do nothing
                            break;
                        case Parser.CommandTypes.A_COMMAND:
                            // A-instruction
                            romLocation++;
                            break;
                        case Parser.CommandTypes.L_COMMAND:
                            symbols.Add(parser.Symbol, romLocation);
                            break;
                        case Parser.CommandTypes.C_COMMAND:
                            // C-instruction
                            romLocation++;
                            break;
                    }
                }

                // second pass
                parser.Reset();
                while (parser.HasMoreCommands)
                {
                    parser.Advance();
                    Console.WriteLine(parser.Raw);
                    switch (parser.CommandType)
                    {
                        case Parser.CommandTypes.NO_COMMAND:
                            // do nothing
                            break;
                        case Parser.CommandTypes.A_COMMAND:
                            // A-instruction
                            target.Add($"0{code.Resolve(parser.Symbol)}");
                            break;
                        case Parser.CommandTypes.L_COMMAND:
                            // do nothing
                            break;
                        case Parser.CommandTypes.C_COMMAND:
                            // C-instruction
                            target.Add(code.Instruction(parser.Compute, parser.Destination, parser.Jump));
                            break;
                    }
                }
            }

            var ml = info.FullName.Replace(info.Extension, ".hack");
            Console.WriteLine($"Saving \"{ml}\"...");

            using (var sw = new StreamWriter(ml, false, System.Text.Encoding.ASCII))
                foreach (var line in target)
                    await sw.WriteLineAsync(line);

            Console.WriteLine("DONE.");
        }
    }
}
