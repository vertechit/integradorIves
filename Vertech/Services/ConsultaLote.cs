using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertech.apiConsultaXML;
using System.Xml.Linq;
using System.Net.Http;
using System.Net;
using System.ServiceModel;
using System.IO;
using Vertech.DAO;
using Vertech.Modelos;


namespace Vertech.Services
{
    public class ConsultaLote
    {
        public ConsultaLote()
        {

        }

        public void Job()
        {
            DirectoryInfo di = new DirectoryInfo(string.Concat(Parametros.GetDirArq(), "\\logs"));
            
            if (di.Exists == false)
                di.Create();

            ClassException ex = new ClassException();
            Processos processo = new Processos();
            var s = processo.MontaCaminhoDir(Parametros.GetDirArq(), "\\logs\\logConsulta.log");
            int i = 0;

            List<string> lista = processo.Listar_arquivos(".xml");

            if (lista.Count > 0)
            {
                foreach (var arq_name in lista)
                {
                    bool valida = Helper.ExistsProtocolo(arq_name);

                    if (valida == true)
                    {
                        var protocolo = Helper.GetProtocolo(arq_name);
                        var retorno = ConsultaProtocolo(protocolo.NroProtocolo);

                        try
                        {
                            processo.GeraLogConsultaXML(arq_name, retorno);

                            if (processo.VerificaConsultaXML(retorno) == true)
                            {
                                //processo.GeraLogConsultaDetalhadaXML(arq_name, protocolo.NroProtocolo, retorno, w);
                                processo.Mover_Consultado(arq_name);
                                Helper.DeleteProtocolo(arq_name);
                            }
                        }
                        catch (Exception e)
                        {
                            ex.ImprimeException(2, "Tente consultar novamente em alguns minutos");
                            /*StreamWriter vWriter = new StreamWriter(@s, true);

                            vWriter.WriteLine("");
                            vWriter.WriteLine("Ocorrencia ConsultaXML");
                            vWriter.WriteLine("Data/Hora: " + DateTime.Now.ToString());
                            vWriter.WriteLine("Codigo Erro: " + 1);
                            vWriter.WriteLine("Descrição: Tente consultar novamente em alguns minutos.");
                            vWriter.WriteLine("");
                            vWriter.Flush();
                            vWriter.Close();*/
                        }
                        i++;
                    }
                }
                if (i == 0)
                {
                    ex.ImprimeMsgDeErro_NoFilesFound(2);
                }
            }

            else
            {
                var l = processo.Listar_arquivos(".txt");
                if (l.Count <= 0)
                {
                    ex.ImprimeMsgDeErro_NoFilesFound(2);
                }
            }
        }

        public string ConsultaProtocolo(string prot)
        {
            var wsClient = new ServicoConsultarLoteEventosClient();
            var request = new ConsultarLoteEventosRequestBody();
            string strResponse="";
            request.consulta = System.Xml.Linq.XElement.Parse(MontaRequest(prot));//System.Xml.Linq.XElement.Load(string.Concat(Parametros.GetDirArq(),"\\retorno.xml"));//

            try
            {
                wsClient.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior(Convert.ToString(Parametros.GetGrupo()), Parametros.GetToken()));

                wsClient.Open();
                var response = wsClient.ConsultarLoteEventos(request.consulta);
                wsClient.Close();
                strResponse = Convert.ToString(response);
            }
            catch(Exception e)
            {
                ClassException ex = new ClassException();
                ex.ImprimeException(2, "Erro no Consulta XML! Tente novamente mais tarde.");
            }

            return strResponse;
        }
        
        public string MontaRequest(string prot)
        {
            prot = string.Concat("<protocoloEnvio>", prot, "</protocoloEnvio>");
            string ini = "<eSocial xmlns=\"http://www.esocial.gov.br/schema/lote/eventos/envio/consulta/retornoProcessamento/v1_0_0\"><consultaLoteEventos>";
            string fim = "</consultaLoteEventos></eSocial>";
            return String.Concat(ini, prot, fim);
        }
    }
}
