using Labb_3.Command;
using Labb_3.Dialogs;
using Labb_3.Model;
using System.Windows;

namespace Labb_3.ViewModel
{
    class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        public DelegateCommand AddButtonCommand { get; }
        public DelegateCommand RemoveButtonCommand { get; }
        public DelegateCommand BtnOptionsOpenCommand { get; }
        public DelegateCommand StartEditModeCommand { get; }

        public QuestionPackViewModel? ActivePack => mainWindowViewModel.ActivePack;

        private bool _visibilityModeConfigurationView;
        public bool VisibilityModeConfigurationView
        {
            get => _visibilityModeConfigurationView;
            set
            {
                _visibilityModeConfigurationView = value;
                RaisePropertyChanged();
                RaisePropertyChanged("VisibilityModeConfigurationView");
            }
        }
       

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            VisibilityModeConfigurationView = true;

            AddButtonCommand = new DelegateCommand(AddButton, ButtonActive);
            RemoveButtonCommand = new DelegateCommand(RemoveButton, RemoveButtonActive);
            BtnOptionsOpenCommand = new DelegateCommand(Optionsbutton, ButtonActive);
            StartEditModeCommand = new DelegateCommand(StartEditMode, StartEditModeActive);

            ActiveQuestion = ActivePack.Questions.FirstOrDefault();
        }

        private bool ButtonActive(object? arg)
        {
            if (mainWindowViewModel.PlayerViewModel.VisibilityModePlayerView) return false;
            else if (mainWindowViewModel.PlayerViewModel.VisibilityModePlayerEndView) return false;
            else return true;
        }

        private bool StartEditModeActive(object? arg)
        {
            if (VisibilityModeConfigurationView) return false;
            else return true;

        }

        private void StartEditMode(object obj)
        {
            AddButtonCommand.RaiseCanExecuteChanged();
            RemoveButtonCommand.RaiseCanExecuteChanged();
            BtnOptionsOpenCommand.RaiseCanExecuteChanged();
            mainWindowViewModel.PlayerViewModel.Timer.Stop();

            mainWindowViewModel.PlayerViewModel.VisibilityModePlayerView = false;
            RaisePropertyChanged("VisibilityModePlayerView");
            mainWindowViewModel.PlayerViewModel.VisibilityModePlayerEndView = false;
            RaisePropertyChanged("VisibilityModePlayerEndView");

            VisibilityModeConfigurationView = true;
            RaisePropertyChanged("VisibilityModeConfigurationView");

            mainWindowViewModel.PlayerViewModel.StartPlayModeCommand.RaiseCanExecuteChanged();
            StartEditModeCommand.RaiseCanExecuteChanged();            
        }

        private void Optionsbutton(object obj)
        {
            PackOptionsDialog dialog = new PackOptionsDialog();
            dialog.DataContext = mainWindowViewModel;
            dialog.ShowDialog();
        }

        private bool RemoveButtonActive(object? arg)
        {
            if (mainWindowViewModel.PlayerViewModel.VisibilityModePlayerView) return false;
            else if (mainWindowViewModel.PlayerViewModel.VisibilityModePlayerEndView) return false;
            else if (ActiveQuestion != null) return true;
            else return false;
        }

        private void RemoveButton(object obj)
        {
            ActivePack.Questions.Remove(ActiveQuestion);  
            mainWindowViewModel.PlayerViewModel.StartPlayModeCommand.RaiseCanExecuteChanged();
        }

        private void AddButton(object obj)
        {
            ActivePack.Questions.Add(new Question(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty));

            AddButtonCommand.RaiseCanExecuteChanged();
            mainWindowViewModel.PlayerViewModel.StartPlayModeCommand.RaiseCanExecuteChanged();
        }

        private Question? _activeQuestion;
        public Question? ActiveQuestion
        {
            get => _activeQuestion; 
            set 
            {
                _activeQuestion = value;              
                RaisePropertyChanged();
                RaisePropertyChanged("IsEnabled");
                RemoveButtonCommand.RaiseCanExecuteChanged();
            }
        }
        public bool IsEnabled
        {
            get => ActiveQuestion is not null;
        }
    }
}
