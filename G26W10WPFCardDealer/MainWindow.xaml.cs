using System.Windows;

namespace G26W10WPFCardDealer;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataContext = new CardViewModel(new CardModel());
    }
}