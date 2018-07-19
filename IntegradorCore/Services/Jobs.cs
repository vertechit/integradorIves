using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegradorCore.DAO;
using IntegradorCore.API;
using IntegradorCore.Modelos;
using IntegradorCore.Services;
using System.Threading;
using IntegradorCore.NHibernate.DAO;
using IntegradorCore.NHibernate;
using NHibernate;

namespace IntegradorCore.Services
{
    public class Jobs
    {
        Processos proc = new Processos();
        EnviaTXT apiTXT = new EnviaTXT(StaticParametros.GetGrupo(), StaticParametros.GetToken(), StaticParametros.GetAmbiente(), StaticParametros.GetBase());
        EnviaXML apiXML = new EnviaXML(StaticParametros.GetGrupo(), StaticParametros.GetToken(), StaticParametros.GetBase());
        ConsultaTXT apiConTXT = new ConsultaTXT(StaticParametros.GetGrupo(), StaticParametros.GetToken());
        ConsultaXML apiConXML = new ConsultaXML(StaticParametros.GetGrupo(), StaticParametros.GetToken());
        ExceptionCore ex = new ExceptionCore();

        public void Envia()
        {
            var sessao = AuxiliarNhibernate.AbrirSessao();
            DirectoryInfo di = new DirectoryInfo(string.Concat(StaticParametros.GetDirArq(), "\\logs"));

            if (di.Exists == false)
                di.Create();


            var ArquivosTXT = proc.ListarArquivos(".txt");
            var ArquivosXML = proc.ListarArquivos(".xml");

            if (ArquivosTXT.Count > 0)
            {
                foreach (var item in ArquivosTXT)
                {
                    var result = proc.VerificacaoIntegra(item, sessao);

                    if (result == 0)
                    {
                        var retorno = apiTXT.SendTXT(proc.LerArquivo(StaticParametros.GetDirArq(), item));

                        if (retorno.protocolo > 0)
                        {
                            proc.SalvaProtocolo(retorno, item, sessao);
                            proc.GeraLogIntegra(item, "Foi enviado com sucesso!");
                        }
                        else
                        {
                            proc.GeraLogIntegra(item, "Erro no envio, retorno invalido!");
                        }
                    }
                    else if (result == 1)
                    {
                        //proc.GeraLogIntegra(item, "Já foi enviado!");
                    }
                    else
                    {
                        proc.GeraLogIntegra(item, "O arquivo está com o formato inválido");
                    }
                }
            }

            if (ArquivosXML.Count > 0)
            {
                foreach (var item in ArquivosXML)
                {
                    if (proc.VerificacaoEnviaLote(item, sessao) == true)
                    {
                        var xmlString = proc.MontaXML(item);
                        var response = apiXML.SendXML(xmlString, item);
                        if (proc.VerificaResponseXML(response) == true)
                        {
                            proc.SalvaProtocoloXML(item, response, 1, sessao);
                            proc.GeraLogEnviaXML(item, "Foi enviado com sucesso!");
                        }
                        else
                        {
                            proc.GeraLogEnviaXML(item, "Não foi enviado");
                        }
                    }
                    else
                    {
                        //proc.GeraLogEnviaXML(item, "Já foi enviado!");
                    }
                }
            }

            proc.RemoveFileBuffer();

            sessao.Close();
        }

