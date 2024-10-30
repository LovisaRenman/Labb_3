using Labb_3.Model;
using Labb_3.View;
using System.Collections.ObjectModel;
using System.Windows;
namespace Labb_3.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }

        public PlayerViewModel PlayerViewModel { get; }

        public ConfigurationViewModel ConfigurationViewModel { get; }

        private QuestionPackViewModel? _activePack;
        public QuestionPackViewModel? ActivePack
        {
            get => _activePack; 
            set 
            { 
                _activePack = value;
                RaisePropertyChanged("ActivePack");
            }
        }

        public MainWindowViewModel()
        {
            ActivePack = new QuestionPackViewModel(new QuestionPack("Default Question Pack"));
            ConfigurationViewModel = new ConfigurationViewModel(this);

            PlayerViewModel = new PlayerViewModel(this);
        }        
    }
}
