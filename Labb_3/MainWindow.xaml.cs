using Labb_3.Model;
using Labb_3.ViewModel;
using System.Windows;

namespace Labb_3
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}