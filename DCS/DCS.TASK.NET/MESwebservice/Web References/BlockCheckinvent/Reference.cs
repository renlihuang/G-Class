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

namespace MESwebservice.BlockCheckinvent {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="MiCheckInventoryAttributesServiceBinding", Namespace="http://machineintegration.ws.atlmes.com/")]
    public partial class MiCheckInventoryAttributesServiceService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback miCheckInventoryAttributesOperationCompleted;
        
        private System.Threading.SendOrPostCallback getNcListByInventoryIDOperationCompleted;
        
        private System.Threading.SendOrPostCallback getInventoryIDDataFieldOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public MiCheckInventoryAttributesServiceService() {
            this.Url = global::MESwebservice.Properties.Settings.Default.MESwebservice_BlockCheckinvent_MiCheckInventoryAttributesServiceService;
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
        public event miCheckInventoryAttributesCompletedEventHandler miCheckInventoryAttributesCompleted;
        
        /// <remarks/>
        public event getNcListByInventoryIDCompletedEventHandler getNcListByInventoryIDCompleted;
        
        /// <remarks/>
        public event getInventoryIDDataFieldCompletedEventHandler getInventoryIDDataFieldCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("miCheckInventoryAttributesResponse", Namespace="http://machineintegration.ws.atlmes.com/")]
        public miCheckInventoryAttributesResponse miCheckInventoryAttributes([System.Xml.Serialization.XmlElementAttribute("miCheckInventoryAttributes", Namespace="http://machineintegration.ws.atlmes.com/")] miCheckInventoryAttributes miCheckInventoryAttributes1) {
            object[] results = this.Invoke("miCheckInventoryAttributes", new object[] {
                        miCheckInventoryAttributes1});
            return ((miCheckInventoryAttributesResponse)(results[0]));
        }
        
        /// <remarks/>
        public void miCheckInventoryAttributesAsync(miCheckInventoryAttributes miCheckInventoryAttributes1) {
            this.miCheckInventoryAttributesAsync(miCheckInventoryAttributes1, null);
        }
        
        /// <remarks/>
        public void miCheckInventoryAttributesAsync(miCheckInventoryAttributes miCheckInventoryAttributes1, object userState) {
            if ((this.miCheckInventoryAttributesOperationCompleted == null)) {
                this.miCheckInventoryAttributesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnmiCheckInventoryAttributesOperationCompleted);
            }
            this.InvokeAsync("miCheckInventoryAttributes", new object[] {
                        miCheckInventoryAttributes1}, this.miCheckInventoryAttributesOperationCompleted, userState);
        }
        
        private void OnmiCheckInventoryAttributesOperationCompleted(object arg) {
            if ((this.miCheckInventoryAttributesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.miCheckInventoryAttributesCompleted(this, new miCheckInventoryAttributesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlArrayAttribute("getNcListByInventoryIDResponse", Namespace="http://machineintegration.ws.atlmes.com/")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public string[] getNcListByInventoryID([System.Xml.Serialization.XmlElementAttribute("getNcListByInventoryID", Namespace="http://machineintegration.ws.atlmes.com/")] getNcListByInventoryID getNcListByInventoryID1) {
            object[] results = this.Invoke("getNcListByInventoryID", new object[] {
                        getNcListByInventoryID1});
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void getNcListByInventoryIDAsync(getNcListByInventoryID getNcListByInventoryID1) {
            this.getNcListByInventoryIDAsync(getNcListByInventoryID1, null);
        }
        
        /// <remarks/>
        public void getNcListByInventoryIDAsync(getNcListByInventoryID getNcListByInventoryID1, object userState) {
            if ((this.getNcListByInventoryIDOperationCompleted == null)) {
                this.getNcListByInventoryIDOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetNcListByInventoryIDOperationCompleted);
            }
            this.InvokeAsync("getNcListByInventoryID", new object[] {
                        getNcListByInventoryID1}, this.getNcListByInventoryIDOperationCompleted, userState);
        }
        
        private void OngetNcListByInventoryIDOperationCompleted(object arg) {
            if ((this.getNcListByInventoryIDCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getNcListByInventoryIDCompleted(this, new getNcListByInventoryIDCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("getInventoryIDDataFieldResponse", Namespace="http://machineintegration.ws.atlmes.com/")]
        public getInventoryIDDataFieldResponse getInventoryIDDataField([System.Xml.Serialization.XmlElementAttribute("getInventoryIDDataField", Namespace="http://machineintegration.ws.atlmes.com/")] getInventoryIDDataField getInventoryIDDataField1) {
            object[] results = this.Invoke("getInventoryIDDataField", new object[] {
                        getInventoryIDDataField1});
            return ((getInventoryIDDataFieldResponse)(results[0]));
        }
        
        /// <remarks/>
        public void getInventoryIDDataFieldAsync(getInventoryIDDataField getInventoryIDDataField1) {
            this.getInventoryIDDataFieldAsync(getInventoryIDDataField1, null);
        }
        
        /// <remarks/>
        public void getInventoryIDDataFieldAsync(getInventoryIDDataField getInventoryIDDataField1, object userState) {
            if ((this.getInventoryIDDataFieldOperationCompleted == null)) {
                this.getInventoryIDDataFieldOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetInventoryIDDataFieldOperationCompleted);
            }
            this.InvokeAsync("getInventoryIDDataField", new object[] {
                        getInventoryIDDataField1}, this.getInventoryIDDataFieldOperationCompleted, userState);
        }
        
        private void OngetInventoryIDDataFieldOperationCompleted(object arg) {
            if ((this.getInventoryIDDataFieldCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getInventoryIDDataFieldCompleted(this, new getInventoryIDDataFieldCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public partial class miCheckInventoryAttributes {
        
        private ModuleCellMarkingOrTimeCheckRequest checkInventoryAttributesRequestField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ModuleCellMarkingOrTimeCheckRequest CheckInventoryAttributesRequest {
            get {
                return this.checkInventoryAttributesRequestField;
            }
            set {
                this.checkInventoryAttributesRequestField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://machineintegration.ws.atlmes.com/")]
    public partial class ModuleCellMarkingOrTimeCheckRequest {
        
        private string siteField;
        
        private string sfcField;
        
        private string operationField;
        
        private string operationRevisionField;
        
        private string resourceField;
        
        private string userField;
        
        private string activityIdField;
        
        private modeCheckInventory modeCheckInventoryField;
        
        private int requiredQuantityField;
        
        private string[] inventoryArrayField;
        
        private string[] inventoryAttributeListField;
        
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
        public modeCheckInventory modeCheckInventory {
            get {
                return this.modeCheckInventoryField;
            }
            set {
                this.modeCheckInventoryField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int requiredQuantity {
            get {
                return this.requiredQuantityField;
            }
            set {
                this.requiredQuantityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("inventoryArray", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string[] inventoryArray {
            get {
                return this.inventoryArrayField;
            }
            set {
                this.inventoryArrayField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("inventoryAttributeList", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string[] inventoryAttributeList {
            get {
                return this.inventoryAttributeListField;
            }
            set {
                this.inventoryAttributeListField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://machineintegration.ws.atlmes.com/")]
    public enum modeCheckInventory {
        
        /// <remarks/>
        MODE_MARK_ONLY,
        
        /// <remarks/>
        MODE_TIME_ONLY,
        
        /// <remarks/>
        MODE_MARK_AND_TIME,
        
        /// <remarks/>
        MODE_NONE,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://machineintegration.ws.atlmes.com/")]
    public partial class getInventoryIDDataFieldResponse {
        
        private string returnField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string @return {
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
    public partial class getInventoryIDDataField {
        
        private string arg0Field;
        
        private string arg1Field;
        
        private string arg2Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string arg0 {
            get {
                return this.arg0Field;
            }
            set {
                this.arg0Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string arg1 {
            get {
                return this.arg1Field;
            }
            set {
                this.arg1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string arg2 {
            get {
                return this.arg2Field;
            }
            set {
                this.arg2Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://machineintegration.ws.atlmes.com/")]
    public partial class getNcListByInventoryID {
        
        private string arg0Field;
        
        private string arg1Field;
        
        private string arg2Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string arg0 {
            get {
                return this.arg0Field;
            }
            set {
                this.arg0Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string arg1 {
            get {
                return this.arg1Field;
            }
            set {
                this.arg1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string arg2 {
            get {
                return this.arg2Field;
            }
            set {
                this.arg2Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://machineintegration.ws.atlmes.com/")]
    public partial class checkInventoryDataFields {
        
        private string inventoryField;
        
        private string attributeField;
        
        private string valueField;
        
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
    public partial class checkInventoryAttributesResponse {
        
        private int codeField;
        
        private string messageField;
        
        private string failedSfcField;
        
        private string failedInventoryField;
        
        private checkInventoryDataFields[] inventoryDataFieldsField;
        
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
        public string failedSfc {
            get {
                return this.failedSfcField;
            }
            set {
                this.failedSfcField = value;
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
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("inventoryDataFields", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public checkInventoryDataFields[] inventoryDataFields {
            get {
                return this.inventoryDataFieldsField;
            }
            set {
                this.inventoryDataFieldsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://machineintegration.ws.atlmes.com/")]
    public partial class miCheckInventoryAttributesResponse {
        
        private checkInventoryAttributesResponse returnField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public checkInventoryAttributesResponse @return {
            get {
                return this.returnField;
            }
            set {
                this.returnField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void miCheckInventoryAttributesCompletedEventHandler(object sender, miCheckInventoryAttributesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class miCheckInventoryAttributesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal miCheckInventoryAttributesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public miCheckInventoryAttributesResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((miCheckInventoryAttributesResponse)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void getNcListByInventoryIDCompletedEventHandler(object sender, getNcListByInventoryIDCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getNcListByInventoryIDCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getNcListByInventoryIDCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void getInventoryIDDataFieldCompletedEventHandler(object sender, getInventoryIDDataFieldCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getInventoryIDDataFieldCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getInventoryIDDataFieldCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public getInventoryIDDataFieldResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((getInventoryIDDataFieldResponse)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591