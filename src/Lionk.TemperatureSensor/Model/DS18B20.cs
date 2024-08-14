// Copyright © 2024 Lionk Project

using Lionk.Core.Component;
using Lionk.Log;

namespace Lionk.DS18B20;

/// <summary>
/// This class is used to get the temperature of the DS18B20 sensor connected to a Raspberry Pi.
/// </summary>
public class DS18B20 : ITemperatureSensor, ICyclicComponent
{
    private static readonly string _path = "/sys/bus/w1/devices/";

    private static readonly string _sensorPattern = "28-";

    private static readonly IStandardLogger? _logger = LogService.CreateLogger("DS18B20");

    /// <inheritdoc/>
    public TimeSpan ExecutionFrequency { get; set; }

    /// <inheritdoc/>
    public DateTime LastExecution { get; }

    /// <inheritdoc/>
    public int NbCycle { get; set; }

    /// <inheritdoc/>
    public DateTime StartedDate { get; set; }

    /// <inheritdoc/>
    public TimeSpan? Execute()
    {
        DateTime dateTime = DateTime.UtcNow;
        Measure();
        return DateTime.UtcNow - dateTime;
    }

    /// <inheritdoc/>
    public event EventHandler<MeasureEventArgs<double>>? NewValueAvailable;

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
    /// Gets the address of the sensor.
    /// </summary>
    public string Address { get; private set; }

    /// <summary>
    /// Gets or sets the type of the temperature.
    /// </summary>
    public TemperatureType TemperatureType { get; set; } = TemperatureType.Celsius;

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

    /// <inheritdoc />
    public List<Measure<double>> Measures { get; } = new()
    {
        new Measure<double>("Temperature", TemperatureType.Celsius.GetUnit()),
        new Measure<double>("Temperature", TemperatureType.Fahrenheit.GetUnit()),
        new Measure<double>("Temperature", TemperatureType.Kelvin.GetUnit()),
    };

    /// <inheritdoc />
    public string? InstanceName { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DS18B20"/> class.
    /// </summary>
    /// <param name="name"> The name of the sensor.</param>
    /// <param name="address"> The address of the sensor.</param>
    public DS18B20(string name, string address)
    {
        InstanceName = name;
        Address = address;
    }

    /// <inheritdoc/>
    public void Measure()
    {
        if (!Exists)
        {
            _logger?.Log(LogSeverity.Debug, $"Sensor file does not exist: {SensorFile}");
            LogService.LogDebug($"Sensor file does not exist: {SensorFile}");
            SetTemperature(double.NaN);
            IsInterfered = null;
        }

        string[] lines = File.ReadAllLines(SensorFile);
        if (!lines[0].Contains("YES"))
        {
            LogService.LogDebug($"Sensor is interfered: {SensorFile}");
            SetTemperature(double.NaN);
            IsInterfered = true;
        }

        string tempData = lines[1].Split('=')[1];
        SetTemperature(Convert.ToDouble(tempData) / 1000.0);
        IsInterfered = false;
        LastRead = DateTime.Now;
        NewValueAvailable?.Invoke(this, new MeasureEventArgs<double>(Measures));
    }

    /// <summary>
    /// This method is used to set the temperature of the sensor.
    /// </summary>
    /// <param name="value"> The value of the temperature.</param>
    public void SetTemperature(double value)
    {
        Measures[0].Value = value;
        Measures[1].Value = (value * 9.0 / 5.0) + 32.0;
        Measures[2].Value = value + 273.15;
    }

    /// <summary>
    /// This method is used to get the temperature of the sensor.
    /// </summary>
    /// <returns> The value of the temperature.</returns>
    protected double GetTemperature() => Measures[(int)TemperatureType].Value;

    /// <summary>
    /// Method to get the status of the sensor.
    /// </summary>
    /// <returns> The status of the sensor.</returns>
    public string GetSensorStatus()
    {
       string status = $@"
Sensor:         {InstanceName}
Address:        {Address} 
Exists:         {Exists}
LastRead:       {LastRead}
Temperature:    {GetTemperature()} {TemperatureType.GetUnit()} 
IsInterfered:   {IsInterfered}";

       return status;
    }
}
