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
//using Vertech.EsoConsulta;


namespace Vertech
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        //private static string nomeAbreviadoArquivo = "";

        public MainWindow()
        {
            InitializeComponent();

            //Parametros.SetGrupo(1);
            //Parametros.SetToken("8EE07DE66C97D8CFBAE04C47E8F51D76");

        }

        private void BtnEnviar_Click(object sender, RoutedEventArgs e)
        {
            if ((Parametros.GetDirArq()) != null && Parametros.GetGrupo() != null && Parametros.GetToken() != null)
            {
                Thread t = new Thread(Envia_Esocial);
                t.Start();

                DirectoryInfo dir = new DirectoryInfo(Parametros.GetDirArq());

                Contagem(dir);
            }

            else
            {
                System.Windows.Forms.MessageBox.Show("Você precisa selecionar um pasta de origem dos arquivos");
            }

            
        }

        private void BtnConsultar_Click(object sender, RoutedEventArgs e)
        {
            if ((Parametros.GetDirArq()) != null && (Parametros.GetDirFim()) != null && Parametros.GetGrupo() != null && Parametros.GetToken() != null)
            {
                if ((Parametros.GetDirArq()) != (Parametros.GetDirFim()))
                {
                    Thread t = new Thread(Consulta_Retorno);
                    t.Start();

                    DirectoryInfo dir = new DirectoryInfo(Parametros.GetDirArq());

                    Contagem(dir);
                }
                //Consulta_Retorno();
                else
                {
                    System.Windows.Forms.MessageBox.Show("Os diretórios devem ser diferentes");
                }
            }

            else if((Parametros.GetDirArq()) == null )
            {
                System.Windows.Forms.MessageBox.Show("Você precisa selecionar um pasta de origem dos arquivos");

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Você precisa selecionar um pasta de destino para os arquivos");
            }

            
        }

        private void BtnProcurarIni_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlgf = new FolderBrowserDialog();
            dlgf.ShowDialog();
            txtFolderIni.Text = dlgf.SelectedPath;

            Parametros.SetDirArq(txtFolderIni.Text);

            DirectoryInfo dir = new DirectoryInfo(Parametros.GetDirArq());

            Contagem(dir);
            /*OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Envio de Arquivo - Cliente";
            dlg.ShowDialog();
            txtArquivo.Text = dlg.FileName;
            nomeAbreviadoArquivo = dlg.SafeFileName;*/
        }

        private void BtnProcurarFim_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlgf = new FolderBrowserDialog();
            dlgf.ShowDialog();
            txtFolderFim.Text = dlgf.SelectedPath;

            Parametros.SetDirFim(txtFolderFim.Text);
        }

        private void Envia_Esocial()
        {
            Integra Vertech = new Integra();

            Vertech.Job();
        }

        private void Consulta_Retorno()
        {
            Consulta Vertech = new Consulta();

            Vertech.Job();
        }

        private void Contagem(DirectoryInfo dir)
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
                        i++;
                        ltxt.Add(file.Name);
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

        private void DefineToken(string dir)
        {
            string [] lines = System.IO.File.ReadAllLines(@dir);

            Parametros.SetGrupo(Convert.ToInt32(lines[0]));
            Parametros.SetToken(lines[1]);
        }

        private void BtnProcurarToken_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Arquivo de token";
            dlg.ShowDialog();
            txtFolderToken.Text = dlg.FileName;

            Parametros.SetDirToke(txtFolderToken.Text);

            DefineToken(Parametros.GetDirToke());
        }
    }
}
