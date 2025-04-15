using System.Text.RegularExpressions;
using task2.Interfaces;

namespace task2
{
    /// <summary>
    /// Handles file-related operations (load and save) for <see cref="Device"/> objects.
    /// </summary>
    public class FileHandler : IFileManager
    {
        private int _counter = 0;

        /// <summary>
        /// Loads a file and parses it into a list of <see cref="Device"/> objects.
        /// </summary>
        /// <param name="path">The path to the file containing device data.</param>
        /// <returns>A list of <see cref="Device"/> objects if successful; otherwise <c>null</c>.</returns>
        public List<Device> LoadFile(string path)
        {
            if (!File.Exists(path))
                return null;

            List<Device> devices = new();
            string[] lines = File.ReadAllLines(path);
            Regex regex = new(@"(\S+,*)");

            foreach (string line in lines)
            {
                if (regex.IsMatch(line))
                {
                    _counter++;
                    string id = _counter.ToString();
                    string[] values = line.Split(',');

                    try
                    {
                        string type = values[0];

                        switch (type)
                        {
                            case string s when s.Contains("SW"):
                                bool swStatus = bool.Parse(values[2]);
                                long power = long.Parse(values[3].TrimEnd('%'));
                                devices.Add(new Smartwatch(id, values[1], swStatus, power));
                                break;

                            case string p when p.Contains("P"):
                                bool pcStatus = bool.Parse(values[2]);
                                if (values.Length == 4)
                                {
                                    devices.Add(new PersonalComputer(id, values[1], pcStatus, values[3]));
                                }
                                else
                                {
                                    devices.Add(new PersonalComputer(id, values[1], pcStatus));
                                }
                                break;

                            case string ed when ed.Contains("ED"):
                                devices.Add(new EmbededDevice(id, values[1], false, values[2], values[3]));
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing line: {line} - {ex.Message}");
                    }
                }
            }

            return devices;
        }

        /// <summary>
        /// Saves a list of <see cref="Device"/> objects to a file.
        /// </summary>
        /// <param name="path">The path to the file where data should be saved.</param>
        /// <param name="deviceStorage">The list of devices to save.</param>
        /// <returns><c>true</c> if saving was successful; otherwise <c>false</c>.</returns>
        public bool SaveFile(string path, List<Device> deviceStorage)
        {
            if (File.Exists(path))
                File.Delete(path);

            foreach (Device device in deviceStorage)
            {
                File.AppendAllText(path, $"{device.GetFileFormat()}\n");
            }

            return true;
        }
    }
}
