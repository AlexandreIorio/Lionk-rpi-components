// Copyright © 2024 Lionk Project

using Lionk.Core.Component;
using Lionk.Log;

namespace Lionk.TemperatureSensor;

/// <summary>
/// This class is used to simulate the temperature sensor.
/// </summary>
public class SimulatedTemperatureSensor : ITemperatureSensor, ICyclicComponent
{
    private static readonly IStandardLogger? _logger = LogService.CreateLogger("SimulatedTemperatureSensor");
    private readonly Random _random = new();

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
    /// Gets or sets the type of the temperature.
    /// </summary>
    public TemperatureType TemperatureType { get; set; } = TemperatureType.Celsius;

    /// <summary>
    /// Gets the last read time of the sensor.
    /// </summary>
    public DateTime LastRead { get; private set; } = DateTime.MinValue;

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
    /// Initializes a new instance of the <see cref="SimulatedTemperatureSensor"/> class.
    /// </summary>
    /// <param name="name"> The name of the sensor.</param>
    /// <param name="readCycle"> The cycle of reading the sensor.</param>
    public SimulatedTemperatureSensor(string name, TimeSpan readCycle)
    {
        InstanceName = name;
        ExecutionFrequency = readCycle;
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
        double tempData = (_random.NextDouble() * 30) + 5;
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
LastRead:       {LastRead}
Temperature:    {GetTemperature()} {TemperatureType.GetUnit()}";
        return status;
    }
}
