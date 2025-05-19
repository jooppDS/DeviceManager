using System;

namespace DeviceManagerLib
{
    public class PersonalComputer : Device
    {
        public string OS { get; set; }
        public byte[] Version { get; set; }
    }
} 