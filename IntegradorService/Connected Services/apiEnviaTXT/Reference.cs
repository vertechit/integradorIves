﻿//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IntegradorService.apiEnviaTXT {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.esocial.gov.br/ws", ConfigurationName="apiEnviaTXT.EsocialService")]
    public interface EsocialService {
        
        // CODEGEN: Gerando contrato de mensagem porque a operação integraRequest não é RPC nem documento codificado.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        IntegradorService.apiEnviaTXT.integraResponse1 integraRequest(IntegradorService.apiEnviaTXT.integraRequest1 request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<IntegradorService.apiEnviaTXT.integraResponse1> integraRequestAsync(IntegradorService.apiEnviaTXT.integraRequest1 request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2558.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.esocial.gov.br/ws")]
    public partial class integraRequest : object, System.ComponentModel.INotifyPropertyChanged {
        
        private esocial esocialField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public esocial esocial {
            get {
                return this.esocialField;
            }
            set {
                this.esocialField = value;
                this.RaisePropertyChanged("esocial");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2558.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.esocial.gov.br/ws")]
    public partial class esocial : object, System.ComponentModel.INotifyPropertyChanged {
        
        private identificador identificadorField;
        
        private string[] registroField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public identificador identificador {
            get {
                return this.identificadorField;
            }
            set {
                this.identificadorField = value;
                this.RaisePropertyChanged("identificador");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", IsNullable=false)]
        public string[] registro {
            get {
                return this.registroField;
            }
            set {
                this.registroField = value;
                this.RaisePropertyChanged("registro");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2558.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.esocial.gov.br/ws")]
    public partial class identificador : object, System.ComponentModel.INotifyPropertyChanged {
        
        private long grupoField;
        
        private string tokenField;
        
        private long tpambField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public long grupo {
            get {
                return this.grupoField;
            }
            set {
                this.grupoField = value;
                this.RaisePropertyChanged("grupo");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string token {
            get {
                return this.tokenField;
            }
            set {
                this.tokenField = value;
                this.RaisePropertyChanged("token");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public long tpamb {
            get {
                return this.tpambField;
            }
            set {
                this.tpambField = value;
                this.RaisePropertyChanged("tpamb");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2558.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.esocial.gov.br/ws")]
    public partial class integraResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private long protocoloField;
        
        private bool protocoloFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public long protocolo {
            get {
                return this.protocoloField;
            }
            set {
                this.protocoloField = value;
                this.RaisePropertyChanged("protocolo");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool protocoloSpecified {
            get {
                return this.protocoloFieldSpecified;
            }
            set {
                this.protocoloFieldSpecified = value;
                this.RaisePropertyChanged("protocoloSpecified");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class integraRequest1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.esocial.gov.br/ws", Order=0)]
        public IntegradorService.apiEnviaTXT.integraRequest integraRequest;
        
        public integraRequest1() {
        }
        
        public integraRequest1(IntegradorService.apiEnviaTXT.integraRequest integraRequest) {
            this.integraRequest = integraRequest;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class integraResponse1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.esocial.gov.br/ws", Order=0)]
        public IntegradorService.apiEnviaTXT.integraResponse integraResponse;
        
        public integraResponse1() {
        }
        
        public integraResponse1(IntegradorService.apiEnviaTXT.integraResponse integraResponse) {
            this.integraResponse = integraResponse;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface EsocialServiceChannel : IntegradorService.apiEnviaTXT.EsocialService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class EsocialServiceClient : System.ServiceModel.ClientBase<IntegradorService.apiEnviaTXT.EsocialService>, IntegradorService.apiEnviaTXT.EsocialService {
        
        public EsocialServiceClient() {
        }
        
        public EsocialServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public EsocialServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EsocialServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EsocialServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        IntegradorService.apiEnviaTXT.integraResponse1 IntegradorService.apiEnviaTXT.EsocialService.integraRequest(IntegradorService.apiEnviaTXT.integraRequest1 request) {
            return base.Channel.integraRequest(request);
        }
        
        public IntegradorService.apiEnviaTXT.integraResponse integraRequest(IntegradorService.apiEnviaTXT.integraRequest integraRequest1) {
            IntegradorService.apiEnviaTXT.integraRequest1 inValue = new IntegradorService.apiEnviaTXT.integraRequest1();
            inValue.integraRequest = integraRequest1;
            IntegradorService.apiEnviaTXT.integraResponse1 retVal = ((IntegradorService.apiEnviaTXT.EsocialService)(this)).integraRequest(inValue);
            return retVal.integraResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<IntegradorService.apiEnviaTXT.integraResponse1> IntegradorService.apiEnviaTXT.EsocialService.integraRequestAsync(IntegradorService.apiEnviaTXT.integraRequest1 request) {
            return base.Channel.integraRequestAsync(request);
        }
        
        public System.Threading.Tasks.Task<IntegradorService.apiEnviaTXT.integraResponse1> integraRequestAsync(IntegradorService.apiEnviaTXT.integraRequest integraRequest) {
            IntegradorService.apiEnviaTXT.integraRequest1 inValue = new IntegradorService.apiEnviaTXT.integraRequest1();
            inValue.integraRequest = integraRequest;
            return ((IntegradorService.apiEnviaTXT.EsocialService)(this)).integraRequestAsync(inValue);
        }
    }
}
