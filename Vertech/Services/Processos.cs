using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using Vertech.DAO;
using Vertech.Modelos;
using System.Security.AccessControl;
using System.Runtime.InteropServices;

namespace Vertech.Services
{
    public class Processos
    {

        [DllImport("wininet.dll")]
        private extern static Boolean InternetGetConnectedState(out int Description, int ReservedValue);

        public static Boolean IsConnected()
        {
            int Description;

            return InternetGetConnectedState(out Description, 0);
        }

        public string[] RetornaData(){

            string stri = Convert.ToString(DateTime.Now);
            string[] strArr = null;
            char[] splitchar = { ' ' };

            return strArr = stri.Split(splitchar);
        }

        public string MontaCaminhoDir(string dir, string name)
        {
            var s = string.Concat(dir, '\\', name);

            return s;
        }

        public string MontaXML(string arqname)
        {
            string[] xmlLinhas = LerArquivo(Parametros.GetDirArq(), arqname);
            var xmlString = "";

            foreach (var item in xmlLinhas)
            {
                xmlString = String.Concat(xmlString, item);
            }

            string xsdInicio = "<eSocial><envioLoteEventos grupo=\"1\"><eventos><evento id=\"123\">";

            xsdInicio = xsdInicio.Replace("123", arqname);

            string xsdFim = " </evento></eventos></envioLoteEventos></eSocial>";

            var xml = String.Concat(xsdInicio,xmlString,xsdFim);

            return xml;
        }

        public Boolean VerificaConsultaXML(string response)
        {
            int sti = 0;
            int stf = 0;

            if (response == "")
            {
                return false;
            }
            var tagIniStatus = "<status>";
            var tagFimStatus = "</status>";

            var tagIniCdResp = "<cdResposta>";
            var tagFimCdResp = "</cdResposta>";

            sti = response.IndexOf(tagIniStatus) + tagIniStatus.Length;
            stf = response.IndexOf(tagFimStatus) - tagFimStatus.Length;
            var retorno = response.Substring(sti, stf + tagFimStatus.Length - sti);

            sti = retorno.IndexOf(tagIniCdResp) + tagIniCdResp.Length;
            stf = retorno.IndexOf(tagFimCdResp) - tagFimCdResp.Length;
            var codigo = retorno.Substring(sti, stf + tagFimCdResp.Length - sti);

            if (codigo == "201")
            {
                return true;
            }

            return false;
        }

        public List<string> ListarArquivos(string ext)
        {
            string dir = Parametros.GetDirArq();
            DirectoryInfo dirInfo = new DirectoryInfo(@dir);
            List<string> Arquivo = BuscaArquivos(dirInfo, ext);

            return Arquivo;
        }

        public string[] LerArquivo(string dir, string arq)
        {
            string[] lines = null;

            string s = dir;

            if (arq != "")
            {
                s = MontaCaminhoDir(dir, arq);
            }

            try
            {
                lines = System.IO.File.ReadAllLines(@s);
            }
            catch (Exception)
            {
                ClassException ex = new ClassException();
                ex.ExProcessos(1,"Erro na leitura do arquivo");
            }

            return lines;
        }

        public Boolean VerificaResponseXML(string response)
        {
            if(response == "")
            {
                return false;
            }
            var tagIni = "<cdResposta>";
            var tagFim = "</cdResposta>";

            int sti = response.IndexOf(tagIni) + 12;
            int stf = response.IndexOf(tagFim) - 13;

            var codigo = response.Substring(sti, stf + tagFim.Length - sti);

            if(codigo == "201")
            {
                return true;
            }

            return false;
        }

        public void SalvaProtocolo(apiIntegra.integraResponse Response, string arq)
        {
            try
            {
                Helper.AddProtocolo(new Protocolo { Id = 0, NomeArquivo = arq, NroProtocolo = Convert.ToString(Response.protocolo) });
            }
            catch (Exception e)
            {
                ClassException ex = new ClassException();
                ex.ExProcessos(2,e.Message.ToString());
            }
        }

        public void SalvaProtocoloXML(string arq, string Response)
        {
            var tagIni = "<protocoloEnvio>";
            var tagFim = "</protocoloEnvio>";

            int sti = Response.IndexOf(tagIni) + 16;
            int stf = Response.IndexOf(tagFim) - 17;

            var protocolo = Response.Substring(sti, stf + tagFim.Length - sti);

            try
            {
                Helper.AddProtocolo(new Protocolo { Id = 0, NomeArquivo = arq, NroProtocolo = protocolo });
            }
            catch (Exception e)
            {
                ClassException ex = new ClassException();
                ex.ExProcessos(2, e.Message.ToString());
            }
        }

        public Boolean VerificacaoEnviaLote(string arq)
        {
            string dir = Parametros.GetDirArq();
            DirectoryInfo diretorio = new DirectoryInfo(dir);

            if (Helper.ExistsProtocolo(arq) == true)
            {
                 return false;
            }

            return true;
        }

