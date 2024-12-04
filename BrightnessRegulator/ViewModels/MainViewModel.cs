using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using BrightnessRegulator.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BrightnessRegulator.ViewModels;

public partial class MainViewModel : ViewModelBase, INotifyPropertyChanged
{
    // TextBlock per nome applicazione
    [ObservableProperty] private string _name = "Brightness Regulator";
    Settings settings;

    /// <summary>
    /// Proprietà per accedere all'istanza della classe Settings
    /// </summary>
    public Settings _Settings
    {
        get => settings;
        set
        {
            if (settings != value)
            {
                settings = value;
                OnPropertyChanged(nameof(Settings));
            }
        }
    }

    // Dichiarazione del comando di assegnamento
    public ICommand AssignmentCommand { get; }

    public MainViewModel()
    {
        _Settings = new Settings();
        AssignmentCommand = new RelayCommand<Tuple<int, int>>(AssignmentSettings);
    }

    /// <summary>
    /// Gestione OnProperyChanged per MainView
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Metodo per assegnare i valori passati dal client alla mia classe settings
    /// </summary>
    /// <param name="values"></param>
    public void AssignmentSettings(Tuple<int, int> values)
    {
        _Settings.Brightness = values.Item1;
        _Settings.Contrast = values.Item2;
    }


}

