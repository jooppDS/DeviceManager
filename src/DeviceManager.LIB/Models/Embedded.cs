using System;

namespace DeviceManagerLib
{
    public class EmbeddedDevice : Device
    {
        public string Ip { get; set; }
        public string NetworkName { get; set; }
        public byte[] Version { get; set; }
    }
} 