        public int VerificacaoIntegra(string arq)
        {

            string dir = Parametros.GetDirArq();
            DirectoryInfo diretorio = new DirectoryInfo(dir);

            if (Helper.ExistsProtocolo(arq) == true)
            {
                 return 1;
            }

            int tam = 0;
            string[] lines = LerArquivo(dir, arq);

            tam = lines.Length;

            if (tam >= 3)
            {
                if (lines[0].Contains("|OPEN|") && lines[tam - 1].Contains("|CLOSE|"))
                {
                    return 0;
                }

                else
                {
                    return 2;
                }
            }

            else
            {
                return 2;
            }
        }

        private List<string> BuscaArquivos(DirectoryInfo dir, string ext)
        {
            List<string> File_Names = new List<string>();

            try
            {
                foreach (FileInfo file in dir.GetFiles())
                {
                    if (file.Extension == ext || file.Extension == ext.ToUpper())
                    {
                        if (file.Name != "logEnvio.log" && file.Name != "logConsulta.log" && file.Name.Contains("log_") == false)
                            File_Names.Add(file.Name);
                    }
                }
            }
            catch (Exception)
            {
                ClassException ex = new ClassException();
                ex.ExProcessos(3,"Erro ao buscar arquivos na pasta indicada");
            }

            return File_Names;
        }


        public void MoverConsultado(string filename)
        {
            string origem = Parametros.GetDirArq();
            string destino = Parametros.GetDirFim();

            string o = MontaCaminhoDir(origem, filename);
            string d = MontaCaminhoDir(destino, filename);

            try
            {
                System.IO.File.Move(o, d);
            }
            catch(Exception e)
            {
                ClassException ex = new ClassException();
                ex.ExProcessos(4,e.Message.ToString());
            }
        }

        public void GeraLogConsultaXML(string filename, string response)
        {
            DirectoryInfo di = new DirectoryInfo(string.Concat(Parametros.GetDirArq(), "\\logs\\retornoXML"));

            if (di.Exists == false)
                di.Create();
            var nome = string.Concat("log_", filename);

            string s = MontaCaminhoDir(string.Concat(Parametros.GetDirArq(), "\\logs\\retornoXML"), nome);

            int sti = 0;
            int stf = 0;

            string tagIni = "<retornoEventos>";
            string tagFim = "</retornoEventos>";

            sti = response.IndexOf(tagIni);
            stf = response.IndexOf(tagFim);

            var protocolo = response.Substring(sti, stf + tagFim.Length - sti);

            try
            {
                StreamWriter w = File.AppendText(@s);
                w.Write(protocolo);
                w.Close();
            }
            catch(Exception e)
            {
                ClassException ex = new ClassException();
                ex.ExProcessos(30, e.Message.ToString());
            }
        }

        public void GeraLogConsulta(string filename, string nroprt, string desc, int cd)
        {

            if (desc == "Pendente")
            {
                try
                {
                    if (IsConnected())
                    {
                        InsereLog(2,desc,filename,"Consulta","Tente fazer a consulta do protocolo novamente dentro de alguns minutos",nroprt,"");
                    }
                    else
                    {
                        InsereLog(2,"Erro, sem conexão com a internet!",filename,"Consulta","Conecte a uma rede para a consulta",nroprt,"");
                    }
                }
                catch (Exception e)
                {
                    ClassException ex = new ClassException();
                    ex.ExProcessos(4,e.Message.ToString());
                }

            }

            else if(cd == 2)
            {
                try
                {
                    InsereLog(2,desc,filename,"Consulta","Consulte o ambiente iVes na pagina de importação para ter o detalhe completo",nroprt,"");
                }
                catch (Exception e)
                {
                    ClassException ex = new ClassException();
                    ex.ExProcessos(4,e.Message.ToString());
                }
            }

            else if (cd == 99)
            {
                try
                {
                    InsereLog(2,desc,filename,"Consulta","Falha ao consultar o protocolo, por favor, consulte manualmente atraves do portal iVes",nroprt,"");
                }
                catch (Exception e)
                {
                    ClassException ex = new ClassException();
                    ex.ExProcessos(4,e.Message.ToString());
                }
            }

            else if(cd != 3)
            {
                try
                {
                    InsereLog(2,desc,filename,"Consulta","Tente fazer a consulta do protocolo novamente dentro de alguns minutos.",nroprt,"");
                }
                catch (Exception e)
                {
                    ClassException ex = new ClassException();
                    ex.ExProcessos(4,e.Message.ToString());
                }
            }

            else
            {
                try
                {
                    InsereLog(2,desc,filename,"Consulta","",nroprt,"");
                }
                catch (Exception e)
                {
                    ClassException ex = new ClassException();
                    ex.ExProcessos(5,e.Message.ToString());
                }
            }

            
        }

