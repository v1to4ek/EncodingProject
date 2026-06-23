using System;
using System.Collections.Generic;
using System.Linq;

namespace Encoding.BitEncodingTypes
{
    class EncodingB8ZS : IBitEncoder
    {
        public string Name { get { return "B8ZS"; } }

        public Axes Encode(List<int> bitSequence)
        {
            bitSequence.Add(bitSequence[bitSequence.Count - 1]);
            List<int> axisX = new List<int>();
            List<int> axisY = new List<int>();
            bool inverted = false;

            for (int i = 0; i < bitSequence.Count - 1; i++)
            {
                if (bitSequence[i] == 0)
                {
                    if (HasOneInNextEight(i, bitSequence)) 
                    {
                        CreateZeroLevel(axisX, axisY, i);
                    }
                    else
                    {
                        CreateSubstitution(axisX, axisY, ref i, inverted);
                    }
                }
                else
                {
                    CreateOneLevel(axisX, axisY, i, inverted);
                    inverted = !inverted;
                }
            }
            return new Axes(axisX, axisY);
        }

        private static bool HasOneInNextEight(int i, List<int> bitSequence)
        {
            var hasOne = true;
            if(i + 9 <= bitSequence.Count) hasOne = bitSequence.GetRange(i, 8).Distinct().Contains(1);
            return hasOne;
        }

        private static void CreateZeroLevel(List<int> axisX, List<int> axisY, int i)
        {
            axisX.AddRange([i, i + 1]);
            axisY.AddRange([0, 0]);
        }

        private static void CreateOneLevel(List<int> axisX, List<int> axisY, int i, bool inverted)
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

        private static void CreateSubstitution(List<int> axisX, List<int> axisY,ref int i, bool inverted)
        {
            axisX.AddRange([i, i+1, i+2, i+3, i+3, i+4, i+4, i+5, i+5, i+6, i+6, i+7, i+7, i+8, i+8]);
            if (inverted) axisY.AddRange([0, 0, 0, 0, 1, 1, -1, -1, 0, 0, -1, -1, 1, 1, -1]);
            else axisY.AddRange([0, 0, 0, 0, -1, -1, 1, 1, 0, 0, 1, 1, -1, -1, 1]);
            i += 7;
        }
    }
}
