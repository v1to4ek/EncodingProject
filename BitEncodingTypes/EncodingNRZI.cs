using System.Collections.Generic;

namespace Encoding.BitEncodingTypes
{
    class EncodingNRZI : IBitEncoder
    {
        public string Name { get { return "NRZI"; } }

        public Axes Encode(List<int> bitSequence)
        {
            bitSequence.Add(bitSequence[bitSequence.Count - 1]);
            List<int> axisX = new List<int>();
            List<int> axisY = new List<int>();
            bool isInverted = false;

            for (int i = 0; i < bitSequence.Count - 1; i++) 
            {
                if (bitSequence[i] == 1) isInverted = !isInverted;
                CreateLevel(axisX, axisY, i, isInverted);
            }
            return new Axes(axisX, axisY);
        }

        private static void CreateLevel(List<int> axisX,List<int> axisY,int i,bool isInverted)
        {
            axisX.AddRange([i, i + 1]);

            if (isInverted)
            {
                axisY.AddRange([1, 1]);
            }
            else
            {
                axisY.AddRange([0, 0]);
            }
        }
    }
}
