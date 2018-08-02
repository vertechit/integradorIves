using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegradorCore.Services;
using System.IO;
using System.Windows;
using IntegradorCore.DAO;
using IntegradorCore.Modelos;

namespace IntegradorCore.Services
{
    public class ExceptionCore
    {
        Processos proc = new Processos();

        public void Exception(string msg, string arquivo, string servico, string acao, Exception ex)
        {
            var codErro = " ";

            var erro = TrataMensagemErro(msg);

            if (erro != null)
            {
                msg = erro.Msg;
                acao = erro.Acao;
                codErro = erro.CodErro;
            }

            if (StaticParametros.GetTipoApp() == "Service")
            {
                if (servico == "Consulta")
                {
                    proc.InsereLog(2, msg, arquivo, "Consulta", acao, " ", " ");
                    proc.InsereLogInterno("Consulta", ex, codErro, arquivo);
                }
                else
                {
                    proc.InsereLog(1, msg, arquivo, "Integra", acao, " ", " ");
                    proc.InsereLogInterno("Integra", ex, codErro, arquivo);
                }
            }
            else
            {
                if (servico == "Consulta")
                {
                    proc.InsereLog(2, msg, arquivo, "Consulta", acao, " ", " ");
                    proc.InsereLogInterno("Consulta", ex, codErro, arquivo);
                }
                else
                {
                    proc.InsereLog(1, msg, arquivo, "Integra", acao, " ", " ");
                    proc.InsereLogInterno("Integra", ex, codErro, arquivo);
                }
            }
        }

        public void ImprimeException(int tipo, string msg, Exception ex, string id)
        {
            var acao = " ";
            var codErro = " ";

            var erro = TrataMensagemErro(msg);

            if(erro.Acao != null && erro.CodErro != null && erro.Msg != null)
            {
                msg = erro.Msg;
                acao = erro.Acao;
                codErro = erro.CodErro;
            }


            switch (tipo)
            {

                case 1:
                    if (StaticParametros.GetTipoApp() == "Service")
                    {
                        proc.InsereLog(3, msg, " ", "Integra", acao, " ", codErro);
                        proc.InsereLogInterno("Integra", ex, codErro, id);
                    }
                    else
                    {
                        proc.InsereLog(3, msg, " ", "Integra", acao, " ", codErro);
                        proc.InsereLogInterno("Integra", ex, codErro, id);
                    }
                    break;

                case 2:
                    if (StaticParametros.GetTipoApp() == "Service")
                    {
                        proc.InsereLog(3, msg, " ", "Consulta", acao, " ", codErro);
                        proc.InsereLogInterno("Consulta", ex, codErro, id);
                    }
                    else
                    {
                        proc.InsereLog(3, msg, " ", "Consulta", acao, " ", codErro);
                        proc.InsereLogInterno("Consulta", ex, codErro, id);
                    }
                    break;
            }

        }

