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
    /// Occurs when a new value is available.
    /// </summary>
    public new event EventHandler<MeasureEventArgs<int>>? NewValueAvailable;

    /// <summary>
    /// Initializes a new instance of the <see cref="InputGpio"/> class.
    /// </summary>
    public InputGpio()
        : base()
    {
        Mode = PinMode.Input;
        Measures.Add(new Measure<int>("Read", string.Empty));
    }

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
        if (Measures[0].Value != value)
        {
            Measures[0].Value = value;
            NewValueAvailable?.Invoke(this, new MeasureEventArgs<int>(Measures));
        }
    }
}
