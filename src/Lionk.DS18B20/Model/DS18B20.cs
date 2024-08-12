
using Lionk.Log;
using Serilog.Core;

namespace Lionk.DS18B20;

public class DS18B20
{
    private static readonly string Path = "/sys/bus/w1/devices/";

    private static readonly string SensorPattern = "28-";

    private static readonly IStandardLogger? Logger = LogService.CreateLogger("DS18B20");
    
    /// <summary>
    /// Static method to get the connected sensors.
    /// </summary>
    /// <returns></returns>
    public static List<string> ConnectedSensors()
    {
        List<string> sensors = new List<string>();
        foreach (string sensor in Directory.GetDirectories(Path))
        { 
            string Address = sensor.Split("/").Last();
            if (Address.Contains(SensorPattern))
            {
                sensors.Add(Address);
            }

        }
        return sensors;
    }

    /// <summary>
    /// Gets the name of the sensor.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets the address of the sensor.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Gets the temperature of the sensor.
    /// </summary>
    public double Temperature { get; private set; } = double.NaN;

    /// <summary>
    /// Gets a value indicating whether the sensor is interfered.
    /// </summary>
    /// <remarks> <b>Null</b> if the sensor file does not exist or if the value is not read, <b>true</b> if the sensor is interfered, <b>false</b> otherwise.</remarks>
    public bool? IsInterfered { get; private set; }

    DateTime LastRead { get; set; } = DateTime.MinValue;

    /// <summary>
    /// Gets the full path of the sensor file.
    /// </summary>

    public string SensorFile => System.IO.Path.Combine(Path, Address, "w1_slave");

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
            Logger?.Log(LogSeverity.Debug, $"Sensor file does not exist: {SensorFile}");
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


