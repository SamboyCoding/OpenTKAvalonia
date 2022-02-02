using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace NonFullScreenSample
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;
        
        public MainWindow()
        {
            DataContext = _viewModel = new();
            
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void ShowHideControl(object? sender, RoutedEventArgs e)
        {
            _viewModel.ShowOpenGlControl = !_viewModel.ShowOpenGlControl;
        }
    }
}