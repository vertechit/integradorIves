using System;
using System.Collections.Generic;
using System.Data;
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
using IntegradorCore.DAO;
using IntegradorCore.Modelos;
using NHibernate;
using IntegradorCore.NHibernate;
using IntegradorCore.Services;
using IntegradorCore.NHibernate.DAO;
using System.ComponentModel;
using System.IO;
using System.Threading;

namespace IntegradorApp.Telas
{
    /// <summary>
    /// Lógica interna para SistemaLog.xaml
    /// </summary>
    public partial class SistemaLog : Window
    {
        MainWindow window;
        int ctrl = 0;

        public SistemaLog(MainWindow current)
        {
            InitializeComponent();

            init();

            window = current;
        }

        public void init()
        {
            
            var list = new List<string>();

            list.Add("");
            list.Add("Consulta");
            list.Add("Envia");
            list.Add("Erros");
            
            CboTipo.ItemsSource = list;

            var list1 = new List<string>();

            list1.Add("");

            CboCampos.ItemsSource = list1;
            CboCampos.SelectedIndex = -1;
            CboTipo.SelectedIndex = -1;
            TxbValor.Text = "";

            DtData.ItemsSource = null;
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            var sessao = AuxiliarNhibernate.AbrirSessao();
            var logConsultDao = new LogConsultaDAO(sessao);
            var logEnvioDao = new LogEnvioDAO(sessao);
            var logErroDao = new LogErroDAO(sessao);
            var proc = new Processos();


            string escolha = (string)CboTipo.SelectedItem;
            string campo = (string)CboCampos.SelectedItem;
            

            if(escolha == "Consulta")
            {
                if(campo == null || campo == "")
                {
                    DataTable dt = proc.ConvertToDataTable(logConsultDao.BuscaTodos());
                    DtData.ItemsSource = dt.DefaultView;
                }
                else
                {
                    if(campo == "Identificador")
                    {
                        DataTable dt = proc.ConvertToDataTable(logConsultDao.BuscaPorIdentificador(TxbValor.Text));
                        DtData.ItemsSource = dt.DefaultView;
                    }
                    else
                    {
                        DataTable dt = proc.ConvertToDataTable(logConsultDao.BuscaPorData(TxbValor.Text));
                        DtData.ItemsSource = dt.DefaultView;
                    }
                }
            }
            else if(escolha == "Envia")
            {
                if (campo == null || campo == "")
                {
                    DataTable dt = proc.ConvertToDataTable(logEnvioDao.BuscaTodos());
                    DtData.ItemsSource = dt.DefaultView;
                }
                else
                {
                    if (campo == "Identificador")
                    {
                        DataTable dt = proc.ConvertToDataTable(logEnvioDao.BuscaPorIdentificador(TxbValor.Text));
                        DtData.ItemsSource = dt.DefaultView;
                    }
                    else
                    {
                        DataTable dt = proc.ConvertToDataTable(logEnvioDao.BuscaPorData(TxbValor.Text));
                        DtData.ItemsSource = dt.DefaultView;
                    }
                }
               
            }
            else if (escolha == "Erros")
            {
                if (campo == null || campo == "")
                {
                    DataTable dt = proc.ConvertToDataTable(logErroDao.BuscaTodos());
                    DtData.ItemsSource = dt.DefaultView;
                }
                else
                {
                    if (campo == "Serviço")
                    {
                        DataTable dt = proc.ConvertToDataTable(logErroDao.BuscaPorServico(TxbValor.Text));
                        DtData.ItemsSource = dt.DefaultView;
                    }
                    else
                    {
                        DataTable dt = proc.ConvertToDataTable(logErroDao.BuscaPorData(TxbValor.Text));
                        DtData.ItemsSource = dt.DefaultView;
                    }
                }
            }
            else if (escolha == "")
            {
                DtData.ItemsSource = null;
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            window.Show();
        }

        private void CboTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string escolha = (string)CboTipo.SelectedItem;

            if (escolha == "Consulta")
            {
                var list = new List<string>();

                list.Add("");
                list.Add("Identificador");
                list.Add("Data");
                
                CboCampos.ItemsSource = list;
            }
            else if (escolha == "Envia")
            {
                var list = new List<string>();

                list.Add("");
                list.Add("Identificador");
                list.Add("Data");
                
                CboCampos.ItemsSource = list;
            }
            else if (escolha == "Erros")
            {
                var list = new List<string>();

                list.Add("");
                list.Add("Serviço");
                list.Add("Data");
                
                CboCampos.ItemsSource = list;
            }
            else
            {
                var list = new List<string>();

                list.Add("");

                CboCampos.ItemsSource = list;
            }
        }

