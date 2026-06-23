using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoding.SymbolEncodingTypes
{
    class EncodingSimple : ISymbolEncoder
    {
        public string Name { get { return "Простое кодирование"; } }

        private Dictionary<string, int[]> _symbolBits = new();

        public Dictionary<string, int[]> EncodeSymbols(List<string> symbolSequence)
        {
            var uniqueSymbols = symbolSequence.Distinct().ToList();
            var uniqueSymbolsCount = symbolSequence.Distinct().Count();
            var bitsPerSymbol = Math.Max(1, (int)Math.Ceiling(Math.Log(uniqueSymbolsCount, 2)));


            for (int i = 0; i < symbolSequence.Distinct().Count(); i++)
            {
                int[] bitCode = CreateBinaryCode(i, bitsPerSymbol);
                _symbolBits.Add(uniqueSymbols[i], bitCode);
            }


            return _symbolBits
                .ToDictionary(pair => pair.Key,pair => pair.Value.ToArray());
        }

        private static int[] CreateBinaryCode(int number, int codeLength)
        {
            int[] bitCode = new int[codeLength];

            for (int i = 0; i < codeLength; i++)
            {
                bitCode[codeLength - 1 - i] = (number >> i) & 1;
            }

            return bitCode;
        }
    }
}
