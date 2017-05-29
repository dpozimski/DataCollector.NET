﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataCollector.Client.UI.DataAccess {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MeasureDevice", Namespace="http://schemas.datacontract.org/2004/07/DataCollector.Server.DataAccess.Models")]
    [System.SerializableAttribute()]
    public partial class MeasureDevice : DataCollector.Client.UI.DataAccess.BaseTable {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ArchitectureField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IPv4Field;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsConnectedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MacAddressField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double MeasurementsMsRequestIntervalField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ModelField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string WinVerField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Architecture {
            get {
                return this.ArchitectureField;
            }
            set {
                if ((object.ReferenceEquals(this.ArchitectureField, value) != true)) {
                    this.ArchitectureField = value;
                    this.RaisePropertyChanged("Architecture");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IPv4 {
            get {
                return this.IPv4Field;
            }
            set {
                if ((object.ReferenceEquals(this.IPv4Field, value) != true)) {
                    this.IPv4Field = value;
                    this.RaisePropertyChanged("IPv4");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsConnected {
            get {
                return this.IsConnectedField;
            }
            set {
                if ((this.IsConnectedField.Equals(value) != true)) {
                    this.IsConnectedField = value;
                    this.RaisePropertyChanged("IsConnected");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MacAddress {
            get {
                return this.MacAddressField;
            }
            set {
                if ((object.ReferenceEquals(this.MacAddressField, value) != true)) {
                    this.MacAddressField = value;
                    this.RaisePropertyChanged("MacAddress");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double MeasurementsMsRequestInterval {
            get {
                return this.MeasurementsMsRequestIntervalField;
            }
            set {
                if ((this.MeasurementsMsRequestIntervalField.Equals(value) != true)) {
                    this.MeasurementsMsRequestIntervalField = value;
                    this.RaisePropertyChanged("MeasurementsMsRequestInterval");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Model {
            get {
                return this.ModelField;
            }
            set {
                if ((object.ReferenceEquals(this.ModelField, value) != true)) {
                    this.ModelField = value;
                    this.RaisePropertyChanged("Model");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string WinVer {
            get {
                return this.WinVerField;
            }
            set {
                if ((object.ReferenceEquals(this.WinVerField, value) != true)) {
                    this.WinVerField = value;
                    this.RaisePropertyChanged("WinVer");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="BaseTable", Namespace="http://schemas.datacontract.org/2004/07/DataCollector.Server.DataAccess.Models")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(DataCollector.Client.UI.DataAccess.MeasureDevice))]
    public partial class BaseTable : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IDField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ID {
            get {
                return this.IDField;
            }
            set {
                if ((this.IDField.Equals(value) != true)) {
                    this.IDField = value;
                    this.RaisePropertyChanged("ID");
                }
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MeasureType", Namespace="http://schemas.datacontract.org/2004/07/DataCollector.Server.DataAccess.Models")]
    public enum MeasureType : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Humidity = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Temperature = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AirPressure = 2,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SphereMeasureType", Namespace="http://schemas.datacontract.org/2004/07/DataCollector.Server.DataAccess.Models")]
    public enum SphereMeasureType : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Gyroscope = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Accelerometer = 1,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="DataAccess.IMeasureAccessService")]
    public interface IMeasureAccessService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMeasureAccessService/UpdateDeviceRequestInterval", ReplyAction="http://tempuri.org/IMeasureAccessService/UpdateDeviceRequestIntervalResponse")]
        void UpdateDeviceRequestInterval(string macAddress, double requestInterval);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMeasureAccessService/UpdateDeviceRequestInterval", ReplyAction="http://tempuri.org/IMeasureAccessService/UpdateDeviceRequestIntervalResponse")]
        System.Threading.Tasks.Task UpdateDeviceRequestIntervalAsync(string macAddress, double requestInterval);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMeasureAccessService/GetMeasureDevices", ReplyAction="http://tempuri.org/IMeasureAccessService/GetMeasureDevicesResponse")]
        DataCollector.Client.UI.DataAccess.MeasureDevice[] GetMeasureDevices();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMeasureAccessService/GetMeasureDevices", ReplyAction="http://tempuri.org/IMeasureAccessService/GetMeasureDevicesResponse")]
        System.Threading.Tasks.Task<DataCollector.Client.UI.DataAccess.MeasureDevice[]> GetMeasureDevicesAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMeasureAccessService/GetMeasures", ReplyAction="http://tempuri.org/IMeasureAccessService/GetMeasuresResponse")]
        LiveCharts.Defaults.DateTimePoint[][] GetMeasures(DataCollector.Client.UI.DataAccess.MeasureType type, DataCollector.Client.UI.DataAccess.MeasureDevice device, System.DateTime lowerRange, System.DateTime upperRange);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMeasureAccessService/GetMeasures", ReplyAction="http://tempuri.org/IMeasureAccessService/GetMeasuresResponse")]
        System.Threading.Tasks.Task<LiveCharts.Defaults.DateTimePoint[][]> GetMeasuresAsync(DataCollector.Client.UI.DataAccess.MeasureType type, DataCollector.Client.UI.DataAccess.MeasureDevice device, System.DateTime lowerRange, System.DateTime upperRange);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMeasureAccessService/GetSphereMeasures", ReplyAction="http://tempuri.org/IMeasureAccessService/GetSphereMeasuresResponse")]
        LiveCharts.Defaults.DateTimePoint[][] GetSphereMeasures(DataCollector.Client.UI.DataAccess.SphereMeasureType type, DataCollector.Client.UI.DataAccess.MeasureDevice device, System.DateTime lowerRange, System.DateTime upperRange);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMeasureAccessService/GetSphereMeasures", ReplyAction="http://tempuri.org/IMeasureAccessService/GetSphereMeasuresResponse")]
        System.Threading.Tasks.Task<LiveCharts.Defaults.DateTimePoint[][]> GetSphereMeasuresAsync(DataCollector.Client.UI.DataAccess.SphereMeasureType type, DataCollector.Client.UI.DataAccess.MeasureDevice device, System.DateTime lowerRange, System.DateTime upperRange);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMeasureAccessServiceChannel : DataCollector.Client.UI.DataAccess.IMeasureAccessService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MeasureAccessServiceClient : System.ServiceModel.ClientBase<DataCollector.Client.UI.DataAccess.IMeasureAccessService>, DataCollector.Client.UI.DataAccess.IMeasureAccessService {
        
        public MeasureAccessServiceClient() {
        }
        
        public MeasureAccessServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MeasureAccessServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MeasureAccessServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MeasureAccessServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void UpdateDeviceRequestInterval(string macAddress, double requestInterval) {
            base.Channel.UpdateDeviceRequestInterval(macAddress, requestInterval);
        }
        
        public System.Threading.Tasks.Task UpdateDeviceRequestIntervalAsync(string macAddress, double requestInterval) {
            return base.Channel.UpdateDeviceRequestIntervalAsync(macAddress, requestInterval);
        }
        
        public DataCollector.Client.UI.DataAccess.MeasureDevice[] GetMeasureDevices() {
            return base.Channel.GetMeasureDevices();
        }
        
        public System.Threading.Tasks.Task<DataCollector.Client.UI.DataAccess.MeasureDevice[]> GetMeasureDevicesAsync() {
            return base.Channel.GetMeasureDevicesAsync();
        }
        
        public LiveCharts.Defaults.DateTimePoint[][] GetMeasures(DataCollector.Client.UI.DataAccess.MeasureType type, DataCollector.Client.UI.DataAccess.MeasureDevice device, System.DateTime lowerRange, System.DateTime upperRange) {
            return base.Channel.GetMeasures(type, device, lowerRange, upperRange);
        }
        
        public System.Threading.Tasks.Task<LiveCharts.Defaults.DateTimePoint[][]> GetMeasuresAsync(DataCollector.Client.UI.DataAccess.MeasureType type, DataCollector.Client.UI.DataAccess.MeasureDevice device, System.DateTime lowerRange, System.DateTime upperRange) {
            return base.Channel.GetMeasuresAsync(type, device, lowerRange, upperRange);
        }
        
        public LiveCharts.Defaults.DateTimePoint[][] GetSphereMeasures(DataCollector.Client.UI.DataAccess.SphereMeasureType type, DataCollector.Client.UI.DataAccess.MeasureDevice device, System.DateTime lowerRange, System.DateTime upperRange) {
            return base.Channel.GetSphereMeasures(type, device, lowerRange, upperRange);
        }
        
        public System.Threading.Tasks.Task<LiveCharts.Defaults.DateTimePoint[][]> GetSphereMeasuresAsync(DataCollector.Client.UI.DataAccess.SphereMeasureType type, DataCollector.Client.UI.DataAccess.MeasureDevice device, System.DateTime lowerRange, System.DateTime upperRange) {
            return base.Channel.GetSphereMeasuresAsync(type, device, lowerRange, upperRange);
        }
    }
}
