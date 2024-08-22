// Copyright © 2024 Lionk Project

namespace Lionk.Rpi.Gpio;

/// <summary>
/// This extension class provides methods to get the GPIO type of a pin.
/// </summary>
public static class Rpi4GpioExtensions
{
    /// <summary>
    /// This method returns the GPIO type of a pin.
    /// </summary>
    /// <param name="pin"> The pin to get the GPIO type from. </param>
    /// <returns> The GPIO type of the pin. </returns>
    public static int GetGpioType(this Rpi4Gpio pin) => pin switch
    {
        Rpi4Gpio.GPIO0 => (int)GpioType.DPI + (int)GpioType.I2C + (int)GpioType.UART,
        Rpi4Gpio.GPIO1 => (int)GpioType.DPI + (int)GpioType.I2C + (int)GpioType.UART,
        Rpi4Gpio.GPIO2 => (int)GpioType.DPI + (int)GpioType.I2C + (int)GpioType.UART,
        Rpi4Gpio.GPIO3 => (int)GpioType.DPI + (int)GpioType.I2C + (int)GpioType.UART,
        Rpi4Gpio.GPIO4 => (int)GpioType.DPI + (int)GpioType.OneWire + (int)GpioType.JTag + (int)GpioType.GPCLK + (int)GpioType.UART,
        Rpi4Gpio.GPIO5 => (int)GpioType.DPI + (int)GpioType.JTag + (int)GpioType.GPCLK + (int)GpioType.UART,
        Rpi4Gpio.GPIO6 => (int)GpioType.DPI + (int)GpioType.JTag + (int)GpioType.GPCLK + (int)GpioType.UART,
        Rpi4Gpio.GPIO7 => (int)GpioType.DPI + (int)GpioType.SPI + (int)GpioType.UART,
        Rpi4Gpio.GPIO8 => (int)GpioType.DPI + (int)GpioType.SPI + (int)GpioType.UART,
        Rpi4Gpio.GPIO9 => (int)GpioType.DPI + (int)GpioType.SPI + (int)GpioType.UART,
        Rpi4Gpio.GPIO10 => (int)GpioType.DPI + (int)GpioType.SPI + (int)GpioType.UART,
        Rpi4Gpio.GPIO11 => (int)GpioType.DPI + (int)GpioType.SPI + (int)GpioType.UART,
        Rpi4Gpio.GPIO12 => (int)GpioType.DPI + (int)GpioType.JTag + (int)GpioType.PWM + (int)GpioType.UART,
        Rpi4Gpio.GPIO13 => (int)GpioType.DPI + (int)GpioType.JTag + (int)GpioType.PWM + (int)GpioType.UART,
        Rpi4Gpio.GPIO14 => (int)GpioType.DPI + (int)GpioType.UART,
        Rpi4Gpio.GPIO15 => (int)GpioType.DPI + (int)GpioType.UART,
        Rpi4Gpio.GPIO16 => (int)GpioType.DPI + (int)GpioType.SPI,
        Rpi4Gpio.GPIO17 => (int)GpioType.DPI + (int)GpioType.SPI,
        Rpi4Gpio.GPIO18 => (int)GpioType.DPI + (int)GpioType.SPI + (int)GpioType.PWM + (int)GpioType.PCM,
        Rpi4Gpio.GPIO19 => (int)GpioType.DPI + (int)GpioType.SPI + (int)GpioType.PWM + (int)GpioType.PCM,
        Rpi4Gpio.GPIO20 => (int)GpioType.DPI + (int)GpioType.SPI + (int)GpioType.PCM,
        Rpi4Gpio.GPIO21 => (int)GpioType.DPI + (int)GpioType.SPI + (int)GpioType.PCM,
        Rpi4Gpio.GPIO22 => (int)GpioType.DPI + (int)GpioType.SDIO + (int)GpioType.JTag,
        Rpi4Gpio.GPIO23 => (int)GpioType.DPI + (int)GpioType.SDIO + (int)GpioType.JTag,
        Rpi4Gpio.GPIO24 => (int)GpioType.DPI + (int)GpioType.SDIO + (int)GpioType.JTag,
        Rpi4Gpio.GPIO25 => (int)GpioType.DPI + (int)GpioType.SDIO + (int)GpioType.JTag,
        Rpi4Gpio.GPIO26 => (int)GpioType.DPI + (int)GpioType.SDIO + (int)GpioType.JTag,
        Rpi4Gpio.GPIO27 => (int)GpioType.DPI + (int)GpioType.SDIO + (int)GpioType.JTag,
        _ => (int)GpioType.None,
    };

    /// <summary>
    /// Method to check if a pin is of a specific GPIO type.
    /// </summary>
    /// <param name="pin"> The pin to check. </param>
    /// <param name="type"> The GPIO type to check. </param>
    /// <returns> True if the pin is of the specified GPIO type, false otherwise. </returns>
    public static bool Is(this Rpi4Gpio pin, GpioType type) => (GetGpioType(pin) & (int)type) == (int)type;

    /// <summary>
    /// This method returns the PWM chip of a pin.
    /// </summary>
    /// <param name="pin"> The pin to get the PWM chip from. </param>
    /// <returns> The PWM chip of the pin. </returns>
    public static int PwmChip(this Rpi4Gpio pin) => pin switch
    {
        Rpi4Gpio.GPIO12 => 0,
        Rpi4Gpio.GPIO13 => 0,
        Rpi4Gpio.GPIO18 => 0,
        Rpi4Gpio.GPIO19 => 0,
        _ => -1,
    };

    /// <summary>
    /// This method returns the PWM channel of a pin.
    /// </summary>
    /// <param name="pin"> The pin to get the PWM channel from. </param>
    /// <returns> The PWM channel of the pin. </returns>
    public static int PwmChannel(this Rpi4Gpio pin) => pin switch
    {
        Rpi4Gpio.GPIO12 => 0,
        Rpi4Gpio.GPIO13 => 1,
        Rpi4Gpio.GPIO18 => 0,
        Rpi4Gpio.GPIO19 => 1,
        _ => -1,
    };
}
