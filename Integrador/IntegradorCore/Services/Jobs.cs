﻿using System;
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
            DirectoryInfo di = new DirectoryInfo(string.Concat(StaticParametros.GetDirArq(), "\\logs"));

            if (di.Exists == false)
                di.Create();


            var ArquivosTXT = proc.ListarArquivos(".txt");
            var ArquivosXML = proc.ListarArquivos(".xml");

            if (ArquivosTXT.Count > 0)
            {
                foreach (var item in ArquivosTXT)
                {
                    var result = proc.VerificacaoIntegra(item);

                    if (result == 0)
                    {
                        var retorno = apiTXT.SendTXT(proc.LerArquivo(StaticParametros.GetDirArq(), item));

                        if (retorno.protocolo > 0)
                        {
                            proc.SalvaProtocolo(retorno, item);
                            proc.GeraLogIntegra(item, "Foi enviado com sucesso!");
                        }
                        else
                        {
                            proc.GeraLogIntegra(item, "Erro no envio, retorno invalido!");
                        }
                    }
                    else if (result == 1)
                    {
                        proc.GeraLogIntegra(item, "Já foi enviado!");
                    }
                    else
                    {
                        proc.GeraLogIntegra(item, "É invalido");
                    }
                }
            }

            if (ArquivosXML.Count > 0)
            {
                foreach (var item in ArquivosXML)
                {
                    if (proc.VerificacaoEnviaLote(item) == true)
                    {
                        var xmlString = proc.MontaXML(item);
                        var response = apiXML.SendXML(xmlString);
                        if (proc.VerificaResponseXML(response) == true)
                        {
                            proc.SalvaProtocoloXML(item, response, 1);
                            proc.GeraLogEnviaXML(item, "Foi enviado com sucesso!");
                        }
                        else
                        {
                            proc.GeraLogEnviaXML(item, "Não foi enviado");
                        }
                    }
                    else
                    {
                        proc.GeraLogEnviaXML(item, "Já foi enviado!");
                    }
                }
            }
        }

        public void Consulta()
        {
            DirectoryInfo di = new DirectoryInfo(string.Concat(StaticParametros.GetDirArq(), "\\logs"));

            if (di.Exists == false)
                di.Create();

            var ArquivosTXT = proc.ListarArquivos(".txt");
            var ArquivosXML = proc.ListarArquivos(".xml");

            if (ArquivosTXT.Count > 0)
            {
                foreach (var item in ArquivosTXT)
                {
                    bool result = Armazenamento.ExistsProtocolo(item);

                    if (result == true)
                    {
                        try
                        {
                            var protocolo = Armazenamento.GetProtocolo(item);

                            if(protocolo != null)
                            {
                                var retorno = apiConTXT.ConsultaProtocolo(protocolo, item);

                                proc.GeraLogConsulta(item, retorno.consultaProtocolo.identificador.protocolo.ToString()
                                                        , Convert.ToString(retorno.consultaProtocolo.status.descResposta)
                                                        , Convert.ToInt32(retorno.consultaProtocolo.status.cdResposta));

                                //processo.GeraLogDetalhado(filename, Response);

                                //proc.CreateFileRetornoTXT(item);

                                if (retorno.consultaProtocolo.status.cdResposta == 3 || retorno.consultaProtocolo.status.cdResposta == 2)
                                {
                                    proc.MoverConsultado(item);
                                    Armazenamento.DeleteProtocolo(item);
                                }
                            }
                            
                        }
                        catch(Exception e)
                        {
                            ex.Exception(e.Message, item, "ConsultaTXT", "");
                        }
                    }


                }
            }

            if (ArquivosXML.Count > 0)
            {
                foreach (var item in ArquivosXML)
                {
                    bool result = Armazenamento.ExistsProtocolo(item);

                    if (result == true)
                    {
                        var protocolo = Armazenamento.GetProtocolo(item);

                        if(protocolo != null)
                        {
                            var retorno = apiConXML.ConsultaProtocolo(protocolo.NroProtocolo, protocolo.Base, item);

                            try
                            {
                                proc.GeraLogConsultaXML(item, retorno, protocolo.NroProtocolo);

                                if (proc.VerificaConsultaXML(retorno) == true)
                                {
                                    proc.MoverConsultado(item);
                                    Armazenamento.DeleteProtocolo(item);
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

            try
            {
                System.IO.File.Delete(string.Concat(StaticParametros.GetDirArq(), "\\logs\\retornoTXT\\buffer.dat"));
            }
            catch (Exception e)
            {
                ex.Exception(e.Message, "buffer.dat", "ConsultaTXT", "");
            }
        }

        public void GetXMLDataBase()
        {
            if(StaticParametros.GetIntegraBanco() == true)
            {
                OracleDB.GetData();
            }
        }

        public int EnviaDB()
        {
            GetXMLDataBase();
            int ctrl = 0;
            EnviaXML apiXMLTeste = new EnviaXML(StaticParametros.GetGrupo(), StaticParametros.GetToken(), true);
            var lista = Armazenamento.GetProtocolosDBEnv();

            if (lista != null)
            {
                ctrl = 1;
                foreach (var item in lista)
                {
                    var xmlString = proc.MontaXMLDB(item.idEvento, item.xmlEvento);
                    var response = apiXMLTeste.SendXML(xmlString);
                    if (proc.VerificaResponseXML(response) == true)
                    {
                        proc.SalvaProtocoloXML(item.idEvento, response, 2);
                        Armazenamento.AddProtocoloDB(new ProtocoloDB { idEvento = item.idEvento, baseEnv = Convert.ToString(true) });
                        proc.GeraLogEnviaXML(item.idEvento, "Foi enviado com sucesso!");
                    }
                    else
                    {
                        proc.GeraLogEnviaXML(item.idEvento, "Não foi enviado");
                    }
                }
            }

            return ctrl;
        }

        public void ConsultaDB()
        {
            ConsultaXML apiConXMLTeste = new ConsultaXML(StaticParametros.GetGrupo(), StaticParametros.GetToken());
            var lista = Armazenamento.GetProtocolosDBCon();
            if(lista != null)
            {
                foreach (var item in lista)
                {
                    var retorno = apiConXML.ConsultaProtocolo(item.nroProt, item.baseEnv, item.idEvento);

                    try
                    {
                        //proc.GeraLogConsultaXML(item.idEvento, retorno, item.nroProt);

                        if (proc.VerificaXMLRetornoConsulta(retorno) == true)
                        {
                            var xmlRec = proc.ExtraiXMLRecibo(retorno);
                            var nrRec = proc.ExtraiNumRecibo(retorno);
                            Armazenamento.AddProtocoloDB(new ProtocoloDB { idEvento = item.idEvento, xmlRec = xmlRec, nroRec = nrRec, consultado = true });
                        }
                        else
                        {
                            var erros = proc.ExtraiErrosXmlDB(retorno);
                            Armazenamento.AddProtocoloDB(new ProtocoloDB { idEvento = item.idEvento, erros = erros, consultado = true });
                        }
                    }
                    catch (Exception e)
                    {
                        ex.Exception(e.Message, item.idEvento, "Consulta", "Tente consultar novamente em alguns minutos");
                    }
                }
            }
        }

        public void UpdateDB()
        {
            var lista = Armazenamento.GetProtocolosDBReadyUpdate();
            if(lista != null)
            {
                foreach (var item in lista)
                {
                    try
                    {
                        if(!item.salvoDB)
                        {
                            var ret = OracleDB.UpdateDB(item);

                            if (ret == true)
                            {
                                Armazenamento.updateSalvoDB(item);
                            }
                        }
                        
                    }
                    catch(Exception ex)
                    {

                    }
                    
                }
            }
        }
    }
}
