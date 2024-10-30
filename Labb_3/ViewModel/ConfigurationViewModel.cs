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
        public DelegateCommand OptionsButtonOpen { get; }
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

        public bool VisibilityModePlayerView
        {
            get => mainWindowViewModel.PlayerViewModel.VisibilityModePlayerView;
            set
            {
                mainWindowViewModel.PlayerViewModel.VisibilityModePlayerView = value;
                RaisePropertyChanged();
                mainWindowViewModel.PlayerViewModel.RaisePropertyChanged("VisibilityModePlayerView");
            }
        }

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            VisibilityModeConfigurationView = true;

            AddButtonCommand = new DelegateCommand(AddButton);
            RemoveButtonCommand = new DelegateCommand(RemoveButton, RemoveButtonActive);
            OptionsButtonOpen = new DelegateCommand(Optionsbutton);
            StartEditModeCommand = new DelegateCommand(StartEditMode, StartEditModeActive);

            ActiveQuestion = ActivePack.Questions.FirstOrDefault();
        }

        private bool StartEditModeActive(object? arg)
        {
            if (VisibilityModeConfigurationView) return false;
            else return true;

        }

        private void StartEditMode(object obj)
        {
            VisibilityModePlayerView = false;
            mainWindowViewModel.RaisePropertyChanged("VisibilityModePlayerView");

            VisibilityModeConfigurationView = true;
            RaisePropertyChanged("VisibilityModeConfigurationView");

            mainWindowViewModel.PlayerViewModel.StartPlayModeCommand.RaiseCanExecuteChanged();
            StartEditModeCommand.RaiseCanExecuteChanged();
        }

        private void Optionsbutton(object obj)
        {
            var dialog = new PackOptionsDialog();
            dialog.DataContext = mainWindowViewModel;
            dialog.ShowDialog();
        }

        private bool RemoveButtonActive(object? arg)
        {
            if (ActiveQuestion != null) return true;
            else return false;           
        }

        private void RemoveButton(object obj)
        {
            ActivePack.Questions.Remove(ActiveQuestion);         
        }

        private void AddButton(object obj)
        {
            ActivePack.Questions.Add(new Question(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty));

            AddButtonCommand.RaiseCanExecuteChanged();
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
