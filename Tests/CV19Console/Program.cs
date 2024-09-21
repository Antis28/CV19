using System.Data;
using System.Globalization;
using System.Net;

namespace CV19Console
{
    internal class Program
    {
        private const string DataUrl =
            @"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/refs/heads/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";
        //@"https://srhdpeuwpubsa.blob.core.windows.net/whdh/COVID/WHO-COVID-19-global-data.csv";


        /// <summary>
        /// Позволяет читать файл не скачивая его сразу весь.
        /// </summary>
        /// <returns>Поток для чтения файла</returns>
        private static async Task<Stream> GetDataStream()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(DataUrl, HttpCompletionOption.ResponseHeadersRead);
            return await response.Content.ReadAsStreamAsync();
        }

        private static IEnumerable<string> GetDataLines()
        {
            using var dataStream = GetDataStream().Result;
            using var dataReader = new StreamReader(dataStream);
            while (!dataReader.EndOfStream)
            {
                var line = dataReader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)){continue;}
                yield return line;
            }
        }

        private static DateTime[] GetDates()=> GetDataLines()
            .First()
            .Split(',')
            .Skip(4)
            .Select(s=> DateTime.Parse(s, CultureInfo.InvariantCulture))
            .ToArray();
        

        static void Main(string[] args)
        {
        //foreach (var dataLine in GetDataLines())
        //{   
        //    Console.WriteLine(dataLine);
        //}
            var dates = GetDates();
            Console.WriteLine(string.Join("\r\n", dates));

            Console.ReadLine();
        }
    }
}
