using CV19Core.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV19Core.ViewModels
{
    internal class MainViewModel : ViewModel
    {
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

    }
}
