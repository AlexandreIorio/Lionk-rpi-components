// Copyright © 2024 Lionk Project

using Lionk.Core;
using Lionk.Core.Component;
using Lionk.Core.Component.Cyclic;
using Lionk.Core.DataModel;
using Lionk.Core.Observable;
using Lionk.Log;

namespace Lionk.TemperatureSensor;

/// <summary>
/// This class is used to simulate the temperature sensor.
/// </summary>
[NamedElement("Simulated sensor", "This is a simulated sensor to test gui")]
public class SimulatedTemperatureSensor : ObservableElement, ITemperatureSensor, ICyclicComponent
{
    private static readonly IStandardLogger? _logger = LogService.CreateLogger("SimulatedTemperatureSensor");
    private readonly Random _random = new();

    #region Observable Properties

    private string _instanceName = string.Empty;
    private Guid _id = Guid.NewGuid();
    private TimeSpan _periode = TimeSpan.FromSeconds(5);
    private string _address = string.Empty;
    private TemperatureType _temperatureType = TemperatureType.Celsius;
    private DateTime _lastRead = DateTime.MinValue;
    private double _minSimulatedTemperature = 5;
    private double _maxSimulatedTemperature = 20;
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
        get => _periode;
        set => SetField(ref _periode, value);
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
    /// Gets or sets the minimum simulated temperature.
    /// </summary>
    public double MinSimulatedTemperature
    {
        get => _minSimulatedTemperature;
        set => SetField(ref _minSimulatedTemperature, value);
    }

    /// <summary>
    /// Gets or sets the maximum simulated temperature.
    /// </summary>
    public double MaxSimulatedTemperature
    {
        get => _maxSimulatedTemperature;
        set => SetField(ref _maxSimulatedTemperature, value);
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

    /// <inheritdoc />
    public List<Measure<double>> Measures { get; } =
    [
        new Measure<double>("Temperature", DateTime.UtcNow, TemperatureType.Celsius.GetUnit(), double.NaN),
        new Measure<double>("Temperature", DateTime.UtcNow, TemperatureType.Fahrenheit.GetUnit(), double.NaN),
        new Measure<double>("Temperature", DateTime.UtcNow, TemperatureType.Kelvin.GetUnit(), double.NaN),
    ];

    /// <summary>
    /// Initializes a new instance of the <see cref="SimulatedTemperatureSensor"/> class.
    /// </summary>
    /// <param name="name"> The name of the sensor.</param>
    /// <param name="readCycle"> The cycle of reading the sensor.</param>
    public SimulatedTemperatureSensor(string name, TimeSpan readCycle)
    {
        InstanceName = name;
        Periode = readCycle;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SimulatedTemperatureSensor"/> class.
    /// </summary>
    public SimulatedTemperatureSensor()
        : this("SimulatedTemperatureSensor", TimeSpan.FromSeconds(5))
    {
    }

    /// <inheritdoc/>
    public void Measure()
    {
        // Simulate a temperature value between 5 and 35 degrees.
        double tempData = (_random.NextDouble() * MaxSimulatedTemperature) + MinSimulatedTemperature;
        SetTemperature(tempData);
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
LastRead:       {LastRead}
Temperature:    {GetTemperature()} {TemperatureType.GetUnit()}";
        return status;
    }
}
