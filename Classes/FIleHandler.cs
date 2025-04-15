using System.Text.RegularExpressions;
using task2.Interfaces;

namespace task2
{
    /// <summary>
    /// Handles file-related operations (load and save) for <see cref="Device"/> objects.
    /// </summary>
    public class FIleHandler : IFileManager
    {
       
        private int counter = 0;

        /// <summary>
        /// Loads a file and parses it into a list of <see cref="Device"/> objects.
        /// </summary>
        /// <param name="path">The path to the file containing device data.</param>
        /// <returns>A list of <see cref="Device"/> objects if successful; otherwise <c>null</c>.</returns>
        public List<Device> loadFile(string path)
        {
            if (File.Exists(path))
            {
                List<Device> devices = new List<Device>();
                string[] lines = File.ReadAllLines(path);
                Regex rg = new Regex("(\\S+,*)");

                foreach (string line in lines)
                {
                    if (rg.IsMatch(line))
                    {
                        ++counter;
                        string id = counter.ToString();
                        string[] values = line.Split(',');

                        // Smartwatch
                        if (values[0].Contains("SW"))
                        {
                            bool status = bool.Parse(values[2]);
                            long power = long.Parse(values[3].Substring(0, values[3].Length - 1));
                            devices.Add(new Smartwatch(id, values[1], status, power));
                        }
                        // PersonalComputer
                        else if (values[0].Contains('P'))
                        {
                            bool status = bool.Parse(values[2]);
                            if (values.Length == 4)
                                devices.Add(new PersonalComputer(id, values[1], status, values[3]));
                            else
                                devices.Add(new PersonalComputer(id, values[1], status));
                        }
                        // EmbededDevice
                        else if (values[0].Contains("ED"))
                        {
                            try
                            {
                                devices.Add(new EmbededDevice(id, values[1], false, values[2], values[3]));
                            }
                            catch (ArgumentException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
                return devices;
            }

            return null;
        }

        /// <summary>
        /// Saves a list of <see cref="Device"/> objects to a file.
        /// </summary>
        /// <param name="path">The path to the file where data should be saved.</param>
        /// <param name="deviceStorage">The list of devices to save.</param>
        /// <returns><c>true</c> if saving was successful; otherwise <c>false</c>.</returns>
        public bool saveFile(string path, List<Device> deviceStorage)
        {
            if (File.Exists(path))
                File.Delete(path);

            foreach (Device device in deviceStorage)
            {
                File.AppendAllText(path, device.getFileFormat() + '\n');
            }

            return true;
        }
    }
}
