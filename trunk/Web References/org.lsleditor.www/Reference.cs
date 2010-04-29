// /**
// ********
// *
// * ORIGIONAL CODE BASE IS Copyright (C) 2006-2010 by Alphons van der Heijden
// * The code was donated on 4/28/2010 by Alphons van der Heijden
// * To Brandon'Dimentox Travanti' Husbands & Malcolm J. Kudra which in turn Liscense under the GPLv2.
// * In agreement to Alphons van der Heijden wishes.
// *
// * The community would like to thank Alphons for all of his hard work, blood sweat and tears.
// * Without his work the community would be stuck with crappy editors.
// *
// * The source code in this file ("Source Code") is provided by The LSLEditor Group
// * to you under the terms of the GNU General Public License, version 2.0
// * ("GPL"), unless you have obtained a separate licensing agreement
// * ("Other License"), formally executed by you and The LSLEditor Group.  Terms of
// * the GPL can be found in the gplv2.txt document.
// *
// ********
// * GPLv2 Header
// ********
// * LSLEditor, a External editor for the LSL Language.
// * Copyright (C) 2010 The LSLEditor Group.
// 
// * This program is free software; you can redistribute it and/or
// * modify it under the terms of the GNU General Public License
// * as published by the Free Software Foundation; either version 2
// * of the License, or (at your option) any later version.
// *
// * This program is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with this program; if not, write to the Free Software
// * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// ********
// *
// * The above copyright notice and this permission notice shall be included in all 
// * copies or substantial portions of the Software.
// *
// ********
// */
#pragma warning disable 1591

namespace LSLEditor.org.lsleditor.www {
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="Service1Soap", Namespace="http://tempuri.org/")]
    public partial class Service1 : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback OpenOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetStatusOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetBugReportOperationCompleted;
        
        private System.Threading.SendOrPostCallback WriteOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Service1() {
            this.Url = "http://www.lsleditor.org/UploadBugReport/Service1.asmx";
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
        public event OpenCompletedEventHandler OpenCompleted;
        
        /// <remarks/>
        public event GetStatusCompletedEventHandler GetStatusCompleted;
        
        /// <remarks/>
        public event GetBugReportCompletedEventHandler GetBugReportCompleted;
        
        /// <remarks/>
        public event WriteCompletedEventHandler WriteCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Open", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string Open() {
            object[] results = this.Invoke("Open", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        /// <remarks/>
        public void OpenAsync(object userState) {
            if ((this.OpenOperationCompleted == null)) {
                this.OpenOperationCompleted = new System.Threading.SendOrPostCallback(this.OnOpenOperationCompleted);
            }
            this.InvokeAsync("Open", new object[0], this.OpenOperationCompleted, userState);
        }
        
        private void OnOpenOperationCompleted(object arg) {
            if ((this.OpenCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.OpenCompleted(this, new OpenCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetStatus", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetStatus(string Handle) {
            object[] results = this.Invoke("GetStatus", new object[] {
                        Handle});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetStatusAsync(string Handle) {
            this.GetStatusAsync(Handle, null);
        }
        
        /// <remarks/>
        public void GetStatusAsync(string Handle, object userState) {
            if ((this.GetStatusOperationCompleted == null)) {
                this.GetStatusOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetStatusOperationCompleted);
            }
            this.InvokeAsync("GetStatus", new object[] {
                        Handle}, this.GetStatusOperationCompleted, userState);
        }
        
        private void OnGetStatusOperationCompleted(object arg) {
            if ((this.GetStatusCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetStatusCompleted(this, new GetStatusCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetBugReport", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetBugReport(string Handle) {
            object[] results = this.Invoke("GetBugReport", new object[] {
                        Handle});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetBugReportAsync(string Handle) {
            this.GetBugReportAsync(Handle, null);
        }
        
        /// <remarks/>
        public void GetBugReportAsync(string Handle, object userState) {
            if ((this.GetBugReportOperationCompleted == null)) {
                this.GetBugReportOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetBugReportOperationCompleted);
            }
            this.InvokeAsync("GetBugReport", new object[] {
                        Handle}, this.GetBugReportOperationCompleted, userState);
        }
        
        private void OnGetBugReportOperationCompleted(object arg) {
            if ((this.GetBugReportCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetBugReportCompleted(this, new GetBugReportCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Write", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string Write(string Handle, string FileName, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] buffer, int intLength) {
            object[] results = this.Invoke("Write", new object[] {
                        Handle,
                        FileName,
                        buffer,
                        intLength});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void WriteAsync(string Handle, string FileName, byte[] buffer, int intLength) {
            this.WriteAsync(Handle, FileName, buffer, intLength, null);
        }
        
        /// <remarks/>
        public void WriteAsync(string Handle, string FileName, byte[] buffer, int intLength, object userState) {
            if ((this.WriteOperationCompleted == null)) {
                this.WriteOperationCompleted = new System.Threading.SendOrPostCallback(this.OnWriteOperationCompleted);
            }
            this.InvokeAsync("Write", new object[] {
                        Handle,
                        FileName,
                        buffer,
                        intLength}, this.WriteOperationCompleted, userState);
        }
        
        private void OnWriteOperationCompleted(object arg) {
            if ((this.WriteCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.WriteCompleted(this, new WriteCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.42")]
    public delegate void OpenCompletedEventHandler(object sender, OpenCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class OpenCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal OpenCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.42")]
    public delegate void GetStatusCompletedEventHandler(object sender, GetStatusCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetStatusCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetStatusCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.42")]
    public delegate void GetBugReportCompletedEventHandler(object sender, GetBugReportCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetBugReportCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetBugReportCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.42")]
    public delegate void WriteCompletedEventHandler(object sender, WriteCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class WriteCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal WriteCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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