using System.Collections.Generic;

namespace Encoding.SymbolEncodingTypes
{
    public interface ISymbolEncoder
    {
        public string Name { get; }
        public Dictionary<string, int[]> EncodeSymbols(List<string> symbolSequence);
    }
}
