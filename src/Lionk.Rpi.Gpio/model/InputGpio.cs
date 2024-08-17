// Copyright © 2024 Lionk Project

using System.Device.Gpio;
using Lionk.Core.Component;

namespace Lionk.Rpi.Gpio;

/// <summary>
/// This class represents an input GPIO component.
/// </summary>
public class InputGpio : StandardIOGpio, IExecutableComponent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InputGpio"/> class.
    /// </summary>
    public InputGpio()
        : base()
        => Mode = PinMode.Input;

    /// <summary>
    /// Reads the value of the GPIO pin.
    /// </summary>
    /// <returns> The value of the GPIO pin. </returns>
    public PinValue ReadPin()
    {
        PinValue value;
        value = Controller.Read((int)Pin);
        return value;
    }

    /// <inheritdoc/>
    public override TimeSpan? Execute()
    {
        DateTime start = DateTime.UtcNow;
        Measure();
        return DateTime.UtcNow - start;
    }

    /// <summary>
    /// Measures the value of the GPIO pin.
    /// </summary>
    public override void Measure()
    {
        int value = (int)ReadPin();
        Measures.Clear();
        Measures.Add(new Measure<int>("value", DateTime.Now, "state", value));
        OnNewValueAvailable(new MeasureEventArgs<int>(Measures));
    }
}
