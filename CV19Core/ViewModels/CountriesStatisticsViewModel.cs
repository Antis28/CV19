using CV19Core.Services;
using CV19Core.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV19Core.ViewModels
{
    internal class CountriesStatisticsViewModel : ViewModel
    {
        private DataService _dataService;

        private  MainViewModel MainModel { get; }

        public CountriesStatisticsViewModel(MainViewModel mainViewModel)
        {
            MainModel = mainViewModel;
            _dataService = new DataService();
        }
    }
}
