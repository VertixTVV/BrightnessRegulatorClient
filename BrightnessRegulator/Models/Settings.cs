using System.ComponentModel;

namespace BrightnessRegulator.Models;

public class Settings : INotifyPropertyChanged
{
    /// <summary>
    /// Dichiarazione attributi della classe
    /// </summary>
    private int brightness;
    private int contrast;

    /// <summary>
    /// Proprietà per la luminosità
    /// </summary>
    public int Brightness
    {
        get => brightness;
        set
        {
            if (brightness != value)
            {
                brightness = value;
                OnPropertyChanged(nameof(Brightness));
            }
        }
    }

    /// <summary>
    /// Proprietà per il contrasto
    /// </summary>
    public int Contrast
    {
        get => contrast;
        set
        {
            if (contrast != value)
            {
                contrast = value;
                OnPropertyChanged(nameof(Contrast));
            }
        }
    }

    /// <summary>
    /// Costruttore della classe Settings, ha il compito di prelevare i valori di luminosità
    /// e contrasto dal server per applicarli agli attributi della classe
    /// </summary>
    public Settings() { }

    /// <summary>
    /// Gestione OnProperyChanged per Settings
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}