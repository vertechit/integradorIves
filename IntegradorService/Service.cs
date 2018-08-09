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
using IntegradorCore.NHibernate;
using IntegradorCore.NHibernate.DAO;
using IntegradorCore.Modelos;
using System.Collections;

namespace IntegradorService
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
            var sessao = AuxiliarNhibernate.AbrirSessao();
            var ParametroDAO = new ParametroDAO(sessao);
            var ParametroDBDAO = new ParametroDB_DAO(sessao);
            Processos process = new Processos();
            StaticParametros.SetTipoApp("Service");
            int ctrl = 0;

            try
            {
                
                var ret = ParametroDAO.BuscarPorID(1);//Armazenamento.GetParametros();
                var retdb = ParametroDBDAO.BuscarPorID(1);

                try
                {
                    if (File.Exists(ret.CaminhoToke))
                    {
                        ctrl++;
                        StaticParametros.SetDirToke(ret.CaminhoToke);
                        
                        if (DefineToken(StaticParametros.GetDirToke()) == false)
                        {
                            this.Stop();
                        }
                        if (VerificaProcessoRun() == false)
                        {
                            Thread t = new Thread(process.VerificaParaAtualizar);
                            t.Name = "UpdaterWorker";
                            t.Start();
                        }
                        else
                        {
                            Log("Feche o integrador para iniciar o serviço.", 2);
                            this.Stop();
                        }

                    }
                }
                catch (Exception)
                {
                    Log("Não foi possivel localizar as informações, por favor, abra o integrador e defina os parametros novamente.", 2);
                    this.Stop();
                }

                try
                {
                    if (Directory.Exists(ret.CaminhoDir))
                    {
                        ctrl++;
                        StaticParametros.SetDirOrigem(ret.CaminhoDir);
                        process.CriarPastas();
                    }
                }
                catch (Exception)
                {

                }

                try
                {
                    StaticParametersDB.SetDriver(retdb.Driver);
                    StaticParametersDB.SetHost(retdb.Host);
                    StaticParametersDB.SetPort(retdb.Port);
                    StaticParametersDB.SetServiceName(retdb.ServiceName);
                    StaticParametersDB.SetUser(retdb.User);
                    StaticParametersDB.SetPassword(AESThenHMAC.SimpleDecryptWithPassword(retdb.Password, process.GetMacAdress()));
                    StaticParametros.SetIntegraBanco(true);
                    ctrl++;
                }
                catch (Exception)
                {
                    StaticParametros.SetIntegraBanco(false);
                }

                if(ctrl < 2)
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
                sessao.Close();
                this.Stop();
            }

            sessao.Close();
        }

        protected override void OnStart(string[] args)
        {
            //System.Diagnostics.Debugger.Launch();

            Processos process = new Processos();
            if (process.WritePermissionFile() == false || process.ReadPermissionFile() == false)
            {
                this.Stop();
            }
            if (process.ReadPermissionFolder() == false || process.WritePermissionFolder() == false)
            {
                this.Stop();
            }

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
                Log(ex.Message, 2);
            }
            
            Parametro();

            Timer = new Timer(new TimerCallback(Timer_Tick), null, 30000, 60000);

        }

        protected override void OnStop()
        {
            try
            {
                StreamWriter vWriter = new StreamWriter(@"c:\vch\log\logServico.log", true);

                vWriter.WriteLine("Servico Pausado: " + DateTime.Now.ToString());
                vWriter.WriteLine("--------------------------------------------------");
                vWriter.Flush();
                vWriter.Close();
            }
            catch (Exception)
            {

            }
            
        }

        public bool VerificaProcessoRun()
        {
            var isOpen = Process.GetProcesses().Any(p =>
            p.ProcessName == "IntegradorApp");

            if (isOpen)
                return true;

            return false;
        }

        private void Timer_Tick(object sender)
        {
            if (StaticParametros.GetLockVariavel() == false)
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
            else
            {
                Log("Aguardando Thread de atualização/limpeza finalizar execução.", 1);
            }
        }

        private void Job(Jobs job)
        {
            Controle = 1;
            Log("Job Iniciado: ", 1);
            Log("Integração iniciada: ", 1);
            ActionIntegra();

            Thread.Sleep(60000);

            Log("Consulta iniciada: ", 1);
            ActionConsulta();
            Log("Job finalizado: ", 1);
            Log("--", 2);
            Controle = 0;

        }

         private void ActionIntegra()
        {

            if (StaticParametros.GetIntegraBanco() == true)
            {
                IntegraDB();
            }

            if (StaticParametros.GetDirOrigem() != null && StaticParametros.GetDirOrigem() != "")
            {
                Integra(1);
            }
                
            Log("Integração finalizado: ", 1);
        }

        private void ActionConsulta()
        {
            if (StaticParametros.GetIntegraBanco() == true)
            {
                ConsultaDB();

            }

            if (StaticParametros.GetDirOrigem() != null && StaticParametros.GetDirOrigem() != "")
            {
                Consulta(1);
            }
                
            Log("Consulta finalizada: ", 1);
        }

        private void Integra(int i)
        {
            if (i > 3)
            {
                
                return;
            }
            Processos proc = new Processos();

            if (proc.ValidaStaticParametros())
            {
                proc.DefineNullParametros();
                if (!proc.ValidaStaticParamsJob())
                {
                    proc.AlteraParametro(i);
                    Jobs Job = new Jobs();
                    Job.Envia();
                    Integra(i + 1);
                }
            }
            else
            {
                Log("Não foi possivel localizar as informações, por favor, abra o integrador e defina os parametros novamente.", 2);
                this.Stop();
            }
        }

        private void Consulta(int i)
        {
            if (i > 3)
            {
                
                return;
            }

            Processos proc = new Processos();

            if (proc.ValidaStaticParametros())
            {
                proc.DefineNullParametros();
                if (!proc.ValidaStaticParamsJob())
                {
                    proc.AlteraParametro(i);
                    Jobs Job = new Jobs();
                    Job.Consulta();
                    Consulta(i + 1);
                }
            }
            else
            {
                Log("Não foi possivel localizar as informações, por favor, abra o integrador e defina os parametros novamente.", 2);
                this.Stop();
            }
        }

        private void IntegraDB()
        {
            Jobs Job = new Jobs();
            Job.EnviaDB();
        }

        private void ConsultaDB()
        {
            Jobs Job = new Jobs();
            Job.ConsultaDB();
            Job.UpdateDB();
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
                }catch(Exception)
                {
                    //Log(ex.Message, 2);
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
                catch(Exception)
                {
                    //Log(ex.Message, 2);
                }
            }
            
        }
    }
}