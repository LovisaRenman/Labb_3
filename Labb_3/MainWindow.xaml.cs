using Labb_3.Dialogs;
using Labb_3.Model;
using Labb_3.ViewModel;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace Labb_3
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        private Window _dialog;

        public MainWindow()
        {
            InitializeComponent();
            mainWindowViewModel = new MainWindowViewModel();
            DataContext = mainWindowViewModel;

            mainWindowViewModel.ConfigurationViewModel.RequestShowDialogPackageOptions += ShowDialogPackOptions;
            mainWindowViewModel.RequestShowDialogImportQuestions += ShowDialogImportQuestions;
            mainWindowViewModel.RequestShowDialogCreateNewPack += ShowDialogCreateNewPack;
            mainWindowViewModel.RequestCloseDialogCreateNewPack += CloseDialog;
            mainWindowViewModel.APIViewModel.RequestCloseDialogImportQuestions += CloseDialog;
            mainWindowViewModel.RequestShowMessageBoxRemove += RemoveMessageBox;
            mainWindowViewModel.APIViewModel.RequestShowMessageBoxGettingQuestions += GettingQuestions;
            mainWindowViewModel.APIViewModel.RequestShowMessageBoxDoneGettingQuestions += DoneGettingQuestions;
            mainWindowViewModel.APIViewModel.RequestCloseMessageBoxGettingQuestions += CloseGettingQuestions;
            mainWindowViewModel.APIViewModel.RequestShowMessageBoxProblemLoading += ProblemLoading;
            mainWindowViewModel.APIViewModel.RequestShowMessageBoxWaitClient += WaitClient;
        }


        public void CloseGettingQuestions(object? sender, EventArgs arg)
        {

        }
        public void WaitClient(object? sender, EventArgs arg)
        {
            MessageBox.Show("Wait for client to answer", "Try again", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ProblemLoading(object? sneder, EventArgs arg)
        {
            MessageBox.Show("There was a problem loading Categories from Open Trivia", "Error", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void DoneGettingQuestions(object? sender, EventArgs args)
        {
            MessageBox.Show("Your requested Questions is finished importing");
        }        

        private void GettingQuestions(object? sender, EventArgs args)
        {
            var mresult = MessageBox.Show("Getting Questions");
        }

        private void CloseApplication(object? sender, EventArgs args)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure You want to close the application?", "Closing", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes) Application.Current.MainWindow.Close();
        }

        private void RemoveMessageBox(object? sender, EventArgs args)
        {
            var messageBoxResult = MessageBox.Show("Are you sure you want to Delete this Pack", "Warning", 
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                mainWindowViewModel.Packs.Remove(mainWindowViewModel.ActivePack);
                mainWindowViewModel.RemoveQuestionPackCommand.RaiseCanExecuteChanged();
            }
        }

        public void CloseDialog(object? sender, EventArgs args)
        {
            if (_dialog != null)
            {
                _dialog?.Close();
                _dialog = null;
            }
        }

        public void ShowDialogPackOptions(object? sender, EventArgs args)
        {
            _dialog = new PackOptionsDialog();
            ShowDialog(mainWindowViewModel);
        }

        public void ShowDialogCreateNewPack(object? sender, EventArgs args)
        {
            _dialog = new CreateNewPackDialog();
            ShowDialog(mainWindowViewModel);
        }

        public void ShowDialogImportQuestions(object? sender, EventArgs args)
        {
            _dialog = new ImportQuestionsDialog();
            ShowDialog(mainWindowViewModel);
        }

        public void ShowDialog(object viewModel)
        {
            try
            {
                _dialog.DataContext = viewModel;
                _dialog.Owner = Application.Current.MainWindow;
                _dialog.Show();
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occurred while opening the dialog box {e.Message}");
            }
        }
    }
}
