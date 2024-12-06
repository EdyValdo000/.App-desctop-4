using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using App_desctop_4.Status;
using App_desctop_4.User_informations;
using App_desctop_4.User_informations;
using App_desctop_4;

namespace App_desctop_4
{
    /// <summary>
    /// Lógica interna para Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        userValided userValided = new userValided();
        newUser newUser = new newUser();
        public Window1()
        {
            InitializeComponent();
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userName = tbUsername.Text.Trim();
                string pass = tbPassword.Password.Trim();

                // Verifica se os campos estão vazios
                if (string.IsNullOrEmpty(userName))
                {
                    MessageBox.Show("O campo endereço IP é obrigatório.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                    tbUsername.Focus(); // Foca no campo email
                    return;
                }

                if (string.IsNullOrEmpty(pass))
                {
                    MessageBox.Show("O campo porta é obrigatório.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                    tbPassword.Focus(); // Foca no campo senha
                    return;
                }

                newUser.userName = tbUsername.Text;
                newUser.password = tbPassword.Password;
                newUser.newCount();

                MessageBox.Show("Bem vindo");
                MainWindow mainWindow = new MainWindow();
                mainWindow.ShowDialog();
            }
            catch { }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string userName = tbUsername.Text.Trim();
            string pass = tbPassword.Password.Trim();

            // Verifica se os campos estão vazios
            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("O campo endereço IP é obrigatório.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                tbUsername.Focus(); // Foca no campo email
                return;
            }

            if (string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("O campo porta é obrigatório.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                tbPassword.Focus(); // Foca no campo senha
                return;
            }

            userValided.userName = tbUsername.Text;
            userValided.password = tbPassword.Password;


            if (userValided.valided())
            {
                MessageBox.Show("Bem vindo");
                MainWindow mainWindow = new MainWindow();
                mainWindow.ShowDialog();
            }
        }       
    }
}
