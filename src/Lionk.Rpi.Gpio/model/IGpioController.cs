// Copyright © 2024 Lionk Project

using System.Device.Gpio;

namespace Lionk.Rpi.Gpio;

/// <summary>
/// This interface represents a GPIO controller.
/// </summary>
public interface IGpioController
{
    #region Public Methods

    void ClosePin(int pinNumber);

    void Dispose();

    PinMode GetPinMode(int pinNumber);

    bool IsPinModeSupported(int pinNumber, PinMode mode);

    bool IsPinOpen(int pinNumber);

    void OpenPin(int pinNumber);

    void OpenPin(int pinNumber, PinMode mode);

    PinValue Read(int pinNumber);

    void SetPinMode(int pinNumber, PinMode mode);

    void Toggle(int pinNumber);

    void Write(int pinNumber, PinValue value);

    #endregion Public Methods
}
