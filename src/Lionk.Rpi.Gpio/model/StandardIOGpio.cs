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
    #region Private Fields

    private Rpi4Gpio _pin = Rpi4Gpio.None;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="StandardIOGpio"/> class.
    /// </summary>
    public StandardIOGpio()
    {
        if (IsRpi)
        {
            Controller = new Rpi4GpioController();
        }
        else if (IsWindows)
        {
            Controller = new SimulatedGpioController();
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }

    #endregion Public Constructors

    #region Public Events

    /// <inheritdoc/>
    public event EventHandler<MeasureEventArgs<int>>? NewValueAvailable;

    #endregion Public Events

    #region Protected Properties

    /// <summary>
    /// Gets the pin number of the GPIO component.
    /// </summary>
    public BaseGpioController Controller { get; }

    #endregion Protected Properties

    #region Public Properties

    /// <summary>
    /// Gets the measures of the GPIO component.
    /// </summary>
    public List<Measure<int>> Measures { get; } = [];

    /// <summary>
    /// Gets or sets the pin mode of the GPIO component.
    /// </summary>
    public PinMode Mode { get; protected set; }

    /// <summary>
    /// Gets or sets the pin number of the GPIO component.
    /// </summary>
    public override Rpi4Gpio Pin
    {
        get => _pin;
        set
        {
            if (value is Rpi4Gpio.None || _pin == value) return;

            Rpi4Gpio oldValue = _pin;

            if (Controller is null) return;
            if (Controller.IsPinOpen((int)oldValue)) Controller.ClosePin((int)oldValue);
            if (!Controller.IsPinOpen((int)value)) Controller.OpenPin((int)value, Mode);

            SetField(ref _pin, value);
        }
    }

    #endregion Public Properties

    #region Protected Methods

    /// <summary>
    /// Raises the <see cref="NewValueAvailable"/> event.
    /// </summary>
    /// <param name="e">The event arg.</param>
    protected void OnNewValueAvailable(MeasureEventArgs<int> e)
        => NewValueAvailable?.Invoke(this, e);

    #endregion Protected Methods

    #region Public Methods

    /// <summary>
    /// Closes the GPIO pin.
    /// </summary>
    public void ClosePin()
    {
        if (Pin is Rpi4Gpio.None
            || Controller is null
            || !Controller.IsPinOpen((int)Pin)) return;
        Controller.ClosePin((int)Pin);
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        base.Dispose();
        ClosePin();
        Controller.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Method to check if the GPIO pin is open.
    /// </summary>
    /// <returns> True if the GPIO pin is open, false otherwise. </returns>
    public bool IsOpenPin() => Controller.IsPinOpen((int)Pin);

    /// <summary>
    /// Measures the value of the GPIO pin.
    /// </summary>
    public abstract void Measure();

    /// <summary>
    /// Method to open the GPIO pin.
    /// </summary>
    public void OpenPin() => Controller.OpenPin((int)Pin, Mode);

    #endregion Public Methods
}