        public void GeraLogDetalhado(string filename, apiConsulta.consultaResponse retorno)
        {   

            try
            {
                int i = 0;
                foreach (var item in retorno.consultaProtocolo.retornoEventos)
                {
                  
                    if (item.erros != null || item.ocorrencias != null)
                    {
                        string nome = "";

                        if (item.idErp != null)
                        {
                            nome = string.Concat("log_",item.idErp, ".log");
                        }
                        else
                        {
                            nome = string.Concat("log_",item.id, ".log");
                        }

                        string s = MontaCaminhoDir(string.Concat(Parametros.GetDirArq(), "\\logs"), nome);

                        StreamWriter w = File.AppendText(@s);

                        w.WriteLine("");
                        w.WriteLine("Arquivo: " + filename);
                        w.WriteLine("");
                        w.WriteLine("Data: {0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                        w.WriteLine("");

                        if (item.id != null)
                        {
                            w.WriteLine("ID: " + item.id.ToString());
                        }
                        if(item.cdEvento != null)
                        {
                            w.WriteLine("Codigo do evento: " + item.cdEvento.ToString());
                        }
                        if (item.idErp != null)
                        {
                            w.WriteLine("ID ERP: " + item.idErp.ToString());
                        }
                        if(item.idIves != 0)
                        {
                            w.WriteLine("ID iVES: " + item.idIves.ToString());
                        }
                        if(item.nroProtocolo != null)
                        {
                            w.WriteLine("Nr Protocolo: " + item.nroProtocolo.ToString());
                        }
                        if(item.nroRecibo != null)
                        {
                            w.WriteLine("Nr Recibo: " + item.nroRecibo.ToString());
                        }
                        if(item.acao != null)
                        {
                            w.WriteLine("Ação: " + item.acao.ToString());
                        }
                        if(item.divergente != null)
                        {
                            w.WriteLine("Divergencia: " + item.divergente.ToString());
                        }
                        if(item.situacao != null)
                        {
                            w.WriteLine("Situação: " + item.situacao.ToString());
                        }
                        if(item.dtHrIntegra != null)
                        {
                            w.WriteLine("DtHrIntegra: " + item.dtHrIntegra.ToString());
                        }
                        if(item.dtHrProtocolo != null)
                        {
                            w.WriteLine("DtHrProtocolo: " + item.dtHrProtocolo.ToString());
                        }
                        if(item.dtHrRecibo != null)
                        {
                            w.WriteLine("DtHrRecibo: " + item.dtHrRecibo.ToString());
                        }
                        if(item.ocorrencias != null)
                        {
                            w.WriteLine("");
                            w.WriteLine("Divergencias:");
                            w.WriteLine("");
                            foreach (var diverg in item.ocorrencias.ocorrencia.divergencia)
                            {
                                w.WriteLine(diverg);
                            }

                            
                            if(item.ocorrencias.ocorrencia.estrutura.msg != null)
                            {
                                w.WriteLine("");
                                w.WriteLine("Estrutura:");
                                w.WriteLine("");
                                w.WriteLine(item.ocorrencias.ocorrencia.estrutura.msg);
                                if(item.ocorrencias.ocorrencia.estrutura.Text != null)
                                {
                                    w.WriteLine("");
                                    w.WriteLine("Texto: " + item.ocorrencias.ocorrencia.estrutura.Text);
                                }
                            }
                               
                        }

                        if(item.erros != null)
                        {
                            w.WriteLine("");
                            w.WriteLine("\nErros");
                            w.WriteLine("");
                            if (item.erros.Length > 0)
                            {
                                foreach (var erro in item.erros)
                                {
                                    if (erro.cdErro != 0)
                                    {
                                        w.WriteLine("Codigo do Erro: " + erro.cdErro.ToString());
                                        
                                    }
                                    if(erro.descErro != null)
                                    {
                                        w.WriteLine("Descrição do Erro: " + erro.descErro.ToString());
                                        w.WriteLine("");
                                    }
                                }
                            }
                        }

                        w.WriteLine("");
                        w.WriteLine("----------------------------------------");
                        w.WriteLine("");
                        w.Close();
                    }

                    i++;
                }
            }
            catch(Exception e)
            {
                ClassException ex = new ClassException();
                ex.ExProcessos(5,e.Message.ToString());
            }
            
        }

        public void GeraLogIntegra(string filename, string msg)
        {

            try
            {
                InsereLog(1,msg,filename,"Integra"," "," ", " ");
                //Log.AddLogEnvia(new LogEnvia { Id = 0, Msg = str, NomeArquivo = filename, Data = data, Hora = hora });
            }
            catch (Exception e)
            {
                ClassException ex = new ClassException();
                ex.ExProcessos(5,e.Message.ToString());
            }
        }

        public void GeraLogEnviaXML(string filename, string msg)
        {

            try
            {
                InsereLog(1,msg,filename,"Integra"," "," ", " ");
                //Log.AddLogEnvia(new LogEnvia { Id = 0, Msg = str, NomeArquivo = filename, Data = data, Hora = hora });
            }
            catch(Exception e)
            {
                ClassException ex = new ClassException();
                ex.ExProcessos(5,e.Message.ToString());
            }
        }
        
        public void InsereLog(int tipo, string msg, string arquivo, string servico, string acao, string protocolo, string coderro)
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

    }
}
