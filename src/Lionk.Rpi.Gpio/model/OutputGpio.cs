// Copyright © 2024 Lionk Project

using System.Device.Gpio;
using Lionk.Core;
using Lionk.Core.Component;
using Lionk.Core.DataModel;

namespace Lionk.Rpi.Gpio;

/// <summary>
/// This class represents an output GPIO component.
/// </summary>
[NamedElement("Output Gpio RPI4", "This component represent an Output Gpio from the Raspberry Pi 4")]
public class OutputGpio : StandardIOGpio, IExecutableComponent
{
    /// <summary>
    /// Gets or sets the value of the GPIO pin.
    /// </summary>
    public PinValue? PinValue { get; set; }

    /// <summary>
    /// Reads the value of the GPIO pin.
    /// </summary>
    /// <param name="value"> The value to write to the GPIO pin. </param>
    public void WritePin(PinValue value)
    {
        if (Pin is RaspberryPi4Pin.None) return;
        Controller.Write((int)Pin, value);
    }

    /// <inheritdoc/>
    public override TimeSpan? Execute()
    {
        if (Pin is RaspberryPi4Pin.None || PinValue is null) return null;
        DateTime start = DateTime.UtcNow;
        Measure();
        WritePin(PinValue.Value);
        return DateTime.UtcNow - start;
    }

    /// <inheritdoc/>
    public override void Measure()
    {
        if (Measures is null) return;
        Measures.Clear();
        Measures.Add(new Measure<int>("value", DateTime.Now, "state", (int?)PinValue ?? -1));
        OnNewValueAvailable(new MeasureEventArgs<int>(Measures));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OutputGpio"/> class.
    /// </summary>
    public OutputGpio()
        : base() => Mode = PinMode.Output;
}
