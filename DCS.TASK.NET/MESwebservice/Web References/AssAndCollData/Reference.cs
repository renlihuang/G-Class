﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.VSDesigner 4.0.30319.42000 版自动生成。
// 
#pragma warning disable 1591

namespace MESwebservice.AssAndCollData {
    using System.Diagnostics;
    using System;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System.Web.Services;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="MiAssembleAndCollectDataForSfcServiceBinding", Namespace="http://machineintegration.ws.atlmes.com/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeValue))]
    public partial class MiAssembleAndCollectDataForSfcServiceService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback miAssmebleAndCollectDataForSfcOperationCompleted;
        
        private System.Threading.SendOrPostCallback getInventoryQtyOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public MiAssembleAndCollectDataForSfcServiceService() {
            this.Url = global::MESwebservice.Properties.Settings.Default.MESwebservice_AssAndCollData_MiAssembleAndCollectDataForSfcServiceService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event miAssmebleAndCollectDataForSfcCompletedEventHandler miAssmebleAndCollectDataForSfcCompleted;
        
        /// <remarks/>
        public event getInventoryQtyCompletedEventHandler getInventoryQtyCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("miAssmebleAndCollectDataForSfcResponse", Namespace="http://machineintegration.ws.atlmes.com/")]
        public miAssmebleAndCollectDataForSfcResponse miAssmebleAndCollectDataForSfc([System.Xml.Serialization.XmlElementAttribute("miAssmebleAndCollectDataForSfc", Namespace="http://machineintegration.ws.atlmes.com/")] miAssmebleAndCollectDataForSfc miAssmebleAndCollectDataForSfc1) {
            object[] results = this.Invoke("miAssmebleAndCollectDataForSfc", new object[] {
                        miAssmebleAndCollectDataForSfc1});
            return ((miAssmebleAndCollectDataForSfcResponse)(results[0]));
        }
        
        /// <remarks/>
        public void miAssmebleAndCollectDataForSfcAsync(miAssmebleAndCollectDataForSfc miAssmebleAndCollectDataForSfc1) {
            this.miAssmebleAndCollectDataForSfcAsync(miAssmebleAndCollectDataForSfc1, null);
        }
        
        /// <remarks/>
        public void miAssmebleAndCollectDataForSfcAsync(miAssmebleAndCollectDataForSfc miAssmebleAndCollectDataForSfc1, object userState) {
            if ((this.miAssmebleAndCollectDataForSfcOperationCompleted == null)) {
                this.miAssmebleAndCollectDataForSfcOperationCompleted = new System.Threading.SendOrPostCallback(this.OnmiAssmebleAndCollectDataForSfcOperationCompleted);
            }
            this.InvokeAsync("miAssmebleAndCollectDataForSfc", new object[] {
                        miAssmebleAndCollectDataForSfc1}, this.miAssmebleAndCollectDataForSfcOperationCompleted, userState);
        }
        
