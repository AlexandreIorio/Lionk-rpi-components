// Copyright © 2024 Lionk Project

using System.Device.Gpio;
using Lionk.Core.Component;
using Lionk.Core.DataModel;

namespace Lionk.Rpi.Gpio;

/// <summary>
/// This class represents a standard input/output GPIO component.
/// </summary>
public abstract class StandardIOGpio : Gpio, IMeasurableComponent<int>
{
    private Rpi4Gpio _pin = Rpi4Gpio.None;

    /// <summary>
    /// Initializes a new instance of the <see cref="StandardIOGpio"/> class.
    /// </summary>
    public StandardIOGpio()
    {
        if (IsRpi)
        {
            Controller = new Rpi4GpioController();
        }
        else
        {
            Controller = new SimulatedGpioController();
        }
    }

    /// <inheritdoc/>
    public event EventHandler<MeasureEventArgs<int>>? NewValueAvailable;

    /// <summary>
    /// Gets the measures of the GPIO component.
    /// </summary>
    public List<Measure<int>> Measures { get; } = [];

    /// <summary>
    /// Gets or sets the pin number of the GPIO component.
    /// </summary>
    public override Rpi4Gpio Pin
    {
        get => _pin;
        set
        {
            if (value is Rpi4Gpio.None) return;

            Rpi4Gpio oldValue = _pin;

            if (Controller is null) return;
            if (Controller.IsPinOpen((int)oldValue)) Controller.ClosePin((int)oldValue);
            if (!Controller.IsPinOpen((int)value)) Controller.OpenPin((int)value, Mode);

            SetField(ref _pin, value);
        }
    }

    /// <summary>
    /// Gets the pin number of the GPIO component.
    /// </summary>
    protected IGpioController Controller { get; }

    /// <summary>
    /// Gets or sets the pin mode of the GPIO component.
    /// </summary>
    protected PinMode Mode { get; set; }

    /// <summary>
    /// Closes the GPIO pin.
    /// </summary>
    public void ClosePin() => Controller.ClosePin((int)Pin);

    /// <summary>
    /// Method to open the GPIO pin.
    /// </summary>
    public void OpenPin() => Controller.OpenPin((int)Pin, Mode);

    /// <summary>
    /// Method to check if the GPIO pin is open.
    /// </summary>
    /// <returns> True if the GPIO pin is open, false otherwise. </returns>
    public bool IsOpenPin() => Controller.IsPinOpen((int)Pin);

    /// <inheritdoc/>
    public override void Dispose()
    {
        base.Dispose();
        ClosePin();
        if (IsRpi && Controller is not null)
        {
            Controller.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Measures the value of the GPIO pin.
    /// </summary>
    public abstract void Measure();

    /// <summary>
    /// Raises the <see cref="NewValueAvailable"/> event.
    /// </summary>
    /// <param name="e">The event arg.</param>
    protected void OnNewValueAvailable(MeasureEventArgs<int> e)
        => NewValueAvailable?.Invoke(this, e);
}
