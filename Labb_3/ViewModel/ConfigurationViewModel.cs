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

        public QuestionPackViewModel? ActivePack => mainWindowViewModel.ActivePack; 

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            AddButtonCommand = new DelegateCommand(AddButton);
            RemoveButtonCommand = new DelegateCommand(RemoveButton, RemoveButtonActive);
            OptionsButtonOpen = new DelegateCommand(Optionsbutton);

            ActiveQuestion = ActivePack.Questions.FirstOrDefault();
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
