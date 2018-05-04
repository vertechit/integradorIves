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
            Processos processo = new Processos();

            var s = processo.MontaCaminhoDir(Parametros.GetDirArq(),"\\logs\\logEnvio.log");

            var lista = processo.Listar_arquivos(".xml");

            if(lista.Count > 0)
            {
                foreach (var arq_name in lista)
                {
                    if(VerificacaoEnviaLote(arq_name) == true)
                    {
                        var xmlString = processo.MontaXML(arq_name);
                        Enviar(xmlString);
                    }
                    else
                    {
                        StreamWriter w = File.AppendText(@s);
                        processo.GeraLogEnviaXML(arq_name, "Já foi enviado!", w);
                        w.Close();
                    }
                }
            }

        }

        public string Enviar(string xmlString)
        {

            var wsClient = new ServicoEnviarLoteEventosClient();

            var request = new EnviarLoteEventosRequestBody();

            try
            {
                wsClient.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior(Convert.ToString(Parametros.GetGrupo()), Parametros.GetToken()));

                request.loteEventos = System.Xml.Linq.XElement.Parse(xmlString);

                wsClient.Open();

                var response = wsClient.EnviarLoteEventos(request.loteEventos);

                wsClient.Close();
            }
            catch(Exception ex)
            {

            }

            

        }
    }
}
