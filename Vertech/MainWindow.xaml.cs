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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;
using Vertech.apiIntegra;
using Vertech.Services;
using System.Xml;
using System.Security.Permissions;
using System.Security.AccessControl;
using Vertech.DAO;
using Vertech.Modelos;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Vertech
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public Integra Integra = new Integra();
        public Consulta Consulta = new Consulta();
        int Controle = 0;

        public MainWindow()
        {
            InitializeComponent();
            SplashScreen spScreen = new SplashScreen("Logo grande 3.png");
            spScreen.Show(true);
            Init();
            Parametros.SetTipoApp("Client");
        }

        private void Init()
        {
            Processos process = new Processos();

            DirectoryInfo dir = new DirectoryInfo(@"C:\\vch");

            CboAmbiente.Items.Insert(0, "Produção");
            CboAmbiente.Items.Insert(1, "Produção Restrita - Dados Reais");

            CboAmbiente.SelectedIndex = 2;

            int i = 0;

            try
            {
                foreach (var item in dir.GetFiles())
                {
                    i++;
                }
            }
            catch(Exception)
            {

            }

            if(i > 0)
            {
                if(i == 1)
                {
                    foreach (var item in dir.GetFiles())
                    {
                        if(item.Name == "dados.sqlite")
                        {
                            Log.CriarBancoSQLite();
                            Log.CriarTabelaSQlite();
                        }
                        else
                        {
                            Helper.CriarBancoSQLite();
                            Helper.CriarTabelaSQlite();
                        }
                    }
                }

                var param = Helper.GetParametros();

                if (param != null)
                {
                    if (param.CaminhoToke.Contains(".ives") && param.CaminhoToke != "" && File.Exists(param.CaminhoToke))
                    {
                        Parametros.SetDirToke(param.CaminhoToke);
                        txtFolderToken.Text = param.CaminhoToke;
                        DefineToken(param.CaminhoToke);
                    }

                    if (param.CaminhoDir != "" && param.CaminhoFim != "" && param.CaminhoDir != param.CaminhoFim)
                    {
                        int ctrl = 0;

                        if( Directory.Exists(param.CaminhoDir) )
                        {
                            txtFolderIni.Text = param.CaminhoDir;
                            Parametros.SetDirArq(param.CaminhoDir);
                            ctrl++;
                        }

                        if ( Directory.Exists(param.CaminhoFim) )
                        {
                            txtFolderFim.Text = param.CaminhoFim;
                            Parametros.SetDirFim(param.CaminhoFim);
                            ctrl++;
                        }

                        if(ctrl == 2)
                        {

                            CboAmbiente.SelectedIndex = Convert.ToInt32(param.Ambiente)-1;
                            TxtbAmbiente.Text = (string)CboAmbiente.SelectedItem;

                            if(param.Base == "Vertech Produção")
                            {
                                CboBase.SelectedIndex = 0;
                                TxbBase.Text = param.Base;
                            }
                            else
                            {
                                CboBase.SelectedIndex = 1;
                                TxbBase.Text = param.Base;
                            }

                            Parametros.SetAmbiente(param.Ambiente);
                            Parametros.SetBase(param.Base);

                            OrganizaTelaEvent(2);
                            Job();

                            Thread Tproc = new Thread(process.LimpaLog);
                            Tproc.Start();
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

                Helper.CriarBancoSQLite();
                Helper.CriarTabelaSQlite();
                Log.CriarBancoSQLite();
                Log.CriarTabelaSQlite();

                OrganizaTelaEvent(1);
            }
            
        }

        private void BtnEnviar_Click(object sender, RoutedEventArgs e)
        {
            if(Parametros.GetDirToke() != null)
            {
                if ((Parametros.GetDirArq()) != null && Parametros.GetGrupo() != 0 && Parametros.GetToken() != null)
                {
                    this.Cursor = System.Windows.Input.Cursors.Wait;

                    Thread t = new Thread(Integra_Esocial);
                    t.Start();
                    t.Join();

                    this.Cursor = System.Windows.Input.Cursors.Arrow;

                    Job();
                }

                else
                {
                    System.Windows.Forms.MessageBox.Show("Você precisa selecionar um pasta de origem dos arquivos");
                }
            }
            else
                System.Windows.Forms.MessageBox.Show("Para enviar você precisa anexar um token valido");


        }

        private void BtnConsultar_Click(object sender, RoutedEventArgs e)
        {
            if(Parametros.GetDirToke() != null)
            {
                if ((Parametros.GetDirArq()) != null && (Parametros.GetDirFim()) != null && Parametros.GetGrupo() != 0 && Parametros.GetToken() != null)
                {
                    if ((Parametros.GetDirArq()) != (Parametros.GetDirFim()))
                    {
                        this.Cursor = System.Windows.Input.Cursors.Wait;

                        Thread t = new Thread(Consulta_Retorno);
                        t.Start();
                        t.Join();
                        this.Cursor = System.Windows.Input.Cursors.Arrow;

                        Job();
                    }

                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Os diretórios devem ser diferentes");
                    }
                }

                else if ((Parametros.GetDirArq()) == null)
                {
                    System.Windows.Forms.MessageBox.Show("Você precisa selecionar um pasta de origem dos arquivos");

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Você precisa selecionar um pasta de destino para os arquivos");
                }
            }
            else
                System.Windows.Forms.MessageBox.Show("Para consultar você precisa anexar um token valido");

        }

        private void BtnProcurarIni_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlgf = new FolderBrowserDialog();
            dlgf.ShowDialog();
            var s = dlgf.SelectedPath;

            if(s != "")
            {
                Parametros.SetDirArq(s);

                if (Parametros.GetDirArq() != null)
                {
                    txtFolderIni.Text = s;
                    Job();
                }
            }
            
        }

        private void BtnProcurarFim_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlgf = new FolderBrowserDialog();
            dlgf.ShowDialog();
            var s = dlgf.SelectedPath;

            if(s != "")
            {
                Parametros.SetDirFim(s);
                txtFolderFim.Text = s;
            }

            
        }

        private void BtnProcurarToken_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Arquivo de token";
            dlg.Filter = "Token|*.ives;...";
            dlg.ShowDialog();
            

            var s = dlg.SafeFileName;

            var b = s.Contains(".ives");

            if (b == true)
            {
                Parametros.SetDirToke(dlg.FileName);

                if (DefineToken(Parametros.GetDirToke()) == true)
                {
                    txtFolderToken.Text = dlg.FileName;
                }
            }
            else if (dlg.FileName != "" && b == false)
            {
                System.Windows.Forms.MessageBox.Show("Formato do arquivo não suportado");
                Parametros.SetDirToke(null);
                txtFolderToken.Text = "";
            }

        }

        private void Integra_Esocial()
        {
            var proc = new Processos();
            if(proc.VerificaProcessoRun() == false)
            {
                EnviaLote envLote = new EnviaLote();
                Integra.Job();
                envLote.Job();

                System.Windows.Forms.MessageBox.Show("Finalizado o processo de integração");
            }
            else
            {
                System.Windows.MessageBox.Show("O serviço está em execução");
            }
        }

        private void Consulta_Retorno()
        {
            
            var proc = new Processos();
            if (proc.VerificaProcessoRun() == false)
            {
                ConsultaLote consultLote = new ConsultaLote();
                Consulta.Job();
                consultLote.Job();

                System.Windows.Forms.MessageBox.Show("Finalizado o processo de consulta");
            }
            else
            {
                System.Windows.MessageBox.Show("O serviço está em execução");
            }

        }

        public void Contagem(DirectoryInfo dir)
        {
            var ltxt = new List<string>();
            var lxml = new List<string>();
            int i = 0; 
            int j = 0;
            int x = 0, y = 0;

            try
            {
                foreach (FileInfo file in dir.GetFiles())
                {
                    if (file.Extension == ".txt" || file.Extension == ".TXT")
                    {
                        if (file.Name != "logEnvio.log" && file.Name != "logConsulta.log" && file.Name.Contains("log_") == false)
                        {
                            ltxt.Add(file.Name);
                        }
                    }
                    if(file.Extension == ".xml" || file.Extension == ".XML")
                    {
                        if (file.Name != "logEnvio.log" && file.Name != "logConsulta.log" && file.Name.Contains("log_") == false)
                        {
                            lxml.Add(file.Name);
                        }
                    }
                }

                foreach (var item in ltxt)
                {
                    var ret = Helper.ExistsProtocolo(item);

                    if(ret == false)
                    {
                        i++;
                    }
                    if(ret == true)
                    {
                        j++;
                    }
                }

                foreach (var item in lxml)
                {
                    var ret = Helper.ExistsProtocolo(item);

                    if (ret == false)
                    {
                        x++;
                    }
                    if (ret == true)
                    {
                        y++;
                    }
                }
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Erro ao buscar arquivos na pasta selecionada");
            }

            LblqtdEnv.Content = i+x;
            LblqtdCons.Content = j+y;
        }

        public bool DefineToken(string dir)
        {

            try
            {
                string[] lines = System.IO.File.ReadAllLines(@dir);

                if (lines.Length == 2)
                {
                    Parametros.SetGrupo(Convert.ToInt32(lines[0]));
                    Parametros.SetToken(lines[1]);

                    return true;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Arquivo não suportado");
                    Parametros.SetDirToke(null);
                    txtFolderToken.Text = "";

                    return false;
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Arquivo de token não pode ser importado");
                Parametros.SetDirToke(null);
                txtFolderToken.Text = "";
                return false;
            }
            

        }

        private void Job()
        {
            Controle = 1;
            if(Parametros.GetDirArq() != null)
            {
                DirectoryInfo dir = new DirectoryInfo(Parametros.GetDirArq());

                Contagem(dir);
            }
            Thread.Sleep(100);
            Controle = 0;   
        }

        private void LblqtdEnv_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (Controle == 0)
            {
                Job();
            }

        }

        private void LblqtdCons_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (Controle == 0)
            {
                Job();
            }
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (Parametros.GetDirToke() != null)
            {
                if ((Parametros.GetDirArq()) != null && (Parametros.GetDirFim()) != null && Parametros.GetGrupo() != 0 && Parametros.GetToken() != null)
                {
                    if ((Parametros.GetDirArq()) != (Parametros.GetDirFim()))
                    {
                        if(CboAmbiente.SelectedIndex != -1)
                        {
                            Processos process = new Processos();
                            OrganizaTelaEvent(2);

                            try
                            {
                                TxbBase.Text = (string)CboBase.SelectedItem;
                                TxtbAmbiente.Text = (string)CboAmbiente.SelectedItem;

                                Parametros.SetAmbiente(Convert.ToString((int)CboAmbiente.SelectedIndex + 1));

                                Parametros.SetBase((string)CboBase.SelectedItem);

                                Helper.AddParametros(new Parametro { Id = 1, CaminhoDir = Parametros.GetDirArq(), CaminhoFim = Parametros.GetDirFim(), CaminhoToke = Parametros.GetDirToke(), Ambiente = Parametros.GetAmbiente(), Base = Parametros.GetBase()});

                                Job();
                            }
                            catch (Exception ex)
                            {
                                System.Windows.MessageBox.Show(ex.Message);
                            }

                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Selecione o tipo de ambiente");
                        }
                        
                    }

                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Os diretórios devem ser diferentes");
                    }
                }

                else if ((Parametros.GetDirArq()) == null)
                {
                    System.Windows.Forms.MessageBox.Show("Você precisa selecionar um pasta de origem dos arquivos");

                }
                else if ((Parametros.GetDirFim()) == null)
                {
                    System.Windows.Forms.MessageBox.Show("Você precisa selecionar um pasta de destino para os arquivos");
                }
                else
                    System.Windows.Forms.MessageBox.Show("Token anexado inválido!");
            }
            else
                System.Windows.Forms.MessageBox.Show("Para integrar e consultar você precisa anexar um token valido");
            

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

        private void OrganizaTelaEvent(int tipo)
        {
            if(tipo == 1)
            {
                BtnSalvar.Visibility = Visibility.Visible;
                LblSalvar.Visibility = Visibility.Visible;

                BtnProcurarIni.Visibility = Visibility.Visible;
                BtnProcurarFim.Visibility = Visibility.Visible;
                BtnProcurarToken.Visibility = Visibility.Visible;

                BtnConsultar.Visibility = Visibility.Hidden;
                BtnEnviar.Visibility = Visibility.Hidden;
                LblqtdCons.Visibility = Visibility.Hidden;
                LblqtdEnv.Visibility = Visibility.Hidden;
                LbltmEnv.Visibility = Visibility.Hidden;
                LbltmCons.Visibility = Visibility.Hidden;
                BtnParam.Visibility = Visibility.Hidden;
                BtnLog.Visibility = Visibility.Hidden;

                CboAmbiente.Visibility = Visibility.Visible;
                TxtbAmbiente.Visibility = Visibility.Hidden;
                CboBase.Visibility = Visibility.Visible;
                TxbBase.Visibility = Visibility.Hidden;
            }
            else if(tipo == 2)
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
                BtnProcurarFim.Visibility = Visibility.Hidden;
                BtnProcurarToken.Visibility = Visibility.Hidden;

                CboAmbiente.Visibility = Visibility.Hidden;
                TxtbAmbiente.Visibility = Visibility.Visible;
                CboBase.Visibility = Visibility.Hidden;
                TxbBase.Visibility = Visibility.Visible;
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

        private void CboAmbiente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list1 = new List<string>();

            if(CboAmbiente.SelectedIndex == 0)
            {
                list1.Add("Vertech Produção");
                CboBase.ItemsSource = list1;
                CboBase.SelectedIndex = 0;
            }
            else
            {
                list1.Add("Vertech Produção");
                list1.Add("Vertech Teste");
                CboBase.ItemsSource = list1;
                CboBase.SelectedIndex = 1;
            }
        }
    }
}
