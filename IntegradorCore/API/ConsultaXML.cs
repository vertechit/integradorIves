using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using IntegradorCore.Services;
using IntegradorCore.apiConsultaXML;

namespace IntegradorCore.API
{
    public class ConsultaXML
    {
        public long Grupo { get; set; }
        public string Token { get; set; }

        ExceptionCore ex = new ExceptionCore();

        public ConsultaXML(long grupo, string token)
        {
            try
            {
                this.Grupo = grupo;
                this.Token = token;
            }
            catch(Exception e)
            {
                ex.ImprimeException(2, e.Message, e, " ");
            }

        }

        public string ConsultaProtocolo(string prot, string Base, string filename)
        {
            var wsClient = DefineBaseClient(Base);
            var request = new ConsultarLoteEventosRequestBody();

            string strResponse = "";

            try
            {
                request.consulta = System.Xml.Linq.XElement.Parse(MontaRequest(prot));

                wsClient.Open();
                var response = wsClient.ConsultarLoteEventos(request.consulta);
                wsClient.Close();

                strResponse = Convert.ToString(response);
            }
            catch (Exception e)
            {
                ex.Exception(e.Message, filename, "Consulta", "");
            }

            return strResponse;
        }

        private string MontaRequest(string prot)
        {
            prot = string.Concat("<protocoloEnvio>", prot, "</protocoloEnvio>");
            string ini = "<eSocial xmlns=\"http://www.esocial.gov.br/schema/lote/eventos/envio/consulta/retornoProcessamento/v1_0_0\"><consultaLoteEventos>";
            string fim = "</consultaLoteEventos></eSocial>";
            return String.Concat(ini, prot, fim);
        }

        private ServicoConsultarLoteEventosClient DefineBaseClient(string Base)
        {
            if (Base == "True")
            {
                var urlServicoEnvio = @"https://apiesocial2.vertech-it.com.br/vch-esocial/consultalote?wsdl";

                var address = new EndpointAddress(urlServicoEnvio);

                var binding = new BasicHttpsBinding();

                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

                var wsClient = new ServicoConsultarLoteEventosClient(binding, address);

                wsClient.ClientCredentials.UserName.UserName = Convert.ToString(this.Grupo);
                wsClient.ClientCredentials.UserName.Password = this.Token;

                wsClient.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior(Convert.ToString(this.Grupo), this.Token));

                return wsClient;
            }

            var wsClientP = new ServicoConsultarLoteEventosClient();

            wsClientP.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior(Convert.ToString(this.Grupo), this.Token));

            return wsClientP;
        }
    }
}
