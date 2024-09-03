// Copyright © 2024 Lionk Project

using Lionk.Core;
using Lionk.Core.DataModel;
using Lionk.Log;

namespace Lionk.TemperatureSensor;

/// <summary>
/// This class is used to get the temperature of the DS18B20 sensor connected to a Raspberry Pi.
/// </summary>
[NamedElement("DS18B20", "A DS18B20 temperature sensor")]
public class DS18B20 : BaseTemperatureSensor
{
    private static readonly string _path = "/sys/bus/w1/devices/";
    private static readonly string _sensorPattern = "28-";
    private static readonly IStandardLogger? _logger = LogService.CreateLogger("DS18B20");

    #region Observable Properties

    private string _address = string.Empty;

    /// <summary>
    /// Gets or sets the address of the sensor.
    /// </summary>
    public string Address
    {
        get => _address;
        set => SetField(ref _address, value);
    }

    #endregion

    /// <summary>
    /// Static method to get the connected sensors.
    /// </summary>
    /// <returns> The connected sensors.</returns>
    public static List<string> ConnectedSensors()
    {
        List<string> sensors = [];
        if (!Directory.Exists(_path))
        {
            _logger?.Log(LogSeverity.Debug, $"Path does not exist: {_path}");
            return sensors;
        }

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
    /// Gets a value indicating whether the sensor is interfered.
    /// </summary>
    /// <remarks> <b>Null</b> if the sensor file does not exist or if the value is not read, <b>true</b> if the sensor is interfered, <b>false</b> otherwise.</remarks>
    public bool? IsInterfered { get; private set; }

    /// <summary>
    /// Gets the full path of the sensor file.
    /// </summary>
    public string SensorFile => System.IO.Path.Combine(_path, Address, "w1_slave");

    /// <summary>
    /// Gets a value indicating whether the sensor file exists.
    /// </summary>
    public bool Exists => File.Exists(SensorFile);

    /// <inheritdoc/>
    public override bool CanExecute => Exists;

    /// <inheritdoc/>
    public override void Measure()
    {
        base.Measure();
        if (!Exists)
        {
            _logger?.Log(LogSeverity.Debug, $"Sensor file does not exist: {SensorFile}");
            LogService.LogDebug($"Sensor file does not exist: {SensorFile}");
            SetTemperature(double.NaN);
            IsInterfered = null;
            return;
        }

        string[] lines = File.ReadAllLines(SensorFile);
        if (!lines[0].Contains("YES"))
        {
            LogService.LogDebug($"Sensor is interfered: {SensorFile}");
            SetTemperature(double.NaN);
            IsInterfered = true;
            return;
        }

        string tempData = lines[1].Split('=')[1];
        SetTemperature(Convert.ToDouble(tempData) / 1000.0);
        IsInterfered = false;
    }

    /// <summary>
    /// This method is used to set the temperature of the sensor.
    /// </summary>
    /// <param name="value"> The value of the temperature.</param>
    public void SetTemperature(double value)
    {
        double celsius = value;
        double fahrenheit = (value * 9.0 / 5.0) + 32.0;
        double kelvin = value + 273.15;

        Measures[(int)TemperatureType.Celsius] = new Measure<double>(
            "Temperature",
            DateTime.UtcNow,
            TemperatureType.Celsius.GetUnit(),
            celsius);
        Measures[(int)TemperatureType.Fahrenheit] = new Measure<double>(
            "Temperature",
            DateTime.UtcNow,
            TemperatureType.Fahrenheit.GetUnit(),
            fahrenheit);
        Measures[(int)TemperatureType.Kelvin] = new Measure<double>(
            "Temperature",
            DateTime.UtcNow,
            TemperatureType.Kelvin.GetUnit(),
            kelvin);
    }

    /// <summary>
    /// This method is used to get the temperature of the sensor.
    /// </summary>
    /// <returns> The value of the temperature.</returns>
    protected double GetTemperature() => Measures[(int)TemperatureType].Value;
}
