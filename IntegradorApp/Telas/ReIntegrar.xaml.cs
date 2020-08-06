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
using IntegradorCore.NHibernate;
using IntegradorCore.Services;
using IntegradorCore.NHibernate.DAO;
using IntegradorCore.Modelos;

namespace IntegradorApp.Telas
{
    /// <summary>
    /// Interação lógica para ReIntegrar.xam
    /// </summary>
    public partial class ReIntegrar : Window
    {
        MainWindow window;
        public ReIntegrar(MainWindow current)
        {
            InitializeComponent();
            init();
            window = current;
        }

        private void init()
        {
            
        }

        private void search()
        {
            var sessao = AuxiliarNhibernate.AbrirSessao();
            ProtocoloDB_DAO ProtocoloDAO = new ProtocoloDB_DAO(sessao);
            var protocolos = ProtocoloDAO.BuscaPorData(datePicker.SelectedDate.ToString().Split(' ')[0]);
            if(protocolos.Count > 0)
            {
                var result = MessageBox.Show("Deseja colocar os eventos da data selecionada para serem reconsultados? Total de "+ protocolos.Count, "Confirmação", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    foreach(var protocolo in protocolos)
                    {
                        var procotolodb = new ProtocoloDB();
                        procotolodb.id = protocolo.id;
                        procotolodb.consultado = false;
                        procotolodb.salvoDB = false;
                        procotolodb.erros = null;
                        procotolodb.dtconsulta = null;
                        procotolodb.hrconsulta = null;
                        procotolodb.nroRec = null;
                        procotolodb.xmlRec = null;
                        procotolodb.nroProtGov = null;

                        ProtocoloDAO.SalvarReconsulta(procotolodb);
                    }
                    System.Windows.MessageBox.Show("Sucesso");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Nenhum evento encontrado!");
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            window.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (datePicker.SelectedDate != null)
            {
                this.search();
            }
            else
            {
                System.Windows.MessageBox.Show("Selecione uma data válida");
            }
        }
    }
}
