using System.Drawing;

namespace CV19Core.Models
{
    internal class CountryInfo : PlaceInfo
    {
        private Point? _location;
        

        public override Point Location
        {
            get
            {
                if (_location != null) return (Point)_location;
                if (Provinces is null) return default;

                var averageX = (int)Provinces.Average(p => p.Location.X);
                var averageY = (int)Provinces.Average(p => p.Location.Y);
                return (Point)(_location = new Point(averageX, averageY));
            }
            set => _location = value;
        }
        
        public IEnumerable<PlaceInfo> Provinces { get; set; }

        private IEnumerable<ConfirmedCount> _counts;

        public override IEnumerable<ConfirmedCount> Counts
        {
            get
            {
                if (Counts != null) return _counts;

                var pointsCounts = Provinces.FirstOrDefault()?.Counts?.Count() ?? 0;
                if (pointsCounts == 0) return Enumerable.Empty<ConfirmedCount>();

                var provincePints = Provinces.Select(p => p.Counts.ToArray()).ToArray();

                var points = new ConfirmedCount[pointsCounts];
                foreach (var provinceInfo in provincePints)
                    for (int i = 0; i < pointsCounts; i++)
                    {
                        if (points[i].Date == default)
                            points[i] = provinceInfo[i];
                        else
                            points[i].Count += provinceInfo[i].Count;

                    }

                return points;
            }
            set => _counts = value;
        }

        
    }
}
