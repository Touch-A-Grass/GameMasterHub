using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using GameMasterHub.Infrastructure.Repositories;
using GameMasterHub.Screens.CreateTemplateCharacter;
using GameMasterHub.ViewModels;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;
using ZXing.CoreCompat.System.Drawing;

namespace GameMasterHub.Screens.Lobby
{
    public class LobbyViewModel : ViewModelBase
    {
        private readonly LobbyRepository _lobbyRepository;

        private Bitmap? _qrCodeImage = null;

        public Bitmap? QRCodeImage
        {
            get => _qrCodeImage;
            set => this.RaiseAndSetIfChanged(ref _qrCodeImage, value);
        }
        
        private string _messageText;
        public string MessageText
        {
            get => _messageText;
            set => this.RaiseAndSetIfChanged(ref _messageText, value);
        }

        private ObservableCollection<DiceTemplate> _dices = new ObservableCollection<DiceTemplate>();
        public ObservableCollection<DiceTemplate> Dices
        {
            get => _dices;
            set => this.RaiseAndSetIfChanged(ref _dices, value);
        }

        private bool _isExpanded = false;

        public bool IsExpanded
        {
            get => _isExpanded;
            set => this.RaiseAndSetIfChanged(ref _isExpanded, value);
        }

        public UserTemplate? _currentUser = null;
        public UserTemplate? CurrentUser
        {
            get => _currentUser;
            set => this.RaiseAndSetIfChanged(ref _currentUser, value);
        }

        private ObservableCollection<UserTemplate> _users = new ObservableCollection<UserTemplate>();
        public ObservableCollection<UserTemplate> Users
        {
            get => _users;
            set => this.RaiseAndSetIfChanged(ref _users, value);
        }

        public ReactiveCommand<Unit, Unit> ExpandQrCodeCommand { get; }

        public ReactiveCommand<Unit, Unit> AddDiceCommand { get; }
        public ReactiveCommand<DiceTemplate, Unit> DeleteDiceCommand { get; }
        public ReactiveCommand<UserTemplate, Unit> ChooseUserCommand { get; }
        
        public ReactiveCommand<Unit, Unit> SendMessageCommand { get; }

        public ReactiveCommand<Unit, Unit> RollDicesCommand { get; }

        private string _rollResult = "Waiting for roll";
        public string RollResult
        {
            get => _rollResult;
            set => this.RaiseAndSetIfChanged(ref _rollResult, value);
        }

        public LobbyViewModel(LobbyRepository lobbyRepository)
        {
            _lobbyRepository = lobbyRepository;

            CallGenerateQrCode(_lobbyRepository.GetLobbyToken());

            _lobbyRepository.WatchUsers()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(users =>
                    {
                        Users.Clear();
                        foreach (var user in users)
                        {
                            Users.Add(new UserTemplate
                            {
                                Name = user.Username
                            });
                        }
                    });
            _lobbyRepository.GenerateUsers();
            ExpandQrCodeCommand = ReactiveCommand.Create(ExpandQrCode);
            AddDiceCommand = ReactiveCommand.Create(AddDice);
            DeleteDiceCommand = ReactiveCommand.Create<DiceTemplate>(DeleteDice);

            ChooseUserCommand = ReactiveCommand.Create<UserTemplate>(ChooseUser);

            RollDicesCommand = ReactiveCommand.Create(RollDices);
            SendMessageCommand = ReactiveCommand.Create(SendMessage);

            AddDice();
        }

        private async void RollDices()
        {
            StringBuilder resultBuilder = new StringBuilder();

            /*foreach (var dice in Dices)
            {
                // TODO : Add check for int values in dices
                int randomValue = new Random().Next(1, Convert.ToInt32(dice.Value) + 1);
                resultBuilder.AppendLine($"Dice with value {dice.Value}: Rolled {randomValue}\n");
            }*/
            await Task.Delay(12000);
            resultBuilder.AppendLine($"Dice with value 6: Rolled 5\n");
            resultBuilder.AppendLine($"Dice with value 20: Rolled 3\n");
            resultBuilder.AppendLine($"Dice with value 20: Rolled 12\n");

            RollResult = resultBuilder.ToString();

            CurrentUser = null;

            foreach (var u in Users)
            {
                u.IsEnabled = false;
            }
        }

        private void ChooseUser(UserTemplate user)
        {
            // TODO : Add user id
            foreach (var u in Users)
            {
                u.IsEnabled = false;
            }

            user.IsEnabled = true;

            CurrentUser = user;

        }

        private void AddDice()
        {
            Dices.Add(new DiceTemplate());
        }

        private void DeleteDice(DiceTemplate dice)
        {
            Dices.Remove(dice);
        }

        private void ExpandQrCode()
        {
            IsExpanded = !IsExpanded;
        }

        private async void CallGenerateQrCode(string data)
        {
            QRCodeImage = await GenerateQRCode(data);
        }

        public static async Task<Bitmap?> GenerateQRCode(string data)
        {
            return await Task.Run(() =>
                {
                    var writer = new BarcodeWriter
                    {
                        Format = BarcodeFormat.QR_CODE,
                        Options = new EncodingOptions
                        {
                            Width = 300,
                            Height = 300
                        }
                    };
                    var imageForAvalonia = writer.Write(data);
                    using (MemoryStream memory = new MemoryStream())
                    {
                        imageForAvalonia.Save(memory, ImageFormat.Png);
                        memory.Position = 0;

                        return new Bitmap(memory);
                    }
                });
        }

        private ObservableCollection<Message> _messages = new ObservableCollection<Message>();
        public ObservableCollection<Message> Messages
        {
            get => _messages;
            set => this.RaiseAndSetIfChanged(ref _messages, value);
        }

        async public void SendMessage()
        {
            
            AddMessage(new Message
            {
                Text = MessageText,
                IsMine = true
            });
            MessageText = "";
            await Task.Delay(4000);
            ReceiveMessage("krushiler: hi");
        }

        public void ReceiveMessage(string text)
        {
            AddMessage(new Message
            {
                Text = text,
                IsMine = false
            });
        }

        private void AddMessage(Message message)
        {
            Messages.Add(message);
        }
    }

    public class UserTemplate : ReactiveObject
    {
        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
            set => this.RaiseAndSetIfChanged(ref _isEnabled, value);
        }

        private string _name = "";
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }
    }

    public class DiceTemplate : ReactiveObject
    {
        private Guid _id = Guid.NewGuid();

        private string _value = "6";
        public string Value
        {
            get => _value;
            set => this.RaiseAndSetIfChanged(ref _value, value);
        }
    }

    public class Message
    {
        public string Text { get; set; }
        public bool IsMine { get; set; }
    }

}
