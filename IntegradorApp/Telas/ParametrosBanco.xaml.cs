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
using IntegradorCore.NHibernate;
using IntegradorCore.NHibernate.DAO;

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
            Processos proc = new Processos();
            if(TxbHost.Text != "" && TxbPort.Text != "" && TxbServiceName.Text != "" && TxbUser.Text != "" && PwbSenha.Password != "" && CboDriver.SelectedIndex != -1)
            {
                if (proc.VerificaConexaoBanco(TxbHost.Text, TxbPort.Text, TxbServiceName.Text, TxbUser.Text, PwbSenha.Password, (string)CboDriver.SelectedItem) == true)
                {
                    if (CboDriver.SelectedIndex == 0)
                    {
                        StaticParametersDB.SetDriver("oracle");
                    }
                    else
                    {
                        StaticParametersDB.SetDriver("sqlserver");
                    }
                    StaticParametersDB.SetHost(TxbHost.Text);
                    StaticParametersDB.SetPort(TxbPort.Text);
                    StaticParametersDB.SetServiceName(TxbServiceName.Text);
                    StaticParametersDB.SetUser(TxbUser.Text);
                    StaticParametersDB.SetPassword(PwbSenha.Password);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Preencha todos os campos antes de continuar");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var sessao = AuxiliarNhibernate.AbrirSessao();

            var paramdb = new ParametroDB_DAO(sessao);

            var param = new ParametroDAO(sessao);

            try
            {
                var item = paramdb.BuscarPorID(1);
                var a = item.Driver;
                paramdb.Remover(item);
                var itemUp = param.BuscarPorID(1);
                itemUp.IntegraBanco = false;
                StaticParametros.SetIntegraBanco(false);
                param.Salvar(itemUp);
                StaticParametersDB.SetDriver(null);
                TxbHost.Text = "";
                TxbPort.Text = "";
                TxbServiceName.Text = "";
                TxbUser.Text = "";
                PwbSenha.Password = "";
            }
            catch (Exception)
            {

            }
            

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            __main.Show();
        }

        #endregion

        #region Functions
        
        private void init()
        {
            BtnDelete.Visibility = Visibility.Hidden;
            CboDriver.Items.Insert(0, "Oracle");
            CboDriver.Items.Insert(1, "SQLServer");

            CboDriver.SelectedIndex = -1;
            var sessao = AuxiliarNhibernate.AbrirSessao();
            var ParametroDB = new ParametroDB_DAO(sessao);

            try
            {
                var process = new Processos();

                var param = ParametroDB.BuscarPorID(1);//Armazenamento.GetParametrosDB();
                
                try
                {
                    if(param.Driver == "oracle")
                    {
                        CboDriver.SelectedIndex = 0;
                    }
                    else
                    {
                        CboDriver.SelectedIndex = 1;
                    }
                    TxbHost.Text = param.Host;
                    TxbPort.Text = param.Port;
                    TxbServiceName.Text = param.ServiceName;
                    TxbUser.Text = param.User;
                    

                    StaticParametersDB.SetDriver("oracle");
                    StaticParametersDB.SetHost(TxbHost.Text);
                    StaticParametersDB.SetPort(TxbPort.Text);
                    StaticParametersDB.SetServiceName(TxbServiceName.Text);
                    StaticParametersDB.SetUser(TxbUser.Text);

                    PwbSenha.Password = AESThenHMAC.SimpleDecryptWithPassword(param.Password, process.GetMacAdress());
                    StaticParametersDB.SetPassword(PwbSenha.Password);

                    BtnDelete.Visibility = Visibility.Visible;
                }
                catch (Exception e)
                {
                    if (StaticParametersDB.GetDriver() != null)
                    {
                        var driver = StaticParametersDB.GetDriver();
                        if (driver == "oracle")
                        {
                            CboDriver.SelectedIndex = 0;
                        }
                        else
                        {
                            CboDriver.SelectedIndex = 1;
                        }
                        TxbHost.Text = StaticParametersDB.GetHost();
                        TxbPort.Text = StaticParametersDB.GetPort();
                        TxbServiceName.Text = StaticParametersDB.GetServiceName();
                        TxbUser.Text = StaticParametersDB.GetUser();
                        PwbSenha.Password = StaticParametersDB.GetPassword();

                        BtnDelete.Visibility = Visibility.Visible;
                    }
                }
                

            }catch(Exception e)
            {
                
            }

            sessao.Close();
        }

        #endregion
    }
}
