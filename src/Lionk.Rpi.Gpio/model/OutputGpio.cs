﻿// Copyright © 2024 Lionk Project

using System.Device.Gpio;
using Lionk.Core.Component;

namespace Lionk.Rpi.Gpio;

/// <summary>
/// This class represents an output GPIO component.
/// </summary>
public class OutputGpio : StandardIOGpio, IExecutableComponent
{
    /// <summary>
    /// Occurs when a new value is available.
    /// </summary>
    public new event EventHandler<MeasureEventArgs<int>>? NewValueAvailable;

    /// <summary>
    /// Gets or sets the value of the GPIO pin.
    /// </summary>
    public PinValue PinValue { get; set; }

    /// <summary>
    /// Reads the value of the GPIO pin.
    /// </summary>
    /// <param name="value"> The value to write to the GPIO pin. </param>
    public void WritePin(PinValue value) => Controller.Write((int)Pin, value);

    /// <inheritdoc/>
    public override TimeSpan? Execute()
    {
        DateTime start = DateTime.UtcNow;
        Measure();
        WritePin(PinValue);
        return DateTime.UtcNow - start;
    }

    /// <inheritdoc/>
    public override void Measure()
    {
        if (Measures[0].Value != (int)PinValue)
        {
            Measures[0].Value = (int)PinValue;
            NewValueAvailable?.Invoke(this, new MeasureEventArgs<int>(Measures));
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OutputGpio"/> class.
    /// </summary>
    public OutputGpio()
        : base() => Mode = PinMode.Output;
}
