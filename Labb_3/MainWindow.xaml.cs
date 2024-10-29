using Labb_3.Model;
using Labb_3.ViewModel;
using System.Windows;

namespace Labb_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    // ha oftast en RaisePropertyChanged i setter men gör det någon annanstans om det inte finns en setter
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();

        }
    }
}