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

        public string MontaCaminhoDir(string dir, string name)
        {
            var s = string.Concat(dir, '\\', name);

            return s;
        }

        public List<string> Listar_arquivos(string ext)
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
                    if (file.Extension == ext)
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


        public void Mover_Consultado(string filename)
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

        public void GeraLogConsulta(string filename, string nroprt, string desc, int cd, TextWriter w)
        {
            if (cd == 0)
            {
                w.Write("\r\nLog consulta: ");
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                w.WriteLine("Arquivo: {0}", filename);
                w.WriteLine("Número protocolo: {0}", nroprt);
                if(IsConnected())
                {
                    w.WriteLine("Descrição: Erro!");
                    w.WriteLine("Consulte o ambiente iVes na pagina de importação para ter o detalhe completo");
                }
                else
                    w.WriteLine("Descrição: Erro, sem conexão com a internet!");
                
                w.WriteLine("-------------------------------");
            }

            else if(cd == 2)
            {
                w.Write("\r\nLog consulta: ");
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                w.WriteLine("Arquivo: {0}", filename);
                w.WriteLine("Número protocolo: {0}", nroprt);
                w.WriteLine("Descrição: {0}", desc);
                w.WriteLine("Consulte o ambiente iVes na pagina de importação para ter o detalhe completo");
                w.WriteLine("-------------------------------");
            }

            else if(cd != 3)
            {
                w.Write("\r\nLog consulta: ");
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                w.WriteLine("Arquivo: {0}", filename);
                w.WriteLine("Número protocolo: {0}", nroprt);
                w.WriteLine("Descrição: {0}", desc);
                w.WriteLine("Tente fazer a consulta do protocolo novamente dentro de alguns minutos");
                w.WriteLine("-------------------------------");
            }
            else
            {
                w.Write("\r\nLog consulta: ");
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                w.WriteLine("Arquivo: {0}", filename);
                w.WriteLine("Número protocolo: {0}", nroprt);
                w.WriteLine("Descrição: {0}", desc);
                w.WriteLine("-------------------------------");
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

        public void GeraLogIntegra(string filename, string str, TextWriter w)
        {
            w.Write("\r\nLog integra: ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("Arquivo: {0}", filename);
            w.WriteLine(str);
            w.WriteLine("-------------------------------");
        }

    }
}
