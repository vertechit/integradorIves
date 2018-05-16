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
        public string[] RetornaData(){

            string stri = Convert.ToString(DateTime.Now);
            string[] strArr = null;
            char[] splitchar = { ' ' };

            return strArr = stri.Split(splitchar);
        }

        public void InsereException(int tipo, string msg, string arquivo, string servico, string acao, string protocolo, string coderro)
        {
            var strArr = RetornaData();

            string hora = strArr[1];
            string data = strArr[0];

            if(tipo == 1)
            {
                Log.AddLogEnvia(
                        new LogEnvia { 
                            Id = 0, 
                            Msg = msg, 
                            Acao = acao,
                            NomeArquivo = arquivo, 
                            Data = data, 
                            Hora = hora });
            }
            else if(tipo == 2)
            {
                Log.AddLogConsulta(
                        new LogConsulta{
                            Id = 0,
                            NomeArquivo = arquivo,
                            Protocolo = protocolo,
                            Msg = msg,
                            Acao = acao,
                            Data = data,
                            Hora = hora });
            }
            else
            {
                Log.AddLogErro(new LogErros { 
                                Id = 0, 
                                Servico = servico, 
                                CodErro = coderro, 
                                Msg = msg, 
                                Acao = acao, 
                                Data = data, 
                                Hora = hora 
                                });
            }
        }
        public void Exception(string msg, string arquivo, string servico, string acao)
        {

            /*var strArr = RetornaData();

            string hora = strArr[1];
            string data = strArr[0];*/

            if (Parametros.GetTipoApp() == "Service")
            {
                if(servico == "Consulta")
                {
                    InsereException(2, msg, arquivo, "Consulta", " ", " ", " ");
                    /*Log.AddLogConsulta(
                        new LogConsulta{
                            Id = 0,
                            NomeArquivo = arquivo,
                            Protocolo = "null",
                            Msg = msg,
                            Acao = acao,
                            Data = data,
                            Hora = hora });*/
                }
                else
                {
                    InsereException(1, msg, arquivo, "Integra", " ", " ", " ");
                    /*Log.AddLogEnvia(
                        new LogEnvia { 
                            Id = 0, 
                            Msg = msg, 
                            Acao = acao,
                            NomeArquivo = arquivo, 
                            Data = data, 
                            Hora = hora });*/
                }
            }
            else
            {
                if (servico == "Consulta")
                {
                    InsereException(2, msg, arquivo, "Consulta", " ", " ", " ");
                    /*Log.AddLogConsulta(
                        new LogConsulta{
                            Id = 0,
                            NomeArquivo = arquivo,
                            Protocolo = "null",
                            Msg = msg,
                            Acao = acao,
                            Data = data,
                            Hora = hora });*/
                }
                else
                {
                    InsereException(1, msg, arquivo, "Integra", " ", " ", " ");
                    /*Log.AddLogEnvia(
                        new LogEnvia { 
                            Id = 0, 
                            Msg = msg, 
                            Acao = acao,
                            NomeArquivo = arquivo, 
                            Data = data, 
                            Hora = hora });*/
                }
            }
        }

        public void ImprimeException(int tipo, string msg)
        {
            Processos processo = new Processos();

            /*var strArr = RetornaData();

            string hora = strArr[1];
            string data = strArr[0];*/
            switch (tipo)
            {

                case 1:
                    if (Parametros.GetTipoApp() == "Service")
                    {
                        InsereException(3, msg, " ", "Integra", " ", " ", " ");
                        /*Log.AddLogErro(new LogErros { 
                                Id = 0, 
                                Servico = "Integra", 
                                CodErro = "21", 
                                Msg = msg, 
                                Acao = "", 
                                Data = data, 
                                Hora = hora 
                                });*/
                        /*Log.AddLogEnvia(
                            new LogEnvia { 
                                Id = 0, 
                                Msg = msg, 
                                Acao = "",
                                NomeArquivo = "Integra", 
                                Data = data, 
                                Hora = hora });*/

                    }
                    else
                    {
                        InsereException(3, msg, " ", "Integra", " ", " ", " ");
                        /*Log.AddLogEnvia(
                            new LogEnvia { 
                                Id = 0, 
                                Msg = msg, 
                                Acao = "",
                                NomeArquivo = "Integra", 
                                Data = data, 
                                Hora = hora });*/
                    }
                    break;

                case 2:
                    if (Parametros.GetTipoApp() == "Service")
                    {
                        InsereException(3, msg, " ", "Consulta", " ", " ", " ");
                        /*Log.AddLogConsulta(
                            new LogConsulta{
                            Id = 0,
                            NomeArquivo = "Consulta",
                            Protocolo = "0",
                            Msg = msg,
                            Acao = "",
                            Data = data,
                            Hora = hora });*/
                    }
                    else
                    {
                        InsereException(3, msg, " ", "Consulta", " ", " ", " ");
                        /*Log.AddLogConsulta(
                            new LogConsulta{
                            Id = 0,
                            NomeArquivo = "Consulta",
                            Protocolo = "0",
                            Msg = msg,
                            Acao = "",
                            Data = data,
                            Hora = hora });*/
                    }
                    break;
            }
            
        }
        public void ExNoFilesFound(int tp)
        {
            Processos processo = new Processos();

            /*var strArr = RetornaData();

            string hora = strArr[1];
            string data = strArr[0];*/

            switch (tp)
            {
                case 1:
                    if(Parametros.GetTipoApp() == "Service")
                    {
                        try
                        {
                            InsereException(3, "A pasta selecionada não contem os arquivos necessários para o envio", " ", "Integra", " ", " ", "21");
                            /*Log.AddLogErro(new LogErros { 
                                Id = 0, 
                                Servico = "Integra", 
                                CodErro = "21", 
                                Msg = "A pasta selecionada não contem os arquivos necessários para o envio", 
                                Acao = "Insira arquivos na pasta.", 
                                Data = data, 
                                Hora = hora 
                                });*/
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
                            InsereException(3, "A pasta selecionada não contem os arquivos necessários para o envio", " ", "Integra", " ", " ", "21");
                            /*Log.AddLogErro(
                                new LogErros { 
                                    Id = 0, 
                                    Servico = "Integra", 
                                    CodErro = "21", 
                                    Msg = "A pasta selecionada não contem os arquivos necessários para o envio", 
                                    Acao = "Insira arquivos na pasta.", 
                                    Data = data, 
                                    Hora = hora });*/
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
                            InsereException(3, "A pasta selecionada não contem os arquivos necessários para a consulta", " ", "Consulta", " ", " ", "21");
                            /*Log.AddLogErro(
                                new LogErros { 
                                    Id = 0, 
                                    Servico = "Consulta", 
                                    CodErro = "21",
                                    Msg = "A pasta selecionada não contem os arquivos necessários para a consulta", 
                                    Acao = "",
                                    Data = data, 
                                    Hora = hora });*/

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
                            InsereException(3, "A pasta selecionada não contem os arquivos necessários para a consulta", " ", "Consulta", " ", " ", "21");
                            /*Log.AddLogErro(
                                new LogErros { 
                                    Id = 0, 
                                    Servico = "Consulta", 
                                    CodErro = "21", 
                                    Msg = "A pasta selecionada não contem os arquivos necessários para a consulta", 
                                    Acao = "",
                                    Data = data, 
                                    Hora = hora });*/

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
            Processos processo = new Processos();

            /*var strArr = RetornaData();

            string hora = strArr[1];
            string data = strArr[0];*/

            if (Parametros.GetTipoApp() == "Service")
            {
                try
                {
                    InsereException(3, msg, " ", "Processos", " ", " ", codErro.ToString());
                    /*Log.AddLogErro(
                        new LogErros { 
                            Id = 0, 
                            Servico = "Processos", 
                            CodErro = codErro.ToString(), 
                            Msg = msg, 
                            Acao = "",
                            Data = data, 
                            Hora = hora });*/

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
                    InsereException(3, msg, " ", "Processos", " ", " ", codErro.ToString());
                    /*Log.AddLogErro(
                        new LogErros { 
                            Id = 0, 
                            Servico = "Processos", 
                            CodErro = codErro.ToString(), 
                            Msg = msg, 
                            Acao = "",
                            Data = data, 
                            Hora = hora });*/
                }
                catch(Exception ex)
                {
                    ExOpenFile(999, ex.Message);
                }
                
            }
        }

        public void ExSQLite(int codErro, string msg)
        {
            Processos processo = new Processos();

            /*var strArr = RetornaData();

            string hora = strArr[1];
            string data = strArr[0];*/

            if (Parametros.GetTipoApp() == "Service")
            {
                try
                {
                    InsereException(3, msg, " ", "SQLite", " ", " ", codErro.ToString());
                    /*Log.AddLogErro(
                        new LogErros { 
                            Id = 0, 
                            Servico = "SQLite", 
                            CodErro = codErro.ToString(), 
                            Msg = msg, 
                            Acao = "",
                            Data = data, 
                            Hora = hora });*/
                }
                catch (Exception ex)
                {
                    ExOpenFile(999, ex.Message);
                }

            }
            else
            {
                InsereException(3, msg, " ", "SQLite", " ", " ", codErro.ToString());
                /*Log.AddLogErro(
                    new LogErros { 
                    Id = 0, 
                    Servico = "SQLite", 
                    CodErro = codErro.ToString(),
                    Msg = msg, 
                    Acao = "",
                    Data = data, 
                    Hora = hora });*/
            }
        }

        public void ExSecureFile(int codErro, string msg)
        {
            var p = new Processos();

            /*var strArr = RetornaData();

            string hora = strArr[1];
            string data = strArr[0];*/

            if (Parametros.GetTipoApp() == "Service")
            {
                InsereException(3, msg, " ", "SecureFile", " ", " ", codErro.ToString());
                /*Log.AddLogErro(
                    new LogErros { 
                    Id = 0, 
                    Servico = "SecureFile", 
                    CodErro = codErro.ToString(), 
                    Msg = msg, 
                    Acao = "",
                    Data = data, 
                    Hora = hora });*/

            }
            else
            {
                InsereException(3, msg, " ", "SecureFile", " ", " ", codErro.ToString());
                /*Log.AddLogErro(
                    new LogErros { 
                    Id = 0, 
                    Servico = "SecureFile", 
                    CodErro = codErro.ToString(), 
                    Msg = msg, 
                    Acao = "",
                    Data = data, 
                    Hora = hora });*/
            }
        }

        public void ExOpenFile(int tipo, string msg)
        {
            var p = new Processos();

            /*var strArr = RetornaData();

            string hora = strArr[1];
            string data = strArr[0];*/
            InsereException(3, msg, " ", "SQLite", "Encerre um dos processos (Serviço/Integrador(Tela))", " ", "999");
            /*Log.AddLogErro(
                new LogErros { 
                Id = 0, 
                Servico = "SQLite", 
                CodErro = "999", 
                Msg = msg, 
                Acao = "Encerre um dos processos (Serviço/Integrador(Tela))",
                Data = data, 
                Hora = hora });*/
        }
    }
}
