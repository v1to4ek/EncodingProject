using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoding
{
    public class Axes
    {
        private readonly List<double> _axisX;
        private readonly List<double> _axisY;

        public IReadOnlyList<double> AxisX { get { return _axisX; } }
        public IReadOnlyList<double> AxisY { get { return _axisY; } }

        public Axes(List<double> axisX, List<double> axisY)
        {
            _axisX = axisX;
            _axisY = axisY;
        }

        public Axes(List<int> axisX, List<int> axisY) :
            this(axisX.Select(x =>(double)x).ToList(), axisY.Select(y => (double)y).ToList())
        { }

        public Axes(List<double> axisX, List<int> axisY) :
            this(axisX, axisY.Select(y => (double)y).ToList())
        { }

        public Axes(List<int> axisX, List<double> axisY) :
            this(axisX.Select(x => (double)x).ToList(), axisY)
        { }
    }
}
