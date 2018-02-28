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

        public MainWindow()
        {
            InitializeComponent();
            Init();
            Parametros.SetTipoApp("Client");
        }



        private void Init()
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
            catch(Exception)
            {

            }

            if(i > 0)
            {
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
                            Job();
                        }
                        
                    }

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
            }


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
            Integra.Job();

            System.Windows.Forms.MessageBox.Show("Finalizado o processo de integração, acesse a pasta de origem para verificar o log");
        }

        private void Consulta_Retorno()
        {
            Consulta.Job();

            System.Windows.Forms.MessageBox.Show("Finalizado o processo de consulta, acesse a pasta de origem para verificar o log");
        }

        public void Contagem(DirectoryInfo dir)
        {
            var ltxt = new List<string>();
            int i = 0; 
            int j = 0;

            try
            {
                foreach (FileInfo file in dir.GetFiles())
                {
                    if (file.Extension == ".txt")
                    {
                        if (file.Name != "logEnvio.txt" && file.Name != "logConsulta.txt" && file.Name.Contains("log_") == false)
                        {
                            ltxt.Add(file.Name);
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
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Erro ao buscar arquivos na pasta selecionada");
            }

            LblqtdEnv.Content = i;
            LblqtdCons.Content = j;
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
            if(Parametros.GetDirArq() != null)
            {
                DirectoryInfo dir = new DirectoryInfo(Parametros.GetDirArq());

                Contagem(dir);
            }
                        
        }

        private void LblqtdEnv_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Job();
        }

        private void LblqtdCons_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Job();
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (Parametros.GetDirToke() != null)
            {
                if ((Parametros.GetDirArq()) != null && (Parametros.GetDirFim()) != null && Parametros.GetGrupo() != 0 && Parametros.GetToken() != null)
                {
                    if ((Parametros.GetDirArq()) != (Parametros.GetDirFim()))
                    {
                        Processos process = new Processos();

                        BtnConsultar.Visibility = Visibility.Visible;
                        BtnEnviar.Visibility = Visibility.Visible;
                        LblqtdCons.Visibility = Visibility.Visible;
                        LblqtdEnv.Visibility = Visibility.Visible;
                        LbltmEnv.Visibility = Visibility.Visible;
                        LbltmCons.Visibility = Visibility.Visible;
                        BtnParam.Visibility = Visibility.Visible;

                        BtnSalvar.Visibility = Visibility.Hidden;
                        LblSalvar.Visibility = Visibility.Hidden;
                        BtnProcurarIni.Visibility = Visibility.Hidden;
                        BtnProcurarFim.Visibility = Visibility.Hidden;
                        BtnProcurarToken.Visibility = Visibility.Hidden;

                        try
                        {
                            /*string user = System.Windows.Forms.SystemInformation.UserName;

                            string[] lines = { Parametros.GetDirToke(), Parametros.GetDirArq(), Parametros.GetDirFim() };

                            System.IO.DirectoryInfo folderInfo = new System.IO.DirectoryInfo("C:\\");

                            DirectorySecurity ds = new DirectorySecurity();
                            ds.AddAccessRule(new FileSystemAccessRule(user, FileSystemRights.Modify, AccessControlType.Allow));
                            ds.SetAccessRuleProtection(false, false);
                            folderInfo.Create(ds);
                            folderInfo.CreateSubdirectory("vch");

                            var s = string.Concat(folderInfo, '\\', "vch", '\\', "Parametros.dat");

                            System.IO.File.WriteAllLines(@s, lines);*/

                            Helper.AddParametros(new Parametro { Id = 1, CaminhoDir = Parametros.GetDirArq(), CaminhoFim = Parametros.GetDirFim(), CaminhoToke = Parametros.GetDirToke() });

                            Job();
                        }
                        catch(Exception ex)
                        {
                            System.Windows.MessageBox.Show(ex.Message);
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
                else
                {
                    System.Windows.Forms.MessageBox.Show("Você precisa selecionar um pasta de destino para os arquivos");
                }
            }
            else
                System.Windows.Forms.MessageBox.Show("Para integrar e consultar você precisa anexar um token valido");
            

        }

        private void BtnParam_Click(object sender, RoutedEventArgs e)
        {
            Init();
        }
    }
}
