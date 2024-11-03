using Labb_3.Command;
using Labb_3.Dialogs;
using Labb_3.Model;
using System.Collections.ObjectModel;
using System.Windows;
namespace Labb_3.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }

        public CreateNewPackDialog CreatePackDialog { get; set; }
        public PlayerViewModel PlayerViewModel { get; }
        public DelegateCommand NewQuestionPackCommand { get; }
        public DelegateCommand NewQuestionPackCreateCommand { get; }
        public DelegateCommand NewQuestionPackCancelCommand { get; }
        public DelegateCommand SetActivePackCommand { get; }
        public DelegateCommand RemoveQuestionPackCommand { get; }
        public DelegateCommand FullscreenCommand { get; }
        public ConfigurationViewModel ConfigurationViewModel { get; }

        private QuestionPackViewModel? _activePack;
        public QuestionPackViewModel? ActivePack
        {
            get => _activePack; 
            set 
            { 
                _activePack = value;
                RaisePropertyChanged("ActivePack");
                ConfigurationViewModel?.RaisePropertyChanged();
                PlayerViewModel?.RaisePropertyChanged();
            }
        }

        private QuestionPackViewModel? _newQuestionPack;
        public QuestionPackViewModel? NewQuestionPack
        {
            get => _newQuestionPack;
            set 
            {
                _newQuestionPack = value;
                RaisePropertyChanged();
            }
        }

        private WindowState _windowState;
        public WindowState IsWindowNormal
        {
            get => _windowState; 
            set 
            {
                _windowState = value;
                RaisePropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            ActivePack = new QuestionPackViewModel(new QuestionPack("Default Question Pack"));
            Packs = new ObservableCollection<QuestionPackViewModel>();
            Packs.Add(ActivePack);

            ConfigurationViewModel = new ConfigurationViewModel(this);
            PlayerViewModel = new PlayerViewModel(this);

            NewQuestionPackCommand = new DelegateCommand(CreateNewQuestionPack);
            NewQuestionPackCreateCommand = new DelegateCommand(NewQuestionPackCreate);
            NewQuestionPackCancelCommand = new DelegateCommand(NewQuestionPackCancel);
            SetActivePackCommand = new DelegateCommand(SelectActivePackCommand);
            RemoveQuestionPackCommand = new DelegateCommand(RemoveQuestionPack, RemoveQuestionPackActive);
            FullscreenCommand = new DelegateCommand(Fullscreen);

            IsWindowNormal = WindowState.Normal;
        }

        private void Fullscreen(object obj)
        {
            if (IsWindowNormal == WindowState.Normal) IsWindowNormal = WindowState.Maximized;
            else if (IsWindowNormal == WindowState.Maximized) IsWindowNormal = WindowState.Normal;
        }

        private bool RemoveQuestionPackActive(object? arg)
        {
            if (Packs.Count > 1) return true;
            else return false;
        }

        private void RemoveQuestionPack(object obj)
        {
            var messageBoxResult = MessageBox.Show("Are you sure you want to Delete this Pack", "Are You Sure", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Packs.Remove(ActivePack);
                RemoveQuestionPackCommand.RaiseCanExecuteChanged();
            }

            if (Packs.Count > 0) ActivePack = Packs.FirstOrDefault();
        }

        private void SelectActivePackCommand(object obj)
        {
            ActivePack = (QuestionPackViewModel)obj;
        }

        private void NewQuestionPackCancel(object obj)
        {
            CreatePackDialog.Close();
        }

        private void NewQuestionPackCreate(object obj)
        {
            RemoveQuestionPackCommand.RaiseCanExecuteChanged();

            Packs.Add(NewQuestionPack);
            ActivePack = NewQuestionPack;
            CreatePackDialog.Close();
        }

        private void CreateNewQuestionPack(object obj)
        {
            NewQuestionPack = new QuestionPackViewModel(new QuestionPack("Default Question Pack"));            
            
            CreatePackDialog = new CreateNewPackDialog();
            CreatePackDialog.DataContext = this;
            CreatePackDialog.ShowDialog();

        }
    }
}
