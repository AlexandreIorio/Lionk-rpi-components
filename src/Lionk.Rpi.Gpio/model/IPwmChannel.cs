// Copyright © 2024 Lionk Project

namespace Lionk.Rpi.Gpio;

/// <summary>
/// Represents a contract for PWM channels.
/// </summary>
public interface IPwmChannel : IDisposable
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the duty cycle represented as a value between 0.0 and 1.0.
    /// </summary>
    double DutyCycle { get; set; }

    /// <summary>
    /// Gets or sets the frequency in hertz.
    /// </summary>
    int Frequency { get; set; }

    /// <summary>
    /// Gets a value indicating whether the PWM channel is running.
    /// </summary>
    bool IsRunning { get; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// Starts the PWM channel.
    /// </summary>
    void Start();

    /// <summary>
    /// Stops the PWM channel.
    /// </summary>
    void Stop();

    #endregion Public Methods
}
