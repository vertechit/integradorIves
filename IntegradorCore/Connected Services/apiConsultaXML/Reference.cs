﻿//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IntegradorCore.apiConsultaXML {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/consulta/retorno" +
        "Processamento/v1_1_0", ConfigurationName="apiConsultaXML.ServicoConsultarLoteEventos")]
    public interface ServicoConsultarLoteEventos {
        
        // CODEGEN: Gerando contrato de mensagem porque o nome do elemento consulta no namespace http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/consulta/retornoProcessamento/v1_1_0 não está marcado como nulo
        [System.ServiceModel.OperationContractAttribute(Action="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/consulta/retorno" +
            "Processamento/v1_1_0/ServicoConsultarLoteEventos/ConsultarLoteEventos", ReplyAction="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/consulta/retorno" +
            "Processamento/v1_1_0/ServicoConsultarLoteEventos/ConsultarLoteEventosResponse")]
        IntegradorCore.apiConsultaXML.ConsultarLoteEventosResponse ConsultarLoteEventos(IntegradorCore.apiConsultaXML.ConsultarLoteEventosRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/consulta/retorno" +
            "Processamento/v1_1_0/ServicoConsultarLoteEventos/ConsultarLoteEventos", ReplyAction="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/consulta/retorno" +
            "Processamento/v1_1_0/ServicoConsultarLoteEventos/ConsultarLoteEventosResponse")]
        System.Threading.Tasks.Task<IntegradorCore.apiConsultaXML.ConsultarLoteEventosResponse> ConsultarLoteEventosAsync(IntegradorCore.apiConsultaXML.ConsultarLoteEventosRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ConsultarLoteEventosRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ConsultarLoteEventos", Namespace="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/consulta/retorno" +
            "Processamento/v1_1_0", Order=0)]
        public IntegradorCore.apiConsultaXML.ConsultarLoteEventosRequestBody Body;
        
        public ConsultarLoteEventosRequest() {
        }
        
        public ConsultarLoteEventosRequest(IntegradorCore.apiConsultaXML.ConsultarLoteEventosRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/consulta/retorno" +
        "Processamento/v1_1_0")]
    public partial class ConsultarLoteEventosRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public System.Xml.Linq.XElement consulta;
        
        public ConsultarLoteEventosRequestBody() {
        }
        
        public ConsultarLoteEventosRequestBody(System.Xml.Linq.XElement consulta) {
            this.consulta = consulta;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ConsultarLoteEventosResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ConsultarLoteEventosResponse", Namespace="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/consulta/retorno" +
            "Processamento/v1_1_0", Order=0)]
        public IntegradorCore.apiConsultaXML.ConsultarLoteEventosResponseBody Body;
        
        public ConsultarLoteEventosResponse() {
        }
        
        public ConsultarLoteEventosResponse(IntegradorCore.apiConsultaXML.ConsultarLoteEventosResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/consulta/retorno" +
        "Processamento/v1_1_0")]
    public partial class ConsultarLoteEventosResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public System.Xml.Linq.XElement ConsultarLoteEventosResult;
        
        public ConsultarLoteEventosResponseBody() {
        }
        
        public ConsultarLoteEventosResponseBody(System.Xml.Linq.XElement ConsultarLoteEventosResult) {
            this.ConsultarLoteEventosResult = ConsultarLoteEventosResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ServicoConsultarLoteEventosChannel : IntegradorCore.apiConsultaXML.ServicoConsultarLoteEventos, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServicoConsultarLoteEventosClient : System.ServiceModel.ClientBase<IntegradorCore.apiConsultaXML.ServicoConsultarLoteEventos>, IntegradorCore.apiConsultaXML.ServicoConsultarLoteEventos {
        
        public ServicoConsultarLoteEventosClient() {
        }
        
        public ServicoConsultarLoteEventosClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServicoConsultarLoteEventosClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicoConsultarLoteEventosClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicoConsultarLoteEventosClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        IntegradorCore.apiConsultaXML.ConsultarLoteEventosResponse IntegradorCore.apiConsultaXML.ServicoConsultarLoteEventos.ConsultarLoteEventos(IntegradorCore.apiConsultaXML.ConsultarLoteEventosRequest request) {
            return base.Channel.ConsultarLoteEventos(request);
        }
        
        public System.Xml.Linq.XElement ConsultarLoteEventos(System.Xml.Linq.XElement consulta) {
            IntegradorCore.apiConsultaXML.ConsultarLoteEventosRequest inValue = new IntegradorCore.apiConsultaXML.ConsultarLoteEventosRequest();
            inValue.Body = new IntegradorCore.apiConsultaXML.ConsultarLoteEventosRequestBody();
            inValue.Body.consulta = consulta;
            IntegradorCore.apiConsultaXML.ConsultarLoteEventosResponse retVal = ((IntegradorCore.apiConsultaXML.ServicoConsultarLoteEventos)(this)).ConsultarLoteEventos(inValue);
            return retVal.Body.ConsultarLoteEventosResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<IntegradorCore.apiConsultaXML.ConsultarLoteEventosResponse> IntegradorCore.apiConsultaXML.ServicoConsultarLoteEventos.ConsultarLoteEventosAsync(IntegradorCore.apiConsultaXML.ConsultarLoteEventosRequest request) {
            return base.Channel.ConsultarLoteEventosAsync(request);
        }
        
        public System.Threading.Tasks.Task<IntegradorCore.apiConsultaXML.ConsultarLoteEventosResponse> ConsultarLoteEventosAsync(System.Xml.Linq.XElement consulta) {
            IntegradorCore.apiConsultaXML.ConsultarLoteEventosRequest inValue = new IntegradorCore.apiConsultaXML.ConsultarLoteEventosRequest();
            inValue.Body = new IntegradorCore.apiConsultaXML.ConsultarLoteEventosRequestBody();
            inValue.Body.consulta = consulta;
            return ((IntegradorCore.apiConsultaXML.ServicoConsultarLoteEventos)(this)).ConsultarLoteEventosAsync(inValue);
        }
    }
}
