﻿//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iVesService.apiEnviaXML {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/v1_1_0", ConfigurationName="apiEnviaXML.ServicoEnviarLoteEventos")]
    public interface ServicoEnviarLoteEventos {
        
        // CODEGEN: Gerando contrato de mensagem porque o nome do elemento loteEventos no namespace http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/v1_1_0 não está marcado como nulo
        [System.ServiceModel.OperationContractAttribute(Action="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/v1_1_0/ServicoEn" +
            "viarLoteEventos/EnviarLoteEventos", ReplyAction="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/v1_1_0/ServicoEn" +
            "viarLoteEventos/EnviarLoteEventosResponse")]
        iVesService.apiEnviaXML.EnviarLoteEventosResponse EnviarLoteEventos(iVesService.apiEnviaXML.EnviarLoteEventosRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/v1_1_0/ServicoEn" +
            "viarLoteEventos/EnviarLoteEventos", ReplyAction="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/v1_1_0/ServicoEn" +
            "viarLoteEventos/EnviarLoteEventosResponse")]
        System.Threading.Tasks.Task<iVesService.apiEnviaXML.EnviarLoteEventosResponse> EnviarLoteEventosAsync(iVesService.apiEnviaXML.EnviarLoteEventosRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class EnviarLoteEventosRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="EnviarLoteEventos", Namespace="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/v1_1_0", Order=0)]
        public iVesService.apiEnviaXML.EnviarLoteEventosRequestBody Body;
        
        public EnviarLoteEventosRequest() {
        }
        
        public EnviarLoteEventosRequest(iVesService.apiEnviaXML.EnviarLoteEventosRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/v1_1_0")]
    public partial class EnviarLoteEventosRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public System.Xml.Linq.XElement loteEventos;
        
        public EnviarLoteEventosRequestBody() {
        }
        
        public EnviarLoteEventosRequestBody(System.Xml.Linq.XElement loteEventos) {
            this.loteEventos = loteEventos;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class EnviarLoteEventosResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="EnviarLoteEventosResponse", Namespace="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/v1_1_0", Order=0)]
        public iVesService.apiEnviaXML.EnviarLoteEventosResponseBody Body;
        
        public EnviarLoteEventosResponse() {
        }
        
        public EnviarLoteEventosResponse(iVesService.apiEnviaXML.EnviarLoteEventosResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/v1_1_0")]
    public partial class EnviarLoteEventosResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public System.Xml.Linq.XElement EnviarLoteEventosResult;
        
        public EnviarLoteEventosResponseBody() {
        }
        
        public EnviarLoteEventosResponseBody(System.Xml.Linq.XElement EnviarLoteEventosResult) {
            this.EnviarLoteEventosResult = EnviarLoteEventosResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ServicoEnviarLoteEventosChannel : iVesService.apiEnviaXML.ServicoEnviarLoteEventos, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServicoEnviarLoteEventosClient : System.ServiceModel.ClientBase<iVesService.apiEnviaXML.ServicoEnviarLoteEventos>, iVesService.apiEnviaXML.ServicoEnviarLoteEventos {
        
        public ServicoEnviarLoteEventosClient() {
        }
        
        public ServicoEnviarLoteEventosClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServicoEnviarLoteEventosClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicoEnviarLoteEventosClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicoEnviarLoteEventosClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        iVesService.apiEnviaXML.EnviarLoteEventosResponse iVesService.apiEnviaXML.ServicoEnviarLoteEventos.EnviarLoteEventos(iVesService.apiEnviaXML.EnviarLoteEventosRequest request) {
            return base.Channel.EnviarLoteEventos(request);
        }
        
        public System.Xml.Linq.XElement EnviarLoteEventos(System.Xml.Linq.XElement loteEventos) {
            iVesService.apiEnviaXML.EnviarLoteEventosRequest inValue = new iVesService.apiEnviaXML.EnviarLoteEventosRequest();
            inValue.Body = new iVesService.apiEnviaXML.EnviarLoteEventosRequestBody();
            inValue.Body.loteEventos = loteEventos;
            iVesService.apiEnviaXML.EnviarLoteEventosResponse retVal = ((iVesService.apiEnviaXML.ServicoEnviarLoteEventos)(this)).EnviarLoteEventos(inValue);
            return retVal.Body.EnviarLoteEventosResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<iVesService.apiEnviaXML.EnviarLoteEventosResponse> iVesService.apiEnviaXML.ServicoEnviarLoteEventos.EnviarLoteEventosAsync(iVesService.apiEnviaXML.EnviarLoteEventosRequest request) {
            return base.Channel.EnviarLoteEventosAsync(request);
        }
        
        public System.Threading.Tasks.Task<iVesService.apiEnviaXML.EnviarLoteEventosResponse> EnviarLoteEventosAsync(System.Xml.Linq.XElement loteEventos) {
            iVesService.apiEnviaXML.EnviarLoteEventosRequest inValue = new iVesService.apiEnviaXML.EnviarLoteEventosRequest();
            inValue.Body = new iVesService.apiEnviaXML.EnviarLoteEventosRequestBody();
            inValue.Body.loteEventos = loteEventos;
            return ((iVesService.apiEnviaXML.ServicoEnviarLoteEventos)(this)).EnviarLoteEventosAsync(inValue);
        }
    }
}
