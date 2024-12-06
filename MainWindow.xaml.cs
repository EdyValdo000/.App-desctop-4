using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using App_desctop_4.Status;
using App_desctop_4.Conection;
using App_desctop_4.User_informations;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace App_desctop_4
{
    ///Para baixar
    ///  ModernWpf.UI.

    public partial class MainWindow : Window
    {
        lampStatus lampStatus = new lampStatus();        

        private DispatcherTimer updateTables;
        private DispatcherTimer updateLampStatus;

        ethernet ethernet = new ethernet();

        bool[] lampStatusBool = new bool[4]; // Array para controlar o estado das lâmpadas
        bool[] poolStatusBool = new bool[4]; // Array para controlar o estado da piscina

        bool indexPanel = false;

        public MainWindow()
        {
            InitializeComponent();

            this.Hide();

            lampStatus.loadTable(dataGridLampStatus);

            updateTables = new DispatcherTimer();
            updateTables.Interval = TimeSpan.FromSeconds(10);
            updateTables.Tick += updateTables_Click;

            updateLampStatus = new DispatcherTimer();
            updateLampStatus.Interval = TimeSpan.FromSeconds(3);
            updateLampStatus.Tick += updateLampStatus_Click;
        }

        private async void updateLampStatus_Click(object? sender, EventArgs e)
        {
            ethernet.send("s");
            ReadArduino();
            lampStatus.loadTable(dataGridLampStatus);
        }

        private async void updateTables_Click(object sender, EventArgs e)
        {
            lampStatus.loadTable(dataGridLampStatus);
        }

        private void Registus_click(object sender, RoutedEventArgs e)
        {
            if (!indexPanel)
            {
                painelDeControlo.Visibility = Visibility.Collapsed;
                TabelaLampStatus.Visibility = Visibility.Visible;
                btnRegistoAndPainel.Content = "Controlo";
            }
            else
            {
                painelDeControlo.Visibility = Visibility.Visible;
                TabelaLampStatus.Visibility = Visibility.Collapsed;
                btnRegistoAndPainel.Content = "Registros";
            }
            indexPanel = !indexPanel;
        }

        private async void btnConected_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ip = IpTextBox.Text.Trim();
                string port = PortTextBox.Text.Trim();

                // Verifica se os campos estão vazios
                if (string.IsNullOrEmpty(ip))
                {
                    MessageBox.Show("O campo endereço IP é obrigatório.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                    IpTextBox.Focus(); // Foca no campo email
                    return;
                }

                if (string.IsNullOrEmpty(port))
                {
                    MessageBox.Show("O campo porta é obrigatório.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                    PortTextBox.Focus(); // Foca no campo senha
                    return;
                }
                ethernet.conected(IpTextBox.Text, Int16.Parse(PortTextBox.Text));
                IpTextBox.IsEnabled = false;
                PortTextBox.IsEnabled = false;
                btnConected.Visibility = Visibility.Collapsed;
                btnDisconected.Visibility = Visibility.Visible;
                ethernet.send("s");
                updateTables.Start();
                updateLampStatus.Start();
            }
            catch
            {

            }
        }

        private async void ReadArduino()
        {
            while (ethernet.isConected())
            {
                try
                {
                    string receber = ethernet.read();

                    if (!string.IsNullOrEmpty(receber))
                    {
                        string[] data = receber.Split('*');

                        // Atualiza o estado das lâmpadas na UI thread
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            UpdateStatus(data);
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao ler dados: {ex.Message}");
                    break; // Termina o loop em caso de erro crítico
                }

                await Task.Delay(50); // Aguarda um pouco antes da próxima leitura
            }
        }

        // Atualiza o status das lâmpadas na interface do usuário
        private void UpdateStatus(string[] data)
        {
            if (data.Length >= 5) // Certifica-se de que o array tem pelo menos 5 elementos
            {
                // Atualiza o estado da Lâmpada 1
                if (data[0].StartsWith("Lamp1"))
                {
                    string lamp1Status = data[0].Split(' ')[1]; // Pega "ON" ou "OFF"
                    btnLamp1.Content = lamp1Status == "ON" ? "Lâmpada 1: Ligada" : "Lâmpada 1: Desligada";
                    lampStatus.lamp1Statu = btnLamp1.Content.ToString()!;
                }

                // Atualiza o estado da Lâmpada 2
                if (data[1].StartsWith("Lamp2"))
                {
                    string lamp2Status = data[1].Split(' ')[1];
                    btnLamp2.Content = lamp2Status == "ON" ? "Lâmpada 2: Ligada" : "Lâmpada 2: Desligada";
                    lampStatus.lamp2Statu = btnLamp2.Content.ToString()!;
                }

                // Atualiza o estado da Lâmpada 3
                if (data[2].StartsWith("Lamp3"))
                {
                    string lamp3Status = data[2].Split(' ')[1];
                    btnLamp3.Content = lamp3Status == "ON" ? "Lâmpada 3: Ligada" : "Lâmpada 3: Desligada";
                    lampStatus.lamp3Statu = btnLamp3.Content.ToString()!;
                }

                // Atualiza o estado da Lâmpada 4
                if (data[3].StartsWith("Lamp4"))
                {
                    string lamp4Status = data[3].Split(' ')[1];
                    btnLamp4.Content = lamp4Status == "ON" ? "Lâmpada 4 Ligada" : "Lâmpada 4: Desligada";
                    lampStatus.lamp4Statu = btnLamp4.Content.ToString()!;
                }
               
                lampStatus.timeLampChange = DateTime.Now.ToString("HH:mm dd/MM/yyyy");

                lampStatus.insertDataBase();
            }
        }

        private void btnDisconected_Click(object sender, RoutedEventArgs e)
        {
            ethernet.desconected();
            IpTextBox.IsEnabled = true;
            PortTextBox.IsEnabled = true;
            btnConected.Visibility = Visibility.Visible;
            btnDisconected.Visibility = Visibility.Collapsed;
            updateTables.Stop();
            updateLampStatus.Stop();
        }

        private void LampClicked(object sender, RoutedEventArgs e)
        {
            int lampIndex = int.Parse(((Button)sender).CommandParameter.ToString()) - 1;
            ToggleLamp(lampIndex);
        }

        private void ToggleLamp(int lampIndex)
        {
            if (!ethernet.isConected())
            {
                Console.WriteLine("Não está conectado ao Arduino.");
                return;
            }

            string sendCommand = lampStatusBool[lampIndex] ? $"{2 * lampIndex + 2}s" : $"{2 * lampIndex + 1}s";
            ethernet.send(sendCommand);
            lampStatusBool[lampIndex] = !lampStatusBool[lampIndex]; // Alterna o estado da lâmpada
        }

        bool poolStatu = false;
        private void PoolClicked(object sender, RoutedEventArgs e)
        {
            if (poolStatu == false) { ethernet.send("0s"); poolStatu = true; }
            else
            {
                ethernet.send("9s");
                poolStatu = false;
            }
        }

    }
}