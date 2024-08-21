// Copyright © 2024 Lionk Project

using System.Device;

namespace Lionk.Rpi.Gpio;

/// <summary>
/// Simulates a PWM channel for testing and development purposes.
/// </summary>
public class SimulatedPwmChannel : IPwmChannel
{
    private int _frequency;
    private double _dutyCycle;

    /// <summary>
    /// Initializes a new instance of the <see cref="SimulatedPwmChannel"/> class.
    /// </summary>
    /// <param name="frequency">The initial frequency in hertz.</param>
    /// <param name="dutyCycle">The initial duty cycle as a value between 0.0 and 1.0.</param>
    public SimulatedPwmChannel(int frequency = 400, double dutyCycle = 0.5)
    {
        _frequency = frequency;
        _dutyCycle = dutyCycle;
    }

    /// <summary>
    /// Gets or sets the frequency in hertz.
    /// </summary>
    public int Frequency
    {
        get => _frequency;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Frequency must be a positive value.");
            }

            _frequency = value;
        }
    }

    /// <summary>
    /// Gets or sets the duty cycle represented as a value between 0.0 and 1.0.
    /// </summary>
    public double DutyCycle
    {
        get => _dutyCycle;
        set
        {
            if (value < 0.0 || value > 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Duty cycle must be between 0.0 and 1.0.");
            }

            _dutyCycle = value;
        }
    }

    /// <summary>
    /// Starts the simulated PWM channel.
    /// </summary>
    public void Start() => Console.WriteLine("Simulated PWM channel started.");

    /// <summary>
    /// Stops the simulated PWM channel.
    /// </summary>
    public void Stop() => Console.WriteLine("Simulated PWM channel stopped.");

    /// <summary>
    /// Provides component information for debugging purposes.
    /// </summary>
    /// <returns>A <see cref="ComponentInformation"/> instance containing information about the simulated PWM channel.</returns>
    public ComponentInformation QueryComponentInformation() => new(this, "Simulated PWM Device");

    /// <inheritdoc/>
    public void Dispose() => throw new NotImplementedException();
}
