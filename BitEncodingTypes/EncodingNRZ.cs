using System.Collections.Generic;

namespace Encoding.BitEncodingTypes
{
    class EncodingNRZ : IBitEncoder
    {
        public string Name { get { return "NRZ"; } }

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
                    axisY.Add(InvertBit(bitSequence[i]));
                }
                else
                {
                    axisX.AddRange([i, i]);
                    axisY.AddRange([InvertBit(currentBit), InvertBit(bitSequence[i])]);
                }
                currentBit = bitSequence[i];
            }

            return new Axes(axisX, axisY);

        }

        private static int InvertBit(int bit)
        {
            if (bit == 0) return 1;
            else return 0;
        }
    }
}
