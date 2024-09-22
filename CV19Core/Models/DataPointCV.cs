using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;

namespace CV19Core.Models
{
    internal struct DataPointCV :IDataPointProvider
    {
        public double XValue { get; set; }
        public double YValue { get; set; }
        public DataPoint GetDataPoint()
        {
            return new DataPoint(XValue, YValue);
        }
    }
}
