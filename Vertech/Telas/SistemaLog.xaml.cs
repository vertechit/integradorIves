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
        public SistemaLog()
        {
            InitializeComponent();

            var list = new List<string>();

            list.Add("Consulta");
            list.Add("Envia");
            list.Add("Exception");

            CboTipo.ItemsSource = list;
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string escolha = (string)CboTipo.SelectedItem;

            if(escolha == "Consulta")
            {
                DataTable dt = Log.GetLogsConsulta();
                DtData.ItemsSource = dt.DefaultView;
            }
            else if(escolha == "Envia")
            {
                DataTable dt = Log.GetLogsEnvia();
                DtData.ItemsSource = dt.DefaultView;
            }
            else if (escolha == "Exception")
            {
                DataTable dt = Log.GetLogsErros();
                DtData.ItemsSource = dt.DefaultView;
            }
        }
    }
}
