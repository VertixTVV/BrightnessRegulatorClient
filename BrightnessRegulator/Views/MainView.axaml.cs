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
        /// Declaration of all elements
        /// </summary>
        private TcpClient? tcpClient = new TcpClient();
        private List<int> _values, _values2;
        StackPanel s1; //---> SUCCESFULL
        StackPanel s2; //---> ERROR
        NetworkStream stream;
        
        /// <summary>
        /// Find the StackPanel declared in XAML and hides them until an attempt of connection
        /// </summary>
        public MainView()
        {
            InitializeComponent();
            
            s1 = this.FindControl<StackPanel>("TbSuccesfullConnection");
            s2 = this.FindControl<StackPanel>("TbWrongConnection");
            s1.IsVisible = false;
            s2.IsVisible = false;
        }

        /// <summary>
        /// Call the ConnectPc method asynchronously
        /// </summary>
        private async void StartTask(object? sender, RoutedEventArgs e)
        {
            await ConnectPc(s1, s2);
        }

        /// <summary>
        /// Try to connect to the server
        /// </summary>
        private async Task ConnectPc(StackPanel s1, StackPanel s2)
        {
            try
            {
                string serverIp = "192.168.178.20"; // Ip Server
                int port = 45743; // Server Port for this application

                await tcpClient.ConnectAsync(serverIp, port);

                if (tcpClient.Connected)
                {
                    await ShowStackPanel(s1);

                    // Initialization of the lists that will contain the brightness and contrast values
                    _values = new List<int>();
                    _values2 = new List<int>();

                    // Receives initial connection data on brightness and contrast from the server
                    stream = tcpClient.GetStream();
                    _values = ReceiveSettings(_values, stream);

                    // Assigns the value received from the server
                    if (DataContext is MainViewModel viewModel && viewModel.AssignmentCommand.CanExecute(null))
                    {
                        // Execution of assignment command
                        viewModel.AssignmentCommand.Execute(Tuple.Create(_values.First(), _values.Last()));
                    }
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
        /// Method which, through the use of the dispatcher, guarantees access to the UI thread
        /// not using the main thread.
        /// </summary>
        /// <param name="panel"> StackPanel passed like parameter </param>
        private async Task ShowStackPanel(StackPanel panel)
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                panel.IsVisible = true;
            });

            // Wait 1 Second
            await Task.Delay(1000);

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                panel.IsVisible = false;
            });
        }

        /// <summary>
        /// Method for managing the dynamism provided to the slider.
        /// </summary>
        private void OnSliderChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method for receiving data sent by the server regarding initial brightness and contrast
        /// </summary>
        /// <param name="valuesToReceive"> List of values to apply on Settings </param>
        /// <param name="clientStream"> NetworkStream for hold the connection beetween client and server</param>
        public List<int> ReceiveSettings(List<int> valuesToReceive, NetworkStream clientStream)
        {
            // Read the lenght of the message
            byte[] lenghtBuffer = new byte[4];
            clientStream.Read(lenghtBuffer.AsSpan(0, 4));
            int dataLenght = BitConverter.ToInt32(lenghtBuffer, 0);

            // Read the data
            byte[] data = new byte[dataLenght];
            clientStream.Read(data, 0, dataLenght);
            string jsonString = System.Text.Encoding.UTF8.GetString(data);

            // Deserialize the list of integer
            valuesToReceive = JsonSerializer.Deserialize<List<int>>(jsonString);
            return valuesToReceive;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesToSend"> List of values to send at the server </param>
        /// <param name="clientStream"> NetworkStream for hold the connection beetween client and server </param>
        public async Task SendSettings(List<int> valuesToSend, NetworkStream clientStream)
        {
            // Serialize in Json
            string json = JsonSerializer.Serialize(valuesToSend);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(json);

            // Sending message length and data
            await clientStream.WriteAsync(BitConverter.GetBytes(data.Length), 0, 4);
            await clientStream.WriteAsync(data, 0, data.Length);

            await Task.Delay(100);
        }

        /// <summary>
        /// Thanks to OnValueChanged Property, BrightnessSlider update himself and send new settings to server
        /// </summary>
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

        /// <summary>
        /// Thanks to OnValueChanged Property, ContrastSlider update himself and send new settings to server
        /// </summary>
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

