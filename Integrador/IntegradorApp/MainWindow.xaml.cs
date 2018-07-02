using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IntegradorCore.DAO;
using IntegradorCore.Services;
using IntegradorCore.Modelos;
using IntegradorApp.Telas;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Threading;

namespace IntegradorApp
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            SplashScreen spScreen = new SplashScreen("Logo grande 3.png");
            spScreen.Show(true);
            Init();
            StaticParametros.SetTipoApp("Client");
        }
        
        #region Click events
        private void BtnProcurarToken_Click(object sender, RoutedEventArgs e)
        {
            
            var proc = new Processos();
            if (proc.VerificaProcessoRun() == false)
            {
                System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
                dlg.Title = "Arquivo de token";
                dlg.Filter = "Token|*.ives;...";
                dlg.ShowDialog();


                var s = dlg.SafeFileName;

                var b = s.Contains(".ives");

                if (b == true)
                {
                    StaticParametros.SetDirToke(dlg.FileName);

                    if (DefineToken(StaticParametros.GetDirToke()) == true)
                    {
                        txtFolderToken.Text = dlg.FileName;
                    }
                }
                else if (dlg.FileName != "" && b == false)
                {
                    System.Windows.MessageBox.Show("Formato do arquivo não suportado");
                    StaticParametros.SetDirToke(null);
                    txtFolderToken.Text = "";
                }
            }
            else
            {
                System.Windows.MessageBox.Show("O serviço está em execução");
            }
        }

        private void BtnProcurarIni_Click(object sender, RoutedEventArgs e)
        {
            var proc = new Processos();
            if (proc.VerificaProcessoRun() == false)
            {
                FolderBrowserDialog dlgf = new FolderBrowserDialog();
                dlgf.ShowDialog();
                var s = dlgf.SelectedPath;

                if (s != "")
                {
                    StaticParametros.SetDirOrigem(s);

                    if (StaticParametros.GetDirOrigem() != null)
                    {
                        txtFolderIni.Text = s;
                        //Job();
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("O serviço está em execução");
            }
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            var proc = new Processos();
            if (proc.VerificaProcessoRun() == false)
            {
                Processos process = new Processos();
                var ctrl = 0;
                if (StaticParametros.GetDirToke() != null)
                {
                    if (File.Exists(StaticParametros.GetDirToke()))
                    {
                        ctrl++;
                        if (StaticParametros.GetDirOrigem() != null)
                        {
                            if (Directory.Exists(StaticParametros.GetDirOrigem()))
                            {
                                ctrl++;

                                if (ctrl == 2)
                                {
                                    if(StaticParametersDB.GetDriver() != null)
                                    {
                                        StaticParametros.SetIntegraBanco(true);
                                        Armazenamento.AddParametrosDB(new ParametroDB { Id = 1, Driver = StaticParametersDB.GetDriver(), Host = StaticParametersDB.GetHost(), Port = StaticParametersDB.GetPort(), ServiceName = StaticParametersDB.GetServiceName(), User = StaticParametersDB.GetUser(), Password = AESThenHMAC.SimpleEncryptWithPassword(StaticParametersDB.GetPassword(), process.GetMacAdress()) });
                                    }
                                    
                                    Armazenamento.AddParametros(new Parametro { Id = 1, CaminhoDir = StaticParametros.GetDirOrigem(), CaminhoToke = StaticParametros.GetDirToke(), IntegraBanco = StaticParametros.GetIntegraBanco() });
                                    OrganizaTelaEvent(2);
                                    process.CriarPastas();
                                }
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("Ops! Parece que a pasta de origem foi movida");
                                txtFolderIni.Text = "";
                                StaticParametros.SetDirOrigem(null);
                            }

                        }
                        else
                        {
                            System.Windows.MessageBox.Show("É necessário definir um diretorio para continuar");
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Ops! Parece que o arquivo de token foi movido");
                        txtFolderToken.Text = "";
                        StaticParametros.SetDirToke(null);
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("É necessário definir um token para continuar");
                }
            }

            else
            {
                System.Windows.MessageBox.Show("O serviço está em execução");
            }
        }

        private void BtnParam_Click(object sender, RoutedEventArgs e)
        {
            var proc = new Processos();
            if (proc.VerificaProcessoRun() == false)
            {
                OrganizaTelaEvent(1);
            }
            else
            {
                System.Windows.MessageBox.Show("O serviço está em execução");
            }
        }

        private void BtnLog_Click(object sender, RoutedEventArgs e)
        {
            var proc = new Processos();
            if (proc.VerificaProcessoRun() == false)
            {
                var Tela = new Telas.SistemaLog(this);
                Tela.Show();
                this.Hide();
            }
            else
            {
                System.Windows.MessageBox.Show("O serviço está em execução");
            }
        }

        private void BtnEnviar_Click(object sender, RoutedEventArgs e)
        {
            var proc = new Processos();
            if (proc.VerificaProcessoRun() == false)
            {
                this.Cursor = System.Windows.Input.Cursors.Wait;
                Integra(1);
                this.Cursor = System.Windows.Input.Cursors.Arrow;
            }
            else
            {
                System.Windows.MessageBox.Show("O serviço está em execução");
            }
        }

        private void BtnConsultar_Click(object sender, RoutedEventArgs e)
        {
            var proc = new Processos();
            if (proc.VerificaProcessoRun() == false)
            {
                this.Cursor = System.Windows.Input.Cursors.Wait;
                Consulta(1);
                this.Cursor = System.Windows.Input.Cursors.Arrow;
            }
            else
            {
                System.Windows.MessageBox.Show("O serviço está em execução");
            }
        }

        private void BtnConectarBanco_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var janela = new Telas.ParametrosBanco(this);
            janela.Show();
        }
        #endregion

        #region Functions

        public void Init()
        {
            Processos process = new Processos();

            DirectoryInfo dir = new DirectoryInfo(@"C:\\vch");

            int i = 0;

            try
            {
                foreach (var item in dir.GetFiles())
                {
                    i++;
                }
            }
            catch (Exception)
            {

            }

            if (i > 0)
            {
                int arq1 = 0;
                int arq2 = 0;

                foreach (var item in dir.GetFiles())
                {
                    if (item.Name == "dados.db")
                    {
                        arq1 = 1;
                    }
                    else if (item.Name == "logs.db")
                    {
                        arq2 = 1;
                    }
                }

                if (arq1 == 0)
                {
                    Armazenamento.CriarBancoSQLite();
                    Armazenamento.CriarTabelaSQlite();
                }
                if (arq2 == 0)
                {
                    Log.CriarBancoSQLite();
                    Log.CriarTabelaSQlite();
                }

                var param = Armazenamento.GetParametros();
                int ctrl = 0;

                if (param != null)
                {
                    if (param.CaminhoToke.Contains(".ives") && param.CaminhoToke != "" && File.Exists(param.CaminhoToke))
                    {
                        StaticParametros.SetDirToke(param.CaminhoToke);
                        txtFolderToken.Text = param.CaminhoToke;
                        DefineToken(param.CaminhoToke);
                        ctrl++;
                    }

                    if (param.CaminhoDir != "")
                    {

                        if (Directory.Exists(param.CaminhoDir))
                        {
                            txtFolderIni.Text = param.CaminhoDir;
                            StaticParametros.SetDirOrigem(param.CaminhoDir);
                            ctrl++;
                        }

                        if (ctrl == 2)
                        {
                            try
                            {
                                var paramDB = Armazenamento.GetParametrosDB();

                                if(paramDB != null)
                                {
                                    StaticParametros.SetIntegraBanco(param.IntegraBanco);

                                    StaticParametersDB.SetDriver(paramDB.Driver);
                                    StaticParametersDB.SetHost(paramDB.Host);
                                    StaticParametersDB.SetPort(paramDB.Port);
                                    StaticParametersDB.SetServiceName(paramDB.ServiceName);
                                    StaticParametersDB.SetUser(paramDB.User);
                                    StaticParametersDB.SetPassword(AESThenHMAC.SimpleDecryptWithPassword(paramDB.Password, process.GetMacAdress()));
                                }
                                else
                                {
                                    StaticParametros.SetIntegraBanco(false);
                                    Armazenamento.UpdateParametros(new Parametro { Id = 1, CaminhoDir = param.CaminhoDir, CaminhoToke = param.CaminhoToke, IntegraBanco = false });
                                }

                                OrganizaTelaEvent(2);
                                //Job();
                                process.CriarPastas();
                                Thread Tproc = new Thread(process.LimpaLog);
                                Tproc.Start();
                            }
                            catch (Exception ex)
                            {
                                OrganizaTelaEvent(1);
                            }

                        }
                        else
                        {

                            Thread Tproc = new Thread(process.LimpaLog);
                            Tproc.Start();

                            OrganizaTelaEvent(1);
                        }

                    }

                }

                else if (param == null)
                {
                    OrganizaTelaEvent(1);
                }
            }

            else
            {
                string user = System.Windows.Forms.SystemInformation.UserName;
                System.IO.DirectoryInfo folderInfo = new System.IO.DirectoryInfo("C:\\");

                DirectorySecurity ds = new DirectorySecurity();
                ds.AddAccessRule(new FileSystemAccessRule(user, FileSystemRights.Modify, AccessControlType.Allow));
                ds.SetAccessRuleProtection(false, false);
                folderInfo.Create(ds);
                folderInfo.CreateSubdirectory("vch");

                Armazenamento.CriarBancoSQLite();
                Armazenamento.CriarTabelaSQlite();
                Log.CriarBancoSQLite();
                Log.CriarTabelaSQlite();

                OrganizaTelaEvent(1);
            }

        }

        public bool DefineToken(string dir)
        {

            try
            {
                string[] lines = System.IO.File.ReadAllLines(@dir);

                if (lines.Length == 2)
                {
                    StaticParametros.SetGrupo(Convert.ToInt32(lines[0]));
                    StaticParametros.SetToken(lines[1]);

                    return true;
                }
                else
                {
                    System.Windows.MessageBox.Show("Arquivo não suportado");
                    StaticParametros.SetDirToke(null);
                    txtFolderToken.Text = "";

                    return false;
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Arquivo de token não pode ser importado");
                StaticParametros.SetDirToke(null);
                txtFolderToken.Text = "";
                return false;
            }


        }

        private void Integra(int i)
        {
            if(i > 3)
            {
                System.Windows.MessageBox.Show("Processo de integração concluido!");
                return;
            }
            Processos proc = new Processos();

            if (proc.ValidaStaticParametros())
            {
                proc.DefineNullParametros();
                if (!proc.ValidaStaticParamsJob())
                {
                    proc.AlteraParametro(i);
                    Jobs Job = new Jobs();
                    Job.Envia();
                    Integra(i + 1);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Parametros não foram encontrados");
                OrganizaTelaEvent(1);
            }
        }

        private void Consulta(int i)
        {
            if (i > 3)
            {
                System.Windows.MessageBox.Show("Processo de consulta concluido!");
                return;
            }

            Processos proc = new Processos();

            if (proc.ValidaStaticParametros())
            {
                proc.DefineNullParametros();
                if (!proc.ValidaStaticParamsJob())
                {
                    proc.AlteraParametro(i);
                    Jobs Job = new Jobs();
                    Job.Consulta();
                    Consulta(i + 1);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Parametros não foram encontrados");
                OrganizaTelaEvent(1);
            }
        }

        private void OrganizaTelaEvent(int tipo)
        {
            if (tipo == 1)
            {
                BtnSalvar.Visibility = Visibility.Visible;
                LblSalvar.Visibility = Visibility.Visible;

                BtnProcurarIni.Visibility = Visibility.Visible;
                BtnProcurarToken.Visibility = Visibility.Visible;

                BtnConsultar.Visibility = Visibility.Hidden;
                BtnEnviar.Visibility = Visibility.Hidden;
                LblqtdCons.Visibility = Visibility.Hidden;
                LblqtdEnv.Visibility = Visibility.Hidden;
                LbltmEnv.Visibility = Visibility.Hidden;
                LbltmCons.Visibility = Visibility.Hidden;
                BtnParam.Visibility = Visibility.Hidden;
                BtnLog.Visibility = Visibility.Hidden;
                BtnConectarBanco.Visibility = Visibility.Visible;
            }
            else if (tipo == 2)
            {
                BtnConsultar.Visibility = Visibility.Visible;
                BtnEnviar.Visibility = Visibility.Visible;
                LblqtdCons.Visibility = Visibility.Visible;
                LblqtdEnv.Visibility = Visibility.Visible;
                LbltmEnv.Visibility = Visibility.Visible;
                LbltmCons.Visibility = Visibility.Visible;
                BtnParam.Visibility = Visibility.Visible;
                BtnLog.Visibility = Visibility.Visible;

                BtnSalvar.Visibility = Visibility.Hidden;
                LblSalvar.Visibility = Visibility.Hidden;
                BtnProcurarIni.Visibility = Visibility.Hidden;
                BtnProcurarToken.Visibility = Visibility.Hidden;
                BtnConectarBanco.Visibility = Visibility.Hidden;
            }
        }

        #endregion

        
    }
}
