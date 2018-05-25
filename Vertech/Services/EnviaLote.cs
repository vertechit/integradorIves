using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertech.apiEnviaXML;
using System.Xml.Linq;
using System.Net.Http;
using System.Net;
using System.ServiceModel;
using System.IO;

namespace Vertech.Services
{
    public class EnviaLote
    {
        public EnviaLote()
        {
            
        }

        public void Job()
        {
            DirectoryInfo di = new DirectoryInfo(string.Concat(Parametros.GetDirArq(), "\\logs"));

            if (di.Exists == false)
                di.Create();

            Processos processo = new Processos();

            //var s = processo.MontaCaminhoDir(Parametros.GetDirArq(),"\\logs\\logEnvio.log");

            var lista = processo.ListarArquivos(".xml");

            if(lista.Count > 0)
            {
                foreach (var arq_name in lista)
                {
                    if(processo.VerificacaoEnviaLote(arq_name) == true)
                    {
                        var xmlString = processo.MontaXML(arq_name);
                        var response = Enviar(xmlString);
                        if(processo.VerificaResponseXML(response) == true)
                        {
                            processo.SalvaProtocoloXML(arq_name, response);
                            processo.GeraLogEnviaXML(arq_name, "Foi enviado com sucesso!");
                        }
                        else
                        {
                            processo.GeraLogEnviaXML(arq_name, "Não foi enviado");
                        }
                    }
                    else
                    {
                        processo.GeraLogEnviaXML(arq_name, "Já foi enviado!");
                    }
                }
            }
            else
            {
                ClassException ex = new ClassException();
                var l = processo.ListarArquivos(".txt");
                if (l.Count <= 0)
                {
                    ex.ExNoFilesFound(1);
                }
            }

        }

        public string Enviar(string xmlString)
        {

            var wsClient = DefineBaseClient();

            var request = new EnviarLoteEventosRequestBody();

            var responseString = "";

            try
            {
                request.loteEventos = System.Xml.Linq.XElement.Parse(xmlString);

                wsClient.Open();

                var response = wsClient.EnviarLoteEventos(request.loteEventos);

                responseString = Convert.ToString(response);

                wsClient.Close();

            }
            catch(Exception e)
            {
                ClassException ex = new ClassException();
                ex.ImprimeException(1, e.Message.ToString());
            }

            return responseString;

        }

        private ServicoEnviarLoteEventosClient DefineBaseClient()
        {
            if (Parametros.GetBase() == "Vertech Teste")
            {
                var urlServicoEnvio = @"https://apiesocial2.vertech-it.com.br/vch-esocial/envialote?wsdl";

                var address = new EndpointAddress(urlServicoEnvio);

                var binding = new BasicHttpsBinding();

                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

                var wsClient = new ServicoEnviarLoteEventosClient(binding, address);

                wsClient.ClientCredentials.UserName.UserName = Convert.ToString(Parametros.GetGrupo());
                wsClient.ClientCredentials.UserName.Password = Parametros.GetToken();

                wsClient.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior(Convert.ToString(Parametros.GetGrupo()), Parametros.GetToken()));


                return wsClient;
            }

            var wsClientP = new ServicoEnviarLoteEventosClient();

            wsClientP.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior(Convert.ToString(Parametros.GetGrupo()), Parametros.GetToken()));

            return wsClientP;
        }

    }
}
