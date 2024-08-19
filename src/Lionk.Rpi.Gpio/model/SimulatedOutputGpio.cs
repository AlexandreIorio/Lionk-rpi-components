// Copyright © 2024 Lionk Project

using System.Device.Gpio;
using Lionk.Core;
using Lionk.Core.Component;
using Lionk.Core.DataModel;

namespace Lionk.Rpi.Gpio;

/// <summary>
/// This class represents an output GPIO component.
/// </summary>
[NamedElement("Input simulated Gpio", "This component is used to simulate the gpio gui")]
public class SimulatedOutputGpio : StandardIOGpio, IExecutableComponent
{
    private PinValue _pinValue;

    /// <summary>
    /// Gets or sets the value of the GPIO pin.
    /// </summary>
    public PinValue PinValue { get; set; }

    /// <inheritdoc/>
    public override TimeSpan? Execute()
    {
        DateTime start = DateTime.UtcNow;
        PinValue = _pinValue;
        Measure();
        return DateTime.UtcNow - start;
    }

    /// <inheritdoc/>
    public override void Measure()
    {
        Measures.Clear();
        Measures.Add(new Measure<int>("value", DateTime.Now, "state", (int)PinValue));
        OnNewValueAvailable(new MeasureEventArgs<int>(Measures));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SimulatedOutputGpio"/> class.
    /// </summary>
    public SimulatedOutputGpio() => Mode = PinMode.Output;

    /// <summary>
    /// Sets the value of the GPIO pin.
    /// </summary>
    /// <param name="value"> The value to set. </param>
    public void SetValues(PinValue value) => _pinValue = value;
}
