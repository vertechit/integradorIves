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
using IntegradorCore;
using IntegradorCore.Services;
using IntegradorCore.DAO;
using IntegradorCore.Modelos;
using System.Collections;

namespace iVesService
{
    public partial class Service : ServiceBase
    {

        Timer Timer;
        int Controle = 0;

        Processos proc = new Processos();

        public Service()
        {
            InitializeComponent();
        }

        public void Parametro()
        {
            Processos process = new Processos();
            StaticParametros.SetTipoApp("Service");

            try
            {
                var ret = Armazenamento.GetParametros();

                if(Directory.Exists(ret.CaminhoDir) && Directory.Exists(ret.CaminhoFim) && File.Exists(ret.CaminhoToke))
                {
                    StaticParametros.SetDirToke(ret.CaminhoToke);
                    StaticParametros.SetDirArq(ret.CaminhoDir);
                    StaticParametros.SetDirFim(ret.CaminhoFim);
                    StaticParametros.SetAmbiente(Convert.ToInt64(ret.Ambiente));
                    StaticParametros.SetBase(Convert.ToBoolean(ret.Base));

                    if (DefineToken(StaticParametros.GetDirToke()) == false)
                    {
                        this.Stop();
                    }
                    if(VerificaProcessoRun() == false)
                    {
                        Thread Tproc = new Thread(process.LimpaLog);
                        Tproc.Start();
                    }
                    else
                    {
                        Log("Feche o integrador para iniciar o serviço.", 2);
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
            else
            {
                foreach (var item in di.GetFiles())
                {
                    if(item.Name == "logServico.log")
                    {
                        item.Delete();
                    }
                }
            }
                
            try
            {
                StreamWriter vWriter = new StreamWriter(@"c:\vch\log\logServico.log", true);
                vWriter.WriteLine("--------------------------------------------------");
                vWriter.WriteLine("Serviço iniciado: " + DateTime.Now.ToString());
                vWriter.WriteLine("");
                vWriter.Flush();
                vWriter.Close();
            }
            catch (Exception ex)
            {

            }
            
            Parametro();

            Timer = new Timer(new TimerCallback(Timer_Tick), null, 30000, 60000);

        }

        protected override void OnStop()
        {
            StreamWriter vWriter = new StreamWriter(@"c:\vch\log\logServico.log", true);

            vWriter.WriteLine("Servico Pausado: " + DateTime.Now.ToString());
            vWriter.WriteLine("--------------------------------------------------");
            vWriter.Flush();
            vWriter.Close();
        }

        public bool VerificaProcessoRun()
        {
            var isOpen = Process.GetProcesses().Any(p =>
            p.ProcessName == "Integrador");

            if (isOpen)
                return true;

            return false;
        }

        private void Timer_Tick(object sender)
        {
            if (Controle == 0)
            {
                if (proc.ValidaStaticParametros() == true)
                {
                    Jobs job = new Jobs();
                    Job(job);
                }
                else
                {
                    Log("Não foi possivel localizar as informações, por favor, abra o integrador e defina os parametros novamente.", 2);
                    this.Stop();
                }
            }
            
            
        }

        private void Job(Jobs job)
        {
            Controle = 1;
            Log("Job Iniciado: ", 1);
            Log("Integração iniciada: ", 1);

            job.Envia();

            Log("Integração finalizado: ", 1);

            Thread.Sleep(60000);
            Log("Consulta iniciada: ", 1);

            job.Consulta();

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
                    StaticParametros.SetGrupo(Convert.ToInt32(lines[0]));
                    StaticParametros.SetToken(lines[1]);

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
                try
                {
                    StreamWriter vWriter = new StreamWriter(@"c:\vch\log\logServico.log", true);
                    vWriter.WriteLine(msg + DateTime.Now.ToString());
                    vWriter.WriteLine("");
                    vWriter.Flush();
                    vWriter.Close();
                }catch(Exception ex)
                {
                    //NoOp
                }
                
            }
            else
            {
                try
                {
                    StreamWriter vWriter = new StreamWriter(@"c:\vch\log\logServico.log", true);
                    vWriter.WriteLine(msg);
                    vWriter.WriteLine("");
                    vWriter.Flush();
                    vWriter.Close();
                }
                catch(Exception ex)
                {
                    //NoOp
                }
            }
            
        }
    }
}