using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WPF_PROJECT.BaseClasses;
using WPFWebSearch.BaseClasses;
using WPFWebSearch.View;

namespace WPFWebSearch.ViewModel
{
    public delegate void SaveStringEventHandler(List<string> SaveString);
    public class SaveViewModel : ViewModelBase
    {
        public string s;
        public static SaveStringEventHandler SaveStringEvent;
        public ObservableCollection<ImageCheckBox> allImageCheckBox = new ObservableCollection<ImageCheckBox>();
        public SaveViewModel()
        {
            SaveStringEvent += new SaveStringEventHandler(SaveStringBinding);
            Mediator.Register("getimage", SaveString);
            Mediator.Register("getImageSearch", GetImageSearch);
            Mediator.Register("getAll", GetAll);
        }
        public void SaveStringBinding(List<string> SaveString)
        {
            savestring = SaveString;
        }
        public void GetAll(object ob)
        {
            allImageCheckBox = (ObservableCollection<ImageCheckBox>)ob;
        }
        public void GetImageSearch(object ob)
        {
            s = (string)ob;
        }
        public string saveurl;
        public List<string> savestring = new List<string>();
        public int filenum = 0;
        
        #region FileSaveCommand
        private RelayCommand _FileSaveCommand;
        public ICommand FileSaveCommand
        {
            get
            {
                if (_FileSaveCommand == null)
                    _FileSaveCommand = new RelayCommand(
                        param => this.FileSaveExecuted());
                return _FileSaveCommand;
            }
        }
        #region DeleteCommand
        private RelayCommand _DeleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_DeleteCommand == null)
                    _DeleteCommand = new RelayCommand(
                        param => this.DeleteExecuted());
                return _DeleteCommand;
            }
        }
        private void DeleteExecuted()
        {
            var nondeletecheckbox = new ObservableCollection<ImageCheckBox>();
            foreach (var e in allImageCheckBox)
            {
               if(e.mycheck.IsChecked==false)
                {
                    nondeletecheckbox.Add(e);
                }
            }
            Mediator.NotifyColleagues("deleteimage", nondeletecheckbox);
        }
        #endregion
        private void FileSaveExecuted()
        {
            var filedialog = new CommonOpenFileDialog()
            {
                IsFolderPicker = true,
                Title = "Open File"
            };
            if (filedialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                 saveurl = filedialog.FileName;
            }
            Mediator.NotifyColleagues("saveimage", null);
        }
        #endregion
        #region AllSelectCommand
        private RelayCommand _AllSelectCommand;
        public ICommand AllSelectCommand
        {
            get
            {
                if (_AllSelectCommand == null)
                    _AllSelectCommand = new RelayCommand(
                        param => this.AllSelectExecuted());
                return _AllSelectCommand;
            }
        }
        private void AllSelectExecuted()
        {
            foreach (var e in allImageCheckBox)
            {
                e.mycheck.IsChecked = !e.mycheck.IsChecked;
            }
        }
        #endregion
        public void SaveString(object checkedboxes) 
        {
            filenum = 0;
            var checkedboxlist = (IEnumerable<ImageCheckBox>)checkedboxes;
            //checkedboxes = (IEnumerable<ImageCheckBox>)checkedboxes;
            var savestring = new List<string>();
            foreach(var box in checkedboxlist)
            {
                //try
                //{
                if(box.myImage.Source != null)
                {
                    savestring.Add(box.myImage.Source.ToString());
                }
                //}
                //catch(System.NullReferenceException)
                //{
                //    continue;
                //}
            }
            System.Threading.Tasks.Task.Run(() =>
            {
            foreach (var a in savestring)
            {
                WebClient webclient = new WebClient();
                webclient.Headers.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.3; WOW64; Trident/7.0)");
                FileInfo fileinfo = new FileInfo(s);

                if (!fileinfo.Exists)
                {
                    try
                    {

                        System.Threading.Thread.Sleep(100);
                        webclient.DownloadFile(new Uri(a.ToString()), Path.Combine(saveurl, $"{s + filenum}.jpg"));
                        
                    }
                    catch(Exception)
                    {
                        continue;
                    }
                    filenum++;
                }
                webclient.Dispose();
            }

            MessageBox.Show("All saved.");

            });
        }
        }
    }
