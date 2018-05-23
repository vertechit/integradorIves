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
        ConsultaLote ConsultaLote = new ConsultaLote();
        EnviaLote EnviaLote = new EnviaLote();

        public Service()
        {
            InitializeComponent();
        }

        public void Parametro()
        {
            Processos process = new Processos();
            Parametros.SetTipoApp("Service");

            //process.LimpaLog();
            try
            {
                var ret = Helper.GetParametros();

                if(Directory.Exists(ret.CaminhoDir) && Directory.Exists(ret.CaminhoFim) && File.Exists(ret.CaminhoToke))
                {
                    Parametros.SetDirToke(ret.CaminhoToke);
                    Parametros.SetDirArq(ret.CaminhoDir);
                    Parametros.SetDirFim(ret.CaminhoFim);
                    Parametros.SetAmbiente(ret.Ambiente);
                    Parametros.SetBase(ret.Base);

                    if (DefineToken(Parametros.GetDirToke()) == false)
                    {
                        this.Stop();
                    }
                    Thread Tproc = new Thread(process.LimpaLog);
                    Tproc.Start();
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

            Timer = new Timer(new TimerCallback(Timer_Tick), null, 10000, 60000); //60000 = 1min 3600000 = 60min | 1200000 = 20min

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
            //Executada++;
            if(Controle == 0)
            {
                //Quantidade++;
                Job();
            } 
        }

        private void Job()
        {
            Controle = 1;
            //Log("----------------------------------", 2);
            Log("Job Iniciado: ", 1);
            Log("Integração TXT iniciada: ", 1);
            integra.Job();
            Log("Integração TXT finalizado: ", 1);
            Log("Integração XML iniciada: ", 1);
            EnviaLote.Job();
            Log("Integração XML finalizado: ", 1);
            Thread.Sleep(60000); //300000 = 5min
            Log("Consulta TXT iniciada: ", 1);
            consulta.Job();
            Log("Consulta TXT finalizada: ", 1);
            Log("Consulta XML iniciada: ", 1);
            ConsultaLote.Job();
            Log("Consulta XML finalizada: ", 1);
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
                   //Log("Arquivo não suportado.", 2);
                    return false;
                }
            }
            catch
            {
                //Log("Arquivo de token não pode ser importado.", 2);
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