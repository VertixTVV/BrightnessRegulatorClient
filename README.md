Brightness Regulator è un'applicazione composta da due parti (server e client) che consente di regolare luminosità e contrasto di un monitor direttamente dal telefono.
Il progetto utilizza C# e Avalonia UI per l'interfaccia grafica. La gestione delle impostazioni hardware avviene tramite API di Windows, utilizzando DLL per interagire con i controlli di luminosità e contrasto del monitor.

Descrizione Client.

Il client fornisce un'interfaccia grafica per permettere all'utente di:
    Visualizzare le impostazioni attuali di luminosità e contrasto del monitor.
    Modificare e inviare le nuove impostazioni al server.

Tecnologie
    Avalonia UI: Framework per l'interfaccia grafica multipiattaforma.
    CommunityToolkit.Mvvm: Utilizzato per l'architettura MVVM.

Funzionalità principali
    Interfaccia utente: Semplice e intuitiva, con slider o campi numerici per regolare luminosità e contrasto.
    MVVM: Implementazione basata su separazione tra View, ViewModel e Model.
    Connessione al server: Comunica con il server tramite protocollo TCP per inviare e ricevere le impostazioni.

Struttura
    App: Configura l'applicazione e definisce il ciclo di vita dell'interfaccia.
    MainViewModel: Gestisce la logica dell'interfaccia, incluso l'aggiornamento delle impostazioni.
    Settings: Modello per le impostazioni di luminosità e contrasto.
    MainWindow: Componente visivo principale.

Dipendenze
    Avalonia UI
    CommunityToolkit.Mvvm
