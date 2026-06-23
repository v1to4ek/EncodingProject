using Encoding.BitEncodingTypes;
using Encoding.SymbolEncodingTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Encoding
{
    public class Sequence
    {
        private readonly List<string> _symbolSequence;
        private readonly List<int> _bitSequence = new List<int>();
        private readonly Dictionary<string, int[]> _symbolBits;
        private ISymbolEncoder _symbolEncoder;

        public IReadOnlyList<string> SymbolSequence { get { return _symbolSequence; } }
        public IReadOnlyList<int> BitSequence { get { return _bitSequence;  } }
        public IReadOnlyDictionary<string, int[]> SymbolBits { get { return _symbolBits; } }

        public Sequence(List<string> symbolSequence, ISymbolEncoder symbolEncoder)
        {
            if (symbolSequence == null || symbolSequence.Count == 0) throw new ArgumentException("Получена пустая строка");
            _symbolSequence = symbolSequence;
            _symbolEncoder = symbolEncoder;
            _symbolBits = _symbolEncoder.EncodeSymbols(_symbolSequence);
            CreateBitSequence();
        }

        public Sequence(string textSequence, ISymbolEncoder symbolEncoder) : this(textSequence.Select(text => text.ToString()).ToList(), symbolEncoder)
        { }

        private void CreateBitSequence()
        {
            foreach (var symbol in _symbolSequence)
            {
                var symbolBits = _symbolBits[symbol];
                _bitSequence.AddRange(symbolBits);
            }
        }

        public override string ToString()
        {
            return string.Join(" ", _bitSequence);
        }

        public void PrintEncodedSymbols()
        {
            foreach(var element in _symbolBits)
            {
                var symbol = element.Key;
                var code = string.Join("", element.Value);
                Console.WriteLine(symbol + " " + code);
            }
        }

        public void PrintDecodedSequence()
        {
            var decodedSequence = Decode();
            Console.WriteLine(string.Join("", decodedSequence));
        }

        private List<string> Decode()
        {
            var uniqueSymbolsCount = _symbolSequence.Distinct().Count();
            var bitsPerSymbol = Math.Max(1, (int)Math.Ceiling(Math.Log(uniqueSymbolsCount, 2)));

            List<string> symbols = new();
            List<int> bits = new();

            for(int i = 0; i < _bitSequence.Count; i++)
            {
                foreach(var lines in _symbolBits)
                {
                    var value = lines.Value.ToList();
                    var listLength = value.Count();

                    for(int j = 0;j < listLength; j++)
                    {
                        bits.Add(_bitSequence[i+j]);
                    }

                    if (bits.SequenceEqual(value))
                    {
                        symbols.Add(lines.Key);
                        i += listLength-1;
                        bits.Clear();
                        break;
                    }
                    else
                    {
                        bits.Clear();
                    }
                }
            }
            return symbols;
        }
    }
}
