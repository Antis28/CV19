using CV19Core.ViewModels.Base;
using System.Windows;
using System.Windows.Input;
using CV19Core.Infrastructure.Commands;
using CV19Core.Models;
using OxyPlot;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CV19Core.Models.Decanat;


namespace CV19Core.ViewModels
{
    internal class MainViewModel : ViewModel
    {
        /*----------------------------------------------------------------------------------------------------*/

        public ObservableCollection<Group> Groups { get; }



        #region SelectedGroup : Group - Выбранная группа
        ///<summary>Выбранная группа</summary>
        private Group _SelectedGroup;
        ///<summary>Выбранная группа</summary>
        public Group SelectedGroup { get => _SelectedGroup; set => Set(ref _SelectedGroup, value); }
        #endregion


        #region TestDataPoints : IEnumerable<DataPointCV> - Test data
        ///<summary>Test data</summary>
        private IEnumerable<DataPointCV> _testDataPoints;
        ///<summary>Test data</summary>
        public IEnumerable<DataPointCV> TestDataPoints { get => _testDataPoints; set => Set(ref _testDataPoints, value); }
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

        #region Status : string - Статус программы
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
        #endregion
        

        #region Commands

        #region CloseApplicationCommand
        public ICommand CloseApplicationCommand { get; }
        private bool CanCloseApplicationCommandExecute(object p) => true;

        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }
        #endregion


        #region CreateNewGroupCommand - Создает новую группу

        ///<summary>Создает новую группу</summary>
        public ICommand CreateNewGroupCommand { get; }

        private bool CanCreateNewGroupCommandExecute(object p) => true;

        private void OnCreateNewGroupCommandExecuted(object p)
        {
            var groupMaxIndex = Groups.Count + 1;
            var newGroup = new Group
            {
                Name = $"Группа {groupMaxIndex}",
                Students = new ObservableCollection<Student>()
            };
            Groups.Add(newGroup);
        }
        #endregion


        #region DeleteGroupCommand - Удаляет существующую группу

        ///<summary>Удаляет существующую группу</summary>
        public ICommand DeleteGroupCommand { get; }

        private bool CanDeleteGroupCommandExecute(object p) => p is Group group && Groups.Contains(group);

        private void OnDeleteGroupCommandExecuted(object p)
        {
            if (!(p is Group group)) { return; }

            var groupIndex = Groups.IndexOf(group);
            Groups.Remove(group);
            if (groupIndex < Groups.Count) { SelectedGroup = Groups[groupIndex]; }
        }
        #endregion




        public PlotModel PlotModel { get; set; }
        #endregion

        public DirectoryViewModel DiskRootDir { get; } = new DirectoryViewModel(@"c:\");


        #region SelectedDirectory : DirectoryViewModel - Выбранная директория
        ///<summary>Выбранная директория</summary>
        private DirectoryViewModel _selectedDirectory;

        ///<summary>Выбранная директория</summary>
        public DirectoryViewModel SelectedDirectory
        {
            get => _selectedDirectory; 
            set => Set(ref _selectedDirectory, value);
        }
        #endregion



        #region constructor

        public MainViewModel()
        {
            #region Команды

            CloseApplicationCommand =
                new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);


            CreateNewGroupCommand =
                new LambdaCommand(OnCreateNewGroupCommandExecuted, CanCreateNewGroupCommandExecute);

            DeleteGroupCommand =
                new LambdaCommand(OnDeleteGroupCommandExecuted, CanDeleteGroupCommandExecute);
            #endregion

            #region PlotModel
            PlotModel = new PlotModel { Title = "My First Plot" };
            var series = new LineSeries();
            //series.Points.Add(new DataPoint(0, 0));
            //series.Points.Add(new DataPoint(1, 1));


            /*--------------------------------------------------------------------*/
            var dataPoints = new List<DataPointCV>((int)(360 / 0.1));
            for (var x = 0d; x <= 360; x += 0.1)
            {
                const double toRad = Math.PI / 180;
                var y = Math.Sin(x * toRad);

                dataPoints.Add(new DataPointCV { XValue = x, YValue = y });
                series.Points.Add(new DataPoint(x, y));
            }
            TestDataPoints = dataPoints;
            PlotModel.Series.Add(series);
            /*--------------------------------------------------------------------*/
            #endregion

            #region studet test (Group)

            var studentIndex = 1;
            var students = Enumerable.Range(1, 10).Select(i => new Student
            {
                Name = $"Name - {studentIndex}",
                Surname = $"Surname- {studentIndex}",
                Patronymic = $"Patronymic- {studentIndex++}",
                Birthday = DateTime.Now,
                Rating = 0
            });
            var groups = Enumerable.Range(1, 20).Select(i => new Group
            {
                Name = $"Группа - {i}",
                Students = new ObservableCollection<Student>(students)
            });

            Groups = new ObservableCollection<Group>(groups);

            #endregion
        }
        #endregion


    }
}
