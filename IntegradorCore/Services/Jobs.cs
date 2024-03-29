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
                            proc.GeraLogIntegra(item, "Foi enviado com sucesso! "+ StaticParametros.GetUrl());
                        }
                        else
                        {
                            proc.GeraLogIntegra(item, "Erro no envio, retorno invalido!");
                        }
                    }
                    else if (result == 1)
                    {
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
                            proc.GeraLogEnviaXML(item, "Foi enviado com sucesso!" + StaticParametros.GetUrl());
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

                    var prot = proc.RetornaProtocolo(item, sessao);
                    try
                    {
                        var a = prot.NroProtocolo;
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }

                    if (result == true)
                    {
                        if (prot.Ambiente == StaticParametros.GetAmbiente() && prot.Base == StaticParametros.GetBase())
                        {
                            try
                            {
                                var retorno = apiConTXT.ConsultaProtocolo(prot, item);

                                proc.GeraLogConsulta(item, retorno.consultaProtocolo.identificador.protocolo.ToString()
                                                        , Convert.ToString(retorno.consultaProtocolo.status.descResposta)
                                                        , Convert.ToInt32(retorno.consultaProtocolo.status.cdResposta));

                                try
                                {
                                    proc.CreateFileRetornoTXT(item);
                                }
                                catch (Exception ex)
                                {
                                    ex.ToString();
                                }

                                if (retorno.consultaProtocolo.status.cdResposta == 3 || retorno.consultaProtocolo.status.cdResposta == 2)
                                {
                                    proc.MoverConsultado(item);
                                    proc.RemoveProtocolo(prot, sessao);
                                }

                            }
                            catch (Exception e)
                            {
                                ex.Exception(e.Message, item, "Consulta", "", e);
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
                    var prot = proc.RetornaProtocolo(item, sessao);

                    try
                    {
                        var a = prot.NroProtocolo;
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }

                    if (result == true)
                    {
                        if (prot.Ambiente == StaticParametros.GetAmbiente() && prot.Base == StaticParametros.GetBase())
                        {
                            var retorno = apiConXML.ConsultaProtocolo(prot.NroProtocolo, prot.Base, item);

                            try
                            {
                                if (retorno != "")
                                {
                                    if (proc.VerificaConsultaXML(retorno) == true)
                                    {
                                        proc.GeraLogConsultaXML(item, retorno, prot.NroProtocolo, 1);
                                        proc.MoverConsultado(item);
                                        proc.RemoveProtocolo(prot, sessao);
                                    }
                                }

                            }
                            catch (Exception e)
                            {
                                ex.Exception(e.Message, item, "Consulta", "Tente consultar novamente em alguns minutos", e);
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

            Banco.GetData(sessao);

            var lista = ProtocoloDAO.BuscaEnvio();

            if (lista.Count > 0)
            {
                foreach (var item in lista)
                {
                    var apiXMLDB = new EnviaXML(StaticParametersDB.GetGrupo(), StaticParametersDB.GetToken(), item.baseEnv);

                    if (item.driver == StaticParametersDB.GetDriver())
                    {
                        var xmlString = proc.MontaXMLDB(item.id, item.xmlEvento);
                        var response = apiXMLDB.SendXML(xmlString, item.id);
                        if (proc.VerificaResponseXML(response) == true)
                        {
                            proc.SalvaProtocoloXML(item.id, response, 2, sessao);

                            var data = proc.RetornaData();
                            var protocolo = new ProtocoloDB { id = item.id, dtenvio = data[0], hrenvio = data[1], status = "0 - Enviado", idDB = StaticParametersDB.GetId() };
                            ProtocoloDAO.Salvar(protocolo);
                            proc.GeraLogEnviaXML(item.id, "Foi enviado com sucesso!" + StaticParametros.GetUrl());

                            Banco.CustomUpdateDB(ProtocoloDAO.BuscarPorIDEvento(item.id), 3);
                        }
                        else
                        {
                            proc.GeraLogEnviaXML(item.id, "Não foi enviado");
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

            var lista = ProtocoloDAO.BuscaConsulta();
            if (lista.Count > 0)
            {
                foreach (var item in lista)
                {
                    if (item.driver == StaticParametersDB.GetDriver())
                    {
                        var apiConXMLDB = new ConsultaXML(StaticParametersDB.GetGrupo(), StaticParametersDB.GetToken());
                        var retorno = apiConXMLDB.ConsultaProtocolo(item.nroProt, item.baseEnv, item.id);

                        try
                        {
                            if (proc.VerificaConsultaXML(retorno) == true)
                            {
                                proc.GeraLogConsultaXML(item.id, retorno, item.nroProt, 2);

                                if (proc.VerificaSeTemRecibo(retorno, item.id) == true)
                                {
                                    var xmlRec = proc.ExtraiXMLRecibo(item.id, retorno);
                                    var nrRec = proc.ExtraiInfoXML(xmlRec, "nrRecibo");
                                    var nrProtgov = proc.ExtraiInfoXML(xmlRec, "protocoloEnvioLote");

                                    var data = proc.RetornaData();
                                    var prot = new ProtocoloDB { id = item.id, xmlRec = xmlRec, nroRec = nrRec, consultado = true, dtconsulta = data[0], hrconsulta = data[1], nroProtGov = nrProtgov, status = "2 - Aprovado", idDB = StaticParametersDB.GetId() };
                                    ProtocoloDAO.Salvar(prot);
                                }
                                else
                                {
                                    var erros = proc.ExtraiErrosXmlDB(retorno, item.id);
                                    if(!erros.Equals(""))
                                    {
                                        var data = proc.RetornaData();
                                        var prot = new ProtocoloDB { id = item.id, erros = erros, consultado = true, dtconsulta = data[0], hrconsulta = data[1], status = "3 - Rejeitado", idDB = StaticParametersDB.GetId() };
                                        ProtocoloDAO.Salvar(prot);
                                    }
                                    else
                                    {
                                        throw new Exception($"Erro na consulta do item {item.id}, Retorno: {retorno}");
                                    }
                                }
                            }
                            else
                            {
                                var data = proc.RetornaData();
                                var prot = new ProtocoloDB { id = item.id, dtconsulta = data[0], hrconsulta = data[1], status = "1 - Aguardando Governo/iVeS", idDB = StaticParametersDB.GetId() };
                                ProtocoloDAO.Salvar(prot);
                                Banco.CustomUpdateDB(ProtocoloDAO.BuscarPorIDEvento(item.id), 4);
                            }

                        }
                        catch (Exception e)
                        {
                            ex.Exception(e.Message, item.id, "Consulta", "Tente consultar novamente em alguns minutos", e);
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
                                var ret = Banco.UpdateDB(item);

                                if (ret == true)
                                {
                                    item.salvoDB = true;
                                    ProtocoloDAO.Salvar(item);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                        }
                    }

                }
            }

            sessao.Close();
        }
    }
}
