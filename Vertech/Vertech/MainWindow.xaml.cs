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


namespace Vertech
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string nomeAbreviadoArquivo = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnTeste_Click(object sender, RoutedEventArgs e)
        {

            //Thread thread = new Thread();
            //thread.Start();
        }

        private void BtnProcurarIni_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlgf = new FolderBrowserDialog();
            dlgf.ShowDialog();
            txtFolderIni.Text = dlgf.SelectedPath;

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
    }
}