        public void Consulta()
        {
            var sessao = AuxiliarNhibernate.AbrirSessao();
            //var ProtocoloDAO = new ProtocoloDAO(sessao);

            DirectoryInfo di = new DirectoryInfo(string.Concat(StaticParametros.GetDirArq(), "\\logs"));

            if (di.Exists == false)
                di.Create();

            var ArquivosTXT = proc.ListarArquivos(".txt");
            var ArquivosXML = proc.ListarArquivos(".xml");

            if (ArquivosTXT.Count > 0)
            {
                foreach (var item in ArquivosTXT)
                {
                    bool result = false;

                    var prot = proc.RetornaProtocolo(item, sessao);//ProtocoloDAO.BuscarPorNomeArquivo(item);
                    try
                    {
                        var a = prot.NroProtocolo;
                        result = true;
                    }
                    catch (Exception e)
                    {

                    }
                    //Armazenamento.ExistsProtocolo(item);

                    if (result == true)
                    {
                        if(prot.Ambiente == StaticParametros.GetAmbiente() && Convert.ToBoolean(prot.Base) == StaticParametros.GetBase())
                        {
                            try
                            {
                                //var protocolo = Armazenamento.GetProtocolo(item);
                                var retorno = apiConTXT.ConsultaProtocolo(prot, item);

                                proc.GeraLogConsulta(item, retorno.consultaProtocolo.identificador.protocolo.ToString()
                                                        , Convert.ToString(retorno.consultaProtocolo.status.descResposta)
                                                        , Convert.ToInt32(retorno.consultaProtocolo.status.cdResposta));

                                //processo.GeraLogDetalhado(filename, Response);
                                try
                                {
                                    proc.CreateFileRetornoTXT(item);
                                }
                                catch(Exception e)
                                {

                                }

                                if (retorno.consultaProtocolo.status.cdResposta == 3 || retorno.consultaProtocolo.status.cdResposta == 2)
                                {
                                    proc.MoverConsultado(item);
                                    proc.RemoveProtocolo(prot, sessao);//ProtocoloDAO.Remover(prot);//Armazenamento.DeleteProtocolo(item);
                                }

                            }
                            catch (Exception e)
                            {
                                ex.Exception(e.Message, item, "Consulta", "");
                            }
                        }
                        
                    }


                }
            }

            if (ArquivosXML.Count > 0)
            {
                foreach (var item in ArquivosXML)
                {
                    bool result = false;
                    var prot = proc.RetornaProtocolo(item, sessao);//ProtocoloDAO.BuscarPorNome(item);

                    try
                    {
                        var a = prot.NroProtocolo;
                        result = true;
                    }
                    catch (Exception e)
                    {

                    }
                    //bool result = Armazenamento.ExistsProtocolo(item);

                    if (result == true)
                    {
                        //var protocolo = Armazenamento.GetProtocolo(item);
                        if (prot.Ambiente == StaticParametros.GetAmbiente() && Convert.ToBoolean(prot.Base) == StaticParametros.GetBase())
                        {
                            var retorno = apiConXML.ConsultaProtocolo(prot.NroProtocolo, prot.Base, item);

                            try
                            {
                                if (retorno != "")
                                {
                                    if (proc.VerificaConsultaXML(retorno) == true)
                                    {
                                        proc.GeraLogConsultaXML(item, retorno, prot.NroProtocolo);
                                        proc.MoverConsultado(item);
                                        proc.RemoveProtocolo(prot, sessao);//ProtocoloDAO.Remover(prot);//Armazenamento.DeleteProtocolo(item);
                                    }
                                }

                            }
                            catch (Exception e)
                            {
                                ex.Exception(e.Message, item, "Consulta", "Tente consultar novamente em alguns minutos");
                            }
                        }
                    }
                }
            }

            proc.RemoveFileBuffer();

            sessao.Close();
        }

        public void EnviaDB()
        {
            var sessao = AuxiliarNhibernate.AbrirSessao();
            ProtocoloDB_DAO ProtocoloDAO = new ProtocoloDB_DAO(sessao);

            SelectDriverToGetXMLOnDataBase(sessao);

            //EnviaXML apiXMLTeste = new EnviaXML(StaticParametros.GetGrupo(), StaticParametros.GetToken(), true);
            var lista = ProtocoloDAO.BuscaEnvio();//Armazenamento.GetProtocolosDBEnv();

            if (lista.Count > 0)
            {
                foreach (var item in lista)
                {
                    var apiXMLTeste = new EnviaXML(StaticParametros.GetGrupo(), StaticParametros.GetToken(), Convert.ToBoolean(item.baseEnv));

                    if (item.driver == StaticParametersDB.GetDriver())
                    {
                        var xmlString = proc.MontaXMLDB(item.idEvento, item.xmlEvento);
                        var response = apiXMLTeste.SendXML(xmlString, item.idEvento);
                        if (proc.VerificaResponseXML(response) == true)
                        {
                            proc.SalvaProtocoloXML(item.idEvento, response, 2, sessao);
                            var data = proc.RetornaData();
                            var nprot = new ProtocoloDB { idEvento = item.idEvento, dtenvio = data[0] };
                            ProtocoloDAO.Salvar(nprot);
                            proc.GeraLogEnviaXML(item.idEvento, "Foi enviado com sucesso!");
                        }
                        else
                        {
                            proc.GeraLogEnviaXML(item.idEvento, "Não foi enviado");
                        }
                    }
                    
                }
            }

            proc.RemoveFileBuffer();

            sessao.Close();
        }

