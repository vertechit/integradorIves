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
            //var s = processo.MontaCaminhoDir(Parametros.GetDirArq(), "\\logs\\logConsulta.log");
            int i = 0;

            List<string> lista = processo.ListarArquivos(".xml");

            if (lista.Count > 0)
            {
                foreach (var arq_name in lista)
                {
                    bool valida = Helper.ExistsProtocolo(arq_name);

                    if (valida == true)
                    {
                        var protocolo = Helper.GetProtocolo(arq_name);
                        var retorno = ConsultaProtocolo(protocolo.NroProtocolo, protocolo.Base);

                        try
                        {
                            processo.GeraLogConsultaXML(arq_name, retorno, protocolo.NroProtocolo);

                            if (processo.VerificaConsultaXML(retorno) == true)
                            {
                                processo.MoverConsultado(arq_name);
                                Helper.DeleteProtocolo(arq_name);
                            }
                        }
                        catch (Exception e)
                        {
                            ex.Exception(e.Message, arq_name, "Consulta", "Tente consultar novamente em alguns minutos");
                        }
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
                var l = processo.ListarArquivos(".txt");
                if (l.Count <= 0)
                {
                    ex.ExNoFilesFound(2);
                }
            }
        }

        public string ConsultaProtocolo(string prot, string Base)
        {
            var wsClient = DefineBaseClient(Base);
            var request = new ConsultarLoteEventosRequestBody();

            string strResponse="";
            //System.Xml.Linq.XElement.Load(string.Concat(Parametros.GetDirArq(),"\\retorno.xml"));//

            try
            {
                request.consulta = System.Xml.Linq.XElement.Parse(MontaRequest(prot));
                
                wsClient.Open();
                var response = wsClient.ConsultarLoteEventos(request.consulta);
                wsClient.Close();

                strResponse = Convert.ToString(response);
            }
            catch(Exception e)
            {
                ClassException ex = new ClassException();
                ex.ImprimeException(2, e.Message);
            }

            return strResponse;
        }

        private ServicoConsultarLoteEventosClient DefineBaseClient(string Base)
        {
            if (Base == "Vertech Teste")
            {
                var urlServicoEnvio = @"https://apiesocial2.vertech-it.com.br/vch-esocial/consultalote?wsdl";

                var address = new EndpointAddress(urlServicoEnvio);

                var binding = new BasicHttpsBinding();

                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

                var wsClient = new ServicoConsultarLoteEventosClient(binding, address);

                wsClient.ClientCredentials.UserName.UserName = Convert.ToString(Parametros.GetGrupo());
                wsClient.ClientCredentials.UserName.Password = Parametros.GetToken();

                wsClient.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior(Convert.ToString(Parametros.GetGrupo()), Parametros.GetToken()));

                return wsClient;
            }

            var wsClientP = new ServicoConsultarLoteEventosClient();

            wsClientP.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior(Convert.ToString(Parametros.GetGrupo()), Parametros.GetToken()));

            return wsClientP;
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
