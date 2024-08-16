// Copyright © 2024 Lionk Project

using Lionk.Core.Component;

namespace Lionk.Rpi.Gpio;

/// <summary>
/// This interface is used to mark a class as a GPIO component.
/// </summary>
public interface IGpio : IExecutableComponent, IDisposable
{
    /// <summary>
    /// Gets or sets the pin number of the GPIO component.
    /// </summary>
    RaspberryPi4Pin Pin { get; set; }
}
