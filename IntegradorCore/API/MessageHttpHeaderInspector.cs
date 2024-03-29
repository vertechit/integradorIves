﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegradorCore.Services;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;


namespace IntegradorCore.API
{
    class MessageHttpHeaderInspector : IClientMessageInspector
    {
        private string userName;
        private string password;

        #region Constructor

        public MessageHttpHeaderInspector(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }

        #endregion

        #region IClientMessageInspector Members

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            var stringXML = "";
            try
            {
                var ret = System.Xml.Linq.XElement.Parse(Convert.ToString(reply));
                stringXML = Convert.ToString(ret);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            Processos p = new Processos();
            if (stringXML.Contains("<consultaResponse xmlns=\"http://www.esocial.gov.br/ws\">"))
            {
                p.CreateFileBufferConsulta(stringXML);
            }
            else
            {
                p.CreateFileBufferEnviaXML(stringXML);
            }
            //throw new NotImplementedException();
        }

        public object BeforeSendRequest(ref Message request, System.ServiceModel.IClientChannel channel)
        {
            HttpRequestMessageProperty httpRequest;

            if (request.Properties.ContainsKey(HttpRequestMessageProperty.Name))
            {
                httpRequest = request.Properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
            }
            else
            {
                httpRequest = new HttpRequestMessageProperty();
                request.Properties.Add(HttpRequestMessageProperty.Name, httpRequest);
            }

            if (httpRequest != null)
            {
                string credentials = this.CreateBasicAuthenticationCredentials(this.userName, this.password);
                httpRequest.Headers.Add(System.Net.HttpRequestHeader.Authorization, credentials);
            }

            return request;
            //return httpRequest;
        }

        #endregion

        #region Private Worker Methods

        private string CreateBasicAuthenticationCredentials(string userName, string password)
        {
            string returnValue = string.Empty;
            string base64UsernamePassword = Convert.ToBase64String(Encoding.ASCII.GetBytes(String.Format("{0}:{1}", userName, password)));

            returnValue = String.Format("Basic {0}", base64UsernamePassword);

            return returnValue;
        }

        #endregion
    }
}
