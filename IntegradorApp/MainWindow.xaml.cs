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
using IntegradorCore.NHibernate;
using IntegradorCore.NHibernate.DAO;
using IntegradorCore;
using NHibernate;
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

        private void BtnReIntegrar_Click(object sender, RoutedEventArgs e)
        {
            var proc = new Processos();
            if (proc.VerificaProcessoRun() == false)
            {
                var Tela = new Telas.ReIntegrar(this);
                Tela.Show();
                this.Hide();
            }
            else
            {
                System.Windows.MessageBox.Show("O serviço está em execução");
            }
        }
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

        private void BtnHabilitaLog_Click(object sender, RoutedEventArgs e)
        {
            StaticParametros.SetGeraLogs(!StaticParametros.GetGeraLogs());
            BtnHabilitaLog.Content = StaticParametros.GetGeraLogs() ? "Deabilitar Logs" : "Habilitar Logs";

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

            if(proc.ReadPermissionFolder() == false || proc.WritePermissionFolder() == false)
            {
                System.Windows.Forms.MessageBox.Show("Ops, você não tem permissão para leitura ou escrita na pasta c:/vch", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (proc.WritePermissionFile() == false || proc.ReadPermissionFile() == false)
            {
                System.Windows.Forms.MessageBox.Show("Ops, você não tem permissão para leitura ou escrita no arquivo dados.db | c:/vch/dados.db", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var sessao = AuxiliarNhibernate.AbrirSessao();
            var parametroDAO = new ParametroDAO(sessao);
            var parametroDBDAO = new ParametroDB_DAO(sessao);

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
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("Ops! Parece que a pasta de origem foi movida");
                                txtFolderIni.Text = "";
                                StaticParametros.SetDirOrigem(null);
                            }

                        }

                        if (StaticParametersDB.GetDriver() != null)
                        {
                            ctrl++;

                            StaticParametros.SetIntegraBanco(true);
                        }

                        if (ctrl >= 2)
                        {

                            var newParam = new Parametro { Id = 1, CaminhoDir = StaticParametros.GetDirOrigem(), CaminhoToke = StaticParametros.GetDirToke(), IntegraBanco = StaticParametros.GetIntegraBanco(), GeraLog = StaticParametros.GetGeraLogs(), UrlProd = StaticParametros.GetUrlProd(), UrlQa = StaticParametros.GetUrlQa(), UrlTeste = StaticParametros.GetUrlTeste() };
                            //var newParam = new Parametro { Id = 1, CaminhoDir = StaticParametros.GetDirOrigem(), CaminhoToke = StaticParametros.GetDirToke(), IntegraBanco = StaticParametros.GetIntegraBanco(), GeraLog = StaticParametros.GetGeraLogs()};
                            parametroDAO.Salvar(newParam);

                            if(StaticParametersDB.GetDriver() != null)
                            {
                                TxtStatusBanco.Text = "Conectado";
                            }
                            else
                            {
                                TxtStatusBanco.Text = "Desconectado";
                            }
                            //Armazenamento.AddParametros(new Parametro { Id = 1, CaminhoDir = StaticParametros.GetDirOrigem(), CaminhoToke = StaticParametros.GetDirToke(), IntegraBanco = StaticParametros.GetIntegraBanco() });
                            OrganizaTelaEvent(2);

                            if(StaticParametros.GetDirOrigem() != null && StaticParametros.GetDirOrigem() != "")
                                process.CriarPastas();
                        }
                        else
                        {
                            TxtStatusBanco.Text = "Desconectado";
                            System.Windows.MessageBox.Show("É necessário definir um diretorio ou configurar uma conexão com banco de dados para continuar");
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

            sessao.Close();
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
                ActionIntegra();
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
                ActionConsulta();
                this.Cursor = System.Windows.Input.Cursors.Arrow;
            }
            else
            {
                System.Windows.MessageBox.Show("O serviço está em execução");
            }
        }

        private void BtnConectarBanco_Click(object sender, RoutedEventArgs e)
        {
            var proc = new Processos();

            if (proc.ReadPermissionFile() == false || proc.WritePermissionFile() == false)
            {
                System.Windows.Forms.MessageBox.Show("Ops, você não tem permissão para leitura ou escrita no arquivo dados.db | c:/vch/dados.db", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Hide();
            var janela = new Telas.ParametrosBanco(this);
            janela.Show();
        }

        private void ReportBug_Click(object sender, RoutedEventArgs e)
        {
            string target = "https://github.com/vertechit/integradorIves/issues";

            try
            {
                System.Diagnostics.Process.Start(target);
            }
            catch
                (
                 System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    System.Windows.MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                System.Windows.MessageBox.Show(other.Message);
            }

        }
        #endregion

        #region Functions

        public void Init()
        {
            Processos process = new Processos();
            DirectoryInfo dir = new DirectoryInfo(@"C:\\vch");
            FileInfo fil = new FileInfo(@"C:\\vch\\dados.db");
            int ctrlFirstExec = 0;

            if (dir.Exists != true)
            {
                
                ctrlFirstExec = 1;
                string user = System.Windows.Forms.SystemInformation.UserName;
                System.IO.DirectoryInfo folderInfo = new System.IO.DirectoryInfo("C:\\");

                DirectorySecurity ds = new DirectorySecurity();
                ds.AddAccessRule(new FileSystemAccessRule(user, FileSystemRights.Modify, AccessControlType.Allow));
                ds.SetAccessRuleProtection(false, false);
                folderInfo.Create(ds);
                folderInfo.CreateSubdirectory("vch");

                OrganizaTelaEvent(1);
            }

            if (fil.Exists != true)
            {
                try
                {
                    AuxiliarNhibernate.AbrirSessao();
                    //fil.Create();
                }
                catch(Exception e)
                {
                    //System.Windows.Forms.MessageBox.Show(e.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }

            if(process.ReadPermissionFolder() == false || process.ReadPermissionFile() == false)
            {
                OrganizaTelaEvent(1);
                return;
            }

            var sessao = AuxiliarNhibernate.AbrirSessao();

            Thread t = new Thread(process.VerificaParaAtualizar);
            t.Name = "UpdaterWorker";
            t.Start();

            if (ctrlFirstExec == 0)
            {
                var parametroDAO = new ParametroDAO(sessao);
                var param = parametroDAO.BuscarPorID(1);//Armazenamento.GetParametros();

                int ctrl = 0;
                int ctrlVazio = 0;
                try
                {
                    StaticParametros.SetGeraLogs(param.GeraLog);

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

                    }

                }
                catch (Exception ex)
                {
                    //OrganizaTelaEvent(1);
                    ctrlVazio = 1;
                }

                var parametroDBDAO = new ParametroDB_DAO(sessao);
                var paramDB = parametroDBDAO.BuscarTodos();//parametroDBDAO.BuscarPorID(1);
                //var paramDB = Armazenamento.GetParametrosDB();

                try
                {
                    if(paramDB.Count == 1)
                    {
                        StaticParametersDB.SetListBanco(paramDB[0]);

                        if (paramDB[0].Grupo == 0 || paramDB[0].Token == null || paramDB[0].Token == "")
                        {
                            throw new Exception();
                        }
                        else
                        {
                            StaticParametersDB.Setcurrent(paramDB[0].Id);
                        }

                    }
                    else if(paramDB.Count > 1)
                    {

                        foreach(var p in paramDB)
                        {
                            StaticParametersDB.SetListBanco(p);
                        }

                        foreach (var p in paramDB)
                        {
                            if (p.Grupo == 0 || p.Token == null || p.Token == "")
                            {
                                throw new Exception();
                            }
                        }

                        StaticParametersDB.Setcurrent(paramDB[0].Id);
                    }
                    else
                    {
                        throw new Exception();
                    }
                    StaticParametros.SetIntegraBanco(true);
                    TxtStatusBanco.Text = "Conectado";
                    ctrl++;
                }
                catch (Exception ex)
                {
                    StaticParametros.SetIntegraBanco(false);
                    TxtStatusBanco.Text = "Desconectado";
                    if (ctrlVazio == 0)
                    {
                        var paramn = new Parametro { Id = 1, CaminhoDir = StaticParametros.GetDirOrigem(), CaminhoToke = StaticParametros.GetDirToke(), IntegraBanco = StaticParametros.GetIntegraBanco(), GeraLog = StaticParametros.GetGeraLogs() };
                        parametroDAO.Salvar(param);

                        //Armazenamento.UpdateParametros(new Parametro { Id = 1, CaminhoDir = param.CaminhoDir, CaminhoToke = param.CaminhoToke, IntegraBanco = false });
                    }
                }

                if (ctrl >= 2)
                {
                    try
                    {
                        if (txtFolderToken.Text == "")
                        {
                            OrganizaTelaEvent(1);
                        }
                        else
                        {
                            OrganizaTelaEvent(2);
                        }
                        //Job();
                        if (StaticParametros.GetDirOrigem() != null && StaticParametros.GetDirOrigem() != "")
                        {
                            process.CriarPastas();
                        }
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

            sessao.Close();

            if(process.WritePermissionFolder() == false || process.WritePermissionFile() == false)
            {
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

        private void ActionIntegra()
        {
            if(StaticParametros.GetLockVariavel() == false)
            {
                if (StaticParametros.GetIntegraBanco() == true)
                {
                    foreach(var p in StaticParametersDB.getAllListBanco())
                    {
                        StaticParametersDB.Setcurrent(p.Id);
                        IntegraDB();
                    }
                }

                if (StaticParametros.GetDirOrigem() != null && StaticParametros.GetDirOrigem() != "")
                {
                    IntegraArquivos(1);
                }

                System.Windows.Forms.MessageBox.Show("Processo de integração concluido!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Por favor, aguarde alguns instantes e tente novamente", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ActionConsulta()
        {
            if (StaticParametros.GetLockVariavel() == false)
            {
                if (StaticParametros.GetIntegraBanco() == true)
                {
                    foreach (var p in StaticParametersDB.getAllListBanco())
                    {
                        StaticParametersDB.Setcurrent(p.Id);
                        ConsultaDB();
                    }
                }

                if (StaticParametros.GetDirOrigem() != null && StaticParametros.GetDirOrigem() != "")
                {
                    ConsultaArquivos(1);
                }

                System.Windows.Forms.MessageBox.Show("Processo de consulta concluido!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Por favor, aguarde alguns instantes e tente novamente", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void IntegraDB()
        {
            Jobs Job = new Jobs();
            Job.EnviaDB();
        }

        private void ConsultaDB()
        {
            Jobs Job = new Jobs();
            Job.ConsultaDB();
            Job.UpdateDB();
        }

        private void IntegraArquivos(int i)
        {
            if(i > 4)
            {
                //System.Windows.MessageBox.Show("Processo de integração concluido!");
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
                    IntegraArquivos(i + 1);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Parametros não foram encontrados");
                OrganizaTelaEvent(1);
            }
        }

        private void ConsultaArquivos(int i)
        {
            if (i > 4)
            {
                //System.Windows.MessageBox.Show("Processo de consulta concluido!");
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
                    ConsultaArquivos(i + 1);
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
            LblVersao.Content = string.Concat("v",StaticParametros.GetVersao());

            BtnHabilitaLog.Content = StaticParametros.GetGeraLogs() ? "Deabilitar Logs" : "Habilitar Logs";

            if (tipo == 1)
            {
                BtnReIntegrar.Visibility = Visibility.Hidden;
                BtnSalvar.Visibility = Visibility.Visible;
                LblSalvar.Visibility = Visibility.Visible;

                BtnProcurarIni.Visibility = Visibility.Visible;
                BtnProcurarToken.Visibility = Visibility.Visible;

                BtnConsultar.Visibility = Visibility.Hidden;
                BtnEnviar.Visibility = Visibility.Hidden;
                BtnParam.Visibility = Visibility.Hidden;
                BtnLog.Visibility = Visibility.Hidden;
                BtnConectarBanco.Visibility = Visibility.Visible;
                BtnHabilitaLog.Visibility = Visibility.Visible;
            }
            else if (tipo == 2)
            {
                BtnReIntegrar.Visibility = Visibility.Visible;
                BtnConsultar.Visibility = Visibility.Visible;
                BtnEnviar.Visibility = Visibility.Visible;
                BtnParam.Visibility = Visibility.Visible;
                BtnLog.Visibility = Visibility.Visible;

                BtnSalvar.Visibility = Visibility.Hidden;
                LblSalvar.Visibility = Visibility.Hidden;
                BtnProcurarIni.Visibility = Visibility.Hidden;
                BtnProcurarToken.Visibility = Visibility.Hidden;
                BtnConectarBanco.Visibility = Visibility.Hidden;
                BtnHabilitaLog.Visibility = Visibility.Hidden;
            }
        }

        #endregion

        
    }
}
