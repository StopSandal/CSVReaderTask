using CSVReaderTask.EF;
using CSVReaderTask.Helpers.Interfaces;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CSVReaderTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ICSVReader _csvReader;
        private readonly CSVContext _dbContext;

        public MainWindow(ICSVReader csvReader, CSVContext dbContext)
        {
            _csvReader = csvReader;
            _dbContext = dbContext;

            InitializeComponent();

        }
    }
}