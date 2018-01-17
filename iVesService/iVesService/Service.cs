using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using iVesService.Services;

namespace iVesService
{
    public partial class Service : ServiceBase
    {

        Timer timer;
        Consulta consulta = new Consulta();
        Integra integra = new Integra();

        public Service()
        {
            InitializeComponent();
            Parametro();
        }

        public void Parametro()
        {
            Processos process = new Processos();

            var s = process.LerArquivo("c:", "Parametros.txt");

            Parametros.SetDirToke(s[0]);
            Parametros.SetDirArq(s[1]);
            Parametros.SetDirFim(s[2]);

            var t = process.LerArquivo(Parametros.GetDirToke(), "token.file");

            Parametros.SetGrupo(Convert.ToUInt32(t[0]));
            Parametros.SetToken(t[1]);
        }

        protected override void OnStart(string[] args)
        {
            StreamWriter vWriter = new StreamWriter(@"c:\logServico.txt", true);
            vWriter.WriteLine("--------------------------------------------------");
            vWriter.WriteLine("Serviço iniciado: " + DateTime.Now.ToString());
            vWriter.Flush();
            vWriter.Close();
            
            timer = new Timer(new TimerCallback(timer_Tick), null, 0, 600000);

        }

        protected override void OnStop()
        {
            StreamWriter vWriter = new StreamWriter(@"c:\logServico.txt", true);

            vWriter.WriteLine("Servico Pausado: " + DateTime.Now.ToString());
            vWriter.WriteLine("--------------------------------------------------");
            vWriter.Flush();
            vWriter.Close();
        }

        private void timer_Tick(object sender)
        {
            StreamWriter vWriter = new StreamWriter(@"c:\logServico.txt", true);
            vWriter.WriteLine("");
            vWriter.WriteLine("Job iniciado: " + DateTime.Now.ToString());
            vWriter.WriteLine("");
            vWriter.Flush();
            vWriter.Close();

            integra.Job();
            Thread.Sleep(30000);
            consulta.Job();

           

        }
    }
}

