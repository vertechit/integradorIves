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
            var text = p.LerArquivo(Parametros.GetDirArq(), "exemplo.xml");
            Envia(text);
        }

        public void Envia(string [] text)
        {
            var wsClient = new apiEnviaXML.ServicoEnviarLoteEventosClient();

            wsClient.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior(Parametros.GetGrupo(), Parametros.GetToken());

            apiEnviaXML.EnviarLoteEventosRequestBody r = new EnviarLoteEventosRequestBody();

            var caminho = string.Concat(Parametros.GetDirArq(), "\\exemplo.xml");

            r.loteEventos = System.Xml.Linq.XElement.Load(caminho);
            
            wsClient.Open();
            var retornoEnvioXElement = wsClient.EnviarLoteEventos(r.loteEventos); 
            wsClient.Close();

        }
    }
}
