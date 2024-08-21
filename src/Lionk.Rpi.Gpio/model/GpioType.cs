// Copyright © 2024 Lionk Project

namespace Lionk.Rpi.Gpio;

/// <summary>
/// Dispose the GPIO Types.
/// </summary>
public enum GpioType
{
    /// <summary>
    /// Is an unselected GPIO type.
    /// </summary>
    None = 0,

    /// <summary>
    /// Is an I2C GPIO type.
    /// </summary>
    I2C = 1,

    /// <summary>
    /// Is an SPI GPIO type.
    /// </summary>
    SPI = 2,

    /// <summary>
    /// Is an PWM GPIO type.
    /// </summary>
    PWM = 4,

    /// <summary>
    /// Is an UART GPIO type.
    /// </summary>
    UART = 8,

    /// <summary>
    /// Is an Digital GPIO type.
    /// </summary>
    Digital = 16,

    /// <summary>
    /// Is an Analog GPIO type.
    /// </summary>
    GPCLK = 32,

    /// <summary>
    /// Is an PCM GPIO type.
    /// </summary>
    PCM = 64,

    /// <summary>
    /// Is an SDIO GPIO type.
    /// </summary>
    SDIO = 128,

    /// <summary>
    /// Is an DPI GPIO type.
    /// </summary>
    DPI = 256,

    /// <summary>
    /// Is an 1-Wire GPIO type.
    /// </summary>
    OneWire = 512,

    /// <summary>
    /// Is an JTag GPIO type.
    /// </summary>
    JTag = 1024,
}
