using System.Windows;

namespace BitmapFontViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainViewModel = new ViewModel(this);
            this.DataContext = MainViewModel;
        }

        public ViewModel MainViewModel { get; private set; }
    }
}
