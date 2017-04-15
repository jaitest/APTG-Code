﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.34209.
// 
#pragma warning disable 1591

namespace HSRP.WebReference_TG {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="HSRPWebService_Binding", Namespace="urn:microsoft-dynamics-schemas/codeunit/HSRPWebService")]
    public partial class HSRPWebService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback UpdateWebOrderCreationOperationCompleted;
        
        private System.Threading.SendOrPostCallback UpdateWebInventoryOperationCompleted;
        
        private System.Threading.SendOrPostCallback ShipmentReceivedOperationCompleted;
        
        private System.Threading.SendOrPostCallback PostConsumptionOperationCompleted;
        
        private System.Threading.SendOrPostCallback UpdateItemBYSerialInventoryOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public HSRPWebService() {
            this.Url = global::HSRP.Properties.Settings.Default.HSRP_WebReference_TG_HSRPWebService;
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
        public event UpdateWebOrderCreationCompletedEventHandler UpdateWebOrderCreationCompleted;
        
        /// <remarks/>
        public event UpdateWebInventoryCompletedEventHandler UpdateWebInventoryCompleted;
        
        /// <remarks/>
        public event ShipmentReceivedCompletedEventHandler ShipmentReceivedCompleted;
        
        /// <remarks/>
        public event PostConsumptionCompletedEventHandler PostConsumptionCompleted;
        
        /// <remarks/>
        public event UpdateItemBYSerialInventoryCompletedEventHandler UpdateItemBYSerialInventoryCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:microsoft-dynamics-schemas/codeunit/HSRPWebService:UpdateWebOrderCreation", RequestNamespace="urn:microsoft-dynamics-schemas/codeunit/HSRPWebService", ResponseElementName="UpdateWebOrderCreation_Result", ResponseNamespace="urn:microsoft-dynamics-schemas/codeunit/HSRPWebService", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return_value")]
        public bool UpdateWebOrderCreation(int hSRP_Request_ID) {
            object[] results = this.Invoke("UpdateWebOrderCreation", new object[] {
                        hSRP_Request_ID});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void UpdateWebOrderCreationAsync(int hSRP_Request_ID) {
            this.UpdateWebOrderCreationAsync(hSRP_Request_ID, null);
        }
        
        /// <remarks/>
        public void UpdateWebOrderCreationAsync(int hSRP_Request_ID, object userState) {
            if ((this.UpdateWebOrderCreationOperationCompleted == null)) {
                this.UpdateWebOrderCreationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdateWebOrderCreationOperationCompleted);
            }
            this.InvokeAsync("UpdateWebOrderCreation", new object[] {
                        hSRP_Request_ID}, this.UpdateWebOrderCreationOperationCompleted, userState);
        }
        
        private void OnUpdateWebOrderCreationOperationCompleted(object arg) {
            if ((this.UpdateWebOrderCreationCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UpdateWebOrderCreationCompleted(this, new UpdateWebOrderCreationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:microsoft-dynamics-schemas/codeunit/HSRPWebService:UpdateWebInventory", RequestNamespace="urn:microsoft-dynamics-schemas/codeunit/HSRPWebService", ResponseElementName="UpdateWebInventory_Result", ResponseNamespace="urn:microsoft-dynamics-schemas/codeunit/HSRPWebService", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return_value")]
        public decimal UpdateWebInventory(string locationCode, string itemCode) {
            object[] results = this.Invoke("UpdateWebInventory", new object[] {
                        locationCode,
                        itemCode});
            return ((decimal)(results[0]));
        }
        
        /// <remarks/>
        public void UpdateWebInventoryAsync(string locationCode, string itemCode) {
            this.UpdateWebInventoryAsync(locationCode, itemCode, null);
        }
        
        /// <remarks/>
        public void UpdateWebInventoryAsync(string locationCode, string itemCode, object userState) {
            if ((this.UpdateWebInventoryOperationCompleted == null)) {
                this.UpdateWebInventoryOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdateWebInventoryOperationCompleted);
            }
            this.InvokeAsync("UpdateWebInventory", new object[] {
                        locationCode,
                        itemCode}, this.UpdateWebInventoryOperationCompleted, userState);
        }
        
        private void OnUpdateWebInventoryOperationCompleted(object arg) {
            if ((this.UpdateWebInventoryCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UpdateWebInventoryCompleted(this, new UpdateWebInventoryCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:microsoft-dynamics-schemas/codeunit/HSRPWebService:ShipmentReceived", RequestNamespace="urn:microsoft-dynamics-schemas/codeunit/HSRPWebService", ResponseElementName="ShipmentReceived_Result", ResponseNamespace="urn:microsoft-dynamics-schemas/codeunit/HSRPWebService", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return_value")]
        public string ShipmentReceived(string transferOrderNo) {
            object[] results = this.Invoke("ShipmentReceived", new object[] {
                        transferOrderNo});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ShipmentReceivedAsync(string transferOrderNo) {
            this.ShipmentReceivedAsync(transferOrderNo, null);
        }
        
        /// <remarks/>
        public void ShipmentReceivedAsync(string transferOrderNo, object userState) {
            if ((this.ShipmentReceivedOperationCompleted == null)) {
                this.ShipmentReceivedOperationCompleted = new System.Threading.SendOrPostCallback(this.OnShipmentReceivedOperationCompleted);
            }
            this.InvokeAsync("ShipmentReceived", new object[] {
                        transferOrderNo}, this.ShipmentReceivedOperationCompleted, userState);
        }
        
        private void OnShipmentReceivedOperationCompleted(object arg) {
            if ((this.ShipmentReceivedCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ShipmentReceivedCompleted(this, new ShipmentReceivedCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:microsoft-dynamics-schemas/codeunit/HSRPWebService:PostConsumption", RequestNamespace="urn:microsoft-dynamics-schemas/codeunit/HSRPWebService", ResponseElementName="PostConsumption_Result", ResponseNamespace="urn:microsoft-dynamics-schemas/codeunit/HSRPWebService", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void PostConsumption([System.Xml.Serialization.XmlElementAttribute(DataType="date")] System.DateTime postingDate, string documentNo, string itemCode, decimal quantity, string locationCode) {
            this.Invoke("PostConsumption", new object[] {
                        postingDate,
                        documentNo,
                        itemCode,
                        quantity,
                        locationCode});
        }
        
        /// <remarks/>
        public void PostConsumptionAsync(System.DateTime postingDate, string documentNo, string itemCode, decimal quantity, string locationCode) {
            this.PostConsumptionAsync(postingDate, documentNo, itemCode, quantity, locationCode, null);
        }
        
        /// <remarks/>
        public void PostConsumptionAsync(System.DateTime postingDate, string documentNo, string itemCode, decimal quantity, string locationCode, object userState) {
            if ((this.PostConsumptionOperationCompleted == null)) {
                this.PostConsumptionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPostConsumptionOperationCompleted);
            }
            this.InvokeAsync("PostConsumption", new object[] {
                        postingDate,
                        documentNo,
                        itemCode,
                        quantity,
                        locationCode}, this.PostConsumptionOperationCompleted, userState);
        }
        
        private void OnPostConsumptionOperationCompleted(object arg) {
            if ((this.PostConsumptionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PostConsumptionCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:microsoft-dynamics-schemas/codeunit/HSRPWebService:UpdateItemBYSerialInventor" +
            "y", RequestNamespace="urn:microsoft-dynamics-schemas/codeunit/HSRPWebService", ResponseElementName="UpdateItemBYSerialInventory_Result", ResponseNamespace="urn:microsoft-dynamics-schemas/codeunit/HSRPWebService", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return_value")]
        public decimal UpdateItemBYSerialInventory(string locationCode, string itemCode) {
            object[] results = this.Invoke("UpdateItemBYSerialInventory", new object[] {
                        locationCode,
                        itemCode});
            return ((decimal)(results[0]));
        }
        
        /// <remarks/>
        public void UpdateItemBYSerialInventoryAsync(string locationCode, string itemCode) {
            this.UpdateItemBYSerialInventoryAsync(locationCode, itemCode, null);
        }
        
        /// <remarks/>
        public void UpdateItemBYSerialInventoryAsync(string locationCode, string itemCode, object userState) {
            if ((this.UpdateItemBYSerialInventoryOperationCompleted == null)) {
                this.UpdateItemBYSerialInventoryOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdateItemBYSerialInventoryOperationCompleted);
            }
            this.InvokeAsync("UpdateItemBYSerialInventory", new object[] {
                        locationCode,
                        itemCode}, this.UpdateItemBYSerialInventoryOperationCompleted, userState);
        }
        
        private void OnUpdateItemBYSerialInventoryOperationCompleted(object arg) {
            if ((this.UpdateItemBYSerialInventoryCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UpdateItemBYSerialInventoryCompleted(this, new UpdateItemBYSerialInventoryCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void UpdateWebOrderCreationCompletedEventHandler(object sender, UpdateWebOrderCreationCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UpdateWebOrderCreationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UpdateWebOrderCreationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void UpdateWebInventoryCompletedEventHandler(object sender, UpdateWebInventoryCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UpdateWebInventoryCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UpdateWebInventoryCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public decimal Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((decimal)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void ShipmentReceivedCompletedEventHandler(object sender, ShipmentReceivedCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ShipmentReceivedCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ShipmentReceivedCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void PostConsumptionCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void UpdateItemBYSerialInventoryCompletedEventHandler(object sender, UpdateItemBYSerialInventoryCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UpdateItemBYSerialInventoryCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UpdateItemBYSerialInventoryCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public decimal Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((decimal)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591