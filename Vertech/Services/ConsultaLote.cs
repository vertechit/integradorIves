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
                        var str = ConsultaProtocolo(protocolo.NroProtocolo);
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
                ex.ImprimeMsgDeErro_NoFilesFound(2);
            }
        }

        public string ConsultaProtocolo(string prot)
        {
            var wsClient = new ServicoConsultarLoteEventosClient();
            var request = new ConsultarLoteEventosRequestBody();
            
            request.consulta = System.Xml.Linq.XElement.Parse(MontaRequest(prot));

            try
            {
                wsClient.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior(Convert.ToString(Parametros.GetGrupo()), Parametros.GetToken()));

                wsClient.Open();
                var reponse = wsClient.ConsultarLoteEventos(request.consulta);
                wsClient.Close();
            }
            catch(Exception e)
            {
                ClassException ex = new ClassException();
                ex.ImprimeException(2, e.Message.ToString());
            }

            return "teste";
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
