// Copyright © 2024 Lionk Project

namespace Lionk.DS18B20;

/// <summary>
/// This class extends the <see cref="TemperatureType"/> enumeration.
/// </summary>
public static class TemperatureTypeExtensions
{
    /// <summary>
    /// Method to get the unit of the temperature type.
    /// </summary>
    /// <param name="temperatureType"> The temperature type.</param>
    /// <returns> The unit of the temperature type.</returns>
    /// <exception cref="ArgumentOutOfRangeException"> The temperature type is not supported.</exception>
    public static string GetUnit(this TemperatureType temperatureType) => temperatureType switch
    {
        TemperatureType.Celsius => "°C",
        TemperatureType.Fahrenheit => "°F",
        TemperatureType.Kelvin => "K",
        _ => throw new ArgumentOutOfRangeException(nameof(temperatureType), temperatureType, null),
    };
}
