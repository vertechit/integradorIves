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
using Vertech.DAO;
using Vertech.Modelos;

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
                var s = Helper.GetParametros();

                if(Directory.Exists(s.CaminhoDir) && Directory.Exists(s.CaminhoFim) && File.Exists(s.CaminhoToke))
                {
                    Parametros.SetDirToke(s.CaminhoToke);
                    Parametros.SetDirArq(s.CaminhoDir);
                    Parametros.SetDirFim(s.CaminhoFim);

                    if (DefineToken(Parametros.GetDirToke()) == false)
                    {
                        this.Stop();
                    }
                }
                else
                {
                    Log("Não foi possivel localizar as informações, por favor, abra o integrador e defina os parametros novamente.", 2);
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

            Timer = new Timer(new TimerCallback(Timer_Tick), null, 10000, 1200000);

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
            Log("Job Iniciado: ", 1);

            integra.Job();
            Thread.Sleep(60000);
            consulta.Job();

            Log("Job finalizado: ", 1);
        }

        public bool DefineToken(string dir)
        {

            try
            {
                string[] lines = System.IO.File.ReadAllLines(@dir);

                if (lines.Length == 2)
                {
                    Parametros.SetGrupo(Convert.ToInt32(lines[0]));
                    Parametros.SetToken(lines[1]);

                    return true;
                }
                else
                {
                   Log("Arquivo não suportado.", 2);
                    return false;
                }
            }
            catch
            {
                Log("Arquivo de token não pode ser importado.", 2);
                return false;
            }

        }

        private void Log(string msg, int tp)
        {
            if (tp == 1)
            {
                StreamWriter vWriter = new StreamWriter(@"c:\logServico.txt", true);
                vWriter.WriteLine(msg + DateTime.Now.ToString());
                vWriter.WriteLine("");
                vWriter.Flush();
                vWriter.Close();
            }
            else
            {
                StreamWriter vWriter = new StreamWriter(@"c:\logServico.txt", true);
                vWriter.WriteLine(msg);
                vWriter.WriteLine("");
                vWriter.Flush();
                vWriter.Close();
            }
            
        }
    }
}