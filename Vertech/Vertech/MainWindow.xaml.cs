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

        }

        private void BtnEnviar_Click(object sender, RoutedEventArgs e)
        {
            if(Parametros.GetDirToke() != null)
            {
                if ((Parametros.GetDirArq()) != null && Parametros.GetGrupo() != null && Parametros.GetToken() != null)
                {
                    Thread t = new Thread(Integra_Esocial);
                    t.Start();
                    //t.Join();

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
                        Thread t = new Thread(Consulta_Retorno);
                        t.Start();
                        //t.Join();

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
            txtFolderIni.Text = dlgf.SelectedPath;

            if(txtFolderIni.Text != "")
            {
                Parametros.SetDirArq(txtFolderIni.Text);

                if (Parametros.GetDirArq() != null)
                {

                    Job();

                }
            }
            
        }

        private void BtnProcurarFim_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlgf = new FolderBrowserDialog();
            dlgf.ShowDialog();
            txtFolderFim.Text = dlgf.SelectedPath;

            if(txtFolderFim.Text != "")
            {
                Parametros.SetDirFim(txtFolderFim.Text);
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

            System.Windows.Forms.MessageBox.Show("Finalizado o processo de integração, foi criado um arquivo de log na pasta de origem");
        }

        private void Consulta_Retorno()
        {
            Consulta.Job();

            System.Windows.Forms.MessageBox.Show("Finalizado o processo de consulta, foi criado um arquivo de log na pasta de origem");
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
                        if (file.Name != "logEnvio.txt" && file.Name != "logConsulta.txt")
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
                System.Windows.Forms.MessageBox.Show("Erro ao buscar arquivos na pasta indicada");
            }

            LblqtdEnv.Content = i;
            LblqtdCons.Content = j;
        }

        private bool DefineToken(string dir)
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
    }
}
