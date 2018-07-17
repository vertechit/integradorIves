using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using IntegradorCore.Services;
using IntegradorCore.apiEnviaXML;

namespace IntegradorCore.API
{
    public class EnviaXML
    {
        public long Grupo { get; set; }
        public string Token { get; set; }
        public bool CustomEndPoint { get; set; }

        ExceptionCore ex = new ExceptionCore();

        public EnviaXML(long grupo, string token, bool customendpoint)
        {
            this.Grupo = grupo;
            this.Token = token;
            this.CustomEndPoint = customendpoint;
        }

        public string SendXML(string xml)
        {
            var wsClient = AlteraEndPoint();

            var request = new EnviarLoteEventosRequestBody();

            var responseString = "";

            try
            {
                request.loteEventos = System.Xml.Linq.XElement.Parse(xml);

                wsClient.Open();

                var response = wsClient.EnviarLoteEventos(request.loteEventos);

                responseString = Convert.ToString(response);

                wsClient.Close();

            }
            catch (Exception e)
            {
                ex.ImprimeException(1, e.Message, e);
            }

            return responseString;
        }

        private ServicoEnviarLoteEventosClient AlteraEndPoint()
        {
            if (this.CustomEndPoint)
            {
                var urlServicoEnvio = @"https://apiesocial2.vertech-it.com.br/vch-esocial/envialote?wsdl";

                var address = new EndpointAddress(urlServicoEnvio);

                var binding = new BasicHttpsBinding();

                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

                var wsClient = new ServicoEnviarLoteEventosClient(binding, address);

                wsClient.ClientCredentials.UserName.UserName = Convert.ToString(this.Grupo);
                wsClient.ClientCredentials.UserName.Password = this.Token;

                wsClient.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior(Convert.ToString(this.Grupo), this.Token));


                return wsClient;
            }

            var wsClientP = new ServicoEnviarLoteEventosClient();

            wsClientP.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior(Convert.ToString(this.Grupo), this.Token));

            return wsClientP;
        }
    }
}
