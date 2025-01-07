using CV19Core.Services;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CV19Core
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var serviceTest = new DataService();

            var countries = serviceTest.GetData().ToArray();
        }
    }

}
