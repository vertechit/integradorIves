using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertech.Services;
using System.IO;
using System.Windows;
using Vertech.DAO;
using Vertech.Modelos;

namespace Vertech.Services
{
    public class ClassException
    {
        Processos proc = new Processos();
        public void Exception(string msg, string arquivo, string servico, string acao)
        {

            if (Parametros.GetTipoApp() == "Service")
            {
                if(servico == "Consulta")
                {
                    proc.InsereLog(2, msg, arquivo, "Consulta", " ", " ", " ");
                }
                else
                {
                    proc.InsereLog(1, msg, arquivo, "Integra", " ", " ", " ");
                }
            }
            else
            {
                if (servico == "Consulta")
                {
                    proc.InsereLog(2, msg, arquivo, "Consulta", " ", " ", " ");
                }
                else
                {
                    proc.InsereLog(1, msg, arquivo, "Integra", " ", " ", " ");
                }
            }
        }

        public void ImprimeException(int tipo, string msg)
        {

            switch (tipo)
            {

                case 1:
                    if (Parametros.GetTipoApp() == "Service")
                    {
                        proc.InsereLog(3, msg, " ", "Integra", " ", " ", " ");

                    }
                    else
                    {
                        proc.InsereLog(3, msg, " ", "Integra", " ", " ", " ");
                    }
                    break;

                case 2:
                    if (Parametros.GetTipoApp() == "Service")
                    {
                        proc.InsereLog(3, msg, " ", "Consulta", " ", " ", " ");
                    }
                    else
                    {
                        proc.InsereLog(3, msg, " ", "Consulta", " ", " ", " ");
                    }
                    break;
            }
            
        }
        public void ExNoFilesFound(int tp)
        {

            switch (tp)
            {
                case 1:
                    if(Parametros.GetTipoApp() == "Service")
                    {
                        try
                        {
                            proc.InsereLog(3, "A pasta selecionada não contem os arquivos necessários para o envio", " ", "Integra", " ", " ", "21");
                        }
                        catch(Exception ex)
                        {
                            ExOpenFile(999, ex.Message);
                        }
                        
                    }
                    else
                    {
                        try
                        {
                            proc.InsereLog(3, "A pasta selecionada não contem os arquivos necessários para o envio", " ", "Integra", " ", " ", "21");
                        }
                        catch (Exception ex)
                        {
                            ExOpenFile(999, ex.Message);
                        }
                        
                    }
                        break;
                case 2:
                    if(Parametros.GetTipoApp() == "Service")
                    {
                        try
                        {
                            proc.InsereLog(3, "A pasta selecionada não contem os arquivos necessários para a consulta", " ", "Consulta", " ", " ", "21");

                        }
                        catch (Exception ex)
                        {
                            ExOpenFile(999, ex.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            proc.InsereLog(3, "A pasta selecionada não contem os arquivos necessários para a consulta", " ", "Consulta", " ", " ", "21");

                        }
                        catch (Exception ex)
                        {
                            ExOpenFile(999, ex.Message);
                        }
                        
                    }
                    break;
            }
        }

        public void ExProcessos(int codErro, string msg)
        {

            if (Parametros.GetTipoApp() == "Service")
            {
                try
                {
                    proc.InsereLog(3, msg, " ", "Processos", " ", " ", codErro.ToString());
                }
                
                catch (Exception ex)
                {
                    ExOpenFile(999, ex.Message);
                }
            }
            else
            {
                try
                {
                    proc.InsereLog(3, msg, " ", "Processos", " ", " ", codErro.ToString());
                }
                catch(Exception ex)
                {
                    ExOpenFile(999, ex.Message);
                }
                
            }
        }

        public void ExSQLite(int codErro, string msg)
        {

            if (Parametros.GetTipoApp() == "Service")
            {
                try
                {
                    proc.InsereLog(3, msg, " ", "SQLite", " ", " ", codErro.ToString());
                }
                catch (Exception ex)
                {
                    ExOpenFile(999, ex.Message);
                }

            }
            else
            {
                proc.InsereLog(3, msg, " ", "SQLite", " ", " ", codErro.ToString());
            }
        }

        public void ExSecureFile(int codErro, string msg)
        {

            if (Parametros.GetTipoApp() == "Service")
            {
                proc.InsereLog(3, msg, " ", "SecureFile", " ", " ", codErro.ToString());
            }
            else
            {
                proc.InsereLog(3, msg, " ", "SecureFile", " ", " ", codErro.ToString());
            }
        }

        public void ExOpenFile(int tipo, string msg)
        {

            proc.InsereLog(3, msg, " ", "SQLite", "Encerre um dos processos (Serviço/Integrador(Tela))", " ", "999");

        }
    }
}
