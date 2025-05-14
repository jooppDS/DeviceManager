using System;

namespace DeviceManagerLib
{
    public class EmbeddedDevice
    {
        public string Id { get; set; }
        public string Ip { get; set; }
        public string NetworkName { get; set; }
        public byte[] Version { get; set; }
    }
} 