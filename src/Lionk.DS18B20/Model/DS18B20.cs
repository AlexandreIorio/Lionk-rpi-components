// Copyright © 2024 Lionk Project

using Lionk.Log;

namespace Lionk.DS18B20;

/// <summary>
/// This class is used to get the temperature of the DS18B20 sensor connected to a Raspberry Pi.
/// </summary>
public class DS18B20
{
    private static readonly string _path = "/sys/bus/w1/devices/";

    private static readonly string _sensorPattern = "28-";

    private static readonly IStandardLogger? _logger = LogService.CreateLogger("DS18B20");

    /// <summary>
    /// Static method to get the connected sensors.
    /// </summary>
    /// <returns> The connected sensors.</returns>
    public static List<string> ConnectedSensors()
    {
        List<string> sensors = new();
        foreach (string sensor in Directory.GetDirectories(_path))
        {
            string busAddress = sensor.Split("/").Last();
            if (busAddress.Contains(_sensorPattern))
            {
                sensors.Add(busAddress);
            }
        }

        return sensors;
    }

    /// <summary>
    /// Gets the name of the sensor.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the address of the sensor.
    /// </summary>
    public string Address { get; private set; }

    /// <summary>
    /// Gets the temperature of the sensor.
    /// </summary>
    public double Temperature { get; private set; } = double.NaN;

    /// <summary>
    /// Gets a value indicating whether the sensor is interfered.
    /// </summary>
    /// <remarks> <b>Null</b> if the sensor file does not exist or if the value is not read, <b>true</b> if the sensor is interfered, <b>false</b> otherwise.</remarks>
    public bool? IsInterfered { get; private set; }

    /// <summary>
    /// Gets the last read time of the sensor.
    /// </summary>
    public DateTime LastRead { get; private set; } = DateTime.MinValue;

    /// <summary>
    /// Gets the full path of the sensor file.
    /// </summary>
    public string SensorFile => System.IO.Path.Combine(_path, Address, "w1_slave");

    /// <summary>
    /// Gets a value indicating whether the sensor file exists.
    /// </summary>
    public bool Exists => File.Exists(SensorFile);

    /// <summary>
    /// Initializes a new instance of the <see cref="DS18B20"/> class.
    /// </summary>
    /// <param name="name"> The name of the sensor.</param>
    /// <param name="address"> The address of the sensor.</param>
    public DS18B20(string name, string address)
    {
        Name = name;
        Address = address;
    }

    /// <summary>
    /// Method to get the temperature of the sensor.
    /// </summary>
    public void ReadSensor()
    {
        if (!Exists)
        {
            _logger?.Log(LogSeverity.Debug, $"Sensor file does not exist: {SensorFile}");
            LogService.LogDebug($"Sensor file does not exist: {SensorFile}");
            Temperature = double.NaN;
            IsInterfered = null;
        }

        string[] lines = File.ReadAllLines(SensorFile);
        if (!lines[0].Contains("YES"))
        {
            LogService.LogDebug($"Sensor is interfered: {SensorFile}");
            Temperature = double.NaN;
            IsInterfered = true;
        }

        string tempData = lines[1].Split('=')[1];
        Temperature = Convert.ToDouble(tempData) / 1000.0;
        IsInterfered = false;
        LastRead = DateTime.Now;
    }

    /// <summary>
    /// Method to get the status of the sensor.
    /// </summary>
    /// <returns> The status of the sensor.</returns>
    public string GetSensorStatus()
    {
       string status = $@"
Sensor:         {Name}
Address:        {Address} 
Exists:         {Exists}
LastRead:       {LastRead}
Temperature:    {Temperature} 
IsInterfered:   {IsInterfered}";

       return status;
    }
}
