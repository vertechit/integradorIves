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
using IntegradorCore.Services;
using IntegradorCore.DAO;

namespace IntegradorApp.Telas
{
    /// <summary>
    /// Lógica interna para ParametrosBanco.xaml
    /// </summary>
    public partial class ParametrosBanco : Window
    {
        MainWindow __main;

        public ParametrosBanco(MainWindow a)
        {
            InitializeComponent();
            __main = a;
            init();
        }
        #region Click Events

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if(TxbHost.Text != "" && TxbPort.Text != "" && TxbServiceName.Text != "" && TxbUser.Text != "" && PwbSenha.Password != "")
            {
                StaticParametersDB.SetDriver("oracle");
                StaticParametersDB.SetHost(TxbHost.Text);
                StaticParametersDB.SetPort(TxbPort.Text);
                StaticParametersDB.SetServiceName(TxbServiceName.Text);
                StaticParametersDB.SetUser(TxbUser.Text);
                StaticParametersDB.SetPassword(PwbSenha.Password);
                this.Close();
            }
            else
            {
                MessageBox.Show("Preencha todos os campos antes de continuar");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            __main.Show();
        }

        #endregion

        #region Functions
        
        private void init()
        {
            try
            {
                var process = new Processos();
                var param = Armazenamento.GetParametrosDB();
                
                if(param != null)
                {
                    TxbHost.Text = param.Host;
                    TxbPort.Text = param.Port;
                    TxbServiceName.Text = param.ServiceName;
                    TxbUser.Text = param.User;
                    PwbSenha.Password = AESThenHMAC.SimpleDecryptWithPassword(param.Password, process.GetMacAdress());

                    StaticParametersDB.SetDriver("oracle");
                    StaticParametersDB.SetHost(TxbHost.Text);
                    StaticParametersDB.SetPort(TxbPort.Text);
                    StaticParametersDB.SetServiceName(TxbServiceName.Text);
                    StaticParametersDB.SetUser(TxbUser.Text);
                    StaticParametersDB.SetPassword(PwbSenha.Password);
                }

            }catch(Exception e)
            {
                
            }
        }

        #endregion
    }
}
