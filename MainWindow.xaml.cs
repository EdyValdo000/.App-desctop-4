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

namespace App_desctop_4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && (textBox.Text == "IP do servidor" || textBox.Text == "Porta do servidor"))
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.White;
            }
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (textBox.Name == "IpEntry")
                    textBox.Text = "IP do servidor";
                else if (textBox.Name == "PortEntry")
                    textBox.Text = "Porta do servidor";

                textBox.Foreground = Brushes.Gray;
            }
        }

        private void OnConnectClicked(object sender, RoutedEventArgs e)
        {

        }

        private void OnDisconnectClicked(object sender, RoutedEventArgs e)
        {

        }

        private void OnLampControlClicked(object sender, RoutedEventArgs e)
        {

        }
    }
}