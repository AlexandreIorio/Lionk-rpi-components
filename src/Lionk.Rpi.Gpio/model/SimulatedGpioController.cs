// Copyright © 2024 Lionk Project
using System.Device.Gpio;

namespace Lionk.Rpi.Gpio;

/// <summary>
/// This class simulates a GPIO controller by storing and manipulating the states of GPIO pins.
/// </summary>
public class SimulatedGpioController : IGpioController
{
    private static readonly Dictionary<int, bool> _openedPins = new();
    private static readonly Dictionary<int, PinMode> _pinModes = new();
    private static readonly Dictionary<int, PinValue> _pinValues = new();
    private static readonly HashSet<int> _openPins = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="SimulatedGpioController"/> class.
    /// </summary>
    public SimulatedGpioController()
    {
        foreach (int pin in Enum.GetValues(typeof(Rpi4Gpio)))
        {
            _openedPins[pin] = false;
            _pinValues[pin] = PinValue.Low;
        }
    }

    /// <summary>
    /// Opens a pin.
    /// </summary>
    /// <param name="pinNumber"> The pin number to open. </param>
    public void OpenPin(int pinNumber)
    {
        _openPins.Add(pinNumber);
        _openedPins[pinNumber] = true;
    }

    /// <summary>
    /// Opens a pin and sets its mode.
    /// </summary>
    /// <param name="pinNumber"> The pin number to open. </param>
    /// <param name="mode"> The mode of the pin. </param>
    public void OpenPin(int pinNumber, PinMode mode)
    {
        OpenPin(pinNumber);
        _pinModes[pinNumber] = mode;
    }

    /// <summary>
    /// Closes a pin.
    /// </summary>
    /// <param name="pinNumber"> The pin number to close. </param>
    public void ClosePin(int pinNumber)
    {
        _openPins.Remove(pinNumber);
        _pinValues.Remove(pinNumber);
    }

    /// <summary>
    /// Checks if a pin is open.
    /// </summary>
    /// <param name="pinNumber"> The pin number to check. </param>
    /// <returns> True if the pin is open, false otherwise. </returns>
    public bool IsPinOpen(int pinNumber) => _openPins.Contains(pinNumber);

    /// <summary>
    /// Method to set the mode of a pin.
    /// </summary>
    /// <param name="pinNumber"> The pin number to set the mode. </param>
    /// <param name="mode"> The mode to set. </param>
    /// <exception cref="InvalidOperationException"> Thrown when the pin is not open. </exception>
    public void SetPinMode(int pinNumber, PinMode mode)
    {
        if (!IsPinOpen(pinNumber))
        {
            throw new InvalidOperationException("Pin must be opened before setting its mode.");
        }

        _pinModes[pinNumber] = mode;
    }

    /// <summary>
    /// Gets the mode of a pin.
    /// </summary>
    /// <param name="pinNumber"> The pin number to get the mode. </param>
    /// <returns> The mode of the pin. </returns>
    public PinMode GetPinMode(int pinNumber) => _pinModes[pinNumber];

    /// <summary>
    /// Method to check if a pin mode is supported.
    /// </summary>
    /// <param name="pinNumber"> The pin number to check. </param>
    /// <param name="mode"> The mode to check. </param>
    /// <returns> Always returns true on simulated controller. </returns>
    public bool IsPinModeSupported(int pinNumber, PinMode mode) => true;

    /// <summary>
    /// Reads the value of a pin.
    /// </summary>
    /// <param name="pinNumber"> The pin number to read. </param>
    /// <returns> The value of the pin. </returns>
    /// <exception cref="ArgumentException"> Thrown when the pin is not open. </exception>
    public PinValue Read(int pinNumber)
    {
        if (!_pinValues.ContainsKey(pinNumber))
        {
            throw new ArgumentException("Pin is not open.", nameof(pinNumber));
        }

        return _pinValues[pinNumber];
    }

    /// <summary>
    /// Writes a value to a pin.
    /// </summary>
    /// <param name="pinNumber"> The pin number to write. </param>
    /// <param name="value"> The value to write. </param>
    /// <exception cref="ArgumentException"> Thrown when the pin is not open. </exception>
    public void Write(int pinNumber, PinValue value)
    {
        if (!_openedPins.ContainsKey(pinNumber) || !_openedPins[pinNumber])
        {
            throw new ArgumentException("Pin is not open.", nameof(pinNumber));
        }

        _pinValues[pinNumber] = value;
    }

    /// <summary>
    /// Toggles a pin.
    /// </summary>
    /// <param name="pinNumber"> The pin number to toggle. </param>
    /// <exception cref="ArgumentException"> Thrown when the pin is not open. </exception>
    public void Toggle(int pinNumber)
    {
        if (!_pinValues.ContainsKey(pinNumber))
        {
            throw new ArgumentException("Pin is not open.", nameof(pinNumber));
        }

        _pinValues[pinNumber] = _pinValues[pinNumber] == PinValue.High ? PinValue.Low : PinValue.High;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        foreach (int pin in _openPins)
        {
            ClosePin(pin);
        }
    }
}
