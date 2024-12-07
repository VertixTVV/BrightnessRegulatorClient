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
    
    [ObservableProperty] private string _name = "Brightness Regulator";
    Settings settings;

    /// <summary>
    /// Property to access the instance of the Settings class
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

    // Declaration of assignment command
    public ICommand AssignmentCommand { get; }
    
    public MainViewModel()
    {
        _Settings = new Settings();
        AssignmentCommand = new RelayCommand<Tuple<int, int>>(AssignmentSettings);
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Method to assign values passed by the client to my settings class
    /// </summary>
    /// <param name="values"> List of values received by the server </param>
    public void AssignmentSettings(Tuple<int, int> values)
    {
        _Settings.Brightness = values.Item1;
        _Settings.Contrast = values.Item2;
    }


}

