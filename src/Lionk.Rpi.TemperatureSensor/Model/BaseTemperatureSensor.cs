// Copyright © 2024 Lionk Project

using Lionk.Core.Component;
using Lionk.Core.DataModel;

namespace Lionk.TemperatureSensor;

/// <summary>
/// This interface is used to get the temperature of sensor connected to a Raspberry Pi.
/// </summary>
public abstract class BaseTemperatureSensor : BaseCyclicComponent, IMeasurableComponent<double>
{
    private TemperatureType _temperatureType = TemperatureType.Celsius;

    /// <inheritdoc/>
    public List<Measure<double>> Measures { get; } =
    [
        new Measure<double>("Temperature", DateTime.UtcNow, TemperatureType.Celsius.GetUnit(), double.NaN),
        new Measure<double>("Temperature", DateTime.UtcNow, TemperatureType.Fahrenheit.GetUnit(), double.NaN),
        new Measure<double>("Temperature", DateTime.UtcNow, TemperatureType.Kelvin.GetUnit(), double.NaN),
    ];

    /// <summary>
    /// Gets or sets the type of the temperature.
    /// </summary>
    public TemperatureType TemperatureType
    {
        get => _temperatureType;
        set => SetField(ref _temperatureType, value);
    }

    /// <summary>
    /// Event raised when a new value is available.
    /// </summary>
    public event EventHandler<MeasureEventArgs<double>>? NewValueAvailable;

    /// <summary>
    /// Method to get the temperature.
    /// </summary>
    /// <param name="nbDecimal"> The number of decimal to keep. Default is 2. </param>
    /// <returns> The temperature. </returns>
    public double GetTemperature(int nbDecimal = 2) => Math.Round(Measures[(int)TemperatureType].Value, nbDecimal);

    /// <summary>
    /// Method to get the unit of the temperature.
    /// </summary>
    /// <returns> The unit of the temperature. </returns>
    public string GetUnit() => TemperatureType.GetUnit();

    /// <inheritdoc/>
    public virtual void Measure() => NewValueAvailable?.Invoke(this, new MeasureEventArgs<double>(Measures));

    /// <inheritdoc/>
    protected override void OnExecute(CancellationToken ct)
    {
        base.OnExecute(ct);
        Measure();
    }

    /// <inheritdoc/>
    protected override void OnInitialize()
    {
        Period = TimeSpan.FromSeconds(5);
        base.OnInitialize();
    }

    /// <summary>
    /// This method is used to set the temperature of the sensor.
    /// </summary>
    /// <param name="nbDecimal"> The number of decimal to keep.</param>
    /// <returns> The value of the temperature.</returns>
    public double GetTemperature(int nbDecimal = 2) => Math.Round(Measures[(int)TemperatureType].Value, nbDecimal);

    /// <summary>
    /// This method is used to get the unit of the temperature.
    /// </summary>
    /// <returns> The unit of the temperature.</returns>
    public string GetUnit() => Measures[(int)TemperatureType].Unit;
}
