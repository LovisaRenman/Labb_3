using Labb_3.Model;
using System.Collections.ObjectModel;
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
                //ConfigurationViewModel.RaisePropertyChanged("ActivePack");
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
