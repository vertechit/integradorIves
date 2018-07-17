using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using IntegradorCore.Services;
using IntegradorCore.apiEnviaTXT;

namespace IntegradorCore.API
{
    public class EnviaTXT
    {
        public long Grupo { get; set; }
        public string Token { get; set; }
        public long Ambiente { get; set; }
        public bool CustomEndpoint { get; set; }

        ExceptionCore ex = new ExceptionCore();

        public EnviaTXT(long grupo, string token, long ambiente, bool customendpoint)
        {
            try
            {
                this.Grupo = grupo;
                this.Token = token;
                this.Ambiente = ambiente;
                this.CustomEndpoint = customendpoint;

            }catch(Exception e)
            {
                ex.ImprimeException(1, e.Message, e);
            }
            
        }

        public integraResponse SendTXT(string[] registro)
        {
            try
            {
                var id = new identificador { grupo = this.Grupo, token = this.Token, tpamb = this.Ambiente };

                var eso = new esocial();
                eso.identificador = id;
                eso.registro = registro;

                var request = new integraRequest();
                request.esocial = eso;

                var response = new integraResponse();
                var wsClient = AlteraEndPoint();

            
                wsClient.Open();
                response = wsClient.integraRequest(request);
                wsClient.Close();

                return response;

            }catch(Exception e)
            {
                ex.ImprimeException(1, e.Message, e);
            }

            return new integraResponse();
        }

        private EsocialServiceClient AlteraEndPoint()
        {
            if (this.CustomEndpoint)
            {
                var urlServicoEnvio = @"https://apiesocial2.vertech-it.com.br/vch-esocial/enviaintegra?wsdl";

                var address = new EndpointAddress(urlServicoEnvio);

                var binding = new BasicHttpsBinding();

                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

                var wsClient = new EsocialServiceClient(binding, address);

                wsClient.ClientCredentials.UserName.UserName = Convert.ToString(this.Grupo);
                wsClient.ClientCredentials.UserName.Password = this.Token;

                wsClient.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior(Convert.ToString(this.Grupo), this.Token));

                return wsClient;
            }
            var wsClientp = new EsocialServiceClient();

            return wsClientp;
        }

    }
}
