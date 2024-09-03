// Copyright © 2024 Lionk Project

using System.Device;
using Lionk.Core;

namespace Lionk.Rpi.Gpio;

/// <summary>
/// Simulates a PWM channel for testing and development purposes.
/// </summary>
[NamedElement("Simulated PWM Gpio", "This component represent a simulation of a PWM GPIO")]
public class SimulatedPwmChannel : StandardPwmGpio
{
    /// <summary>
    /// Provides component information for debugging purposes.
    /// </summary>
    /// <returns>A <see cref="ComponentInformation"/> instance containing information about the simulated PWM channel.</returns>
    public ComponentInformation QueryComponentInformation() => new(this, "Simulated PWM Device");

    /// <summary>
    /// Starts the simulated PWM channel.
    /// </summary>
    public override void Start()
    {
        base.Start();
        Console.WriteLine("Simulated PWM channel started.");
    }

    /// <summary>
    /// Stops the simulated PWM channel.
    /// </summary>
    public override void Stop()
    {
        base.Stop();
        Console.WriteLine("Simulated PWM channel stopped.");
    }
}
