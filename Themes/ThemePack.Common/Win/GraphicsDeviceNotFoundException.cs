using System;

namespace ThemePack.Common.Win
{
    public sealed class GraphicsDeviceNotFoundException : Exception
    {
        private const string _notProvided = "-- NOT PROVIDED --";

        public string DeviceName { get; }
        public string DeviceString { get; }

        public GraphicsDeviceNotFoundException(string deviceName, string deviceString)
        {
            DeviceName = deviceName;
            DeviceString = deviceString;
        }

        public override string Message
        {
            get
            {
                return $"graphics device not found. device name: '{ DeviceName ?? _notProvided}'; device string: '{DeviceString ?? _notProvided}'";
            }
        }
    }
}
