using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using IntegradorCore.DAO;
using IntegradorCore.Modelos;
using IntegradorCore.Services;
using System.Security.AccessControl;
using System.Runtime.InteropServices;
using System.Data;
using System.Diagnostics;
using System.Security.Principal;
using System.Reflection;
using System.Net.NetworkInformation;
using IntegradorCore.NHibernate;
using IntegradorCore.NHibernate.DAO;
using NHibernate;
using System.Xml.Linq;
using System.ComponentModel;

namespace IntegradorCore.Services
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

        public string GetMacAdress()
        {
            var networks = NetworkInterface.GetAllNetworkInterfaces();
            var activeNetworks = networks.Where(ni => ni.OperationalStatus == OperationalStatus.Up && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback);
            var sortedNetworks = activeNetworks.OrderByDescending(ni => ni.Speed);
            return sortedNetworks.First().GetPhysicalAddress().ToString();
            //return NetworkInterface.GetAllNetworkInterfaces().Where(nic => nic.OperationalStatus == OperationalStatus.Up).Select(nic => nic.GetPhysicalAddress().ToString()).FirstOrDefault();
        }

        public string[] RetornaData()
        {

            string stri = Convert.ToString(DateTime.Now);
            string[] strArr = null;
            char[] splitchar = { ' ' };

            return strArr = stri.Split(splitchar);
        }

        public void CriarPastas()
        {
            DirectoryInfo di1 = new DirectoryInfo(string.Concat(StaticParametros.GetDirOrigem(), "\\VPGP"));
            DirectoryInfo di2 = new DirectoryInfo(string.Concat(StaticParametros.GetDirOrigem(), "\\VPGT"));
            DirectoryInfo di3 = new DirectoryInfo(string.Concat(StaticParametros.GetDirOrigem(), "\\VTGT"));

            if (di1.Exists == false)
                di1.Create();

            if (di2.Exists == false)
                di2.Create();

            if (di3.Exists == false)
                di3.Create();

            StaticParametros.SetVPGP();
            StaticParametros.SetVPGT();
            StaticParametros.SetVTGT();

            CriarSubPastas();
        }

        public void CriarSubPastas()
        {
            DirectoryInfo di1 = new DirectoryInfo(string.Concat(StaticParametros.GetDirOrigem(), "\\VPGP\\Consultados"));
            DirectoryInfo di2 = new DirectoryInfo(string.Concat(StaticParametros.GetDirOrigem(), "\\VPGT\\Consultados"));
            DirectoryInfo di3 = new DirectoryInfo(string.Concat(StaticParametros.GetDirOrigem(), "\\VTGT\\Consultados"));

            if (di1.Exists == false)
                di1.Create();

            if (di2.Exists == false)
                di2.Create();

            if (di3.Exists == false)
                di3.Create();
        }

        public void AlteraParametro(int tipo)
        {
            if(tipo == 1)
            {
                var param = StaticParametros.GetVPGP();
                StaticParametros.SetAmbiente(param.Ambiente);
                StaticParametros.SetBase(Convert.ToBoolean(param.Base));
                StaticParametros.SetDirArq(param.CaminhoDir);
                StaticParametros.SetDirFim(string.Concat(param.CaminhoDir, "\\Consultados"));
            }
            else if(tipo == 2)
            {
                var param = StaticParametros.GetVPGT();
                StaticParametros.SetAmbiente(param.Ambiente);
                StaticParametros.SetBase(Convert.ToBoolean(param.Base));
                StaticParametros.SetDirArq(param.CaminhoDir);
                StaticParametros.SetDirFim(string.Concat(param.CaminhoDir, "\\Consultados"));
            }
            else if(tipo == 3)
            {
                var param = StaticParametros.GetVTGT();
                StaticParametros.SetAmbiente(param.Ambiente);
                StaticParametros.SetBase(Convert.ToBoolean(param.Base));
                StaticParametros.SetDirArq(param.CaminhoDir);
                StaticParametros.SetDirFim(string.Concat(param.CaminhoDir, "\\Consultados"));
            }
            else
            {
                //noOp
            }
        }

        public string MontaCaminhoDir(string dir, string name)
        {
            var s = string.Concat(dir, '\\', name);

            return s;
        }

        public string MontaXML(string arqname)
        {
            string[] xmlLinhas = LerArquivo(StaticParametros.GetDirArq(), arqname);
            var xmlString = "";
            var tagIni = "<tpAmb>";
            var tagFim = "</tpAmb>";
            foreach (var item in xmlLinhas)
            {
                xmlString = String.Concat(xmlString, item);
            }
            if (xmlString.Contains("<tpAmb>1</tpAmb>"))
            {
                xmlString = xmlString.Replace("<tpAmb>1</tpAmb>", string.Concat(tagIni, StaticParametros.GetAmbiente(), tagFim));
            }
            else if (xmlString.Contains("<tpAmb>2</tpAmb>"))
            {
                xmlString = xmlString.Replace("<tpAmb>2</tpAmb>", string.Concat(tagIni, StaticParametros.GetAmbiente(), tagFim));
            }
            else if (xmlString.Contains("<tpAmb>3</tpAmb>"))
            {
                xmlString = xmlString.Replace("<tpAmb>3</tpAmb>", string.Concat(tagIni, StaticParametros.GetAmbiente(), tagFim));
            }

            string xsdInicio = "<eSocial><envioLoteEventos grupo=\"1\"><eventos><evento Id=\"123\">";

            xsdInicio = xsdInicio.Replace("123", arqname);

            string xsdFim = " </evento></eventos></envioLoteEventos></eSocial>";

            var xml = String.Concat(xsdInicio, xmlString, xsdFim);

            return xml;
        }

        public string MontaXMLDB(string idevento, string Xml)
        {

            string xsdInicio = "<eSocial><envioLoteEventos grupo=\"1\"><eventos><evento Id=\"123\">";

            xsdInicio = xsdInicio.Replace("123", idevento);

            string xsdFim = " </evento></eventos></envioLoteEventos></eSocial>";

            var xml = String.Concat(xsdInicio, Xml, xsdFim);

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

        public bool VerificaXMLRetornoConsulta(string xml)
        {
            var v1 = xml.IndexOf("<recibo>");
            var v2 = xml.IndexOf("</recibo>");

            if(v1 != -1 && v2 != -1)
                return true;

            return false;
        }

        public string ExtraiXMLRecibo(string xml)
        {
            var tagIni = "<retornoEventos>";
            var tagFim = "</retornoEventos>";

            int sti = xml.IndexOf(tagIni) + tagIni.Length;
            int stf = xml.IndexOf(tagFim) - tagFim.Length;

            var nwxml = xml.Substring(sti, stf + tagFim.Length - sti);

            var tagIniE = "<retornoEvento>";
            var tagFimE = "</retornoEvento>";

            int stie = nwxml.IndexOf(tagIniE) + tagIniE.Length;
            int stfe = nwxml.LastIndexOf(tagFimE) - tagFimE.Length;

            var xm = nwxml.Substring(stie, stfe + tagFimE.Length - stie);

            //xm = xm.Replace("\r\n", "");

            //xm = xm.Replace(" ", "");

            return xm;
        }

        public string ExtraiNumRecibo(string xml)
        {
            var tagIni = "<nrRecibo>";
            var tagFim = "</nrRecibo>";

            int sti = xml.IndexOf(tagIni) + tagIni.Length;
            int stf = xml.IndexOf(tagFim) - tagFim.Length;

            var nrRec = xml.Substring(sti, stf + tagFim.Length - sti);

            return nrRec;
        }

        public string ExtraiErrosXmlDB(string xml)
        {
            var tagIni = "<ocorrencias>";
            var tagFim = "</ocorrencias>";

            int sti = xml.IndexOf(tagIni);// + tagIni.Length;
            int stf = xml.IndexOf(tagFim);// - tagFim.Length;

            var nwxml = xml.Substring(sti, stf + tagFim.Length - sti);
            //nwxml.Replace();
            var XML = System.Xml.Linq.XElement.Parse(nwxml);

            return XML.Value;
        }

        public List<string> ListarArquivos(string ext)
        {
            string dir = StaticParametros.GetDirArq();
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
                ExceptionCore ex = new ExceptionCore();
                ex.ExProcessos(1, "Erro na leitura do arquivo");
            }

            return lines;
        }

        public Boolean VerificaResponseXML(string response)
        {
            if (response == "")
            {
                return false;
            }
            var tagIni = "<cdResposta>";
            var tagFim = "</cdResposta>";

            int sti = response.IndexOf(tagIni) + 12;
            int stf = response.IndexOf(tagFim) - 13;

            var codigo = response.Substring(sti, stf + tagFim.Length - sti);

            if (codigo == "201")
            {
                return true;
            }

            return false;
        }

        public void SalvaProtocolo(apiEnviaTXT.integraResponse Response, string arq, ISession sessao)
        {
            var protocoloDAO = new ProtocoloDAO(sessao); 
            try
            {
                var prot = new Protocolo { Id = 0, NomeArquivo = arq, NroProtocolo = Convert.ToString(Response.protocolo), Base = Convert.ToString(StaticParametros.GetBase()), Ambiente = Convert.ToInt64(StaticParametros.GetAmbiente()) };
                //Armazenamento.AddProtocolo(new Protocolo { Id = 0, NomeArquivo = arq, NroProtocolo = Convert.ToString(Response.protocolo), Base = Convert.ToString(StaticParametros.GetBase()), Ambiente = Convert.ToInt64(StaticParametros.GetAmbiente()) });
                protocoloDAO.Salvar(prot);
            }
            catch (Exception e)
            {
                ExceptionCore ex = new ExceptionCore();
                ex.ExProcessos(2, e.Message.ToString());
            }
        }

        public void SalvaProtocoloXML(string id, string Response, int tipo, ISession sessao)
        {

            var tagIni = "<protocoloEnvio>";
            var tagFim = "</protocoloEnvio>";

            int sti = Response.IndexOf(tagIni) + 16;
            int stf = Response.IndexOf(tagFim) - 17;

            var protocolo = Response.Substring(sti, stf + tagFim.Length - sti);

            try
            {
                if(tipo == 1)
                {
                    var ProtocoloDAO = new ProtocoloDAO(sessao);
                    var prot = new Protocolo { Id = 0, NomeArquivo = id, NroProtocolo = protocolo, Base = Convert.ToString(StaticParametros.GetBase()), Ambiente = StaticParametros.GetAmbiente() };
                    ProtocoloDAO.Salvar(prot);
                    //Armazenamento.AddProtocolo(new Protocolo { Id = 0, NomeArquivo = id, NroProtocolo = protocolo, Base = Convert.ToString(StaticParametros.GetBase()) });
                }
                else
                {
                    ProtocoloDB_DAO ProtocoloDAO = new ProtocoloDB_DAO(sessao);
                    var prot = new ProtocoloDB { idEvento = id, nroProt = protocolo, xmlProt = Response };
                    //Armazenamento.AddProtocoloDB(new ProtocoloDB { idEvento = id, nroProt = protocolo, xmlProt = Response });
                    ProtocoloDAO.Salvar(prot);
                }
            }
            catch (Exception e)
            {
                ExceptionCore ex = new ExceptionCore();
                ex.ExProcessos(3, e.Message.ToString());
            }
        }

        public Boolean VerificacaoEnviaLote(string arq, ISession sessao)
        {
            string dir = StaticParametros.GetDirArq();
            DirectoryInfo diretorio = new DirectoryInfo(dir);

            bool result = false;
            var ProtocoloDAO = new ProtocoloDAO(sessao);
            var prot = ProtocoloDAO.BuscarPorNomeArquivo(arq);
            
            try
            {
                var a = prot.NroProtocolo;
                result = true;
            }catch(Exception e){

            }

            if (result == true)
            {
                return false;
            }

            return true;
        }

        public int VerificacaoIntegra(string arq, ISession sessao)
        {
            string dir = StaticParametros.GetDirArq();
            DirectoryInfo diretorio = new DirectoryInfo(dir);

            bool result = false;
            var ProtocoloDAO = new ProtocoloDAO(sessao);
            var prot = ProtocoloDAO.BuscarPorNomeArquivo(arq);
            try
            {
                var a = prot.NroProtocolo;
                result = true;
            }catch(Exception e){

            }

            if (result == true)
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
                ExceptionCore ex = new ExceptionCore();
                ex.ExProcessos(4, "Erro ao buscar arquivos na pasta indicada");
            }

            return File_Names;
        }


        public void MoverConsultado(string filename)
        {
            string origem = StaticParametros.GetDirArq();
            string destino = StaticParametros.GetDirFim();

            string o = MontaCaminhoDir(origem, filename);
            string d = MontaCaminhoDir(destino, filename);

            try
            {
                System.IO.File.Move(o, d);
            }
            catch (Exception e)
            {
                ExceptionCore ex = new ExceptionCore();
                ex.ExProcessos(5, e.Message.ToString());
            }
        }

        public void GeraLogConsultaXML(string filename, string response, string prot)
        {
            DirectoryInfo di = new DirectoryInfo(string.Concat(StaticParametros.GetDirArq(), "\\logs\\retornoXML"));

            if (di.Exists == false)
                di.Create();
            var nome = string.Concat("log_", filename);

            string s = MontaCaminhoDir(string.Concat(StaticParametros.GetDirArq(), "\\logs\\retornoXML"), nome);

            int sti = 0;
            int stf = 0;

            string tagIni = "<retornoEventos>";
            string tagFim = "</retornoEventos>";

            sti = response.IndexOf(tagIni);
            stf = response.IndexOf(tagFim);

            var protocolo = response.Substring(sti, stf + tagFim.Length - sti);

            try
            {
                var tagDescIni = "<descResposta>";
                var tagDescFim = "</descResposta>";

                int stiDesc = protocolo.IndexOf(tagDescIni);
                int stfDesc = protocolo.IndexOf(tagDescFim);

                var desc = protocolo.Substring(stiDesc, stfDesc + tagDescFim.Length - stiDesc);

                desc = desc.Replace(tagDescIni, "");
                desc = desc.Replace(tagDescFim, "");

                InsereLog(2, desc, filename, "Consulta", "Consulte a pasta de log para mais detalhes", prot, "");

            }
            catch (Exception e)
            {
                ExceptionCore ex = new ExceptionCore();
                ex.ExProcessos(5, e.Message.ToString());
            }

            try
            {
                System.IO.File.WriteAllText(@s, protocolo);

            }
            catch (Exception e)
            {
                ExceptionCore ex = new ExceptionCore();
                ex.ExProcessos(5, e.Message.ToString());
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
                        InsereLog(2, desc, filename, "Consulta", "Tente fazer a consulta do protocolo novamente dentro de alguns minutos", nroprt, "");
                    }
                    else
                    {
                        InsereLog(2, "Erro, sem conexão com a internet!", filename, "Consulta", "Conecte a uma rede para a consulta", nroprt, "");
                    }
                }
                catch (Exception e)
                {
                    ExceptionCore ex = new ExceptionCore();
                    ex.ExProcessos(6, e.Message.ToString());
                }

            }

            else if (cd == 2)
            {
                try
                {
                    InsereLog(2, desc, filename, "Consulta", "Consulte o ambiente iVes na pagina de importação para ter o detalhe completo", nroprt, "");
                }
                catch (Exception e)
                {
                    ExceptionCore ex = new ExceptionCore();
                    ex.ExProcessos(6, e.Message.ToString());
                }
            }

            else if (cd == 99)
            {
                try
                {
                    InsereLog(2, desc, filename, "Consulta", "Falha ao consultar o protocolo, por favor, consulte manualmente atraves do portal iVes", nroprt, "");
                }
                catch (Exception e)
                {
                    ExceptionCore ex = new ExceptionCore();
                    ex.ExProcessos(6, e.Message.ToString());
                }
            }

            else if (cd != 3)
            {
                try
                {
                    InsereLog(2, desc, filename, "Consulta", "Tente fazer a consulta do protocolo novamente dentro de alguns minutos.", nroprt, "");
                }
                catch (Exception e)
                {
                    ExceptionCore ex = new ExceptionCore();
                    ex.ExProcessos(6, e.Message.ToString());
                }
            }

            else
            {
                try
                {
                    InsereLog(2, desc, filename, "Consulta", "", nroprt, "");
                }
                catch (Exception e)
                {
                    ExceptionCore ex = new ExceptionCore();
                    ex.ExProcessos(6, e.Message.ToString());
                }
            }


        }

        public void CreateFileBufferConsulta(string XML)
        {
            DirectoryInfo di = new DirectoryInfo(string.Concat(StaticParametros.GetDirArq(), "\\logs\\retornoTXT"));

            if (di.Exists == false)
                di.Create();

            try
            {
                System.IO.File.WriteAllText(string.Concat(StaticParametros.GetDirArq(), "\\logs\\retornoTXT\\buffer.dat"), XML);
            }
            catch (Exception err)
            {

            }

        }

        public void CreateFileRetornoTXT(string filename)
        {
            DirectoryInfo di = new DirectoryInfo(string.Concat(StaticParametros.GetDirArq(), "\\logs\\retornoTXT"));

            if (di.Exists == false)
                di.Create();
            var nome = string.Concat("log_", filename, ".xml");

            string s = MontaCaminhoDir(string.Concat(StaticParametros.GetDirArq(), "\\logs\\retornoTXT"), nome);

            var ret = LerArquivo(string.Concat(StaticParametros.GetDirArq(), "\\logs\\retornoTXT"), "buffer.dat");
            var xml = "";
            for (int i = 0; i < ret.Length; i++)
            {
                if (i != 0 && i != 1 && i != 2 && i != ret.Length - 1 && i != ret.Length - 2)
                {
                    xml = string.Concat(xml, ret[i]);
                }
            }

            try
            {
                System.IO.File.WriteAllText(@s, xml);
                //StreamWriter w = File.AppendText(@s);
                //w.Write(xml);
                //w.Close();
            }
            catch (Exception e)
            {
                ExceptionCore ex = new ExceptionCore();
                ex.ExProcessos(5, e.Message.ToString());
            }
        }

        public void GeraLogDetalhado(string filename, apiConsultaTXT.consultaResponse retorno)
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
                            nome = string.Concat("log_", item.idErp, ".log");
                        }
                        else
                        {
                            nome = string.Concat("log_", item.id, ".log");
                        }

                        DirectoryInfo di = new DirectoryInfo(string.Concat(StaticParametros.GetDirArq(), "\\logs\\retornoTXT"));

                        if (di.Exists == false)
                            di.Create();

                        string s = MontaCaminhoDir(string.Concat(StaticParametros.GetDirArq(), "\\logs\\retornoTXT"), nome);

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
                        if (item.cdEvento != null)
                        {
                            w.WriteLine("Codigo do evento: " + item.cdEvento.ToString());
                        }
                        if (item.idErp != null)
                        {
                            w.WriteLine("ID ERP: " + item.idErp.ToString());
                        }
                        if (item.idIves != 0)
                        {
                            w.WriteLine("ID iVES: " + item.idIves.ToString());
                        }
                        if (item.nroProtocolo != null)
                        {
                            w.WriteLine("Nr Protocolo: " + item.nroProtocolo.ToString());
                        }
                        if (item.nroRecibo != null)
                        {
                            w.WriteLine("Nr Recibo: " + item.nroRecibo.ToString());
                        }
                        if (item.acao != null)
                        {
                            w.WriteLine("Ação: " + item.acao.ToString());
                        }
                        if (item.divergente != null)
                        {
                            w.WriteLine("Divergencia: " + item.divergente.ToString());
                        }
                        if (item.situacao != null)
                        {
                            w.WriteLine("Situação: " + item.situacao.ToString());
                        }
                        if (item.dtHrIntegra != null)
                        {
                            w.WriteLine("DtHrIntegra: " + item.dtHrIntegra.ToString());
                        }
                        if (item.dtHrProtocolo != null)
                        {
                            w.WriteLine("DtHrProtocolo: " + item.dtHrProtocolo.ToString());
                        }
                        if (item.dtHrRecibo != null)
                        {
                            w.WriteLine("DtHrRecibo: " + item.dtHrRecibo.ToString());
                        }
                        if (item.ocorrencias != null)
                        {
                            w.WriteLine("");
                            w.WriteLine("Divergencias:");
                            w.WriteLine("");
                            foreach (var diverg in item.ocorrencias.ocorrencia.divergencia)
                            {
                                w.WriteLine(diverg);
                            }


                            if (item.ocorrencias.ocorrencia.estrutura.msg != null)
                            {
                                w.WriteLine("");
                                w.WriteLine("Estrutura:");
                                w.WriteLine("");
                                w.WriteLine(item.ocorrencias.ocorrencia.estrutura.msg);
                                if (item.ocorrencias.ocorrencia.estrutura.Text != null)
                                {
                                    w.WriteLine("");
                                    w.WriteLine("Texto: " + item.ocorrencias.ocorrencia.estrutura.Text);
                                }
                            }

                        }

                        if (item.erros != null)
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
                                    if (erro.descErro != null)
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
            catch (Exception e)
            {
                ExceptionCore ex = new ExceptionCore();
                ex.ExProcessos(7, e.Message.ToString());
            }

        }

        public void GeraLogIntegra(string filename, string msg)
        {

            try
            {
                InsereLog(1, msg, filename, "Integra", " ", " ", " ");
                //Log.AddLogEnvia(new LogEnvia { Id = 0, Msg = str, NomeArquivo = filename, Data = data, Hora = hora });
            }
            catch (Exception e)
            {
                ExceptionCore ex = new ExceptionCore();
                ex.ExProcessos(8, e.Message.ToString());
            }
        }

        public void GeraLogEnviaXML(string filename, string msg)
        {

            try
            {
                InsereLog(1, msg, filename, "Integra", " ", " ", " ");
                //Log.AddLogEnvia(new LogEnvia { Id = 0, Msg = str, NomeArquivo = filename, Data = data, Hora = hora });
            }
            catch (Exception e)
            {
                ExceptionCore ex = new ExceptionCore();
                ex.ExProcessos(9, e.Message.ToString());
            }
        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }

        public void LimpaLog()
        {
            var sessao = AuxiliarNhibernate.AbrirSessao();
            var logConsultDao = new LogConsultaDAO(sessao);
            var logEnvioDao = new LogEnvioDAO(sessao);
            var logErroDao = new LogErroDAO(sessao);
            //string err = "logerro";
            //string con = "logconsulta";
            //string env = "logenvia";

            DataTable dtErro = ConvertToDataTable(logErroDao.BuscaTodos());//Log.GetLogs(err);
            DataTable dtConsu = ConvertToDataTable(logConsultDao.BuscaTodos());//Log.GetLogs(con);
            DataTable dtEnvia = ConvertToDataTable(logEnvioDao.BuscaTodos());//Log.GetLogs(env);

            try
            {
                if (dtErro.Rows.Count > 0)
                {
                    var remover = new List<string>();

                    foreach (DataRow row in dtErro.Rows)
                    {
                        var dt = Convert.ToDateTime(row.ItemArray[5]);
                        var qtd = DiferencaDataDias(dt, DateTime.Now);
                        if (qtd > 25)
                        {
                            if (remover.Find(x => x.Contains(Convert.ToString(row.ItemArray[5]))) == null)
                                remover.Add(Convert.ToString(row.ItemArray[5]));
                            //Log.DeleteByID(err, Convert.ToInt32(row.ItemArray[0]));
                        }
                    }

                    foreach (var item in remover)
                    {
                        logErroDao.DeleteByData(item);
                        //Log.DeleteByData(err, item);
                    }
                }

                if (dtConsu.Rows.Count > 0)
                {
                    var remover = new List<string>();

                    foreach (DataRow row in dtConsu.Rows)
                    {
                        var dt = Convert.ToDateTime(row.ItemArray[5]);
                        var qtd = DiferencaDataDias(dt, DateTime.Now);
                        if (qtd > 25)
                        {
                            if (remover.Find(x => x.Contains(Convert.ToString(row.ItemArray[5]))) == null)
                                remover.Add(Convert.ToString(row.ItemArray[5]));
                            //Log.DeleteByID(con, Convert.ToInt32(row.ItemArray[0]));
                        }
                    }

                    foreach (var item in remover)
                    {
                        logConsultDao.DeleteByData(item);
                        //Log.DeleteByData(con, item);
                    }
                }

                if (dtEnvia.Rows.Count > 0)
                {
                    var remover = new List<string>();

                    foreach (DataRow row in dtEnvia.Rows)
                    {
                        var dt = Convert.ToDateTime(row.ItemArray[4]);//4
                        var qtd = DiferencaDataDias(dt, DateTime.Now);
                        if (qtd > 25)
                        {
                            if (remover.Find(x => x.Contains(Convert.ToString(row.ItemArray[4]))) == null)
                                remover.Add(Convert.ToString(row.ItemArray[4]));
                            //Log.DeleteByID(env, Convert.ToInt32(row.ItemArray[0]));
                        }
                    }

                    foreach (var item in remover)
                    {
                        logEnvioDao.DeleteByData(item);
                        //Log.DeleteByData(env, item);
                    }
                }
            }
            catch (Exception ex)
            {
                //noOp
            }

            sessao.Close();
        }

        public int DiferencaDataDias(DateTime Inicio, DateTime Fim)
        {
            TimeSpan ts = Fim.Subtract(Inicio);
            return Convert.ToInt32(ts.TotalDays);
        }

        public bool ValidaStaticParametros()
        {
            if (StaticParametros.GetDirOrigem() != null)
            {
                DirectoryInfo origem = new DirectoryInfo(StaticParametros.GetDirOrigem());
                if (origem.Exists == false)
                {
                    return false;
                }
            }
            
            FileInfo diTok = new FileInfo(StaticParametros.GetDirToke());

            

            if (diTok.Exists == false)
            {
                return false;
            }

            return true;
        }

        public bool ValidaStaticParamsJob()
        {
            try
            {
                DirectoryInfo diArq = new DirectoryInfo(StaticParametros.GetDirArq());
                DirectoryInfo diCon = new DirectoryInfo(StaticParametros.GetDirFim());
            }catch(Exception ex)
            {
                return false;
            }
            

            return true;
        }

        public void DefineNullParametros()
        {
            StaticParametros.SetDirArq(null);
            StaticParametros.SetDirFim(null);
        }

        public bool VerificaProcessoRun()
        {
            var isOpen = Process.GetProcesses().Any(p =>
            p.ProcessName == "iVesService");

            if (isOpen)
                return true;

            return false;
        }

        /*public void GerenciaServico(int action)
        {
            rodarComoAdmin();

            switch (action)
            {
                case 1:
                    try
                    {
                        ServicosWin.ServicosWin.StopService("Integrador Vertech Ives");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro : " + ex.Message + Environment.NewLine + ex.InnerException, "Parar Serviço");
                    }
                    break;
                case 2:
                    try
                    {
                        ServicosWin.ServicosWin.StartService("Integrador Vertech Ives");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro : " + ex.Message + Environment.NewLine + ex.InnerException, "Iniciar Serviço");
                    }

                    break;

                case 3:
                    try
                    {
                        ServicosWin.ServicosWin.RestartService("Integrador Vertech Ives");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro : " + ex.Message + Environment.NewLine + ex.InnerException, "Reiniciar Serviço");
                    }

                    break;
            }


        }*/

        /*private void rodarComoAdmin()
        {
            WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool administrativeMode = principal.IsInRole(WindowsBuiltInRole.Administrator);
            if (!administrativeMode)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.Verb = "runas";
                startInfo.FileName = Assembly.GetExecutingAssembly().CodeBase;
                try
                {
                    Process.Start(startInfo);
                    //MessageBox.Show("Você esta executando o projeto com nível de Administrador !", "Admin");
                }
                catch
                {
                    throw new Exception("Não foi possível conceder acesso como Admin" + Environment.NewLine + "As operações realizadas poderão ter Acesso Negado !");
                }
            }
        }*/

        public void InsereLog(int tipo, string msg, string arquivo, string servico, string acao, string protocolo, string coderro)
        {
            var sessao =  AuxiliarNhibernate.AbrirSessao();

            var logConsultDao = new LogConsultaDAO(sessao);
            var logEnvioDao = new LogEnvioDAO(sessao);
            var logErroDao = new LogErroDAO(sessao);

            var strArr = RetornaData();

            string hora = strArr[1];
            string data = strArr[0];

            if (tipo == 1)
            {
                logEnvioDao.Salvar(
                        new LogEnvia
                        {
                            Id = 0,
                            Msg = msg,
                            Acao = acao,
                            Identificador = arquivo,
                            Data = data,
                            Hora = hora
                        });
            }
            else if (tipo == 2)
            {
                logConsultDao.Salvar(
                        new LogConsulta
                        {
                            Id = 0,
                            Identificador = arquivo,
                            Protocolo = protocolo,
                            Msg = msg,
                            Acao = acao,
                            Data = data,
                            Hora = hora
                        });
            }
            else
            {
                logErroDao.Salvar(new LogErros
                {
                    Id = 0,
                    Servico = servico,
                    CodErro = coderro,
                    Msg = msg,
                    Acao = acao,
                    Data = data,
                    Hora = hora
                });
            }

            //sessao.Close();
        }

    }
}
