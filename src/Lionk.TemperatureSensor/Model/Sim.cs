// Copyright © 2024 Lionk Project

using Lionk.Core.Component;
using Lionk.DS18B20;
using Lionk.Log;

namespace Lionk.TemperatureSensor;

/// <summary>
/// This class is used to simulate the temperature sensor.
/// </summary>
public class Sim
{
    private readonly Random _random = new();

    /// <summary>
    /// Gets the temperature value.
    /// </summary>
    public double Temperature { get; private set; }

    /// <summary>
    /// Measures the temperature.
    /// </summary>
    public void Measure() =>
        Temperature = (_random.NextDouble() * 30) + 5;
}
