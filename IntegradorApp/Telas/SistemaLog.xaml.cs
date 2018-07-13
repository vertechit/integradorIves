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

namespace IntegradorApp.Telas
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

            list.Add("");
            list.Add("Consulta");
            list.Add("Envia");
            list.Add("Exception");
            
            CboTipo.ItemsSource = list;

            var list1 = new List<string>();

            list1.Add("");

            CboCampos.ItemsSource = list1;
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            var proc = new Processos();


            string escolha = (string)CboTipo.SelectedItem;
            string campo = (string)CboCampos.SelectedItem;
            

            if(escolha == "Consulta")
            {
                if(campo == null || campo == "")
                {
                    DataTable dt = Log.GetLogs("logconsulta");// proc.ConvertToDataTable(logConsultDao.BuscaTodos());//DataTable dt = 
                    DtData.ItemsSource = dt.DefaultView;
                }
                else
                {
                    if(campo == "Identificador")
                    {
                        DataTable dt = Log.GetLogsWithParam(1, TxbValor.Text, "logconsulta"); // proc.ConvertToDataTable(logConsultDao.BuscaPorIdentificador(TxbValor.Text));//
                        DtData.ItemsSource = dt.DefaultView;
                    }
                    else
                    {
                        DataTable dt = Log.GetLogsWithParam(2, TxbValor.Text, "logconsulta"); //proc.ConvertToDataTable(logConsultDao.BuscaPorData(TxbValor.Text));//
                        DtData.ItemsSource = dt.DefaultView;
                    }
                }
            }
            else if(escolha == "Envia")
            {
                if (campo == null || campo == "")
                {
                    DataTable dt = Log.GetLogs("logenvia"); //proc.ConvertToDataTable(logEnvioDao.BuscaTodos());/
                    DtData.ItemsSource = dt.DefaultView;
                }
                else
                {
                    if (campo == "Identificador")
                    {
                        DataTable dt = Log.GetLogsWithParam(1, TxbValor.Text, "logenvia"); //proc.ConvertToDataTable(logEnvioDao.BuscaPorIdentificador(TxbValor.Text));//
                        DtData.ItemsSource = dt.DefaultView;
                    }
                    else
                    {
                        DataTable dt = Log.GetLogsWithParam(2, TxbValor.Text, "logenvia"); //proc.ConvertToDataTable(logEnvioDao.BuscaPorData(TxbValor.Text));//
                        DtData.ItemsSource = dt.DefaultView;
                    }
                }
               
            }
            else if (escolha == "Exception")
            {
                if (campo == null || campo == "")
                {
                    //DataTable dt = proc.ConvertToDataTable(logErroDao.BuscaTodos());//
                    DataTable dt = Log.GetLogs("logerro");
                    DtData.ItemsSource = dt.DefaultView;
                }
                else
                {
                    if (campo == "Serviço")
                    {
                        //DataTable dt = proc.ConvertToDataTable(logErroDao.BuscaPorServico(TxbValor.Text));//
                        DataTable dt = Log.GetLogsWithParam(1, TxbValor.Text, "logerro");Log.GetLogsWithParam(1, TxbValor.Text, "logerro");
                        DtData.ItemsSource = dt.DefaultView;
                    }
                    else
                    {
                        DataTable dt = Log.GetLogsWithParam(2, TxbValor.Text, "logerro"); //proc.ConvertToDataTable(logErroDao.BuscaPorData(TxbValor.Text));//
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
            else if (escolha == "Exception")
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
    }
}
