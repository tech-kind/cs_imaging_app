using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsImagingApp.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region IsChecked
        private bool isChecked;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }

        #endregion

        #region UI更新
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region IsLoad
        private bool isLoad;

        /// <summary>
        /// 是否加载
        /// </summary>
        public bool IsLoad
        {
            get { return isLoad; }
            set
            {
                isLoad = value;
                OnPropertyChanged(nameof(IsLoad));
            }
        }
        #endregion

        #region Update
        private bool update;
        /// <summary>
        /// 刷新
        /// </summary>
        public bool Update
        {
            get { return update; }
            set
            {
                update = value;
                OnPropertyChanged(nameof(Update));
            }
        }
        #endregion
    }
}
