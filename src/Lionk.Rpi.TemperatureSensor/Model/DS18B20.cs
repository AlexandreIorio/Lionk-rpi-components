// Copyright © 2024 Lionk Project

using Lionk.Core;
using Lionk.Core.Component;
using Lionk.Core.Component.Cyclic;
using Lionk.Core.DataModel;
using Lionk.Core.Observable;
using Lionk.Log;

namespace Lionk.TemperatureSensor;

/// <summary>
/// This class is used to get the temperature of the DS18B20 sensor connected to a Raspberry Pi.
/// </summary>
[NamedElement("DS18B20", "A DS18B20 temperature sensor")]
public class DS18B20 : ObservableElement, ITemperatureSensor, ICyclicComponent
{
    private static readonly string _path = "/sys/bus/w1/devices/";
    private static readonly string _sensorPattern = "28-";
    private static readonly IStandardLogger? _logger = LogService.CreateLogger("DS18B20");

    #region Observable Properties

    private string _instanceName = string.Empty;
    private Guid _id = Guid.NewGuid();
    private TimeSpan _period = TimeSpan.FromSeconds(5);
    private string _address = string.Empty;
    private TemperatureType _temperatureType = TemperatureType.Celsius;
    private DateTime _lastRead = DateTime.MinValue;
    private CyclicComputationMethod _cyclicComputationMethod = CyclicComputationMethod.RelativeToLastExecution;

    /// <inheritdoc/>
    public string InstanceName
    {
        get => _instanceName;
        set => SetField(ref _instanceName, value);
    }

    /// <inheritdoc/>
    public Guid Id
    {
        get => _id;
        set => SetField(ref _id, value);
    }

    /// <inheritdoc/>
    public TimeSpan Periode
    {
        get => _period;
        set => SetField(ref _period, value);
    }

    /// <summary>
    /// Gets or sets the address of the sensor.
    /// </summary>
    public string Address
    {
        get => _address;
        set => SetField(ref _address, value);
    }

    /// <summary>
    /// Gets or sets the type of the temperature.
    /// </summary>
    public TemperatureType TemperatureType
    {
        get => _temperatureType;
        set => SetField(ref _temperatureType, value);
    }

    /// <summary>
    /// Gets or sets the last read time of the sensor.
    /// </summary>
    public DateTime LastRead
    {
        get => _lastRead;
        set => SetField(ref _lastRead, value);
    }

    /// <summary>
    /// Gets or sets the cyclic computation method.
    /// </summary>
    public CyclicComputationMethod CyclicComputationMethod
    {
        get => _cyclicComputationMethod;
        set => SetField(ref _cyclicComputationMethod, value);
    }

    #endregion

    /// <inheritdoc/>
    public DateTime LastExecution { get; set; }

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

    /// <inheritdoc />
    public List<Measure<double>> Measures { get; } =
    [
        new Measure<double>("Temperature", DateTime.UtcNow, TemperatureType.Celsius.GetUnit(), double.NaN),
        new Measure<double>("Temperature", DateTime.UtcNow, TemperatureType.Fahrenheit.GetUnit(), double.NaN),
        new Measure<double>("Temperature", DateTime.UtcNow, TemperatureType.Kelvin.GetUnit(), double.NaN),
    ];

    /// <inheritdoc/>
    public void Measure()
    {
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
        LastRead = DateTime.Now;
        NewValueAvailable?.Invoke(this, new MeasureEventArgs<double>(Measures));
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
