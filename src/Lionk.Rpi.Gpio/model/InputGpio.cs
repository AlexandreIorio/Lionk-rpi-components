// Copyright © 2024 Lionk Project

using System.Device.Gpio;
using Lionk.Core;
using Lionk.Core.DataModel;

namespace Lionk.Rpi.Gpio;

/// <summary>
/// This class represents an input GPIO component.
/// </summary>
[NamedElement("Input Gpio", "This component represent an input Gpio")]
public class InputGpio : StandardIOGpio
{
    /// <inheritdoc/>
    public override bool CanExecute => IsOpenPin();

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
    public PinValue? ReadPin()
    {
        if (Pin is Rpi4Gpio.None) return null;
        PinValue value;
        value = Controller.Read((int)Pin);
        return value;
    }

    /// <inheritdoc/>
    protected override void OnExecute(CancellationToken ct)
    {
        base.OnExecute(ct);
        Measure();
    }

    /// <summary>
    /// Measures the value of the GPIO pin.
    /// </summary>
    public override void Measure()
    {
        if (Measures is null) return;
        int value = (int?)ReadPin() ?? -1;
        Measures.Clear();
        Measures.Add(new Measure<int>("value", DateTime.Now, "state", value));
        OnNewValueAvailable(new MeasureEventArgs<int>(Measures));
    }
}