        public void ExNoFilesFound(int tp)
        {

            switch (tp)
            {
                case 1:
                    if (StaticParametros.GetTipoApp() == "Service")
                    {
                        try
                        {
                            //proc.InsereLog(3, "A pasta selecionada não contem os arquivos necessários para o envio", " ", "Integra", " ", " ", "21");
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
                            proc.InsereLog(3, "A pasta selecionada não contem os arquivos necessários para o envio", " ", "Integra", " ", " ", "21");
                        }
                        catch (Exception ex)
                        {
                            ExOpenFile(999, ex.Message);
                        }

                    }
                    break;
                case 2:
                    if (StaticParametros.GetTipoApp() == "Service")
                    {
                        try
                        {
                            //proc.InsereLog(3, "A pasta selecionada não contem os arquivos necessários para a consulta", " ", "Consulta", " ", " ", "21");

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

            if (StaticParametros.GetTipoApp() == "Service")
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
                catch (Exception ex)
                {
                    ExOpenFile(999, ex.Message);
                }

            }
        }

        public void ExSQLite(int codErro, string msg)
        {

            if (StaticParametros.GetTipoApp() == "Service")
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

        public void EncryptException(string msg, int codErro)
        {
            if (StaticParametros.GetTipoApp() == "Service")
            {
                proc.InsereLog(3, msg, " ", "Encrypt/Decrypt", " ", " ", codErro.ToString());
            }
            else
            {
                proc.InsereLog(3, msg, " ", "Encrypt/Decrypt", " ", " ", codErro.ToString());
            }
        }

        public void ExDriverOracle(int codErro, string msg)
        {
            if (StaticParametros.GetTipoApp() == "Service")
            {
                proc.InsereLog(3, msg, " ", "OracleDriver", " ", " ", codErro.ToString());
            }
            else
            {
                proc.InsereLog(3, msg, " ", "OracleDriver", " ", " ", codErro.ToString());
            }
        }

        public void ExDriverSQLServer(int codErro, string msg)
        {
            if (StaticParametros.GetTipoApp() == "Service")
            {
                proc.InsereLog(3, msg, " ", "SQLServerDriver", " ", " ", codErro.ToString());
            }
            else
            {
                proc.InsereLog(3, msg, " ", "SQLServerDriver", " ", " ", codErro.ToString());
            }
        }

        public void ExBanco(int codErro, string msg, string driver, Exception ex)
        {
            if (StaticParametros.GetTipoApp() == "Service")
            {
                proc.InsereLog(3, msg, " ", driver, " ", " ", codErro.ToString());
                proc.InsereLogInterno(driver, ex, Convert.ToString(codErro), msg);
            }
            else
            {
                proc.InsereLog(3, msg, " ", driver, " ", " ", codErro.ToString());
                proc.InsereLogInterno(driver, ex, Convert.ToString(codErro), msg);
            }
        }

        public void ExSecureFile(int codErro, string msg)
        {

            if (StaticParametros.GetTipoApp() == "Service")
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

        public ErroModel TrataMensagemErro(string msg)
        {
            var erro = new ErroModel();

            if (msg == "O servidor retornou uma Falha SOAP inválida. Consulte a InnerException para obter mais detalhes.")
            {
                erro.Msg = "Ops, isso não era para ter acontecido.";
                erro.Acao = "Por favor contate o desenvolvedor";
                erro.CodErro = "1001";
            }
            else if(msg == "Server returned an invalid SOAP Fault.  Please see InnerException for more details.")
            {
                erro.Msg = "Ops, isso não era para ter acontecido.";
                erro.Acao = "Por favor contate o desenvolvedor";
                erro.CodErro = "1001";
            }
            else if (msg == "O servidor remoto retornou uma resposta inesperada: (502) Bad Gateway.")
            {
                erro.Msg = "Não foi possivel conectar com o servidor";
                erro.Acao = "Tente novamente dentro de alguns minutos, caso persistir, contate o suporte.";
                erro.CodErro = "502";
            }
            else if (msg == "Não havia um ponto de extremidade em escuta em https://apiesocial.vertech-it.com.br/vch-esocial/envialote capaz de aceitar a mensagem. Em geral, isso é causado por um endereço ou ação de SOAP incorreta. Consulte InnerException, se presente, para obter mais detalhes.")
            {
                erro.Msg = "Não foi possivel conectar com o servidor - envialote";
                erro.Acao = "Verifique sua conexão com a internet ou se há proxy";
                erro.CodErro = "1000";
            }
            else if (msg == "Não havia um ponto de extremidade em escuta em https://apiesocial2.vertech-it.com.br/vch-esocial/envialote capaz de aceitar a mensagem. Em geral, isso é causado por um endereço ou ação de SOAP incorreta. Consulte InnerException, se presente, para obter mais detalhes.")
            {
                erro.Msg = "Não foi possivel conectar com o servidor - envialote 2";
                erro.Acao = "Verifique sua conexão com a internet ou se há proxy";
                erro.CodErro = "1000";
            }
            else if (msg == "Não havia um ponto de extremidade em escuta em https://apiesocial.vertech-it.com.br/vch-esocial/consultalote capaz de aceitar a mensagem. Em geral, isso é causado por um endereço ou ação de SOAP incorreta. Consulte InnerException, se presente, para obter mais detalhes.")
            {
                erro.Msg = "Não foi possivel conectar com o servidor";
                erro.Acao = "Verifique sua conexão com a internet ou se há proxy";
                erro.CodErro = "1000";
            }
            else if (msg == "Não havia um ponto de extremidade em escuta em https://apiesocial2.vertech-it.com.br/vch-esocial/consultalote capaz de aceitar a mensagem. Em geral, isso é causado por um endereço ou ação de SOAP incorreta. Consulte InnerException, se presente, para obter mais detalhes.")
            {
                erro.Msg = "Não foi possivel conectar com o servidor - consultalote 2";
                erro.Acao = "Verifique sua conexão com a internet ou se há proxy";
                erro.CodErro = "1000";
            }
            else if (msg == "Não havia um ponto de extremidade em escuta em https://apiesocial.vertech-it.com.br/vch-esocial/consultaintegra capaz de aceitar a mensagem. Em geral, isso é causado por um endereço ou ação de SOAP incorreta. Consulte InnerException, se presente, para obter mais detalhes.")
            {
                erro.Msg = "Não foi possivel conectar com o servidor - consultaintegra";
                erro.Acao = "Verifique sua conexão com a internet ou se há proxy";
                erro.CodErro = "1000";
            }
            else if (msg == "Não havia um ponto de extremidade em escuta em https://apiesocial2.vertech-it.com.br/vch-esocial/consultaintegra capaz de aceitar a mensagem. Em geral, isso é causado por um endereço ou ação de SOAP incorreta. Consulte InnerException, se presente, para obter mais detalhes.")
            {
                erro.Msg = "Não foi possivel conectar com o servidor - consultaintegra 2";
                erro.Acao = "Verifique sua conexão com a internet ou se há proxy";
                erro.CodErro = "1000";
            }
            else if (msg == "Não havia um ponto de extremidade em escuta em https://apiesocial.vertech-it.com.br/vch-esocial/enviaintegra capaz de aceitar a mensagem. Em geral, isso é causado por um endereço ou ação de SOAP incorreta. Consulte InnerException, se presente, para obter mais detalhes.")
            {
                erro.Msg = "Não foi possivel conectar com o servidor - enviaintegra";
                erro.Acao = "Verifique sua conexão com a internet ou se há proxy";
                erro.CodErro = "1000";
            }
            else if (msg == "Não havia um ponto de extremidade em escuta em https://apiesocial2.vertech-it.com.br/vch-esocial/enviaintegra capaz de aceitar a mensagem. Em geral, isso é causado por um endereço ou ação de SOAP incorreta. Consulte InnerException, se presente, para obter mais detalhes.")
            {
                erro.Msg = "Não foi possivel conectar com o servidor - enviaintegra 2";
                erro.Acao = "Verifique sua conexão com a internet ou se há proxy";
                erro.CodErro = "1000";
            }
            else if (msg == "Erro de rede ou específico à instância ao estabelecer conexão com o SQL Server. O servidor não foi encontrado ou não estava acessível. Verifique se o nome da instância está correto e se o SQL Server está configurado para permitir conexões remotas. (provider: TCP Provider, error: 0 - O computador remoto recusou a conexão de rede.)")
            {

            }else if(msg == "O formatador gerou uma exceção ao tentar desserializar a mensagem: Erro ao tentar desserializar o parâmetro http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/consulta/retornoProcessamento/v1_1_0:ConsultarLoteEventosResponse. A mensagem da InnerException foi 'Houve um erro ao desserializar o objeto do tipo IntegradorCore.apiConsultaXML.ConsultarLoteEventosResponseBody. O prefixo 'xsi' não está definido. Linha 1, posição 818.'. Consulte a InnerException para obter mais detalhes.")
            {

            }

            return erro;
        }
    }
}
