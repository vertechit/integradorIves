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
using System.Collections;

namespace iVesService
{
    public partial class Service : ServiceBase
    {

        Timer Timer;
        int Controle = 0;
        //int Quantidade = 0;
        //int Executada = 0;
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
                StreamWriter vWriter = new StreamWriter(@"c:\vch\log\logServico.log", true);
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

            DirectoryInfo di = new DirectoryInfo("c:\\vch\\log");

            if (di.Exists == false)
                di.Create();

            StreamWriter vWriter = new StreamWriter(@"c:\vch\log\logServico.log", true);
            vWriter.WriteLine("--------------------------------------------------");
            vWriter.WriteLine("Serviço iniciado: " + DateTime.Now.ToString());
            vWriter.WriteLine("");
            vWriter.Flush();
            vWriter.Close();

            Parametro();

            Timer = new Timer(new TimerCallback(Timer_Tick), null, 10000, 1200000); //3600000 = 60min | 1200000 = 20min

        }

        protected override void OnStop()
        {
            StreamWriter vWriter = new StreamWriter(@"c:\vch\log\logServico.log", true);

            vWriter.WriteLine("Servico Pausado: " + DateTime.Now.ToString());
            vWriter.WriteLine("--------------------------------------------------");
            vWriter.Flush();
            vWriter.Close();
        }

        private void Timer_Tick(object sender)
        {
            if(Controle == 0)
            {
                Job();
            } 
        }

        private void Job()
        {
            Controle = 1;
            //Log("----------------------------------", 2);
            Log("Job Iniciado: ", 1);
            Log("Integração iniciada: ", 1);
            integra.Job();
            Log("Integração finalizado: ", 1);
            Thread.Sleep(300000); //300000 = 5min
            Log("Consulta iniciada: ", 1);
            consulta.Job();
            Log("Consulta finalizada: ", 1);
            Log("Job finalizado: ", 1);
            Log("--", 2);
            Controle = 0;

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
                StreamWriter vWriter = new StreamWriter(@"c:\vch\log\logServico.log", true);
                vWriter.WriteLine(msg + DateTime.Now.ToString());
                vWriter.WriteLine("");
                vWriter.Flush();
                vWriter.Close();
            }
            else
            {
                StreamWriter vWriter = new StreamWriter(@"c:\vch\log\logServico.log", true);
                vWriter.WriteLine(msg);
                vWriter.WriteLine("");
                vWriter.Flush();
                vWriter.Close();
            }
            
        }
    }
}