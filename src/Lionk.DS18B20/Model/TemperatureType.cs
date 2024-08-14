// Copyright © 2024 Lionk Project

namespace Lionk.DS18B20;

/// <summary>
/// The type of the temperature.
/// </summary>
public enum TemperatureType
{
    /// <summary>
    /// The temperature is in Celsius.
    /// </summary>
    Celsius = 0,

    /// <summary>
    /// The temperature is in Fahrenheit.
    /// </summary>
    Fahrenheit = 1,

    /// <summary>
    /// The temperature is in Kelvin.
    /// </summary>
    Kelvin = 2,
}
