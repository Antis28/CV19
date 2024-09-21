using System.Net;

namespace CV19Console
{
    internal class Program
    {
        private const string DataUrl =
            @"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/refs/heads/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";
        private const string DataUrlActual = @"https://srhdpeuwpubsa.blob.core.windows.net/whdh/COVID/WHO-COVID-19-global-data.csv";
       
        static void Main(string[] args)
        {
            //var client = new WebClient();
            var client = new HttpClient();

            var response = client.GetAsync(DataUrl).Result;
            
            var csvString = response.Content.ReadAsStringAsync().Result;
            Console.ReadLine();
        }
    }
}
