using System.ComponentModel;

namespace BrightnessRegulator.Models;

public class Settings : INotifyPropertyChanged
{
    /// <summary>
    /// Declaration of attribute in the class
    /// </summary>
    private int brightness;
    private int contrast;

    /// <summary>
    /// Property for the brightness
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
    /// Property for the contrast
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
    /// Empty Constructor for Settings class
    /// </summary>
    public Settings() { }
    
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}