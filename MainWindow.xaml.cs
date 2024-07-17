using CSVReaderTask.Models.ViewModels;
using MahApps.Metro.Controls;
using System.Windows;


namespace CSVReaderTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        /// <param name="filterVM">View model for filtering and displaying data.</param>
        public MainWindow(FilterVM filterVM)
        {
            DataContext = filterVM;
            InitializeComponent();
        }
    }
}