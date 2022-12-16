using StartToWork_OneClick.Context;
using StartToWork_OneClick.Models;
using StartToWork_OneClick.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartToWork_OneClick.ViewModels
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            
        }
        private ObservableCollection<SettingsModel> _allData = new ObservableCollection<SettingsModel>();
        public ObservableCollection<SettingsModel> AllData
        {
            get => _allData;
            set
            {
                if (value != null)
                {
                    _allData = value;
                }
            }
        }

        private void GetData()
        {
            using (DbContextApplication dbContext = new DbContextApplication())
            {
                var getApp = dbContext.settingsModels.ToList();
                AllData = new ObservableCollection<SettingsModel>(getApp);
            }
        }

        private async Task StartApplication()
        {
            GetData();
            foreach (var item in AllData)
            {
                if (item.PathApplication.Contains(".txt"))
                {
                    Process txt = new Process();
                    txt.StartInfo.FileName = "notepad.exe";
                    txt.StartInfo.Arguments = item.PathApplication;
                    txt.Start();
                }
                else
                    await Task.Run(() => Process.Start(item.PathApplication!));
            }
        }

        private RelayCommand? _startApp { get; set; }
        public RelayCommand? StartAppCommand
        {
            get
            {
                return _startApp ?? new RelayCommand(async parameter =>
                {
                    await StartApplication();
                });
            }
        }
    }
}
