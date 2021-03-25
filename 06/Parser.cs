using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace HackAssembler
{
    public class Parser : IDisposable
    {
        public enum CommandTypes { NO_COMMAND, A_COMMAND, C_COMMAND, L_COMMAND };

        public bool HasMoreCommands
        {
            get
            {
                if (_reader == null)
                    return false;
                return !_reader.EndOfStream;
            }
        }

        public CommandTypes CommandType { get; private set; }

        public string Raw { get; private set; }
        public string Symbol { get; private set; }

        public string Destination { get; private set; }

        public string Compute { get; private set; }

        public string Jump { get; private set; }

        private StreamReader _reader;

        public Parser(string filename)
        {
            _reader = new StreamReader(filename);
            CommandType = CommandTypes.NO_COMMAND;
            Raw = null;
            Symbol = null;
            Destination = null;
            Compute = null;
            Jump = null;
        }

        public void Reset()
        {
            if (_reader != null)
                _reader.DiscardBufferedData();
            _reader.BaseStream.Seek(0, SeekOrigin.Begin);
        }

        public void Advance()
        {
            CommandType = CommandTypes.NO_COMMAND;
            Symbol = null;
            Destination = null;
            Compute = null;
            Jump = null;

            if (!HasMoreCommands)
                return;

            var line = _reader.ReadLine();
            Raw = line;

            // comments
            var commentPos = line.IndexOf("//");
            if (commentPos >= 0)
                line = line.Substring(0, commentPos);

            // whitespace
            line = line.Trim();

            if (line == string.Empty) // skip comments and whitespace-only lines
                return;

            if (line.StartsWith("@"))
            {
                CommandType = CommandTypes.A_COMMAND;
                Symbol = line.Substring(1);
                return;
            }

            if (line.StartsWith("(") && line.EndsWith(")"))
            {
                CommandType = CommandTypes.L_COMMAND;
                Symbol = line.Substring(1, line.Length - 2);
                return;
            }

            if (line.Contains("=") || line.Contains(";"))
            {
                CommandType = CommandTypes.C_COMMAND;

                string remain;
                var eqPos = line.IndexOf("=");
                if (eqPos >= 0)
                {
                    Destination = line.Substring(0, eqPos);
                    remain = line.Substring(eqPos + 1);
                }
                else
                    remain = line;

                var semiPos = remain.IndexOf(";");
                if (semiPos >= 0)
                {
                    Compute = remain.Substring(0, semiPos);
                    Jump = remain.Substring(semiPos + 1);
                }
                else
                    Compute = remain;

                return;
            }

            throw new NotSupportedException($"Input of \"{line}\" is not supported");
        }

        public void Dispose()
        {
            if (_reader != null)
            {
                _reader.Close();
                _reader.Dispose();
            }
            _reader = null;
        }
    }
}
