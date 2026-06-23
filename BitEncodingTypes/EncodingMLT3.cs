using System.Collections.Generic;

namespace Encoding.BitEncodingTypes
{
    class EncodingMLT3 : IBitEncoder
    {
        public string Name { get { return "MLT3"; } }

        public Axes Encode(List<int> bitSequence)
        {
            bitSequence.Add(bitSequence[bitSequence.Count - 1]);
            List<int> axisX = new List<int>();
            List<int> axisY = new List<int>();
            int level = 1;

            for (int i = 0; i < bitSequence.Count - 1; i++)
            {
                if (bitSequence[i] == 1) level++;
                level %= 4;
                switch (level)
                {
                    case 2: 
                        CreateUpperLevel(axisX, axisY, i);
                        break;
                    case 0: 
                        CreateLowerLevel(axisX, axisY, i);
                        break;
                    default:
                        CreateMiddleLevel(axisX, axisY, i);
                        break;
                }
            }
            return new Axes(axisX, axisY);
        }

        private static void CreateUpperLevel(List<int> axisX, List<int> axisY, int i)
        {
            axisX.AddRange([i, i + 1]);
            axisY.AddRange([1, 1]);
        }

        private static void CreateMiddleLevel(List<int> axisX, List<int> axisY, int i)
        {
            axisX.AddRange([i, i + 1]);
            axisY.AddRange([0, 0]);
        }

        private static void CreateLowerLevel(List<int> axisX, List<int> axisY, int i)
        {
            axisX.AddRange([i, i + 1]);
            axisY.AddRange([-1, -1]);
        }
    }
}
