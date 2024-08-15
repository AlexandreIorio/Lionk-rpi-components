// Copyright © 2024 Lionk Project

using Lionk.Core.Component;

namespace Lionk.TemperatureSensor;

/// <summary>
/// This class extends the <see cref="SimulatedTemperatureSensor"/> class.
/// </summary>
public static class SimulatedTemperatureSensorExtensions
{
    /// <summary>
    /// This method returns the configuration type of the <see cref="SimulatedTemperatureSensor"/>.
    /// </summary>
    /// <param name="component"> The sensor.</param>
    /// <returns> The configuration type.</returns>
    public static Type? GetConfigurationType(this IComponent component)
    {
        if (component is null) return null;
        return typeof(SimulatedTemperatureSensorConfig);
    }
}
