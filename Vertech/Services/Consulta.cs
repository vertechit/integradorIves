using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertech.apiConsulta;

using Vertech.Services;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Windows;
using Vertech.DAO;
using Vertech.Modelos;
using System.ServiceModel;

namespace Vertech.Services
{
    public class Consulta
    {
        
        public void Job()
        {
            DirectoryInfo di = new DirectoryInfo(string.Concat(Parametros.GetDirArq(), "\\logs"));

            if (di.Exists == false)
                di.Create();

            ClassException ex = new ClassException();
            Processos processo = new Processos();

            int i = 0;
            List<string> lista = processo.ListarArquivos(".txt");

            if (lista.Count > 0)
            {
                foreach (var arq_name in lista)
                {
                    bool valida = Helper.ExistsProtocolo(arq_name);
                    if(valida == true)
                    {
                        var p = Helper.GetProtocolo(arq_name);
                        ConsultaProtocolo(Set_Protocolo(p), p.NomeArquivo, p.Base);
                        i++;
                    }
                }
                if (i == 0)
                {
                    ex.ExNoFilesFound(2);
                }
            }

            else
            {
                var l = processo.ListarArquivos(".xml");
                if (l.Count <= 0)
                {
                    ex.ExNoFilesFound(2);
                }
            }
            
        }

        public void ConsultaProtocolo(apiIntegra.integraResponse Prot, string filename, string Base)
        {
            var Request = RetornaRequest(Prot);
            consultaResponse Response = new consultaResponse();
            Processos processo = new Processos();

            //var s = processo.MontaCaminhoDir(Parametros.GetDirArq(), "\\logs\\logConsulta.log");

            try
            {
                var wsClient = DefineBaseClient(Base);
                //req.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior(Convert.ToString(Parametros.GetGrupo()), Parametros.GetToken()));

                wsClient.Open();
                Response = wsClient.consultaRequest(Request);
                wsClient.Close();

                processo.GeraLogConsulta(filename
                    , Response.consultaProtocolo.identificador.protocolo.ToString()
                    , Convert.ToString(Response.consultaProtocolo.status.descResposta)
                    , Convert.ToInt32(Response.consultaProtocolo.status.cdResposta));

                processo.GeraLogDetalhado(filename, Response);

                if(Response.consultaProtocolo.status.cdResposta == 3 || Response.consultaProtocolo.status.cdResposta == 2)
                {
                    processo.MoverConsultado(filename);
                    Helper.DeleteProtocolo(filename);
                }
            }
            catch(Exception e)
            {
                ClassException ex = new ClassException();

                ex.Exception(e.Message, filename, "Consulta", " ");
            }

        }

        private EsocialServiceClient DefineBaseClient(string Base)
        {
            if (Base == "Vertech Teste")
            {
                var urlServicoEnvio = @"https://apiesocial2.vertech-it.com.br/vch-esocial/consultaintegra?wsdl";

                var address = new EndpointAddress(urlServicoEnvio);

                var binding = new BasicHttpsBinding();

                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
                
                var wsClient = new EsocialServiceClient(binding, address);

                wsClient.ClientCredentials.UserName.UserName = Convert.ToString(Parametros.GetGrupo());
                wsClient.ClientCredentials.UserName.Password = Parametros.GetToken();

                wsClient.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior(Convert.ToString(Parametros.GetGrupo()), Parametros.GetToken()));

                return wsClient;
            }
            var wsClientP = new EsocialServiceClient();

            wsClientP.ClientCredentials.UserName.UserName = Convert.ToString(Parametros.GetGrupo());
            wsClientP.ClientCredentials.UserName.Password = Parametros.GetToken();

            wsClientP.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior(Convert.ToString(Parametros.GetGrupo()), Parametros.GetToken()));

            return wsClientP;
        }

        private apiIntegra.integraResponse Set_Protocolo(Protocolo p)
        {
            Processos processo = new Processos();
            var response = new apiIntegra.integraResponse();

            //var retorno = processo.LerArquivo(Parametros.GetDirArq(), arq_name);

            response.protocolo = Convert.ToUInt32(p.NroProtocolo);
            response.protocoloSpecified = true; 

            return response;
        }

        private consultaRequest RetornaRequest(apiIntegra.integraResponse prot)
        {
            consultaRequest Request = new consultaRequest();
            Request.protocolo = prot.protocolo;
            Request.protocoloSpecified = prot.protocoloSpecified;
            Request.token = Parametros.GetToken();
            Request.grupo = Parametros.GetGrupo();
            Request.grupoSpecified = true;

            return Request;
        }
    }
}