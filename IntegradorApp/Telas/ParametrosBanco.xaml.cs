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
using IntegradorCore.Modelos;

namespace IntegradorApp.Telas
{
    /// <summary>
    /// Lógica interna para ParametrosBanco.xaml
    /// </summary>
    public partial class ParametrosBanco : Window
    {
        MainWindow __main;
        static IList<ParametroDB> paramDB;
        Int32 lastIndex = -1;
        public ParametrosBanco(MainWindow a)
        {
            InitializeComponent();
            __main = a;
            init2();
        }
        #region Click Events

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            ParametroDB paramdbNew = new ParametroDB();
            Processos proc = new Processos();
            if(TxbHost.Text != "" && TxbPort.Text != "" && TxbServiceName.Text != "" && TxbUser.Text != "" && PwbSenha.Password != "" && CboDriver.SelectedIndex != -1 && CboConexao.SelectedIndex != -1 && TxbDescr.Text != "")
            {
                if (proc.VerificaConexaoBanco(TxbHost.Text, TxbPort.Text, TxbServiceName.Text, TxbUser.Text, PwbSenha.Password, (string)CboDriver.SelectedItem) == true)
                {
                    if(lastIndex != 0)
                    {
                        if (CboDriver.SelectedIndex == 0)
                        {
                            paramDB[lastIndex-1].Driver = "oracle";
                        }
                        else
                        {
                            paramDB[lastIndex - 1].Driver = "sqlserver";
                        }
                        paramDB[lastIndex - 1].Descr = TxbDescr.Text;
                        paramDB[lastIndex - 1].Host = TxbHost.Text;
                        paramDB[lastIndex - 1].Port = TxbPort.Text;
                        paramDB[lastIndex - 1].ServiceName = TxbServiceName.Text;
                        paramDB[lastIndex - 1].User = TxbUser.Text;
                        paramDB[lastIndex - 1].Password = AESThenHMAC.SimpleEncryptWithPassword(PwbSenha.Password, proc.GetMacAdress());
                        paramDB[lastIndex - 1].Trusted_Conn = "True";
                        salvaDB(paramDB[lastIndex-1]);
                    }
                    else
                    {
                        if (CboDriver.SelectedIndex == 0)
                        {
                            paramdbNew.Driver = "oracle";
                        }
                        else
                        {
                            paramdbNew.Driver = "sqlserver";
                        }
                        paramdbNew.Descr = TxbDescr.Text;
                        paramdbNew.Host = TxbHost.Text;
                        paramdbNew.Port = TxbPort.Text;
                        paramdbNew.ServiceName = TxbServiceName.Text;
                        paramdbNew.User = TxbUser.Text;
                        paramdbNew.Password = AESThenHMAC.SimpleEncryptWithPassword(PwbSenha.Password, proc.GetMacAdress());
                        paramdbNew.Trusted_Conn = "True";
                        salvaDB(paramdbNew);
                    }
                   
                    cleanScreen();
                    lastIndex = -1;
                }
                else if (proc.VerificaConexaoBanco(TxbHost.Text, TxbPort.Text, TxbServiceName.Text, TxbUser.Text, PwbSenha.Password, (string)CboDriver.SelectedItem, "False") == true)
                {
                    if (lastIndex != 0)
                    {
                        if (CboDriver.SelectedIndex == 0)
                        {
                            paramDB[lastIndex - 1].Driver = "oracle";
                        }
                        else
                        {
                            paramDB[lastIndex - 1].Driver = "sqlserver";
                        }
                        paramDB[lastIndex - 1].Descr = TxbDescr.Text;
                        paramDB[lastIndex - 1].Host = TxbHost.Text;
                        paramDB[lastIndex - 1].Port = TxbPort.Text;
                        paramDB[lastIndex - 1].ServiceName = TxbServiceName.Text;
                        paramDB[lastIndex - 1].User = TxbUser.Text;
                        paramDB[lastIndex - 1].Password = AESThenHMAC.SimpleEncryptWithPassword(PwbSenha.Password, proc.GetMacAdress());
                        paramDB[lastIndex - 1].Trusted_Conn = "False";
                        salvaDB(paramDB[lastIndex - 1]);
                    }
                    else
                    {
                        if (CboDriver.SelectedIndex == 0)
                        {
                            paramdbNew.Driver = "oracle";
                        }
                        else
                        {
                            paramdbNew.Driver = "sqlserver";
                        }
                        paramdbNew.Descr = TxbDescr.Text;
                        paramdbNew.Host = TxbHost.Text;
                        paramdbNew.Port = TxbPort.Text;
                        paramdbNew.ServiceName = TxbServiceName.Text;
                        paramdbNew.User = TxbUser.Text;
                        paramdbNew.Password = AESThenHMAC.SimpleEncryptWithPassword(PwbSenha.Password, proc.GetMacAdress());
                        paramdbNew.Trusted_Conn = "False";
                        StaticParametersDB.SetListBanco(paramdbNew);
                        salvaDB(paramdbNew);
                    }
                    
                    cleanScreen();
                    lastIndex = -1;
                }
            }
            else
            {
                MessageBox.Show("Preencha todos os campos antes de continuar", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var sessao = AuxiliarNhibernate.AbrirSessao();
            var paramdb = new ParametroDB_DAO(sessao);

            try
            {
                paramdb.Remover(paramDB[lastIndex-1]);
                paramDB.RemoveAt(lastIndex - 1);
                if(paramDB.Count >= 1)
                {
                    StaticParametersDB.clearListBanco();
                    foreach(var p in paramDB)
                    {
                        StaticParametersDB.SetListBanco(p);
                    }
                    StaticParametersDB.Setcurrent(paramDB[0].Id);
                    refreshCBO(paramDB[0], 1);
                }
                else
                {
                    StaticParametersDB.clearListBanco();
                    StaticParametersDB.clearAllStatic();
                    StaticParametros.SetIntegraBanco(false);
                    BtnDelete.Visibility = Visibility.Hidden;
                    CboConexao.Items.Clear();
                    CboConexao.Items.Insert(0, "Nova conexão");
                    CboConexao.SelectedIndex = 0;

                }
                cleanScreen();
            }
            catch (Exception ex)
            {

            }

            //this.Close();
        }

        private void BtnVoltar_Click(object sender, RoutedEventArgs e)
        {
            if(paramDB.Count >= 1)
            {
                StaticParametersDB.clearListBanco();
                foreach(var p in paramDB)
                {
                    StaticParametersDB.SetListBanco(p);
                }
                StaticParametersDB.Setcurrent(paramDB[0].Id);
            }
            this.Close();
        }

        private void CboConexao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Console.WriteLine("aqui"+ lastIndex);
            lastIndex = CboConexao.SelectedIndex;
            if (lastIndex != 0 && lastIndex != -1)
            {
                loadInfo(lastIndex-1);
            }
            else
            {
                BtnDelete.Visibility = Visibility.Hidden;
                validaAlteracoes();
                cleanScreen();
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
            init2();
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

        private void init2()
        {
            BtnDelete.Visibility = Visibility.Hidden;
            CboDriver.Items.Insert(0, "Oracle");
            CboDriver.Items.Insert(1, "SQLServer");
            CboDriver.SelectedIndex = -1;

            CboConexao.Items.Insert(0, "Nova conexão");
            CboConexao.SelectedIndex = 0;

            try
            {
                var process = new Processos();
                paramDB = null;
                paramDB = StaticParametersDB.getAllListBanco();

                if (paramDB.Count > 0)
                {
                    
                    for(int i = 0; i < paramDB.Count; i++)
                    {
                        CboConexao.Items.Insert(i+1, paramDB[i].Descr);
                    }
                        
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {

            }
        }

        private void refreshCBO(ParametroDB paramdbNew, int tipo)
        {
            BtnDelete.Visibility = Visibility.Hidden;
            BtnDelete.Visibility = Visibility.Hidden;
            if (tipo == 1)
            {
                try
                {
                    CboConexao.Items.Clear();
                    CboConexao.Items.Insert(0, "Nova conexão");
                    CboConexao.SelectedIndex = 0;

                    if (paramDB.Count > 1)
                    {
                        StaticParametersDB.clearListBanco();
                        foreach (var p in paramDB)
                        {
                            StaticParametersDB.SetListBanco(p);
                        }
                        for (int i = 0; i < paramDB.Count; i++)
                        {
                            CboConexao.Items.Insert(i + 1, paramDB[i].Descr);
                        }
                    }
                    else
                    {
                        StaticParametersDB.clearListBanco();
                        StaticParametersDB.SetListBanco(paramDB[0]);
                        CboConexao.Items.Insert(1, paramDB[0].Descr);
                    }

                }
                catch (Exception e)
                {

                }
            }
            else
            {
                try
                {
                    var process = new Processos();
                    paramDB.Add(paramdbNew);

                    CboConexao.Items.Clear();
                    CboConexao.Items.Insert(0, "Nova conexão");
                    CboConexao.SelectedIndex = 0;

                    if (paramDB.Count > 1)
                    {
                        StaticParametersDB.clearListBanco();
                        foreach (var p in paramDB)
                        {
                            StaticParametersDB.SetListBanco(p);
                        }
                        for (int i = 0; i < paramDB.Count; i++)
                        {
                            CboConexao.Items.Insert(i + 1, paramDB[i].Descr);
                        }
                    }
                    else
                    {
                        StaticParametersDB.clearListBanco();
                        StaticParametersDB.SetListBanco(paramDB[0]);
                        CboConexao.Items.Insert(1, paramDB[0].Descr);
                    }
                }
                catch (Exception e)
                {

                }
            }
            
        }
        private void validaAlteracoes()
        {
            //throw new NotImplementedException();
        }

        private void salvaDB(ParametroDB paramdbNew)
        {
            var sessao = AuxiliarNhibernate.AbrirSessao();
            var ParametroDB = new ParametroDB_DAO(sessao);
            var process = new Processos();

            if (lastIndex != 0)
            {
                ParametroDB.Salvar(paramDB[lastIndex-1]);
                refreshCBO(paramDB[lastIndex - 1], 1);
            }
            else
            {
                ParametroDB.Salvar(paramdbNew);
                refreshCBO(paramdbNew, 0);
            }
        }

        private void loadInfo(int index)
        {
            var process = new Processos();

            try
            {
                BtnDelete.Visibility = Visibility.Visible;
                if (paramDB[index].Driver == "oracle")
                {
                    CboDriver.SelectedIndex = 0;
                }
                else
                {
                    CboDriver.SelectedIndex = 1;
                }
                TxbDescr.Text = paramDB[index].Descr;
                TxbHost.Text = paramDB[index].Host;
                TxbPort.Text = paramDB[index].Port;
                TxbServiceName.Text = paramDB[index].ServiceName;
                TxbUser.Text = paramDB[index].User;
                PwbSenha.Password = AESThenHMAC.SimpleDecryptWithPassword(paramDB[index].Password, process.GetMacAdress());
            }
            catch(Exception e)
            {

            }
        }

        private void cleanScreen()
        {
            TxbDescr.Text = "";
            CboDriver.SelectedIndex = -1;
            TxbHost.Text = "";
            TxbPort.Text = "";
            TxbServiceName.Text = "";
            TxbUser.Text = "";
            PwbSenha.Password = "";
        }
        #endregion
    }
}
