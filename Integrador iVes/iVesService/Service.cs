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

        Timer Timer;
        Consulta consulta = new Consulta();
        Integra integra = new Integra();

        public Service()
        {
            InitializeComponent();
            
        }

        public void Parametro()
        {
            Processos process = new Processos();
            Parametros.SetTipoApp("Service");

            try
            {
                var s = process.LerArquivo("c:\\vch", "Parametros.dat");

                Parametros.SetDirToke(s[0]);
                Parametros.SetDirArq(s[1]);
                Parametros.SetDirFim(s[2]);

                if(DefineToken(Parametros.GetDirToke()) == false)
                {
                    this.Stop();
                }
            }
            catch
            {
                StreamWriter vWriter = new StreamWriter(@"c:\logServico.txt", true);
                vWriter.WriteLine("--------------------------------------------------");
                vWriter.WriteLine("Erro " + DateTime.Now.ToString());
                vWriter.Flush();
                vWriter.Close();

                this.Stop();
            }
        }

        protected override void OnStart(string[] args)
        {
            //System.Diagnostics.Debugger.Launch();
            StreamWriter vWriter = new StreamWriter(@"c:\logServico.txt", true);
            vWriter.WriteLine("--------------------------------------------------");
            vWriter.WriteLine("Serviço iniciado: " + DateTime.Now.ToString());
            vWriter.WriteLine("");
            vWriter.Flush();
            vWriter.Close();

            Parametro();

            Timer = new Timer(new TimerCallback(Timer_Tick), null, 10000, 600000);

        }

        protected override void OnStop()
        {
            StreamWriter vWriter = new StreamWriter(@"c:\logServico.txt", true);

            vWriter.WriteLine("Servico Pausado: " + DateTime.Now.ToString());
            vWriter.WriteLine("--------------------------------------------------");
            vWriter.Flush();
            vWriter.Close();
        }

        private void Timer_Tick(object sender)
        {
            Log("Job Iniciado: ");

            integra.Job();
            Thread.Sleep(30000);
            consulta.Job();

            Log("Job finalizado: ");
        }

        public bool DefineToken(string dir)
        {
            string[] lines = System.IO.File.ReadAllLines(@dir);

            try
            {
                if (lines.Length == 2)
                {
                    Parametros.SetGrupo(Convert.ToInt32(lines[0]));
                    Parametros.SetToken(lines[1]);

                    return true;
                }
                else
                {
                   Log("Arquivo não suportado");
                    return false;
                }
            }
            catch
            {
                Log("Infomações inválidas");
                return false;
            }


        }

        private void Log(string msg)
        {
            StreamWriter vWriter = new StreamWriter(@"c:\logServico.txt", true);
            vWriter.WriteLine(msg + DateTime.Now.ToString());
            vWriter.WriteLine("");
            vWriter.Flush();
            vWriter.Close();
        }
    }
}

