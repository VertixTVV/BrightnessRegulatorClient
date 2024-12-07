Brightness Regulator is a two-part (server and client) application that allows you to adjust the brightness and contrast of a monitor directly from your phone.
The project uses C# and Avalonia UI for the GUI. Hardware settings are managed via Windows API, using DLLs to interact with the monitor's brightness and contrast controls.

Client Description.

The client provides a GUI to allow the user to:
    View current monitor brightness and contrast settings.
    Modify and send the new settings to the server.

Technologies
    Avalonia UI: Cross-platform graphical user interface framework.
    CommunityToolkit.Mvvm: Used for the MVVM architecture.

Key Features.
    User Interface: Simple and intuitive, with sliders or numeric fields to adjust brightness and contrast.
    MVVM: Implementation based on separation between View, ViewModel and Model.
    Server Connection: Communicates with the server via TCP protocol to send and receive settings.

Structure
    App: Configures the application and defines the interface life cycle.
    MainViewModel: Manages the interface logic, including updating the settings.
    Settings: Model for brightness and contrast settings.
    MainWindow: Main visual component.

Dependencies
    Avalonia UI
    CommunityToolkit.Mvvm
