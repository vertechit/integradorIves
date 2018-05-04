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

            var s = processo.MontaCaminhoDir(Parametros.GetDirArq(),"\\logs\\logEnvio.log");

            var lista = processo.Listar_arquivos(".xml");

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
                            StreamWriter w = File.AppendText(@s);
                            processo.SalvaProtocoloXML(arq_name, response);
                            processo.GeraLogIntegra(arq_name, "Foi enviado com sucesso!", w);
                            w.Close();
                        }
                        else
                        {
                            StreamWriter w = File.AppendText(@s);
                            processo.GeraLogIntegra(arq_name, "Não foi enviado", w);
                            w.Close();
                        }
                    }
                    else
                    {
                        StreamWriter w = File.AppendText(@s);
                        processo.GeraLogEnviaXML(arq_name, "Já foi enviado!", w);
                        w.Close();
                    }
                }
            }
            else
            {
                ClassException ex = new ClassException();
                ex.ImprimeMsgDeErro_NoFilesFound(1);
            }

        }

        public string Enviar(string xmlString)
        {

            var wsClient = new ServicoEnviarLoteEventosClient();

            var request = new EnviarLoteEventosRequestBody();

            var responseString = "";

            try
            {
                wsClient.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior(Convert.ToString(Parametros.GetGrupo()), Parametros.GetToken()));

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
    }
}
