using Encoding.BitEncodingTypes;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Encoding.EncodingHandling
{
    public class Encoder
    {
        private IBitEncoder _sequenceEncoder;

        private Plot _signalPlot;

        public Encoder(IBitEncoder sequenceEncoder)
        {
            _sequenceEncoder = sequenceEncoder;
            _signalPlot = new Plot();
        }

        public void Encode(List<int> bitSequence)
        {
            CheckBitSequence(bitSequence);
            var axes = _sequenceEncoder.Encode(bitSequence);
            var name = CreateName();
            CreateSignalPlot(axes, bitSequence, name);
            Console.WriteLine();
            Console.WriteLine("Успешно!");
        }

        private static void CheckBitSequence(List<int> bitSequence)
        {
            var distinctedbitSequence = bitSequence.Distinct();
            bool isInvalid = distinctedbitSequence.Any(x => x != 0 && x != 1);
            if (isInvalid) throw new ArgumentException("Последовательность содержит некорректное значние");
        }

        private string CreateName()
        {
            var time = System.DateTime.Now.ToString().Replace(" ","_").Replace(":","_").Replace(".","_");
            var name = $"{_sequenceEncoder.Name}-{time}.png";
            return name ;
        }

        private void CreateSignalPlot(Axes axes, List<int> bitSequence, string name)
        {
            _signalPlot.Add.Scatter(axes.AxisX.ToList(), axes.AxisY.ToList());
            _signalPlot.Axes.SetLimits(0, bitSequence.Count - 1, -1, 2);
            _signalPlot.Axes.ZoomOutY(2);
            _signalPlot.SavePng(name, 1300, 300);
        }
    }
}
