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
using Vertech.DAO;
using Vertech.Modelos;

namespace Vertech.Telas
{
    /// <summary>
    /// Lógica interna para SistemaLog.xaml
    /// </summary>
    public partial class SistemaLog : Window
    {
        MainWindow window;

        public SistemaLog(MainWindow current)
        {
            InitializeComponent();

            init();

            window = current;
        }

        public void init()
        {

            var list = new List<string>();

            list.Add("Consulta");
            list.Add("Envia");
            list.Add("Exception");
            list.Add("");

            CboTipo.ItemsSource = list;
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string escolha = (string)CboTipo.SelectedItem;
            string campo = (string)CboCampos.SelectedItem;
            

            if(escolha == "Consulta")
            {
                if(campo == null || campo == "")
                {
                    DataTable dt = Log.GetLogsConsulta();
                    DtData.ItemsSource = dt.DefaultView;
                }
                else
                {
                    if(campo == "Nome Arquivo")
                    {
                        DataTable dt = Log.GetLogsWithParam(1, TxbValor.Text, "Consulta");
                        DtData.ItemsSource = dt.DefaultView;
                    }
                    else
                    {
                        DataTable dt = Log.GetLogsWithParam(2, TxbValor.Text, "Consulta");
                        DtData.ItemsSource = dt.DefaultView;
                    }
                }
            }
            else if(escolha == "Envia")
            {
                if (campo == null || campo == "")
                {
                    DataTable dt = Log.GetLogsEnvia();
                    DtData.ItemsSource = dt.DefaultView;
                }
                else
                {
                    if (campo == "Nome Arquivo")
                    {
                        DataTable dt = Log.GetLogsWithParam(1, TxbValor.Text, "Envia");
                        DtData.ItemsSource = dt.DefaultView;
                    }
                    else
                    {
                        DataTable dt = Log.GetLogsWithParam(2, TxbValor.Text, "Envia");
                        DtData.ItemsSource = dt.DefaultView;
                    }
                }
               
            }
            else if (escolha == "Exception")
            {
                if (campo == null || campo == "")
                {
                    DataTable dt = Log.GetLogsErros();
                    DtData.ItemsSource = dt.DefaultView;
                }
                else
                {
                    if (campo == "Serviço")
                    {
                        DataTable dt = Log.GetLogsWithParam(1, TxbValor.Text, "Erro");
                        DtData.ItemsSource = dt.DefaultView;
                    }
                    else
                    {
                        DataTable dt = Log.GetLogsWithParam(2, TxbValor.Text, "Erro");
                        DtData.ItemsSource = dt.DefaultView;
                    }
                }
            }
            else if (escolha == "")
            {
                DtData.ItemsSource = null;
            }
            else
            {
                MessageBox.Show("Selecione uma opção para realizar a busca");
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

                list.Add("Nome Arquivo");
                list.Add("Data");
                list.Add("");

                CboCampos.ItemsSource = list;
            }
            else if (escolha == "Envia")
            {
                var list = new List<string>();

                list.Add("Nome Arquivo");
                list.Add("Data");
                list.Add("");

                CboCampos.ItemsSource = list;
            }
            else if (escolha == "Exception")
            {
                var list = new List<string>();

                list.Add("Serviço");
                list.Add("Data");
                list.Add("");

                CboCampos.ItemsSource = list;
            }
        }
    }
}
