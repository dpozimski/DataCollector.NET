﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataCollector.Client.UI.DeviceCommunication {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="BaseTable", Namespace="http://schemas.datacontract.org/2004/07/DataCollector.Server.DataAccess.Models.En" +
        "tities")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(DataCollector.Client.UI.DeviceCommunication.MeasureDevice))]
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MeasureDevice", Namespace="http://schemas.datacontract.org/2004/07/DataCollector.Server.DataAccess.Models.En" +
        "tities")]
    [System.SerializableAttribute()]
    public partial class MeasureDevice : DataCollector.Client.UI.DeviceCommunication.BaseTable {
        
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
    [System.Runtime.Serialization.DataContractAttribute(Name="MeasuresArrivedEventArgs", Namespace="http://schemas.datacontract.org/2004/07/DataCollector.Server.DeviceHandlers.Model" +
        "s")]
    [System.SerializableAttribute()]
    public partial class MeasuresArrivedEventArgs : System.EventArgs, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private DataCollector.Client.UI.DeviceCommunication.MeasureDevice SourceField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime TimeStampField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private DataCollector.Client.UI.DeviceCommunication.Measures ValueField;
        
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
        public DataCollector.Client.UI.DeviceCommunication.MeasureDevice Source {
            get {
                return this.SourceField;
            }
            set {
                if ((object.ReferenceEquals(this.SourceField, value) != true)) {
                    this.SourceField = value;
                    this.RaisePropertyChanged("Source");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime TimeStamp {
            get {
                return this.TimeStampField;
            }
            set {
                if ((this.TimeStampField.Equals(value) != true)) {
                    this.TimeStampField = value;
                    this.RaisePropertyChanged("TimeStamp");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public DataCollector.Client.UI.DeviceCommunication.Measures Value {
            get {
                return this.ValueField;
            }
            set {
                if ((object.ReferenceEquals(this.ValueField, value) != true)) {
                    this.ValueField = value;
                    this.RaisePropertyChanged("Value");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Measures", Namespace="http://schemas.datacontract.org/2004/07/DataCollector.Device.Models")]
    [System.SerializableAttribute()]
    public partial class Measures : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private DataCollector.Client.UI.DeviceCommunication.SpherePoint AccelerometerField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<float> AirPressureField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private DataCollector.Client.UI.DeviceCommunication.SpherePoint GyroscopeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<float> HumidityField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<bool> IsLedActiveField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<float> TemperatureField;
        
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
        public DataCollector.Client.UI.DeviceCommunication.SpherePoint Accelerometer {
            get {
                return this.AccelerometerField;
            }
            set {
                if ((object.ReferenceEquals(this.AccelerometerField, value) != true)) {
                    this.AccelerometerField = value;
                    this.RaisePropertyChanged("Accelerometer");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<float> AirPressure {
            get {
                return this.AirPressureField;
            }
            set {
                if ((this.AirPressureField.Equals(value) != true)) {
                    this.AirPressureField = value;
                    this.RaisePropertyChanged("AirPressure");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public DataCollector.Client.UI.DeviceCommunication.SpherePoint Gyroscope {
            get {
                return this.GyroscopeField;
            }
            set {
                if ((object.ReferenceEquals(this.GyroscopeField, value) != true)) {
                    this.GyroscopeField = value;
                    this.RaisePropertyChanged("Gyroscope");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<float> Humidity {
            get {
                return this.HumidityField;
            }
            set {
                if ((this.HumidityField.Equals(value) != true)) {
                    this.HumidityField = value;
                    this.RaisePropertyChanged("Humidity");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<bool> IsLedActive {
            get {
                return this.IsLedActiveField;
            }
            set {
                if ((this.IsLedActiveField.Equals(value) != true)) {
                    this.IsLedActiveField = value;
                    this.RaisePropertyChanged("IsLedActive");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<float> Temperature {
            get {
                return this.TemperatureField;
            }
            set {
                if ((this.TemperatureField.Equals(value) != true)) {
                    this.TemperatureField = value;
                    this.RaisePropertyChanged("Temperature");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SpherePoint", Namespace="http://schemas.datacontract.org/2004/07/DataCollector.Device.Models")]
    [System.SerializableAttribute()]
    public partial class SpherePoint : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private float XField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private float YField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private float ZField;
        
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
        public float X {
            get {
                return this.XField;
            }
            set {
                if ((this.XField.Equals(value) != true)) {
                    this.XField = value;
                    this.RaisePropertyChanged("X");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public float Y {
            get {
                return this.YField;
            }
            set {
                if ((this.YField.Equals(value) != true)) {
                    this.YField = value;
                    this.RaisePropertyChanged("Y");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public float Z {
            get {
                return this.ZField;
            }
            set {
                if ((this.ZField.Equals(value) != true)) {
                    this.ZField = value;
                    this.RaisePropertyChanged("Z");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DeviceUpdatedEventArgs", Namespace="http://schemas.datacontract.org/2004/07/DataCollector.Server.DeviceHandlers.Model" +
        "s")]
    [System.SerializableAttribute()]
    public partial class DeviceUpdatedEventArgs : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private DataCollector.Client.UI.DeviceCommunication.MeasureDevice DeviceField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private DataCollector.Client.UI.DeviceCommunication.UpdateStatus UpdateStatusField;
        
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
        public DataCollector.Client.UI.DeviceCommunication.MeasureDevice Device {
            get {
                return this.DeviceField;
            }
            set {
                if ((object.ReferenceEquals(this.DeviceField, value) != true)) {
                    this.DeviceField = value;
                    this.RaisePropertyChanged("Device");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public DataCollector.Client.UI.DeviceCommunication.UpdateStatus UpdateStatus {
            get {
                return this.UpdateStatusField;
            }
            set {
                if ((this.UpdateStatusField.Equals(value) != true)) {
                    this.UpdateStatusField = value;
                    this.RaisePropertyChanged("UpdateStatus");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="UpdateStatus", Namespace="http://schemas.datacontract.org/2004/07/DataCollector.Server.BroadcastListener.Mo" +
        "dels")]
    public enum UpdateStatus : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Found = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Updated = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Lost = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ConnectedToRestService = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DisconnectedFromRestService = 4,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="DeviceCommunication.ICommunicationService", CallbackContract=typeof(DataCollector.Client.UI.DeviceCommunication.ICommunicationServiceCallback))]
    public interface ICommunicationService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommunicationService/IsStarted", ReplyAction="http://tempuri.org/ICommunicationService/IsStartedResponse")]
        bool IsStarted();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommunicationService/IsStarted", ReplyAction="http://tempuri.org/ICommunicationService/IsStartedResponse")]
        System.Threading.Tasks.Task<bool> IsStartedAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommunicationService/RegisterCallbackChannel", ReplyAction="http://tempuri.org/ICommunicationService/RegisterCallbackChannelResponse")]
        void RegisterCallbackChannel();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommunicationService/RegisterCallbackChannel", ReplyAction="http://tempuri.org/ICommunicationService/RegisterCallbackChannelResponse")]
        System.Threading.Tasks.Task RegisterCallbackChannelAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommunicationService/Start", ReplyAction="http://tempuri.org/ICommunicationService/StartResponse")]
        void Start();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommunicationService/Start", ReplyAction="http://tempuri.org/ICommunicationService/StartResponse")]
        System.Threading.Tasks.Task StartAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommunicationService/Stop", ReplyAction="http://tempuri.org/ICommunicationService/StopResponse")]
        void Stop();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommunicationService/Stop", ReplyAction="http://tempuri.org/ICommunicationService/StopResponse")]
        System.Threading.Tasks.Task StopAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommunicationService/ConnectDevice", ReplyAction="http://tempuri.org/ICommunicationService/ConnectDeviceResponse")]
        bool ConnectDevice(DataCollector.Client.UI.DeviceCommunication.MeasureDevice device);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommunicationService/ConnectDevice", ReplyAction="http://tempuri.org/ICommunicationService/ConnectDeviceResponse")]
        System.Threading.Tasks.Task<bool> ConnectDeviceAsync(DataCollector.Client.UI.DeviceCommunication.MeasureDevice device);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommunicationService/DisconnectDevice", ReplyAction="http://tempuri.org/ICommunicationService/DisconnectDeviceResponse")]
        bool DisconnectDevice(DataCollector.Client.UI.DeviceCommunication.MeasureDevice device);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommunicationService/DisconnectDevice", ReplyAction="http://tempuri.org/ICommunicationService/DisconnectDeviceResponse")]
        System.Threading.Tasks.Task<bool> DisconnectDeviceAsync(DataCollector.Client.UI.DeviceCommunication.MeasureDevice device);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommunicationService/GetLedState", ReplyAction="http://tempuri.org/ICommunicationService/GetLedStateResponse")]
        bool GetLedState(DataCollector.Client.UI.DeviceCommunication.MeasureDevice deviceHandler);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommunicationService/GetLedState", ReplyAction="http://tempuri.org/ICommunicationService/GetLedStateResponse")]
        System.Threading.Tasks.Task<bool> GetLedStateAsync(DataCollector.Client.UI.DeviceCommunication.MeasureDevice deviceHandler);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommunicationService/ChangeLedState", ReplyAction="http://tempuri.org/ICommunicationService/ChangeLedStateResponse")]
        bool ChangeLedState(DataCollector.Client.UI.DeviceCommunication.MeasureDevice target, bool state);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommunicationService/ChangeLedState", ReplyAction="http://tempuri.org/ICommunicationService/ChangeLedStateResponse")]
        System.Threading.Tasks.Task<bool> ChangeLedStateAsync(DataCollector.Client.UI.DeviceCommunication.MeasureDevice target, bool state);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommunicationService/AddSimulatorDevice", ReplyAction="http://tempuri.org/ICommunicationService/AddSimulatorDeviceResponse")]
        void AddSimulatorDevice();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommunicationService/AddSimulatorDevice", ReplyAction="http://tempuri.org/ICommunicationService/AddSimulatorDeviceResponse")]
        System.Threading.Tasks.Task AddSimulatorDeviceAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICommunicationServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/ICommunicationService/MeasuresArrived")]
        void MeasuresArrived(DataCollector.Client.UI.DeviceCommunication.MeasuresArrivedEventArgs measures);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/ICommunicationService/DeviceChangedState")]
        void DeviceChangedState(DataCollector.Client.UI.DeviceCommunication.DeviceUpdatedEventArgs deviceUpdated);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICommunicationServiceChannel : DataCollector.Client.UI.DeviceCommunication.ICommunicationService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CommunicationServiceClient : System.ServiceModel.DuplexClientBase<DataCollector.Client.UI.DeviceCommunication.ICommunicationService>, DataCollector.Client.UI.DeviceCommunication.ICommunicationService {
        
        public CommunicationServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public CommunicationServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public CommunicationServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public CommunicationServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public CommunicationServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public bool IsStarted() {
            return base.Channel.IsStarted();
        }
        
        public System.Threading.Tasks.Task<bool> IsStartedAsync() {
            return base.Channel.IsStartedAsync();
        }
        
        public void RegisterCallbackChannel() {
            base.Channel.RegisterCallbackChannel();
        }
        
        public System.Threading.Tasks.Task RegisterCallbackChannelAsync() {
            return base.Channel.RegisterCallbackChannelAsync();
        }
        
        public void Start() {
            base.Channel.Start();
        }
        
        public System.Threading.Tasks.Task StartAsync() {
            return base.Channel.StartAsync();
        }
        
        public void Stop() {
            base.Channel.Stop();
        }
        
        public System.Threading.Tasks.Task StopAsync() {
            return base.Channel.StopAsync();
        }
        
        public bool ConnectDevice(DataCollector.Client.UI.DeviceCommunication.MeasureDevice device) {
            return base.Channel.ConnectDevice(device);
        }
        
        public System.Threading.Tasks.Task<bool> ConnectDeviceAsync(DataCollector.Client.UI.DeviceCommunication.MeasureDevice device) {
            return base.Channel.ConnectDeviceAsync(device);
        }
        
        public bool DisconnectDevice(DataCollector.Client.UI.DeviceCommunication.MeasureDevice device) {
            return base.Channel.DisconnectDevice(device);
        }
        
        public System.Threading.Tasks.Task<bool> DisconnectDeviceAsync(DataCollector.Client.UI.DeviceCommunication.MeasureDevice device) {
            return base.Channel.DisconnectDeviceAsync(device);
        }
        
        public bool GetLedState(DataCollector.Client.UI.DeviceCommunication.MeasureDevice deviceHandler) {
            return base.Channel.GetLedState(deviceHandler);
        }
        
        public System.Threading.Tasks.Task<bool> GetLedStateAsync(DataCollector.Client.UI.DeviceCommunication.MeasureDevice deviceHandler) {
            return base.Channel.GetLedStateAsync(deviceHandler);
        }
        
        public bool ChangeLedState(DataCollector.Client.UI.DeviceCommunication.MeasureDevice target, bool state) {
            return base.Channel.ChangeLedState(target, state);
        }
        
        public System.Threading.Tasks.Task<bool> ChangeLedStateAsync(DataCollector.Client.UI.DeviceCommunication.MeasureDevice target, bool state) {
            return base.Channel.ChangeLedStateAsync(target, state);
        }
        
        public void AddSimulatorDevice() {
            base.Channel.AddSimulatorDevice();
        }
        
        public System.Threading.Tasks.Task AddSimulatorDeviceAsync() {
            return base.Channel.AddSimulatorDeviceAsync();
        }
    }
}
