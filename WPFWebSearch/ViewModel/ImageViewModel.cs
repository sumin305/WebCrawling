using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using WPF_PROJECT.BaseClasses;
using WPFWebSearch.BaseClasses;
using WPFWebSearch.View;

namespace WPFWebSearch.ViewModel
{
    public delegate void SearchEventHandler(List<string> ImageUrl, int num);
    public class ImageViewModel : ViewModelBase
    {
        List<ImageSource> SaveImageSource = new List<ImageSource>();//저장할 이미지 소스들
        #region boxlist 
        private ObservableCollection<ImageCheckBox> _boxlist = new ObservableCollection<ImageCheckBox>();
        public ObservableCollection<ImageCheckBox> boxlist
        {
            get { return this._boxlist; }
            set
            {
                if (this._boxlist != value)
                {
                    this._boxlist = value;
                    this.OnPropertyChanged("boxlist");
                }
            }
        }
        #endregion
       
        public static SearchEventHandler SearchEvent;
        public ImageViewModel()
        {
            Mediator.Register("saveimage", SetImage);
            Mediator.Register("deleteimage", SetDeleteImage);
            SearchEvent += new SearchEventHandler(ImageBinding);
        }
       
        public void ImageBinding(List<string> ImageUrl, int num)                  
        {
            Num = num/9;
            ImageSource source;
            if (boxlist.Any())
            {
                boxlist.Clear();
            }
            foreach (string s in ImageUrl)
            {
                //try
                {
                    source = (ImageSource)new ImageSourceConverter().ConvertFromString(s);
                }
                //catch(System.NotSupportedException)
                {
                    //source = null;
                }
                if (source == null)
                {
                    
                }
                    boxlist.Add(new ImageCheckBox(source)
                                    {
                                        VerticalContentAlignment = System.Windows.VerticalAlignment.Center,
                                        HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
                                        VerticalAlignment = System.Windows.VerticalAlignment.Center,
                                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                                    });
                
            }
            Mediator.NotifyColleagues("getAll", boxlist);
        }
        public void SetImage(object obj)
        {
            var checkedlist = boxlist.Where(i => i.IsChecked == true);
            if (checkedlist.Any())
            {
                Mediator.NotifyColleagues("getimage", checkedlist);
            }
        }
        public void SetDeleteImage(object obj)
        {
            boxlist=(ObservableCollection<ImageCheckBox>)obj;
        }
        #region Num 
        private int _Num;
        public int Num
        {
            get { return this._Num; }
            set
            {
                if (this._Num != value)
                {
                    this._Num = value;
                    this.OnPropertyChanged("Num");
                }
            }
        }
        #endregion
        public IEnumerable<ImageCheckBox> GetList()
        {
            var checkedlist = boxlist.Where(i => i.IsChecked == true);
            return checkedlist;
        }
    }
}


