using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFWebSearch.ViewModel;

namespace WPFWebSearch.View
{

    public partial class ImageCheckBox : UserControl
    {
        public bool IsChecked { get; set; }

      
        public ImageCheckBox(ImageSource source)
        {
            //imagesource = source;
            InitializeComponent();
            myImage.Source = source;
        }
        private void mycheck_Checked(object sender, RoutedEventArgs e)
        {
            IsChecked = true;
           
        }

        private void mycheck_UnChecked(object sender, RoutedEventArgs e)
        {
            IsChecked = false;
        }


    }
}





