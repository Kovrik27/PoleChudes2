using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PoleChudesWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        HubConnection HubConnection { get; set; }
        private string opponent;
        private bool myTurn;
        string myChar = string.Empty;
        private string nickName;
        private string question;
        public string NickName
        {
            get => nickName;
            set
            {
                nickName = value;
                Signal();
            }
        }

        public string Opponent
        {
            get => opponent;
            set
            {
                opponent = value;
                Signal();
            }
        }
        public bool MyTurn
        {
            get => myTurn;
            set
            {
                myTurn = value;
                Signal();
            }
        }
        public string Question
        {
            get => question;
            set
            {
                question = value;
                Signal();
            }
        }
        public List<WordChar> Word { get => word; set => word = value; }
        private List<WordChar> word;
        public string Answer { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        void Signal([CallerMemberName] string prop = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public MainWindow()
        {
            InitializeComponent();
            CreateConnection();
            HubMethods();
            DataContext = this;
        }

        string gameid = string.Empty;

        private void HubMethods()
        {
            HubConnection.On<string>("hello", s =>
            {
                Dispatcher.Invoke(() =>
                {
                    var win = new WinSetNick(HubConnection, s);
                    win.ShowDialog();
                    NickName = win.Nick;
                });
            });
            HubConnection.On<string, GameWord>("opponent", (s, id) =>
            {
                gameid = id.ID;
                Opponent = s;
                Question = id.Question;
                Word = id.Word;
            });
            HubConnection.On<string>("maketurn", s =>
            {
                myChar = s;
                MyTurn = true;
            });
            HubConnection.On<string>("gameresult", async s =>
            {
            if (s == "win")
            {
                MessageBox.Show("Вы победили, остальные лашки");
            }
            else
            {
                MessageBox.Show($"Победил игрок{NickName}");
            }
            string nextgame = "nein";
                if (MessageBox.Show("Еще раз?", "Играем", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    nextgame = "oda";
                    Dispatcher.Invoke(() =>
                    {
                        foreach (ListBox listbox in bukvi.Items)
                        {
                            bukvi.Items.Clear();
                        }
                    });
                    MyTurn = false;

                    await HubConnection.SendAsync("NextGame", nextgame, NickName);
                    if (nextgame == "nein")
                    {
                        await HubConnection.StopAsync();
                        Dispatcher.Invoke(() =>
                        {
                            Close();
                        });
                    }
                }
            });
        }
        private void CreateConnection()
        {
            var win = new WinOptions();
            if (win.ShowDialog() != true)
            {
                Close();
            }
            string address = win.Address;
            HubConnection = new HubConnectionBuilder().WithUrl(address + "/polechudes").Build();
            HubConnection.StartAsync();
            Unloaded += async (s, e) => await HubConnection.StopAsync();
        }

        private async void MakeTurn(object sender, RoutedEventArgs e)
        {
            var listbox = sender as ListBox;
            if (string.IsNullOrEmpty(Answer) || Answer.Length > 1)
            return;
            string test = Answer.ToUpper();
            await HubConnection.SendAsync("MakeTurn",
                new Turn
                {
                    GameId = gameid,
                    Char = test
                });
            MyTurn = false;
        }
    }
}
