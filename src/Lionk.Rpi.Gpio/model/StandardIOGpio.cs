// Copyright © 2024 Lionk Project

using System.Device.Gpio;

namespace Lionk.Rpi.Gpio;

/// <summary>
/// This class represents a standard input/output GPIO component.
/// </summary>
public abstract class StandardIOGpio : IGpio
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
    public string? InstanceName { get; set; }

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
    /// Initializes a new instance of the <see cref="StandardIOGpio"/> class.
    /// </summary>
    public StandardIOGpio() => Controller = new GpioController();
}
