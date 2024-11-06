using Labb_3.Command;
using Labb_3.Dialogs;
using Labb_3.Model;
using System.Windows;

namespace Labb_3.ViewModel
{
    class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        public DelegateCommand BtnAddCommand { get; }
        public DelegateCommand BtnRemoveCommand { get; }
        public DelegateCommand BtnOptionsOpenCommand { get; }
        public DelegateCommand StartEditModeCommand { get; }

        public event EventHandler RequestShowDialogPackageOptions;
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

            BtnAddCommand = new DelegateCommand(AddButton, ButtonActive);
            BtnRemoveCommand = new DelegateCommand(RemoveButton, RemoveButtonActive);
            BtnOptionsOpenCommand = new DelegateCommand(Optionsbutton, ButtonActive);
            StartEditModeCommand = new DelegateCommand(StartEditMode, StartEditModeActive);

            ActiveQuestion = ActivePack.Questions.FirstOrDefault();
        }

        private bool ButtonActive(object? arg)
        {
            return VisibilityModeConfigurationView;            
        }

        private bool StartEditModeActive(object? arg)
        {
            return !VisibilityModeConfigurationView;

        }

        private void StartEditMode(object obj)
        {
            mainWindowViewModel.PlayerViewModel.Timer.Stop();

            mainWindowViewModel.PlayerViewModel.VisibilityModePlayerView = false;
            RaisePropertyChanged("VisibilityModePlayerView");
            mainWindowViewModel.PlayerViewModel.VisibilityModePlayerEndView = false;
            RaisePropertyChanged("VisibilityModePlayerEndView");

            VisibilityModeConfigurationView = true;
            RaisePropertyChanged("VisibilityModeConfigurationView");

            mainWindowViewModel.PlayerViewModel.StartPlayModeCommand.RaiseCanExecuteChanged();
            StartEditModeCommand.RaiseCanExecuteChanged();
            BtnAddCommand.RaiseCanExecuteChanged();
            BtnRemoveCommand.RaiseCanExecuteChanged();
            BtnOptionsOpenCommand.RaiseCanExecuteChanged();

            mainWindowViewModel.SaveJson();
        }

        private void Optionsbutton(object obj)
        {
            RequestShowDialogPackageOptions.Invoke(this, EventArgs.Empty);
            mainWindowViewModel.SaveJson();
            
        }

        private bool RemoveButtonActive(object? arg)
        {
            if (mainWindowViewModel.PlayerViewModel.VisibilityModePlayerView 
                || mainWindowViewModel.PlayerViewModel.VisibilityModePlayerEndView) return false;
            else if (ActiveQuestion != null) return true;
            else return false;
        }

        private void RemoveButton(object obj)
        {
            ActivePack.Questions.Remove(ActiveQuestion);
            mainWindowViewModel.PlayerViewModel.StartPlayModeCommand.RaiseCanExecuteChanged();
            mainWindowViewModel.SaveJson();
        }

        private void AddButton(object obj)
        {
            ActivePack.Questions.Add(new Question(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty));

            BtnAddCommand.RaiseCanExecuteChanged();
            mainWindowViewModel.PlayerViewModel.StartPlayModeCommand.RaiseCanExecuteChanged();

            mainWindowViewModel.SaveJson();
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
                BtnRemoveCommand.RaiseCanExecuteChanged();
            }
        }
        public bool IsEnabled
        {
            get => ActiveQuestion is not null;
        }
    }
}
