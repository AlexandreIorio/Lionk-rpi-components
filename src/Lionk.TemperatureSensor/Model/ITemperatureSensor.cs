// Copyright © 2024 Lionk Project

using Lionk.Core;
using Lionk.Core.Component;

namespace Lionk.DS18B20;

/// <summary>
/// This interface is used to get the temperature of sensor connected to a Raspberry Pi.
/// </summary>
[NamedElement("Temperature Sensor", "Temperature Sensor")]
public interface ITemperatureSensor : IMeasurableComponent<double>
{
    /// <summary>
    /// Gets the type of the temperature as Celsius, Fahrenheit or Kelvin.
    /// </summary>
    TemperatureType TemperatureType { get; }
}
