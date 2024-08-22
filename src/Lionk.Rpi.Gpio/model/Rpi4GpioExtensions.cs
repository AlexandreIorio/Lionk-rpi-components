// Copyright © 2024 Lionk Project

using Microsoft.AspNetCore.Http;

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
    public static GpioType GetGpioType(this Rpi4Gpio pin) => pin switch
    {
        Rpi4Gpio.GPIO0 => GpioType.DPI & GpioType.I2C & GpioType.UART,
        Rpi4Gpio.GPIO1 => GpioType.DPI & GpioType.I2C & GpioType.UART,
        Rpi4Gpio.GPIO2 => GpioType.DPI & GpioType.I2C & GpioType.UART,
        Rpi4Gpio.GPIO3 => GpioType.DPI & GpioType.I2C & GpioType.UART,
        Rpi4Gpio.GPIO4 => GpioType.DPI & GpioType.OneWire & GpioType.JTag & GpioType.GPCLK & GpioType.UART,
        Rpi4Gpio.GPIO5 => GpioType.DPI & GpioType.JTag & GpioType.GPCLK & GpioType.UART,
        Rpi4Gpio.GPIO6 => GpioType.DPI & GpioType.JTag & GpioType.GPCLK & GpioType.UART,
        Rpi4Gpio.GPIO7 => GpioType.DPI & GpioType.SPI & GpioType.UART,
        Rpi4Gpio.GPIO8 => GpioType.DPI & GpioType.SPI & GpioType.UART,
        Rpi4Gpio.GPIO9 => GpioType.DPI & GpioType.SPI & GpioType.UART,
        Rpi4Gpio.GPIO10 => GpioType.DPI & GpioType.SPI & GpioType.UART,
        Rpi4Gpio.GPIO11 => GpioType.DPI & GpioType.SPI & GpioType.UART,
        Rpi4Gpio.GPIO12 => GpioType.DPI & GpioType.JTag & GpioType.PWM & GpioType.UART,
        Rpi4Gpio.GPIO13 => GpioType.DPI & GpioType.JTag & GpioType.PWM & GpioType.UART,
        Rpi4Gpio.GPIO14 => GpioType.DPI & GpioType.UART,
        Rpi4Gpio.GPIO15 => GpioType.DPI & GpioType.UART,
        Rpi4Gpio.GPIO16 => GpioType.DPI & GpioType.SPI,
        Rpi4Gpio.GPIO17 => GpioType.DPI & GpioType.SPI,
        Rpi4Gpio.GPIO18 => GpioType.DPI & GpioType.SPI & GpioType.PWM & GpioType.PCM,
        Rpi4Gpio.GPIO19 => GpioType.DPI & GpioType.SPI & GpioType.PWM & GpioType.PCM,
        Rpi4Gpio.GPIO20 => GpioType.DPI & GpioType.SPI & GpioType.PCM,
        Rpi4Gpio.GPIO21 => GpioType.DPI & GpioType.SPI & GpioType.PCM,
        Rpi4Gpio.GPIO22 => GpioType.DPI & GpioType.SDIO & GpioType.JTag,
        Rpi4Gpio.GPIO23 => GpioType.DPI & GpioType.SDIO & GpioType.JTag,
        Rpi4Gpio.GPIO24 => GpioType.DPI & GpioType.SDIO & GpioType.JTag,
        Rpi4Gpio.GPIO25 => GpioType.DPI & GpioType.SDIO & GpioType.JTag,
        Rpi4Gpio.GPIO26 => GpioType.DPI & GpioType.SDIO & GpioType.JTag,
        Rpi4Gpio.GPIO27 => GpioType.DPI & GpioType.SDIO & GpioType.JTag,
        _ => GpioType.None,
    };

    /// <summary>
    /// Method to check if a pin is of a specific GPIO type.
    /// </summary>
    /// <param name="pin"> The pin to check. </param>
    /// <param name="type"> The GPIO type to check. </param>
    /// <returns> True if the pin is of the specified GPIO type, false otherwise. </returns>
    public static bool Is(this Rpi4Gpio pin, GpioType type) => (GetGpioType(pin) & type) == type;

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
