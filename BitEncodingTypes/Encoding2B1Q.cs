using System.Collections.Generic;

namespace Encoding.BitEncodingTypes
{
    class Encoding2B1Q : IBitEncoder
    {
        public string Name { get { return "2B1Q"; } }

        public Axes Encode(List<int> bitSequence)
        {
            bitSequence.Add(bitSequence[bitSequence.Count - 1]);
            List<int> axisX = new List<int>();
            List<double> axisY = new List<double>();
            double level = 0;

            for(int i = 0; i < bitSequence.Count - 1; i += 2)
            {
                if (bitSequence[i] == 0 && bitSequence[i + 1] == 0) level = -2.5;
                if (bitSequence[i] == 0 && bitSequence[i + 1] == 1) level = -0.833;
                if (bitSequence[i] == 1 && bitSequence[i + 1] == 1) level = 0.833;
                if (bitSequence[i] == 1 && bitSequence[i + 1] == 0) level = 2.5;
                CreateLevel(axisX, axisY, level, i);
            }
            return new Axes(axisX, axisY);
        }

        private static void CreateLevel(List<int> axisX, List<double> axisY, double level,int i)
        {
            axisX.AddRange([i, i + 2]);
            axisY.AddRange([level, level]);
        }
    }
}
