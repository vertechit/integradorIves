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
            Processos p = new Processos();
            var xmlString = p.MontaXML("exemplo.xml");
            //var text = p.LerArquivo(Parametros.GetDirArq(), "exemplo.xml");
            Envia(xmlString);
        }

        public void Envia(string xmlString)
        {

            var wsClient = new ServicoEnviarLoteEventosClient();

            var request = new EnviarLoteEventosRequestBody();

            try
            {
                wsClient.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior(Convert.ToString(Parametros.GetGrupo()), Parametros.GetToken()));

                request.loteEventos = System.Xml.Linq.XElement.Parse(xmlString);

                wsClient.Open();

                var retornoEnvioXElement = wsClient.EnviarLoteEventos(request.loteEventos);

                wsClient.Close();
            }
            catch(Exception ex)
            {

            }

        }
    }
}
