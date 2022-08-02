using EO.WebBrowser.DOM;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Windows.Input;
using System.Windows.Threading;
using WPF_PROJECT.BaseClasses;
using WPFWebSearch.BaseClasses;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

namespace WPFWebSearch.ViewModel
{
    public delegate void ProgressChangedEventHandler(int target);
    public class SearchViewModel : ViewModelBase
    {
        public SearchViewModel()
        {
            ProgressChangedEvent += new ProgressChangedEventHandler(ProgressBinding);
        }
        public int progress=0;
        public static ProgressChangedEventHandler ProgressChangedEvent;
        public void ProgressBinding(int target)
        {
            ProgressingImage = target;
        }
           #region ProgressingImage 
        private int _ProgressingImage;
        public int ProgressingImage
        {
            get { return this._ProgressingImage; }
            set
            {
                if (this._ProgressingImage != value)
                {
                    this._ProgressingImage = value;
                    this.OnPropertyChanged("ProgressingImage");
                }
            }
        }
        #region noticenum 
        private string _noticenum;
        public string noticenum
        {
            get { return this._noticenum; }
            set
            {
                if (this._noticenum != value)
                {
                    this._noticenum = value;
                    this.OnPropertyChanged("noticenum");
                }
            }
        }
        #endregion
        #region ImageSearch 
        private string _ImageSearch;
        public string ImageSearch
        {
            get { return this._ImageSearch; }
            set
            {
                if (this._ImageSearch != value)
                {
                    this._ImageSearch = value;
                    this.OnPropertyChanged("ImageSearch");
                }
            }
        }
        #endregion
        #region SearchCommand
        private RelayCommand _SearchCommand;
        public ICommand SearchCommand

        {
            get
            {
                if (_SearchCommand == null)
                    _SearchCommand = new RelayCommand(
                        param => this.SearchExecuted());
                return _SearchCommand;
            }
        }

        #endregion
     
        /// 검색어 입력 후 버튼 클릭 시 실행
        public void SearchExecuted()
        {
            Mediator.NotifyColleagues("getImageSearch", ImageSearch);
            var Imageurl = new List<string>();
            string url = $"https://www.google.com/search?q={ImageSearch}&tbm=isch&sxsrf=ALiCzsZpFp2j3yXkvtdqubgAZOxLbCCCFA%3A1658298759717&source=hp&biw=2560&bih=1297&ei=h6HXYsGDKcGB1e8P8rG0wAw&iflsig=AJiK0e8AAAAAYtevl4ZsUvuRaiHw2NFKYBYy3_UG1zMJ&ved=0ahUKEwiB44Gk7Ib5AhXBQPUHHfIYDcgQ4dUDCAc&uact=5&oq=y&gs_lcp=CgNpbWcQAzIFCAAQgAQyCAgAEIAEELEDMggIABCABBCxAzIICAAQgAQQsQMyCwgAEIAEELEDEIMBMggIABCABBCxAzIICAAQgAQQsQMyCAgAEIAEELEDMggIABCABBCxAzIECAAQAzoHCCMQ6gIQJ1CgAligAmDWB2gBcAB4AIABeIgBeJIBAzAuMZgBAKABAaoBC2d3cy13aXotaW1nsAEK&sclient=img";
            var path = @"C:\Users\USER\source\repos\WPFWebSearch\packages\Selenium.WebDriver.ChromeDriver.103.0.5060.5300";
            ChromeOptions options = new ChromeOptions();
            //options.AddArgument("headless");
            //options.AddAdditionalChromeOption("excludeSwitches","enable-logging");
            
                System.Threading.Tasks.Task.Run(() =>
                {
                    using (IWebDriver driver = new ChromeDriver(Path.GetFullPath(path),options))
                    {
                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        driver.Manage().Window.Maximize();
                        var YMaxPosition = driver.Manage().Window.Size.Height;
                        driver.Navigate().GoToUrl(url); 
                        var action = new Actions(driver);
                        while (true)
                        {
                            try
                            {
                                System.Threading.Thread.Sleep(100);
                                if (driver.FindElement(By.CssSelector(".OuJzKb.Yu2Dnd")).Displayed == true)
                                {
                                    break;
                                }
                                if (driver.FindElement(By.CssSelector(".mye4qd")).Displayed == true)
                                {
                                    driver.FindElement(By.CssSelector(".mye4qd")).Click();
                                }
                                if (driver.FindElement(By.CssSelector(".r0zKGf")).Displayed == true)
                                {
                                    driver.FindElement(By.CssSelector(".r0zKGf")).Click();
                                }
                                //driver.FindElement(By.CssSelector(".mye4qd")).Click();
                                System.Threading.Thread.Sleep(100);
                            }
                            catch (OpenQA.Selenium.ElementNotInteractableException)
                            {
                                YMaxPosition += driver.Manage().Window.Size.Height;
                                js.ExecuteScript($"window.scrollTo(0,{YMaxPosition})");
                            }
                            catch (OpenQA.Selenium.NoSuchElementException)
                            {
                                YMaxPosition += driver.Manage().Window.Size.Height;
                                js.ExecuteScript($"window.scrollTo(0,{YMaxPosition})");
                            }
                        }
                        var element = driver.FindElements(By.CssSelector(".rg_i.Q4LuWd"));
                        var count = element.Count;
                        int progressnum = 0;
                        System.Threading.Thread.Sleep(1000);
                        foreach (var e in element)
                        {
                            //noticenum = element.IndexOf(e)+1 + "/" + element.Count;
                            try
                            {
                                e.Click();
                                System.Threading.Thread.Sleep(500);
                                progressnum = element.IndexOf(e);
                                progress = (int)(((double)progressnum / (double)count) * 100);
                                var bigelement = driver.FindElement(By.CssSelector(".n3VNCb.KAlRDb"));
                                var s = bigelement.GetAttribute("src");
                                //if (s.StartsWith("http"))
                                {
                                    Imageurl.Add(s);
                                }

                                System.Threading.Tasks.Task.Run(() =>
                                {
                                    ProgressChangedEvent.Invoke(progress);

                                });

                            }
                            catch (OpenQA.Selenium.NoSuchElementException)
                            {
                                continue;
                            }
                            catch (OpenQA.Selenium.ElementNotInteractableException)
                            {
                                continue;
                            }
                            catch (OpenQA.Selenium.StaleElementReferenceException)
                            {
                                System.Threading.Thread.Sleep(100);
                                continue;
                            }

                        }
                        System.Threading.Thread.Sleep(100);
                        progress = 100;
                        System.Threading.Tasks.Task.Run(() =>
                        {
                            ProgressChangedEvent.Invoke(progress);
                        });
                        App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(delegate
                        {
                            ImageViewModel.SearchEvent.Invoke(Imageurl, element.Count);
                        }));
                        driver.Quit();
                    }

                });

            
           
        }



    }

        #endregion
    }
