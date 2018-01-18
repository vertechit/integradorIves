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
using Vertech.Services;
using Vertech;

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
            
        }

        public void Parametro()
        {
            Processos process = new Processos();
            try
            {
                var s = process.LerArquivo("c:\\vch", "Parametros.dat");

                Parametros.SetDirToke(s[0]);
                Parametros.SetDirArq(s[1]);
                Parametros.SetDirFim(s[2]);

                var t = process.LerArquivo(Parametros.GetDirToke(), "");

                Parametros.SetGrupo(Convert.ToUInt32(t[0]));
                Parametros.SetToken(t[1]);

                Parametros.SetTipoApp("Service");
            }
            catch
            {
                StreamWriter vWriter = new StreamWriter(@"c:\logServico.txt", true);
                vWriter.WriteLine("--------------------------------------------------");
                vWriter.WriteLine("Erro " + DateTime.Now.ToString());
                vWriter.Flush();
                vWriter.Close();

                //OnStop();
            }
        }

        protected override void OnStart(string[] args)
        {
            System.Diagnostics.Debugger.Launch();
            StreamWriter vWriter = new StreamWriter(@"c:\logServico.txt", true);
            vWriter.WriteLine("--------------------------------------------------");
            vWriter.WriteLine("Serviço iniciado: " + DateTime.Now.ToString());
            vWriter.WriteLine("");
            vWriter.Flush();
            vWriter.Close();

            Parametro();

            timer = new Timer(new TimerCallback(timer_Tick), null, 10000, 600000);

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
            log("Job Iniciado: ");

            integra.Job();
            Thread.Sleep(30000);
            consulta.Job();

            log("Job finalizado: ");
        }

        private void log(string msg)
        {
            StreamWriter vWriter = new StreamWriter(@"c:\logServico.txt", true);
            vWriter.WriteLine(msg + DateTime.Now.ToString());
            vWriter.WriteLine("");
            vWriter.Flush();
            vWriter.Close();
        }
    }
}

