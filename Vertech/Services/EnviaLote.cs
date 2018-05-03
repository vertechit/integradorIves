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
            /*EnviarLoteEventosRequest req = new EnviarLoteEventosRequest();

            string s = null;

            foreach (var item in text)
            {
                s = String.Concat(s, item);
            }

            apiEnviaXML.EnviarLoteEventosRequestBody r = new EnviarLoteEventosRequestBody();

            apiEnviaXML.EnviarLoteEventosResponse respons = new EnviarLoteEventosResponse();

            r.loteEventos = System.Xml.Linq.XElement.Parse(s);

            //XElement e = new XElement("apiConsulta");


            //var q = System.Xml.Linq.XDocument.Load("Vertech/EnvioLoteEventos-v1_1_1.xsd");
            //var x = System.Xml.Linq.XDocument.Parse(Convert.ToString(q));
            //r.loteEventos.Add(x);
            // r.loteEventos.Add(o);
            req.Body = r;

            apiEnviaXML.ServicoEnviarLoteEventosClient cli = new ServicoEnviarLoteEventosClient();
            cli.Open();
            respons.Body.EnviarLoteEventosResult = cli.EnviarLoteEventos(req.Body.loteEventos);
            cli.Close();

            //HTTP_GET(req);*/

            /*apiEnviaXML.EnviarLoteEventosRequestBody r = new EnviarLoteEventosRequestBody();

            string s = null;

            foreach (var item in text)
            {
                s = String.Concat(s, item);
            }

            r.loteEventos = System.Xml.Linq.XElement.Parse(s);*/
            //LoadHttpPageWithBasicAuthentication(@"https://apiesocial.vertech-it.com.br/vch-esocial/envialote?wsdl", "2", "25E8340AB03372CB534F5DA555FF29CC", r);
            /*var urlServicoEnvio = @"https://apiesocial.vertech-it.com.br/vch-esocial/envialote?wsdl";
            var address = new EndpointAddress(urlServicoEnvio);
            var binding = new BasicHttpsBinding();  //Disponível desde .NET Framework 4.5
                                                    // ou:
                                                    //var binding = new BasicHttpBinding(BasicHttpsSecurityMode.Transport);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            var wsClient = new apiEnviaXML.ServicoEnviarLoteEventosClient(binding, address);
            //wsClient.ClientCredentials.UserName.UserName = "26:25E8340AB03372CB534F5DA555FF29CC";
            wsClient.ClientCredentials.UserName.UserName = "26";
            wsClient.ClientCredentials.UserName.Password = "25E8340AB03372CB534F5DA555FF29CC";
            wsClient.Open();
            var retornoEnvioXElement = wsClient.EnviarLoteEventos(r.loteEventos);
            wsClient.Close();*/


            /*BasicHttpsBinding basicHttpBinding = new BasicHttpsBinding();

            basicHttpBinding.Security.Mode = BasicHttpsSecurityMode.TransportWithMessageCredential;

            basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;

            EndpointAddress endpoint = new EndpointAddress("https://apiesocial.vertech-it.com.br/vch-esocial/envialote?wsdl");*/

            var wsClient = new apiEnviaXML.ServicoEnviarLoteEventosClient();//(basicHttpBinding, endpoint);
            //ServiceReference2.GravaSolicitacaoSoapClient client = new ServiceReference2.GravaSolicitacaoSoapClient(basicHttpBinding, endpoint);

           // wsClient.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
            wsClient.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior("26", "25E8340AB03372CB534F5DA555FF29CC"));
            //wsClient.ChannelFactory.Credentials.Windows.ClientCredential = System.Net.CredentialCache.DefaultNetworkCredentials;
            // wsClient.ClientCredentials.UserName.UserName = "26";
            // wsClient.ClientCredentials.UserName.Password = "25E8340AB03372CB534F5DA555FF29CC";
            //wsClient.Endpoint.Behaviors.Add(new CustomEndpointCallBehavior("User1", "Pass1"));
            apiEnviaXML.EnviarLoteEventosRequestBody r = new EnviarLoteEventosRequestBody();
            var caminho = string.Concat(Parametros.GetDirArq(), "\\exemplo.xml");
            r.loteEventos = System.Xml.Linq.XElement.Load(caminho);
            wsClient.Open();
            var retornoEnvioXElement = wsClient.EnviarLoteEventos(r.loteEventos); 
            wsClient.Close();
            //var xx = client.Soma(4, 3);
            //var xx = client.GravaSolic(1, 1, 2);

        }

       /* private string LoadHttpPageWithBasicAuthentication(string url, string username, string password, apiEnviaXML.EnviarLoteEventosRequestBody r)
        {
            Uri myUri = new Uri(url);
            WebRequest myWebRequest = HttpWebRequest.Create(myUri);

            HttpWebRequest myHttpWebRequest = (HttpWebRequest)myWebRequest;

            NetworkCredential myNetworkCredential = new NetworkCredential(username, password);

            CredentialCache myCredentialCache = new CredentialCache();
            myCredentialCache.Add(myUri, "Basic", myNetworkCredential);

            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Credentials = myCredentialCache;
            /*
            BasicHttpsBinding basicHttpBinding = new BasicHttpsBinding();

            basicHttpBinding.Security.Mode = BasicHttpsSecurityMode.TransportWithMessageCredential;

            basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;

            EndpointAddress endpoint = new EndpointAddress("https://apiesocial.vertech-it.com.br/vch-esocial/envialote?wsdl");

            var wsClient = new apiEnviaXML.ServicoEnviarLoteEventosClient(basicHttpBinding, endpoint);
            //ServiceReference2.GravaSolicitacaoSoapClient client = new ServiceReference2.GravaSolicitacaoSoapClient(basicHttpBinding, endpoint);

            //wsClient.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
            wsClient.ClientCredentials.UserName.UserName = @"https://apiesocial.vertech-it.com.br/vch-esocial/envialote?wsdl\26";
            wsClient.ClientCredentials.UserName.Password = "25E8340AB03372CB534F5DA555FF29CC";
            wsClient.ChannelFactory.Credentials.Windows.ClientCredential.UserName = "26";
            wsClient.ChannelFactory.Credentials.Windows.ClientCredential.Password = "25E8340AB03372CB534F5DA555FF29CC";//System.Net.CredentialCache.DefaultNetworkCredentials;

            wsClient.Open();
            var retornoEnvioXElement = wsClient.EnviarLoteEventos(r.loteEventos);
            wsClient.Close();

            
            WebResponse myWebResponse = myWebRequest.GetResponse();

            Stream responseStream = myWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

            string pageContent = myStreamReader.ReadToEnd();

            responseStream.Close();

            myWebResponse.Close();

            return pageContent;
        }

        static async void HTTP_GET(EnviarLoteEventosRequest req)
        {
            apiEnviaXML.EnviarLoteEventosResponse respons = new EnviarLoteEventosResponse();

            

            var TARGETURL = "https://esocial.vertech-it.com.br/apex/f?p=250:LOGIN::::::";

            /*HttpClientHandler handler = new HttpClientHandler()
            {
                Proxy = new WebProxy("http://127.0.0.1:8888"),
                UseProxy = true,
            };*/

            /*var credentials = new NetworkCredential("", "");
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
                apiEnviaXML.ServicoEnviarLoteEventosClient cli = new ServicoEnviarLoteEventosClient();
                cli.Open();
                System.Windows.MessageBox.Show(cli.State.ToString());
                respons.Body.EnviarLoteEventosResult = cli.EnviarLoteEventos(req.Body.loteEventos);
                cli.Close();
                //System.Windows.MessageBox.Show(result.Substring(0, 50) + "...");
            }
        }*/
    }
}
