// Copyright © 2024 Lionk Project

using Lionk.Core;
using Lionk.Core.DataModel;
using Lionk.Log;

namespace Lionk.TemperatureSensor;

/// <summary>
/// This class is used to simulate the temperature sensor.
/// </summary>
[NamedElement("Simulated sensor", "This is a simulated sensor to test gui")]
public class SimulatedTemperatureSensor : BaseTemperatureSensor
{
    private static readonly IStandardLogger? _logger = LogService.CreateLogger("SimulatedTemperatureSensor");
    private readonly Random _random = new();

    #region Observable Properties

    private double _maxSimulatedTemperature = 20;
    private double _minSimulatedTemperature = 5;

    /// <summary>
    /// Gets or sets the maximum simulated temperature.
    /// </summary>
    public double MaxSimulatedTemperature
    {
        get => _maxSimulatedTemperature;
        set => SetField(ref _maxSimulatedTemperature, value);
    }

    /// <summary>
    /// Gets or sets the minimum simulated temperature.
    /// </summary>
    public double MinSimulatedTemperature
    {
        get => _minSimulatedTemperature;
        set => SetField(ref _minSimulatedTemperature, value);
    }

    #endregion

    /// <inheritdoc/>
    public override bool CanExecute => true;

    /// <inheritdoc/>
    public override void Measure()
    {
        base.Measure();

        // Simulate a temperature value between MinSimulatedTemperature and MaxSimulatedTemperature
        double simulatedTemperature =
            (_random.NextDouble()
            * (_maxSimulatedTemperature - _minSimulatedTemperature))
            + _minSimulatedTemperature;

        SetTemperature(simulatedTemperature);
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