        private void BtnLimpa_Click(object sender, RoutedEventArgs e)
        {
            init();
        }

        private void BtnDump_Click(object sender, RoutedEventArgs e)
        {
            if(ctrl == 0)
            {
                Thread t = new Thread(ExportLog);
                t.Name = "Exporting";
                t.Start();
            }
            else
            {
                MessageBox.Show("Processo em andamento", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        private void ExportLog()
        {
            ctrl = 1;
            try
            {
                var sessao = AuxiliarNhibernate.AbrirSessao();
                var logErroDao = new LogErroDAO(sessao);
                var logInterno = new LogInternoDAO(sessao);
                var proc = new Processos();

                var lt1 = logErroDao.BuscaTodos();
                var lt2 = logInterno.BuscaTodos();

                DirectoryInfo dir = new DirectoryInfo(@"c:\\vch\\log");
                FileInfo fi1 = new FileInfo(@"c:\\vch\\log\\logErro.log");
                FileInfo fi2 = new FileInfo(@"c:\\vch\\log\\logInterno.log");

                if (dir.Exists != true)
                {
                    dir.Create();
                }
                if (fi1.Exists == true)
                {
                    fi1.Delete();
                }
                if (fi2.Exists == true)
                {
                    fi2.Delete();
                }


                StreamWriter vWriter = new StreamWriter(fi1.FullName, true);
                foreach (var item in lt1)
                {

                    vWriter.WriteLine("Id: " + item.Id);
                    vWriter.WriteLine("Serviço: " + item.Servico);
                    vWriter.WriteLine("Cod erro: " + item.CodErro);
                    vWriter.WriteLine("Mensagem: " + item.Msg);
                    vWriter.WriteLine("Ação: " + item.Acao);
                    vWriter.WriteLine("Data: " + item.Data);
                    vWriter.WriteLine("Hora: " + item.Hora);
                    vWriter.WriteLine("");
                    vWriter.WriteLine("--------------------------------------------------");

                }
                vWriter.Flush();
                vWriter.Close();

                StreamWriter vWriter2 = new StreamWriter(fi2.FullName, true);
                foreach (var item in lt2)
                {

                    vWriter2.WriteLine("Id: " + item.Id);
                    vWriter2.WriteLine("Serviço: " + item.Servico);
                    vWriter2.WriteLine("Cod erro: " + item.CodErro);
                    vWriter2.WriteLine("Mensagem: " + item.Mensagem);
                    vWriter2.WriteLine("InnerException: " + item.InnerException);
                    vWriter2.WriteLine("Stack: " + item.StackTrace);
                    vWriter2.WriteLine("Source: " + item.Source);
                    vWriter2.WriteLine("Custom EndPoint: " + item.Base);
                    vWriter2.WriteLine("Ambiente: " + item.Ambiente);
                    vWriter2.WriteLine("Identificação: " + item.Identificacao);
                    vWriter2.WriteLine("XML: " + item.Xml);
                    vWriter2.WriteLine("SQL: " + item.SQL);
                    vWriter2.WriteLine("Data: " + item.Data);
                    vWriter2.WriteLine("");
                    vWriter2.WriteLine("--------------------------------------------------");

                }
                vWriter2.Flush();
                vWriter2.Close();


                var value = MessageBox.Show("Deseja abrir a pasta de logs?", "Sucesso", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (value == MessageBoxResult.Yes)
                {
                    string argument = @"/select, C:\vch\log\";
                    System.Diagnostics.Process.Start("explorer.exe", argument);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ctrl = 0;
        }
    }
}
