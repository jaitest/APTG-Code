﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.34014.
// 
#pragma warning disable 1591

namespace HSRP.APWebrefrence {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="HSRPAuthorizationServiceSoap", Namespace="http://tempuri.org/")]
    public partial class HSRPAuthorizationService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetHSRPAuthorizationDetailsOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetHSRPAuthorizationnoOperationCompleted;
        
        private System.Threading.SendOrPostCallback UpdateHSRPChargesOperationCompleted;
        
        private System.Threading.SendOrPostCallback UpdateHSRPLaserCodesOperationCompleted;
        
        private System.Threading.SendOrPostCallback UpdateHSRPAffixationOperationCompleted;
        
        private System.Threading.SendOrPostCallback UpdateHSRPAvailableAtOfficeOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public HSRPAuthorizationService() {
            this.Url = global::HSRP.Properties.Settings.Default.HSRP_APWebrefrence_HSRPAuthorizationService;
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
        public event GetHSRPAuthorizationDetailsCompletedEventHandler GetHSRPAuthorizationDetailsCompleted;
        
        /// <remarks/>
        public event GetHSRPAuthorizationnoCompletedEventHandler GetHSRPAuthorizationnoCompleted;
        
        /// <remarks/>
        public event UpdateHSRPChargesCompletedEventHandler UpdateHSRPChargesCompleted;
        
        /// <remarks/>
        public event UpdateHSRPLaserCodesCompletedEventHandler UpdateHSRPLaserCodesCompleted;
        
        /// <remarks/>
        public event UpdateHSRPAffixationCompletedEventHandler UpdateHSRPAffixationCompleted;
        
        /// <remarks/>
        public event UpdateHSRPAvailableAtOfficeCompletedEventHandler UpdateHSRPAvailableAtOfficeCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetHSRPAuthorizationDetails", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetHSRPAuthorizationDetails(string fromdate, string todate, string officecode) {
            object[] results = this.Invoke("GetHSRPAuthorizationDetails", new object[] {
                        fromdate,
                        todate,
                        officecode});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetHSRPAuthorizationDetailsAsync(string fromdate, string todate, string officecode) {
            this.GetHSRPAuthorizationDetailsAsync(fromdate, todate, officecode, null);
        }
        
        /// <remarks/>
        public void GetHSRPAuthorizationDetailsAsync(string fromdate, string todate, string officecode, object userState) {
            if ((this.GetHSRPAuthorizationDetailsOperationCompleted == null)) {
                this.GetHSRPAuthorizationDetailsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetHSRPAuthorizationDetailsOperationCompleted);
            }
            this.InvokeAsync("GetHSRPAuthorizationDetails", new object[] {
                        fromdate,
                        todate,
                        officecode}, this.GetHSRPAuthorizationDetailsOperationCompleted, userState);
        }
        
        private void OnGetHSRPAuthorizationDetailsOperationCompleted(object arg) {
            if ((this.GetHSRPAuthorizationDetailsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetHSRPAuthorizationDetailsCompleted(this, new GetHSRPAuthorizationDetailsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetHSRPAuthorizationno", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetHSRPAuthorizationno(string Authno) {
            object[] results = this.Invoke("GetHSRPAuthorizationno", new object[] {
                        Authno});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetHSRPAuthorizationnoAsync(string Authno) {
            this.GetHSRPAuthorizationnoAsync(Authno, null);
        }
        
        /// <remarks/>
        public void GetHSRPAuthorizationnoAsync(string Authno, object userState) {
            if ((this.GetHSRPAuthorizationnoOperationCompleted == null)) {
                this.GetHSRPAuthorizationnoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetHSRPAuthorizationnoOperationCompleted);
            }
            this.InvokeAsync("GetHSRPAuthorizationno", new object[] {
                        Authno}, this.GetHSRPAuthorizationnoOperationCompleted, userState);
        }
        
        private void OnGetHSRPAuthorizationnoOperationCompleted(object arg) {
            if ((this.GetHSRPAuthorizationnoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetHSRPAuthorizationnoCompleted(this, new GetHSRPAuthorizationnoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/UpdateHSRPCharges", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string UpdateHSRPCharges(string Authno, decimal Amount, string CollectionDate) {
            object[] results = this.Invoke("UpdateHSRPCharges", new object[] {
                        Authno,
                        Amount,
                        CollectionDate});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void UpdateHSRPChargesAsync(string Authno, decimal Amount, string CollectionDate) {
            this.UpdateHSRPChargesAsync(Authno, Amount, CollectionDate, null);
        }
        
        /// <remarks/>
        public void UpdateHSRPChargesAsync(string Authno, decimal Amount, string CollectionDate, object userState) {
            if ((this.UpdateHSRPChargesOperationCompleted == null)) {
                this.UpdateHSRPChargesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdateHSRPChargesOperationCompleted);
            }
            this.InvokeAsync("UpdateHSRPCharges", new object[] {
                        Authno,
                        Amount,
                        CollectionDate}, this.UpdateHSRPChargesOperationCompleted, userState);
        }
        
        private void OnUpdateHSRPChargesOperationCompleted(object arg) {
            if ((this.UpdateHSRPChargesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UpdateHSRPChargesCompleted(this, new UpdateHSRPChargesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/UpdateHSRPLaserCodes", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string UpdateHSRPLaserCodes(string Authno, string Frontlasercode, string Rearlasercode, string Embossingdate) {
            object[] results = this.Invoke("UpdateHSRPLaserCodes", new object[] {
                        Authno,
                        Frontlasercode,
                        Rearlasercode,
                        Embossingdate});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void UpdateHSRPLaserCodesAsync(string Authno, string Frontlasercode, string Rearlasercode, string Embossingdate) {
            this.UpdateHSRPLaserCodesAsync(Authno, Frontlasercode, Rearlasercode, Embossingdate, null);
        }
        
        /// <remarks/>
        public void UpdateHSRPLaserCodesAsync(string Authno, string Frontlasercode, string Rearlasercode, string Embossingdate, object userState) {
            if ((this.UpdateHSRPLaserCodesOperationCompleted == null)) {
                this.UpdateHSRPLaserCodesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdateHSRPLaserCodesOperationCompleted);
            }
            this.InvokeAsync("UpdateHSRPLaserCodes", new object[] {
                        Authno,
                        Frontlasercode,
                        Rearlasercode,
                        Embossingdate}, this.UpdateHSRPLaserCodesOperationCompleted, userState);
        }
        
        private void OnUpdateHSRPLaserCodesOperationCompleted(object arg) {
            if ((this.UpdateHSRPLaserCodesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UpdateHSRPLaserCodesCompleted(this, new UpdateHSRPLaserCodesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/UpdateHSRPAffixation", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string UpdateHSRPAffixation(string Authno, string Affixationdate) {
            object[] results = this.Invoke("UpdateHSRPAffixation", new object[] {
                        Authno,
                        Affixationdate});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void UpdateHSRPAffixationAsync(string Authno, string Affixationdate) {
            this.UpdateHSRPAffixationAsync(Authno, Affixationdate, null);
        }
        
        /// <remarks/>
        public void UpdateHSRPAffixationAsync(string Authno, string Affixationdate, object userState) {
            if ((this.UpdateHSRPAffixationOperationCompleted == null)) {
                this.UpdateHSRPAffixationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdateHSRPAffixationOperationCompleted);
            }
            this.InvokeAsync("UpdateHSRPAffixation", new object[] {
                        Authno,
                        Affixationdate}, this.UpdateHSRPAffixationOperationCompleted, userState);
        }
        
        private void OnUpdateHSRPAffixationOperationCompleted(object arg) {
            if ((this.UpdateHSRPAffixationCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UpdateHSRPAffixationCompleted(this, new UpdateHSRPAffixationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/UpdateHSRPAvailableAtOffice", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string UpdateHSRPAvailableAtOffice(string Authno, string PlateAvailableatOfficeDate) {
            object[] results = this.Invoke("UpdateHSRPAvailableAtOffice", new object[] {
                        Authno,
                        PlateAvailableatOfficeDate});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void UpdateHSRPAvailableAtOfficeAsync(string Authno, string PlateAvailableatOfficeDate) {
            this.UpdateHSRPAvailableAtOfficeAsync(Authno, PlateAvailableatOfficeDate, null);
        }
        
        /// <remarks/>
        public void UpdateHSRPAvailableAtOfficeAsync(string Authno, string PlateAvailableatOfficeDate, object userState) {
            if ((this.UpdateHSRPAvailableAtOfficeOperationCompleted == null)) {
                this.UpdateHSRPAvailableAtOfficeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdateHSRPAvailableAtOfficeOperationCompleted);
            }
            this.InvokeAsync("UpdateHSRPAvailableAtOffice", new object[] {
                        Authno,
                        PlateAvailableatOfficeDate}, this.UpdateHSRPAvailableAtOfficeOperationCompleted, userState);
        }
        
        private void OnUpdateHSRPAvailableAtOfficeOperationCompleted(object arg) {
            if ((this.UpdateHSRPAvailableAtOfficeCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UpdateHSRPAvailableAtOfficeCompleted(this, new UpdateHSRPAvailableAtOfficeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    public delegate void GetHSRPAuthorizationDetailsCompletedEventHandler(object sender, GetHSRPAuthorizationDetailsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetHSRPAuthorizationDetailsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetHSRPAuthorizationDetailsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    public delegate void GetHSRPAuthorizationnoCompletedEventHandler(object sender, GetHSRPAuthorizationnoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetHSRPAuthorizationnoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetHSRPAuthorizationnoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    public delegate void UpdateHSRPChargesCompletedEventHandler(object sender, UpdateHSRPChargesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UpdateHSRPChargesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UpdateHSRPChargesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    public delegate void UpdateHSRPLaserCodesCompletedEventHandler(object sender, UpdateHSRPLaserCodesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UpdateHSRPLaserCodesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UpdateHSRPLaserCodesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    public delegate void UpdateHSRPAffixationCompletedEventHandler(object sender, UpdateHSRPAffixationCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UpdateHSRPAffixationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UpdateHSRPAffixationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    public delegate void UpdateHSRPAvailableAtOfficeCompletedEventHandler(object sender, UpdateHSRPAvailableAtOfficeCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UpdateHSRPAvailableAtOfficeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UpdateHSRPAvailableAtOfficeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
}

#pragma warning restore 1591