using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using BrightnessRegulator.Models;
using BrightnessRegulator.ViewModels;
using CommunityToolkit.Mvvm.Input;

namespace BrightnessRegulator.Views
{
    public partial class MainView : UserControl
    {
        /// <summary>
        /// Dichiarazione di tutti gli elementi
        /// </summary>
        private TcpClient? tcpClient = new TcpClient();

        private List<int> _values, _values2;

        /// <summary>
        /// s1 --> succesfull
        /// s2 --> error
        /// </summary>
        StackPanel s1;
        StackPanel s2;

        NetworkStream stream;


        public MainView()
        {
            InitializeComponent();

            // Trova gli StackPanel definiti in XAML
            s1 = this.FindControl<StackPanel>("TbSuccesfullConnection");
            s2 = this.FindControl<StackPanel>("TbWrongConnection");

            // Devono restare nascosti fino a connessione avvenuta
            s1.IsVisible = false;
            s2.IsVisible = false;
        }

        /// <summary>
        /// Chiama in modo asincrono il metodo ConnectPc
        /// </summary>
        private async void StartTask(object? sender, RoutedEventArgs e)
        {
            await ConnectPc(s1, s2);
        }

        /// <summary>
        /// Prova a connettersi con il server
        /// </summary>
        private async Task ConnectPc(StackPanel s1, StackPanel s2)
        {
            try
            {
                string serverIp = "192.168.178.20"; // Ip Server
                int port = 45743; // Server Port per questa applicazione


                await tcpClient.ConnectAsync(serverIp, port);

                if (tcpClient.Connected)
                {
                    await ShowStackPanel(s1);

                    // Inizializzazione della lista che conterrà i valori di luminosità e contrasto
                    _values = new List<int>();
                    _values2 = new List<int>();

                    // Riceve dal server i dati iniziali di connessione su luminosità e contrasto
                    stream = tcpClient.GetStream();
                    _values = ReceiveSettings(_values, stream);

                    // Assegnare i valori ricevuti dal server
                    if (DataContext is MainViewModel viewModel && viewModel.AssignmentCommand.CanExecute(null))
                    {
                        // Esecuzione del comando di assegnamento
                        viewModel.AssignmentCommand.Execute(Tuple.Create(_values.First(), _values.Last()));
                    }

                    //Continua a inviare al server i valori di Luminosità e contrasto, i quali continuano a modificare
                    //dato che sono bindati allo slider presente nella MainView
                }
                else
                {
                    await ShowStackPanel(s2);
                }
            }
            catch (Exception e)
            {
                await ShowStackPanel(s2);
            }
        }

        /// <summary>
        /// Metodo che, attraverso l'uso del dispatcher, garantisce l'accesso alla thread UI
        /// non utilizzando il thread principale.
        /// </summary>
        /// <param name="panel"> StackPanel passato come parametro </param>
        private async Task ShowStackPanel(StackPanel panel)
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                panel.IsVisible = true;
            });

            // Aspetta 3 secondi
            await Task.Delay(3000);

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                panel.IsVisible = false;
            });
        }

        /// <summary>
        /// Metodo per la gestione della dinamicità fornita allo slider.
        /// </summary>
        private void OnSliderChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ???
        /// </summary>
        /// <param name="valuesToReceive"></param>
        /// <param name="clientStream"></param>
        /// <returns></returns>
        public List<int> ReceiveSettings(List<int> valuesToReceive, NetworkStream clientStream)
        {
            // Legge la lunghezza del messaggio
            byte[] lenghtBuffer = new byte[4];
            clientStream.Read(lenghtBuffer.AsSpan(0, 4));
            int dataLenght = BitConverter.ToInt32(lenghtBuffer, 0);

            // Legge i dati
            byte[] data = new byte[dataLenght];
            clientStream.Read(data, 0, dataLenght);
            string jsonString = System.Text.Encoding.UTF8.GetString(data);

            // Deserializzazione della lista di interi
            valuesToReceive = JsonSerializer.Deserialize<List<int>>(jsonString);
            return valuesToReceive;
        }

        public async Task SendSettings(List<int> valuesToSend, NetworkStream clientStream)
        {
            // Serializzazione in Json
            string json = JsonSerializer.Serialize(valuesToSend);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(json);

            // Invio della lunghezza del messaggio e dei dati
            await clientStream.WriteAsync(BitConverter.GetBytes(data.Length), 0, 4);
            await clientStream.WriteAsync(data, 0, data.Length);

            await Task.Delay(100);
        }

        private async void BrightnessSlider_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                _values2.Clear();
                _values2.Add(viewModel._Settings.Brightness);
                _values2.Add(viewModel._Settings.Contrast);
                await SendSettings(_values2, stream);
            }
        }

        private async void ContrastSlider_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                _values2.Clear();
                _values2.Add(viewModel._Settings.Brightness);
                _values2.Add(viewModel._Settings.Contrast);
                await SendSettings(_values2, stream);
            }
        }
    }
}

