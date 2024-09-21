using System.Text.RegularExpressions;
using CV19Core.ViewModels.Base;
using System.Windows;
using System.Windows.Input;
using CV19Core.Infrastructure.Commands;
using CV19Core.Models;

namespace CV19Core.ViewModels
{
    internal class MainViewModel : ViewModel
    {

        #region TestDataPoints : IEnumerable<DataPoint> - Test data
        ///<summary>Test data</summary>
        private IEnumerable<DataPoint> _testDataPoints;
        ///<summary>Test data</summary>
        public IEnumerable<DataPoint> TestDataPoints { get => _testDataPoints; set => Set(ref _testDataPoints, value); }
        #endregion

        #region Title
        private string _title = "Анализ статистики CV19";
        /// <summary>Title wiwndow</summary>
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }
        #endregion

        /// <summary> Статус программы </summary>
        private string _status = "Готов!";

        /// <summary>
        /// Статус программы
        /// </summary>
        public string Status
        {
            get { return _status; }
            set { Set(ref _status, value); }
        }

        #region Commands

        #region CloseApplicationCommand
        public ICommand CloseApplicationCommand { get; }
        private bool CanCloseApplicationCommandExecute(object p) => true;

        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }
        #endregion



        #endregion
        public MainViewModel()
        {
            #region Команды

            CloseApplicationCommand =
                new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);

            #endregion

            var dataPoints = new List<DataPoint>((int)(360 / 0.1));
            for (var x = 0d; x <= 360; x += 0.1)
            {
                const double toRad = Math.PI / 180;
                var y = Math.Sin(x * toRad);

                dataPoints.Add(new DataPoint{XValue = x,YValue = y});
            }


            TestDataPoints = dataPoints;
        }
    }
}
