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
            init();
            Parametros.SetTipoApp("Client");
        }



        private void init()
        {
            Processos process = new Processos();

            DirectoryInfo dir = new DirectoryInfo(@"C:\\vch");
            int i = 0;
            foreach (var item in dir.GetFiles())
            {
                i++;
            }

            if(i > 0)
            {
                var s = process.LerArquivo("C:\\vch", "Parametros.dat");

                if (s.Length == 3)
                {
                    if (s[0].Contains(".file") && s[0] != "")
                    {
                        Parametros.SetDirToke(s[0]);
                        txtFolderToken.Text = s[0];
                        DefineToken(s[0]);
                    }

                    if (s[1] != "" && s[2] != "" && s[1] != s[2])
                    {
                        txtFolderIni.Text = s[1];
                        txtFolderFim.Text = s[2];
                        Parametros.SetDirArq(s[1]);
                        Parametros.SetDirFim(s[2]);
                    }

                }
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
                if ((Parametros.GetDirArq()) != null && Parametros.GetGrupo() != null && Parametros.GetToken() != null)
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
                if ((Parametros.GetDirArq()) != null && (Parametros.GetDirFim()) != null && Parametros.GetGrupo() != null && Parametros.GetToken() != null)
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
            dlg.ShowDialog();

            var s = dlg.SafeFileName;

            var b = s.Contains(".file");

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
            int i = 0;
            int j = 0;
            List<string> ldat = new List<string>();
            List<string> ltxt = new List<string>();
            try
            {
                foreach (FileInfo file in dir.GetFiles())
                {
                    if (file.Extension == ".txt")
                    {
                        if (file.Name != "logEnvio.txt" && file.Name != "logConsulta.txt" && file.Name.Contains("log_") == false)
                        {
                            i++;
                            ltxt.Add(file.Name);
                        }
                    }
                    if(file.Extension == ".dat")
                    {
                        j++;
                        ldat.Add(file.Name);
                    }
                }

                foreach (var item in ldat)
                {
                    int n = item.Length;
                    string name = item.Remove(n - 3, 3);

                    int m = name.Length;
                    name = name.Remove(j - j, 5);
                    name = string.Concat(name, "txt");

                    foreach (var txt in ltxt)
                    {
                        if(name == txt)
                        {
                            i--;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erro ao buscar arquivos na pasta selecionada");
            }

            LblqtdEnv.Content = i;
            LblqtdCons.Content = j;
        }

        public bool DefineToken(string dir)
        {
            string [] lines = System.IO.File.ReadAllLines(@dir);

            try
            {
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
                System.Windows.Forms.MessageBox.Show("Infomações inválidas");
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
                if ((Parametros.GetDirArq()) != null && (Parametros.GetDirFim()) != null && Parametros.GetGrupo() != null && Parametros.GetToken() != null)
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
                            string user = System.Windows.Forms.SystemInformation.UserName;

                            string[] lines = { Parametros.GetDirToke(), Parametros.GetDirArq(), Parametros.GetDirFim() };

                            System.IO.DirectoryInfo folderInfo = new System.IO.DirectoryInfo("C:\\");

                            DirectorySecurity ds = new DirectorySecurity();
                            ds.AddAccessRule(new FileSystemAccessRule(user, FileSystemRights.Modify, AccessControlType.Allow));
                            ds.SetAccessRuleProtection(false, false);
                            folderInfo.Create(ds);
                            folderInfo.CreateSubdirectory("vch");

                            var s = string.Concat(folderInfo, '\\', "vch", '\\', "Parametros.dat");

                            System.IO.File.WriteAllLines(@s, lines);
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
            init();
        }
    }
}
