using Labb_3.Dialogs;
using Labb_3.Model;
using Labb_3.ViewModel;
using System.Windows;

namespace Labb_3
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
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
        }

        private Window _dialog;
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
