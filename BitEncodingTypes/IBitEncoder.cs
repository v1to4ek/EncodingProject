using System.Collections.Generic;

namespace Encoding.BitEncodingTypes
{
    public interface IBitEncoder
    {
        public string Name { get; }
        public Axes Encode(List<int> bitSequence);
    }
}
