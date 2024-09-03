// Copyright © 2024 Lionk Project

using Lionk.Core;
using Lionk.Core.DataModel;

namespace Lionk.TemperatureSensor;

/// <summary>
/// Manual temperature sensor.
/// </summary>
[NamedElement("Manual temperature sensor", "This is a manual temperature sensor.")]
public class ManualTemperatureSensor : BaseTemperatureSensor
{
    /// <summary>
    /// Gets or sets the temperature.
    /// </summary>
    public double Temperature { get; set; }

    /// <summary>
    /// Gets a value indicating whether gets if the sensor can be executed.
    /// </summary>
    public override bool CanExecute => true;

    /// <summary>
    /// Measures the temperature.
    /// </summary>
    public override void Measure()
    {
        base.Measure();
        double celsius = Temperature;
        double fahrenheit = (Temperature * 9.0 / 5.0) + 32.0;
        double kelvin = Temperature + 273.15;

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
}
