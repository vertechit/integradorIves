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
using System.Windows.Shapes;

namespace IntegradorApp.Telas
{
    /// <summary>
    /// Lógica interna para ParametrosBanco.xaml
    /// </summary>
    public partial class ParametrosBanco : Window
    {
        MainWindow __main;

        public ParametrosBanco(MainWindow a)
        {
            InitializeComponent();
            __main = a;
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            __main.Show();
        }
    }
}
