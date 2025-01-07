using CV19Core.Infrastructure.Commands;
using CV19Core.Models;
using CV19Core.Services;
using CV19Core.ViewModels.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CV19Core.ViewModels
{
    internal class CountriesStatisticsViewModel : ViewModel
    {
        private DataService _dataService;

        private MainViewModel MainModel { get; }



        #region Countries : IEnumerable<CountryInfo> - Статистика по странам
        ///<summary>Статистика по странам</summary>
        private IEnumerable<CountryInfo> _Countries;
        ///<summary>Статистика по странам</summary>
        public IEnumerable<CountryInfo> Countries
        {
            get => _Countries;
            private set => Set(ref _Countries, value);
        }
        #endregion



        #region Комманды


        #region RefreshDataCommand - Обновить данные

        ///<summary>Обновить данные</summary>
        public ICommand RefreshDataCommand { get; }

        private bool CanRefreshDataCommandExecute(object p) => true;

        private void OnRefreshDataCommandExecuted(object p)
        {
            Countries = _dataService.GetData();
        }
        #endregion



        #endregion

        public CountriesStatisticsViewModel(MainViewModel mainViewModel)
        {
            MainModel = mainViewModel;
            _dataService = new DataService();

            #region Комманды
            RefreshDataCommand = new LambdaCommand(OnRefreshDataCommandExecuted, CanRefreshDataCommandExecute);
            #endregion
        }
    }
}
