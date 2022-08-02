using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WPF_PROJECT.BaseClasses;

namespace WPFWebSearch.ViewModel
{
    public class ImageCheckBoxViewModel : ViewModelBase
    {
        #region imagesource 
        private ImageSource _imagesource;
        public ImageSource imagesource
        {
            get { return this._imagesource; }
            set
            {
                if (this._imagesource != value)
                {
                    this._imagesource = value;
                    this.OnPropertyChanged("imagesource");
                }
            }
        }
        #endregion
    }
}
