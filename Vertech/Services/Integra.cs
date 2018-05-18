using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vertech.apiIntegra;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Xml;
using System.Windows;
using Vertech.Services;
using Vertech.DAO;
using System.ServiceModel;

namespace Vertech.Services
{
    public class Integra
    {

        public void Job()
        {
            DirectoryInfo di = new DirectoryInfo(string.Concat(Parametros.GetDirArq(),"\\logs"));

            if (di.Exists == false)
                di.Create();

            Processos processo = new Processos();

            var s = processo.MontaCaminhoDir(Parametros.GetDirArq(),"\\logs\\logEnvio.log");

            var lista = processo.ListarArquivos(".txt");

            if(lista.Count > 0)
            {
                foreach (var arq_name in lista)
                {
                    int ctrl = processo.VerificacaoIntegra(arq_name);

                    if (ctrl == 0)
                    {
                        integraRequest Request = new integraRequest();

                        Request.esocial = Retorna_Esocial(Retorna_Identificador(), arq_name);

                        integraResponse Response = Enviar(Request);

                        if (Response.protocolo > 0)
                        {
                            processo.SalvaProtocolo(Response, arq_name);
                            processo.GeraLogIntegra(arq_name, "Foi enviado com sucesso!");
                        }
                        else
                        {
                            processo.GeraLogIntegra(arq_name, "Erro no envio, retorno invalido!");
                        }
                    }
                    else if (ctrl == 1)
                    {
                        processo.GeraLogIntegra(arq_name, "Já foi enviado!");
                    }
                    else
                    {
                        processo.GeraLogIntegra(arq_name, "É invalido");

                    }
                }
            }

            else
            {
                ClassException ex = new ClassException();
                var l = processo.ListarArquivos(".xml");
                if (l.Count <= 0)
                {
                    ex.ExNoFilesFound(1);
                }
            }

        }

        public identificador Retorna_Identificador()
        {
            identificador Id = new identificador();

            try
            {
                Id.grupo = Parametros.GetGrupo();
                Id.token = Parametros.GetToken();
                Id.tpamb = Convert.ToInt32(Parametros.GetAmbiente());
            }
            catch (Exception)
            {
                ClassException ex = new ClassException();
                ex.ImprimeException(1, "Erro ao atribuir identificador");
            }

            return Id;
        }

        public esocial Retorna_Esocial(identificador id, string arq)
        {
            var dir = Parametros.GetDirArq();

            esocial eso = new esocial();
            Processos process = new Processos();

            try
            {
                eso.identificador = id;

                eso.registro = process.LerArquivo(dir, arq);
            }
            catch (Exception)
            {
                ClassException ex = new ClassException();
                ex.ImprimeException(1, "Erro no retorno do arquivo");
            }

            return eso;
        }

        public integraResponse Enviar(integraRequest Request)
        {
            var wsClient = DefineBaseClient();
            var Response = new integraResponse();

            try
            {
                wsClient.Open();

                Response = wsClient.integraRequest(Request);

                wsClient.Close();

            }
            catch(Exception e)
            {
                ClassException ex = new ClassException();
                ex.ImprimeException(1, e.Message.ToString());
            }

            return Response;
        }

        private EsocialServiceClient DefineBaseClient()
        {
            if(Parametros.GetBase() == "Vertech Teste")
            {
                var urlServicoEnvio = @"https://apiesocial2.vertech-it.com.br/vch-esocial/enviaintegra?wsdl";

                var address = new EndpointAddress(urlServicoEnvio);

                var binding = new BasicHttpsBinding();

                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

                var wsClient = new EsocialServiceClient(binding, address);

                wsClient.ClientCredentials.UserName.UserName = Convert.ToString(Parametros.GetGrupo());
                wsClient.ClientCredentials.UserName.Password = Parametros.GetToken();

                wsClient.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior(Convert.ToString(Parametros.GetGrupo()), Parametros.GetToken()));

                return wsClient;
            }

            return new EsocialServiceClient();
        }

    }
}
