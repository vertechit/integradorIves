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
using Vertech.Uteis;
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

            Parametros.SetGrupo(1);
            Parametros.SetToken("8EE07DE66C97D8CFBAE04C47E8F51D76");

        }

        private void BtnTeste_Click(object sender, RoutedEventArgs e)
        {
            
            List<integraResponse> retorno = null;
            /*var thread = new Thread(
                () =>
                {
                    retorno = Envia_Esocial();
                });
            thread.Start();
            thread.Join();
            */
            retorno = Envia_Esocial();
            txtstate.Text = retorno[0].protocolo.ToString();

            Consulta consult = new Consulta();

            consult.ConsultaProtocolo(retorno);
            /*// manipulador de diretorios
            DirectoryInfo dirInfo = new DirectoryInfo(@txtFolderIni.Text);

            // procurar arquivos
            BuscaArquivos(dirInfo);
            */

        }

        private void BtnProcurarIni_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlgf = new FolderBrowserDialog();
            dlgf.ShowDialog();
            txtFolderIni.Text = dlgf.SelectedPath;

            Parametros.SetDirArq(txtFolderIni.Text);

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

        }

        private List<integraResponse> Envia_Esocial()
        {
            
            Integra Vertech = new Integra();

            List<integraResponse> Response = Vertech.Job();

            //txtstate.Text = Response.protocolo.ToString();

            return Response;
        }
    }
}