        private void OnmiAssmebleAndCollectDataForSfcOperationCompleted(object arg) {
            if ((this.miAssmebleAndCollectDataForSfcCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.miAssmebleAndCollectDataForSfcCompleted(this, new miAssmebleAndCollectDataForSfcCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("getInventoryQtyResponse", Namespace="http://machineintegration.ws.atlmes.com/")]
        public getInventoryQtyResponse getInventoryQty([System.Xml.Serialization.XmlElementAttribute("getInventoryQty", Namespace="http://machineintegration.ws.atlmes.com/")] getInventoryQty getInventoryQty1) {
            object[] results = this.Invoke("getInventoryQty", new object[] {
                        getInventoryQty1});
            return ((getInventoryQtyResponse)(results[0]));
        }
        
        /// <remarks/>
        public void getInventoryQtyAsync(getInventoryQty getInventoryQty1) {
            this.getInventoryQtyAsync(getInventoryQty1, null);
        }
        
        /// <remarks/>
        public void getInventoryQtyAsync(getInventoryQty getInventoryQty1, object userState) {
            if ((this.getInventoryQtyOperationCompleted == null)) {
                this.getInventoryQtyOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetInventoryQtyOperationCompleted);
            }
            this.InvokeAsync("getInventoryQty", new object[] {
                        getInventoryQty1}, this.getInventoryQtyOperationCompleted, userState);
        }
        
        private void OngetInventoryQtyOperationCompleted(object arg) {
            if ((this.getInventoryQtyCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getInventoryQtyCompleted(this, new getInventoryQtyCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://machineintegration.ws.atlmes.com/")]
    public partial class miAssmebleAndCollectDataForSfc {
        
        private assembleAndCollectDataForSfcRequest assembleAndCollectDataForSfcRequestField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public assembleAndCollectDataForSfcRequest AssembleAndCollectDataForSfcRequest {
            get {
                return this.assembleAndCollectDataForSfcRequestField;
            }
            set {
                this.assembleAndCollectDataForSfcRequestField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://machineintegration.ws.atlmes.com/")]
    public partial class assembleAndCollectDataForSfcRequest {
        
        private string siteField;
        
        private string sfcField;
        
        private string dcGroupField;
        
        private string dcGroupRevisionField;
        
        private string operationField;
        
        private string operationRevisionField;
        
        private string resourceField;
        
        private string userField;
        
        private string activityIdField;
        
        private dataCollectForSfcModeProcessSfc modeProcessSFCField;
        
        private bool partialAssemblyField;
        
        private miInventoryData[] inventoryArrayField;
        
        private machineIntegrationParametricData[] parametricDataArrayField;
        
        private nonConfirmCodeArray[] ncCodeArrayField;
        
        private string remarkField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string site {
            get {
                return this.siteField;
            }
            set {
                this.siteField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sfc {
            get {
                return this.sfcField;
            }
            set {
                this.sfcField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dcGroup {
            get {
                return this.dcGroupField;
            }
            set {
                this.dcGroupField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dcGroupRevision {
            get {
                return this.dcGroupRevisionField;
            }
            set {
                this.dcGroupRevisionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string operation {
            get {
                return this.operationField;
            }
            set {
                this.operationField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string operationRevision {
            get {
                return this.operationRevisionField;
            }
            set {
                this.operationRevisionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resource {
            get {
                return this.resourceField;
            }
            set {
                this.resourceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string user {
            get {
                return this.userField;
            }
            set {
                this.userField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string activityId {
            get {
                return this.activityIdField;
            }
            set {
                this.activityIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public dataCollectForSfcModeProcessSfc modeProcessSFC {
            get {
                return this.modeProcessSFCField;
            }
            set {
                this.modeProcessSFCField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool partialAssembly {
            get {
                return this.partialAssemblyField;
            }
            set {
                this.partialAssemblyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("inventoryArray", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public miInventoryData[] inventoryArray {
            get {
                return this.inventoryArrayField;
            }
            set {
                this.inventoryArrayField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("parametricDataArray", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public machineIntegrationParametricData[] parametricDataArray {
            get {
                return this.parametricDataArrayField;
            }
            set {
                this.parametricDataArrayField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ncCodeArray", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public nonConfirmCodeArray[] ncCodeArray {
            get {
                return this.ncCodeArrayField;
            }
            set {
                this.ncCodeArrayField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string remark {
            get {
                return this.remarkField;
            }
            set {
                this.remarkField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://machineintegration.ws.atlmes.com/")]
    public enum dataCollectForSfcModeProcessSfc {
        
        /// <remarks/>
        MODE_NONE,
        
        /// <remarks/>
        MODE_START_SFC_PRE_DC,
        
        /// <remarks/>
        MODE_COMPLETE_SFC_POST_DC,
        
        /// <remarks/>
        MODE_PASS_SFC_POST_DC,
        
        /// <remarks/>
        MODE_REMOVE_PROCESSLOT_COMPLETE_SFC_POST_DC,
        
        /// <remarks/>
        MODE_START_AND_COMPLETE_SFC_POST_DC,
        
        /// <remarks/>
        MODE_START_SFC_PRE_DC_SFC_COMPLETE,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://machineintegration.ws.atlmes.com/")]
    public partial class miInventoryData {
        
        private string inventoryField;
        
        private string qtyField;
        
        private AssemblyDataField[] assemblyDataFieldsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string inventory {
            get {
                return this.inventoryField;
            }
            set {
                this.inventoryField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string qty {
            get {
                return this.qtyField;
            }
            set {
                this.qtyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("assemblyDataFields", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public AssemblyDataField[] assemblyDataFields {
            get {
                return this.assemblyDataFieldsField;
            }
            set {
                this.assemblyDataFieldsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.sap.com/me/production")]
    public partial class AssemblyDataField : AttributeValue {
        
        private decimal sequenceField;
        
        private bool sequenceFieldSpecified;
        
        /// <remarks/>
        public decimal sequence {
            get {
                return this.sequenceField;
            }
            set {
                this.sequenceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool sequenceSpecified {
            get {
                return this.sequenceFieldSpecified;
            }
            set {
                this.sequenceFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AssemblyDataField))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.sap.com/me/common")]
    public partial class AttributeValue {
        
        private string attributeField;
        
        private string valueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string attribute {
            get {
                return this.attributeField;
            }
            set {
                this.attributeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://machineintegration.ws.atlmes.com/")]
    public partial class baseResponse {
        
        private int codeField;
        
        private bool codeFieldSpecified;
        
        private string messageField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int code {
            get {
                return this.codeField;
            }
            set {
                this.codeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool codeSpecified {
            get {
                return this.codeFieldSpecified;
            }
            set {
                this.codeFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://machineintegration.ws.atlmes.com/")]
    public partial class getInventoryQtyResponse {
        
        private baseResponse returnField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public baseResponse @return {
            get {
                return this.returnField;
            }
            set {
                this.returnField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://machineintegration.ws.atlmes.com/")]
    public partial class getInventoryQtyRequest {
        
        private string siteField;
        
        private string inventoryField;
        
        private string resourceField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string site {
            get {
                return this.siteField;
            }
            set {
                this.siteField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string inventory {
            get {
                return this.inventoryField;
            }
            set {
                this.inventoryField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resource {
            get {
                return this.resourceField;
            }
            set {
                this.resourceField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://machineintegration.ws.atlmes.com/")]
    public partial class getInventoryQty {
        
        private getInventoryQtyRequest getInventoryQtyRequestField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public getInventoryQtyRequest GetInventoryQtyRequest {
            get {
                return this.getInventoryQtyRequestField;
            }
            set {
                this.getInventoryQtyRequestField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://machineintegration.ws.atlmes.com/")]
    public partial class assembleAndCollectDataForSfcResponse {
        
        private int codeField;
        
        private string messageField;
        
        private string sfcField;
        
        private string failedInventoryField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int code {
            get {
                return this.codeField;
            }
            set {
                this.codeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sfc {
            get {
                return this.sfcField;
            }
            set {
                this.sfcField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string failedInventory {
            get {
                return this.failedInventoryField;
            }
            set {
                this.failedInventoryField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://machineintegration.ws.atlmes.com/")]
    public partial class miAssmebleAndCollectDataForSfcResponse {
        
        private assembleAndCollectDataForSfcResponse returnField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public assembleAndCollectDataForSfcResponse @return {
            get {
                return this.returnField;
            }
            set {
                this.returnField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://machineintegration.ws.atlmes.com/")]
    public partial class nonConfirmCodeArray {
        
        private string ncCodeField;
        
        private bool hasNcField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ncCode {
            get {
                return this.ncCodeField;
            }
            set {
                this.ncCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool hasNc {
            get {
                return this.hasNcField;
            }
            set {
                this.hasNcField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://machineintegration.ws.atlmes.com/")]
    public partial class machineIntegrationParametricData {
        
        private string nameField;
        
        private string valueField;
        
        private ParameterDataType dataTypeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ParameterDataType dataType {
            get {
                return this.dataTypeField;
            }
            set {
                this.dataTypeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.sap.com/me/datacollection")]
    public enum ParameterDataType {
        
        /// <remarks/>
        NUMBER,
        
        /// <remarks/>
        TEXT,
        
        /// <remarks/>
        FORMULA,
        
        /// <remarks/>
        BOOLEAN,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void miAssmebleAndCollectDataForSfcCompletedEventHandler(object sender, miAssmebleAndCollectDataForSfcCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class miAssmebleAndCollectDataForSfcCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal miAssmebleAndCollectDataForSfcCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public miAssmebleAndCollectDataForSfcResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((miAssmebleAndCollectDataForSfcResponse)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void getInventoryQtyCompletedEventHandler(object sender, getInventoryQtyCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getInventoryQtyCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getInventoryQtyCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public getInventoryQtyResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((getInventoryQtyResponse)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591