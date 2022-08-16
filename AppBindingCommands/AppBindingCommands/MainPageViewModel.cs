using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace AppBindingCommands
{
    public class MainPageViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        
        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ICommand ShowMessageCommand;

        private string name = string.Empty;

        public string Name
        {
            get => name;
            set
            {
                if (name == null)
                    return;

                name = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        public string DisplayName => $"Nome Digitado : {Name}";

        string displayMessage = string.Empty;
                
        public string DisplayMessage
        {
            get => displayMessage;
            set
            {
                if (displayMessage == null)
                    return;

                displayMessage = value;
                OnPropertyChanged(nameof(DisplayMessage));
            }
        }

        public void ShowMessage()
        {
            string dataProperty = Application.Current.Properties["dtAtual"].ToString();

            DisplayMessage = $"Boa Noite {Name}. Hoje é {dataProperty}.";
        }

        public MainPageViewModel()
        {
            ShowMessageCommand = new Command(ShowMessage);
            CountCommand = new Command(async() => await CountCharacters());
            CleanCommand = new Command(async () => await CleanConfirmation());
            OptionCommand = new Command(async () => await ShowOptions());
        }

        public async Task CountCharacters()
        {
            string nameLenght =
                string.Format("Seu nome tem {0} Letras", Name.Length);

            await Application.Current.MainPage
                .DisplayAlert("Informação", nameLenght, "Ok");
        }

        public ICommand CountCommand { get; }

       public async Task CleanConfirmation()
        {
            if(await Application.Current.MainPage
                .DisplayAlert("Confirmação", "Confirma limpeza dos dados?", "Yes", "No"))
            {
                Name = string.Empty;
                DisplayMessage = string.Empty;
                OnPropertyChanged(Name);
                OnPropertyChanged(DisplayMessage);

                await Application.Current.MainPage
                    .DisplayAlert("Informação", "Limpeza realizada com sucesso", "Ok");
            }
        }

        public ICommand CleanCommand { get; }

        public async Task ShowOptions()
        {
            string result;

            result = await Application.Current.MainPage
                .DisplayActionSheet("Seleção", "Selecione uma opção",
                "Cancelar", "Limpar", "Contar Caracteres", "Exibir Saudação");
            if(result != null)
            {
                if(result.Equals("Limpar"))
                    await CleanConfirmation();

                if (result.Equals("Contar Caracteres"))
                    await CountCharacters();

                if (result.Equals("Exibir Saudação"))
                    ShowMessage();
            }
        }

        public ICommand OptionCommand { get; }
    }  
}
