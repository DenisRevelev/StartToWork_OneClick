using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using StartToWork_OneClick.Context;
using StartToWork_OneClick.Models;
using StartToWork_OneClick.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StartToWork_OneClick.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public SettingsViewModel()
        {
            GetData();
        }

        private ObservableCollection<SettingsModel> _applicationsView = new ObservableCollection<SettingsModel>();
        public ObservableCollection<SettingsModel> ApplicationsView
        {
            get => _applicationsView;
            set
            {
                if (value != null)
                {
                    _applicationsView = value;
                    OnPropertyChanged();
                }
            }
        }

        private void GetData()
        {
            using (DbContextApplication dbContext = new DbContextApplication())
            {
                IEnumerable<SettingsModel> getData = dbContext.settingsModels.AsNoTracking().ToList();
                ApplicationsView = new ObservableCollection<SettingsModel>(getData);
            }
        }

        #region // Добавить данные: 
        // Методы
        private async Task AddApp()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Select App (*.exe)|*.exe|Text File (*.txt)|*.txt |All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                using (DbContextApplication dbContext = new DbContextApplication())
                {
                    var info = new SettingsModel()
                    {
                        PathApplication = openFileDialog.FileName,
                        Name = Path.GetFileName(openFileDialog.FileName).Replace(".exe", "").Replace(".txt", ""),
                        FixStart = true
                    };
                    dbContext.settingsModels.Add(info);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        // Команды
        private RelayCommand? _addApp { get; set; }
        public RelayCommand AddAppCommand
        {
            get
            {
                return _addApp ?? new RelayCommand(async parameter =>
                {
                    await AddApp();
                    GetData();
                });
            }
        }
        #endregion

        #region // Удалить данные
        // Общие свойства
        private SettingsModel? _selectedAppInView;
        public SettingsModel? SelectedAppInView
        {
            get => _selectedAppInView;
            set
            {
                if (value != null)
                {
                    _selectedAppInView = value;
                    SetOrRemoveTick();
                    OnPropertyChanged();
                    GetData();
                }
            }
        }

        // Методы
        private async Task DeleteApp(SettingsModel deleteApp)
        {
            using (DbContextApplication dbContext = new DbContextApplication())
            {
                if (deleteApp != null)
                {
                    dbContext.settingsModels.Remove(deleteApp);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        // Комманды
        private RelayCommand? _deleteApp { get; set; }
        public RelayCommand? DeleteAppCommand
        {
            get
            {
                return _deleteApp ?? new RelayCommand(async parameter =>
                {
                    var item = (SettingsModel)parameter;
                    SelectedAppInView = item;
                    await DeleteApp(SelectedAppInView!);
                    GetData();
                }, parameter => parameter is SettingsModel);
            }
        }
        #endregion

        #region // Снять, поставить галку
        private void SetOrRemoveTick()
        {
            using (DbContextApplication dbContext = new DbContextApplication())
            {
                var allItems = dbContext.settingsModels.Where(x => x.Id == SelectedAppInView.Id).ToList();
                foreach (var item in allItems)
                {
                    if (item.FixStart)
                        item.FixStart = false;
                    else
                        item.FixStart = true;
                }
                dbContext.SaveChanges();
            }
        }
        #endregion

        #region // Уведомить об изменении
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
