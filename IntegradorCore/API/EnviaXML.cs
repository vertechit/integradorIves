using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using IntegradorCore.Services;
using IntegradorCore.apiEnviaXML;
using IntegradorCore.NHibernate.DAO;

namespace IntegradorCore.API
{
    public class EnviaXML
    {
        public long Grupo { get; set; }
        public string Token { get; set; }
        public string CustomEndPoint { get; set; }

        ExceptionCore ex = new ExceptionCore();

        public EnviaXML(long grupo, string token, string customendpoint)
        {
            this.Grupo = grupo;
            this.Token = token;
            this.CustomEndPoint = customendpoint;
        }

        public string SendXML(string xml, string identify)
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
                ex.ImprimeException(1, e.Message, e, identify);
            }

            return responseString;
        }

        private ServicoEnviarLoteEventosClient AlteraEndPoint()
        {
            /*
            var sessao = AuxiliarNhibernate.AbrirSessao();
            var parametroDAO = new ParametroDAO(sessao);
            var param = StaticParametros.GetVPGP();
            var pa = parametroDAO.BuscarPorID(1);
            StaticParametros.SetAmbiente(param.Ambiente);
            StaticParametros.SetBase(param.Base);
            StaticParametros.SetDirArq(param.CaminhoDir);
            StaticParametros.SetDirFim(string.Concat(param.CaminhoDir, "\\Consultados"));
            StaticParametros.SetUrl(pa.UrlProd);
            */
            var urlServicoEnvio = @"https://" + StaticParametros.GetUrl() + "/vch-esocial/envialote?wsdl";

            var address = new EndpointAddress(urlServicoEnvio);

            var binding = new BasicHttpsBinding();

            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            var wsClient = new ServicoEnviarLoteEventosClient(binding, address);

            wsClient.ClientCredentials.UserName.UserName = Convert.ToString(this.Grupo);
            wsClient.ClientCredentials.UserName.Password = this.Token;

            wsClient.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior(Convert.ToString(this.Grupo), this.Token));


            return wsClient;
/*

            if (this.CustomEndPoint != "prod")
            {
                //var urlServicoEnvio = @"https://apiesocial2.vertech-it.com.br/vch-esocial/envialote?wsdl";
                var urlServicoEnvio = @"https://"+ StaticParametros.GetUrl() +"/vch-esocial/envialote?wsdl"; 

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
*/
        }
    }
}
