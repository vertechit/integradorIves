using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertech.apiEnviaLote;
using System.Xml.Linq;
using System.Net.Http;
using System.Net;

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
            EnviarLoteEventosRequest req = new EnviarLoteEventosRequest();

            string s = null;

            foreach (var item in text)
            {
                s = String.Concat(s, item);
            }

            apiEnviaLote.EnviarLoteEventosRequestBody r = new EnviarLoteEventosRequestBody();

            apiEnviaLote.EnviarLoteEventosResponse respons = new EnviarLoteEventosResponse();

            r.loteEventos = System.Xml.Linq.XElement.Parse(s);

            //XElement e = new XElement("apiConsulta");


            //var q = System.Xml.Linq.XDocument.Load("Vertech/EnvioLoteEventos-v1_1_1.xsd");
            //var x = System.Xml.Linq.XDocument.Parse(Convert.ToString(q));
            //r.loteEventos.Add(x);
            // r.loteEventos.Add(o);
            req.Body = r;

            apiEnviaLote.ServicoEnviarLoteEventosClient cli = new ServicoEnviarLoteEventosClient();
            cli.Open();
            respons.Body.EnviarLoteEventosResult = cli.EnviarLoteEventos(req.Body.loteEventos);
            cli.Close();

            //HTTP_GET(req);
            
        }

        static async void HTTP_GET(EnviarLoteEventosRequest req)
        {
            apiEnviaLote.EnviarLoteEventosResponse respons = new EnviarLoteEventosResponse();

            

            var TARGETURL = "https://esocial.vertech-it.com.br/apex/f?p=250:LOGIN::::::";

            /*HttpClientHandler handler = new HttpClientHandler()
            {
                Proxy = new WebProxy("http://127.0.0.1:8888"),
                UseProxy = true,
            };*/

            var credentials = new NetworkCredential("", "");
            var handler = new HttpClientHandler { Credentials = credentials };
            //var http = new HttpClient(handler);

            //http.GetAsync(TARGETURL);

            //System.Windows.MessageBox.Show("GET: + " + TARGETURL);

            // ... Use HttpClient.            
            HttpClient client = new HttpClient(handler);
            //var byteArray = Encoding.ASCII.GetBytes("1:8EE07DE66C97D8CFBAE04C47E8F51D76");
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            HttpResponseMessage response = await client.GetAsync(TARGETURL);
            HttpContent content = response.Content;

            // ... Check Status Code                                
            //System.Windows.MessageBox.Show("Response StatusCode: " + response.StatusCode);

            // ... Read the string.
            string result = await content.ReadAsStringAsync();

            // ... Display the result.
            if (result != null &&
            result.Length >= 50)
            {
                apiEnviaLote.ServicoEnviarLoteEventosClient cli = new ServicoEnviarLoteEventosClient();
                cli.Open();
                System.Windows.MessageBox.Show(cli.State.ToString());
                respons.Body.EnviarLoteEventosResult = cli.EnviarLoteEventos(req.Body.loteEventos);
                cli.Close();
                //System.Windows.MessageBox.Show(result.Substring(0, 50) + "...");
            }
        }
    }
}
