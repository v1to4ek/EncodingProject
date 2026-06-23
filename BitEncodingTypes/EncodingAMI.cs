using System.Collections.Generic;

namespace Encoding.BitEncodingTypes
{
    class EncodingAMI : IBitEncoder
    {
        public string Name { get { return "AMI"; } }

        public Axes Encode(List<int> bitSequence)
        {
            bitSequence.Add(bitSequence[bitSequence.Count - 1]);
            List<int> axisX = new List<int>();
            List<int> axisY = new List<int>();
            bool inverted = false;

            for (int i = 0; i < bitSequence.Count-1; i++)
            {
                if(bitSequence[i] == 0)
                {
                    CreateZeroLevel(axisX, axisY, i);
                }
                else
                {
                    CreateOneLevel(axisX, axisY, i, inverted);
                    inverted = !inverted;
                }
            }
            return new Axes(axisX, axisY);
        }

        private static void CreateZeroLevel(List<int> axisX, List<int> axisY, int i)
        {
            axisX.AddRange([i, i + 1]);
            axisY.AddRange([0, 0]);
        }

        private static void CreateOneLevel(List<int> axisX, List<int> axisY,int i, bool inverted)
        {
            if (inverted)
            {
                axisX.AddRange([i, i + 1]);
                axisY.AddRange([-1, -1]);
            }
            else
            {
                axisX.AddRange([i, i + 1]);
                axisY.AddRange([1, 1]);
            }
        }
    }
}
