using System.Collections.Generic;

namespace Encoding.BitEncodingTypes
{
    class EncodingBinary : IBitEncoder
    {
        public string Name { get { return "Binary"; } }
        public Axes Encode(List<int> bitSequence)
        {
            bitSequence.Add(bitSequence[bitSequence.Count - 1]);
            List<int> axisX = new List<int>();
            List<int> axisY = new List<int>();
            int currentBit = bitSequence[0];

            for (int i = 0; i < bitSequence.Count; i++)
            {
                if (currentBit == bitSequence[i])
                {
                    axisX.Add(i);
                    axisY.Add(bitSequence[i]);
                }
                else
                {
                    axisX.AddRange([i, i]);
                    axisY.AddRange([currentBit, bitSequence[i]]);
                    //axisX.Add(i);
                    //axisY.Add(currentBit);
                    //axisX.Add(i);
                    //axisY.Add(bitSequence[i]);
                }
                currentBit = bitSequence[i];
            }

            return new Axes(axisX, axisY);
        }
    }
}
