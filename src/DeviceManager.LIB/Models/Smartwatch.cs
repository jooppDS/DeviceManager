using System;

namespace DeviceManagerLib
{
    public class Smartwatch : Device
    {
        public long Power { get; set; }
        public byte[] Version { get; set; }
    }
} 