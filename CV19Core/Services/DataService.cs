using System.Drawing;
using System.Globalization;
using CV19Core.Models;
using System.IO;
using System.Net.Http;
using System.Linq;

namespace CV19Core.Services
{
    internal class DataService
    {
        private const string _DataSourceAddress =
            @"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/refs/heads/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";


        /// <summary>
        /// Позволяет читать файл не скачивая его сразу весь.
        /// </summary>
        /// <returns>Поток для чтения файла</returns>
        private static async Task<Stream> GetDataStream()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(
                _DataSourceAddress, 
                HttpCompletionOption.ResponseHeadersRead);
            return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Читает содержимое построчно, не скачивая весь файл
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<string> GetDataLines()
        {
            using var dataStream = GetDataStream().Result;
            using var dataReader = new StreamReader(dataStream);
            while (!dataReader.EndOfStream)
            {
                var line = dataReader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) { continue; }
                yield return line.Replace("Korea,", "Korea-")
                    .Replace("Bonaire,", "Bonaire-")
                    .Replace("Saint Helena,", "Saint Helena-");
            }
        }
        /// <summary>
        /// Получает даты для данных
        /// </summary>
        /// <returns></returns>
        private static DateTime[] GetDates() => GetDataLines()
            .First()
            .Split(',')
            .Skip(4)
            .Select(s => DateTime.Parse(s, CultureInfo.InvariantCulture))
            .ToArray();


        /// <summary>
        /// Извлекает информацию по каждой стране
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<(string Province, string Country,(double Lat,double Lon) Place, int[] Counts)> GetCountiesData()
        {
            var lines = GetDataLines()
                .Skip(1)
                .Select(line => line.Split(','));

            foreach (var row in lines)
            {
                var province = row[0].Trim();
                var countryName = row[1].Trim(' ', '"');
                var latitude = double.Parse(row[2]);
                var longitude = double.Parse(row[3]);
                var counts = row.Skip(4)
                    .Select(int.Parse)
                    .ToArray();
                
                yield return (province, countryName,(latitude,longitude), counts);
            }
        }

        public IEnumerable<CountryInfo> GetData()
        {
            var dates = GetDates();
            var data = GetCountiesData().GroupBy(d => d.Country);
            foreach (var countryInfo in data)
            {
                
                var country = new CountryInfo
                {
                    Name = countryInfo.Key,
                    ProvinceCounts = countryInfo.Select(c => new PlaceInfo
                    {
                        Name = c.Province,
                        Location = new Point((int)c.Place.Lat, (int)c.Place.Lon),
                        Counts = dates.Zip(c.Counts, (date,count)=> new ConfirmedCount
                        {
                            Date = date,
                            Count = count
                        })
                    }),
                };
                yield return country;
            }

           // return Enumerable.Empty<CountryInfo>();
        }
    }
}
