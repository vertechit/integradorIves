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
    }
}