        public void ConsultaDB()
        {
            var sessao = AuxiliarNhibernate.AbrirSessao();
            ProtocoloDB_DAO ProtocoloDAO = new ProtocoloDB_DAO(sessao);

            ConsultaXML apiConXMLTeste = new ConsultaXML(StaticParametros.GetGrupo(), StaticParametros.GetToken());
            var lista = ProtocoloDAO.BuscaConsulta();
            if (lista.Count > 0)
            {
                foreach (var item in lista)
                {
                    if (item.driver == StaticParametersDB.GetDriver())
                    {
                        var retorno = apiConXML.ConsultaProtocolo(item.nroProt, item.baseEnv, item.idEvento);

                        try
                        {
                            //proc.GeraLogConsultaXML(item.idEvento, retorno, item.nroProt);
                            if(proc.VerificaConsultaXML(retorno) == true)
                            {
                                if (proc.VerificaXMLRetornoConsulta(retorno) == true)
                                {
                                    var xmlRec = proc.ExtraiXMLRecibo(retorno);
                                    var nrRec = proc.ExtraiNumRecibo(retorno);
                                    var nrProtgov = proc.ExtraiNumProtGov(xmlRec);
                                    var data = proc.RetornaData();
                                    var prot = new ProtocoloDB { idEvento = item.idEvento, xmlRec = xmlRec, nroRec = nrRec, consultado = true, dtconsulta = data[0], nroProtGov = nrProtgov };
                                    ProtocoloDAO.Salvar(prot);
                                    //Armazenamento.AddProtocoloDB(new ProtocoloDB { idEvento = item.idEvento, xmlRec = xmlRec, nroRec = nrRec, consultado = true });
                                }
                                else
                                {
                                    var erros = proc.ExtraiErrosXmlDB(retorno);
                                    var data = proc.RetornaData();
                                    var prot = new ProtocoloDB { idEvento = item.idEvento, erros = erros, consultado = true, dtconsulta = data[0] };
                                    ProtocoloDAO.Salvar(prot);
                                    //Armazenamento.AddProtocoloDB(new ProtocoloDB { idEvento = item.idEvento, erros = erros, consultado = true });
                                }
                            }
                            else
                            {
                                //NoOp
                            }
                            
                        }
                        catch (Exception e)
                        {
                            ex.Exception(e.Message, item.idEvento, "Consulta", "Tente consultar novamente em alguns minutos");
                        }
                    }
                }
            }

            proc.RemoveFileBuffer();

            sessao.Close();
        }

        public void UpdateDB()
        {
            var sessao = AuxiliarNhibernate.AbrirSessao();
            ProtocoloDB_DAO ProtocoloDAO = new ProtocoloDB_DAO(sessao);

            var lista = ProtocoloDAO.BuscaParaAtualizarBanco();
            //Armazenamento.GetProtocolosDBReadyUpdate();
            if (lista.Count > 0)
            {
                foreach (var item in lista)
                {
                    if (item.driver == StaticParametersDB.GetDriver())
                    {
                        try
                        {
                            if (!item.salvoDB)
                            {
                                var ret = SelectDriverToUpdate(item);

                                if (ret == true)
                                {
                                    item.salvoDB = true;
                                    ProtocoloDAO.Salvar(item);
                                    //Armazenamento.updateSalvoDB(item);
                                }
                            }

                        }
                        catch (Exception ex)
                        {

                        }
                    }

                }
            }

            sessao.Close();
        }

        public void SelectDriverToGetXMLOnDataBase(ISession sessao)
        {
            if (StaticParametros.GetIntegraBanco() == true)
            {
                /*if (StaticParametersDB.GetDriver() == "oracle")
                    OracleDB.GetData(sessao);

                else
                {
                    SQLServerDB.GetData(sessao);
                }*/

                Banco.GetData(sessao);
            }
        }

        public bool SelectDriverToUpdate(ProtocoloDB prot)
        {
            if (StaticParametros.GetIntegraBanco() == true)
            {
                /*if (StaticParametersDB.GetDriver() == "oracle")
                {
                    return OracleDB.UpdateDB(prot);
                }

                else
                {
                    return SQLServerDB.UpdateDB(prot);
                }*/

                return Banco.UpdateDB(prot);

            }

            return false;
        }
    }
}
