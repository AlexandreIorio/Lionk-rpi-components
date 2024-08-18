// Copyright © 2024 Lionk Project

using System.Device.Gpio;
using Lionk.Core.Component;
using Lionk.Core.DataModel;

namespace Lionk.Rpi.Gpio;

/// <summary>
/// This class represents a standard input/output GPIO component.
/// </summary>
public abstract class StandardIOGpio : IGpio, IMeasurableComponent<int>
{
    /// <summary>
    /// Gets the pin number of the GPIO component.
    /// </summary>
    protected GpioController Controller { get; }

    /// <summary>
    /// Gets or sets the pin mode of the GPIO component.
    /// </summary>
    protected PinMode Mode { get; set; }

    /// <summary>
    /// Gets or sets the pin number of the GPIO component.
    /// </summary>
    public RaspberryPi4Pin Pin { get; set; }

    /// <inheritdoc/>
    public string InstanceName { get; set; } = string.Empty;

    /// <summary>
    /// Gets the measures of the GPIO component.
    /// </summary>
    public List<Measure<int>> Measures { get; } = [];

    /// <inheritdoc/>
    public event EventHandler<MeasureEventArgs<int>>? NewValueAvailable;

    /// <summary>
    /// Opens the GPIO pin.
    /// </summary>
    public void OpenPin() => Controller.OpenPin((int)Pin, Mode);

    /// <summary>
    /// Closes the GPIO pin.
    /// </summary>
    public void ClosePin()
    {
        if (Controller.IsPinOpen((int)Pin)) Controller.ClosePin((int)Pin);
    }

    /// <inheritdoc/>
    public abstract TimeSpan? Execute();

    /// <inheritdoc/>
    public void Dispose()
    {
        ClosePin();
        Controller.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Measures the value of the GPIO pin.
    /// </summary>
    public abstract void Measure();

    /// <summary>
    /// Initializes a new instance of the <see cref="StandardIOGpio"/> class.
    /// </summary>
    public StandardIOGpio() => Controller = new GpioController();

    /// <summary>
    /// Raises the <see cref="NewValueAvailable"/> event.
    /// </summary>
    /// <param name="e">The event arg.</param>
    protected void OnNewValueAvailable(MeasureEventArgs<int> e)
        => NewValueAvailable?.Invoke(this, e);
}
