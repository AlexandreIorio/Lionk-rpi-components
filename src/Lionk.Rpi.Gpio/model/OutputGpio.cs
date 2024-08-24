// Copyright © 2024 Lionk Project

using System.Device.Gpio;
using Lionk.Core;
using Lionk.Core.DataModel;

namespace Lionk.Rpi.Gpio;

/// <summary>
/// This class represents an output GPIO component.
/// </summary>
[NamedElement("Output Gpio", "This component represent an Output Gpio")]
public class OutputGpio : StandardIOGpio
{
    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="OutputGpio"/> class.
    /// </summary>
    public OutputGpio()
        : base() => Mode = PinMode.Output;

    #endregion Public Constructors

    #region Public Properties

    /// <summary>
    /// Gets a value indicating whether the component can be executed.
    /// </summary>
    public override bool CanExecute => IsOpenPin();

    /// <summary>
    /// Gets or sets the value of the GPIO pin.
    /// </summary>
    public PinValue? PinValue { get; set; }

    #endregion Public Properties

    #region Protected Methods

    /// <inheritdoc/>
    protected override void OnExecute(CancellationToken ct)
    {
        base.OnExecute(ct);
        if (Pin is Rpi4Gpio.None || PinValue is null) return;
        Measure();
        WritePin(PinValue.Value);
    }

    #endregion Protected Methods

    #region Public Methods

    /// <inheritdoc/>
    public override void Measure()
    {
        if (Measures is null) return;
        Measures.Clear();
        Measures.Add(new Measure<int>("value", DateTime.Now, "state", (int?)PinValue ?? -1));
        OnNewValueAvailable(new MeasureEventArgs<int>(Measures));
    }

    /// <summary>
    /// Reads the value of the GPIO pin.
    /// </summary>
    /// <param name="value"> The value to write to the GPIO pin. </param>
    public void WritePin(PinValue value)
    {
        if (Pin is Rpi4Gpio.None) return;
        Controller.Write((int)Pin, value);
    }

    #endregion Public Methods
}
