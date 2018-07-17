using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using IntegradorCore.Modelos;
using IntegradorCore.Services;
using IntegradorCore.apiConsultaTXT;

namespace IntegradorCore.API
{
    public class ConsultaTXT
    {
        public long Grupo { get; set; }
        public string Token { get; set; }

        ExceptionCore ex = new ExceptionCore();

        public ConsultaTXT(long grupo, string token)
        {
            try
            {
                this.Grupo = grupo;
                this.Token = token;

            }
            catch (Exception e)
            {
                ex.ImprimeException(2, e.Message, e);
            }

        }

        public consultaResponse ConsultaProtocolo(Protocolo Prot, string filename)
        {
            var Request = RetornaRequest(Prot);
            var Response = new consultaResponse();

            try
            {
                var wsClient = DefineBaseClient(Prot.Base);

                wsClient.Open();
                Response = wsClient.consultaRequest(Request);
                wsClient.Close();

                return Response;
            }
            catch (Exception e)
            {
                ex.Exception(e.Message, filename, "ConsultaTXT", "");
            }

            return Response;
        }

        private consultaRequest RetornaRequest(Protocolo prot)
        {
            var Request = new consultaRequest();

            Request.protocolo = Convert.ToInt64(prot.NroProtocolo);
            Request.protocoloSpecified = true;
            Request.token = this.Token;
            Request.grupo = this.Grupo;
            Request.grupoSpecified = true;

            return Request;
        }

        private EsocialServiceClient DefineBaseClient(string Base)
        {
            if (Base == "True")
            {
                var urlServicoEnvio = @"https://apiesocial2.vertech-it.com.br/vch-esocial/consultaintegra?wsdl";

                var address = new EndpointAddress(urlServicoEnvio);

                var binding = new BasicHttpsBinding();

                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

                var wsClient = new EsocialServiceClient(binding, address);

                wsClient.ClientCredentials.UserName.UserName = Convert.ToString(this.Grupo);
                wsClient.ClientCredentials.UserName.Password = this.Token;

                wsClient.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior(Convert.ToString(this.Grupo), this.Token));

                return wsClient;
            }
            var wsClientP = new EsocialServiceClient();

            wsClientP.ClientCredentials.UserName.UserName = Convert.ToString(this.Grupo);
            wsClientP.ClientCredentials.UserName.Password = this.Token;

            wsClientP.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior(Convert.ToString(this.Grupo), this.Token));

            return wsClientP;
        }   
    }
}
