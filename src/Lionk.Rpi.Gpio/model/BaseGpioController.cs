// Copyright © 2024 Lionk Project

using System.Device.Gpio;

namespace Lionk.Rpi.Gpio;

/// <summary>
/// This interface represents a GPIO controller.
/// </summary>
public abstract class BaseGpioController
{
    #region Public Events

    /// <summary>
    /// Event that is raised when the value of a pin changes.
    /// </summary>
    public event PinChangeEventHandler? PinValueChanged;

    #endregion Public Events

    /// <summary>
    /// Method to raise the pin value changed event.
    /// </summary>
    /// <param name="sender"> The sender of the event. </param>
    /// <param name="e"> The event arguments. </param>
    protected void OnPinValueChanged(object sender, PinValueChangedEventArgs e) => PinValueChanged?.Invoke(sender, e);

    #region Public Methods

    /// <summary>
    /// Method to close a pin.
    /// </summary>
    /// <param name="pinNumber"> The pin number to open. </param>
    public abstract void ClosePin(int pinNumber);

    /// <summary>
    /// Method to dispose the controller.
    /// </summary>
    public abstract void Dispose();

    /// <summary>
    /// Method to get the mode of a pin.
    /// </summary>
    /// <param name="pinNumber"> The pin number to get the mode. </param>
    /// <returns> The mode of the pin. </returns>
    public abstract PinMode GetPinMode(int pinNumber);

    /// <summary>
    /// Method to check if a pin mode is supported.
    /// </summary>
    /// <param name="pinNumber"> The pin number to check. </param>
    /// <param name="mode"> The mode to check. </param>
    /// <returns> True if the mode is supported, false otherwise. </returns>
    public abstract bool IsPinModeSupported(int pinNumber, PinMode mode);

    /// <summary>
    /// Method to check if a pin is open.
    /// </summary>
    /// <param name="pinNumber"> The pin number to check. </param>
    /// <returns> True if the pin is open, false otherwise. </returns>
    public abstract bool IsPinOpen(int pinNumber);

    /// <summary>
    /// Method to open a pin.
    /// </summary>
    /// <param name="pinNumber"> The pin number to open. </param>
    public abstract void OpenPin(int pinNumber);

    /// <summary>
    /// Method to open a pin and set its mode.
    /// </summary>
    /// <param name="pinNumber"> The pin number to open. </param>
    /// <param name="mode"> The mode of the pin. </param>
    public abstract void OpenPin(int pinNumber, PinMode mode);

    /// <summary>
    /// Method to read the value of a pin.
    /// </summary>
    /// <param name="pinNumber"> The pin number to read. </param>
    /// <returns> The value of the pin. </returns>
    public abstract PinValue Read(int pinNumber);

    /// <summary>
    /// Method to register a callback for pin value changed event.
    /// </summary>
    /// <param name="pin"> The pin to register. </param>
    /// <param name="eventTypes"> The event types to register. </param>
    public abstract void RegisterCallbackForPinValueChangedEvent(Rpi4Gpio pin, PinEventTypes eventTypes);

    /// <summary>
    /// Method to set the mode of a pin.
    /// </summary>
    /// <param name="pinNumber"> The pin number to set the mode. </param>
    /// <param name="mode"> The mode to set. </param>
    public abstract void SetPinMode(int pinNumber, PinMode mode);

    /// <summary>
    /// Method to toggle a pin.
    /// </summary>
    /// <param name="pinNumber"> The pin number to toggle. </param>
    public abstract void Toggle(int pinNumber);

    /// <summary>
    /// Method to write a value to a pin.
    /// </summary>
    /// <param name="pinNumber"> The pin number to write. </param>
    /// <param name="value"> The value to write. </param>
    public abstract void Write(int pinNumber, PinValue value);
    #endregion Public Methods
}
