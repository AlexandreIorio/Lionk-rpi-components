// Copyright © 2024 Lionk Project

using System.Device.Pwm;
using Lionk.Core.Component;

namespace Lionk.Rpi.Gpio;

/// <summary>
/// This class represents a PWM GPIO component.
/// </summary>
public class PwmGpio : IGpio, IExecutableComponent, IDisposable
{
    private readonly PwmChannel _pwmChannel;

    /// <summary>
    /// Gets or sets the pin number of the GPIO component.
    /// </summary>
    public RaspberryPi4Pin Pin { get; set; } = RaspberryPi4Pin.GPIO18;

    /// <summary>
    /// Gets or sets the duty cycle of the PWM signal (0.0 to 1.0).
    /// </summary>
    public double DutyCycle
    {
        get => _pwmChannel?.DutyCycle ?? 0;
        set
        {
            double dutyCycle;
            if (value > 1) dutyCycle = 1;
            else if (value < 0) dutyCycle = 0;
            else dutyCycle = value;

            _pwmChannel.DutyCycle = dutyCycle;
        }
    }

    /// <summary>
    /// Gets or sets the frequency of the PWM signal in Hertz.
    /// </summary>
    public int Frequency { get => _pwmChannel?.Frequency ?? 0; set => _pwmChannel.Frequency = value; }

    /// <inheritdoc/>
    public string? InstanceName { get; set; }

    /// <summary>
    /// Gets or sets the PWM chip of the GPIO component.
    /// </summary>
    public PwmChip PwmChip { get; set; } = PwmChip.Chip0;

    /// <summary>
    /// Starts the PWM signal on the GPIO pin.
    /// </summary>
    public void StartPwm() => _pwmChannel.Start();

    /// <summary>
    /// Stops the PWM signal on the GPIO pin.
    /// </summary>
    public void StopPwm() => _pwmChannel.Stop();

    /// <inheritdoc/>
    public TimeSpan? Execute()
    {
        DateTime start = DateTime.UtcNow;
        StartPwm();
        return DateTime.UtcNow - start;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PwmGpio"/> class.
    /// </summary>
    public PwmGpio()
    {
        _pwmChannel = PwmChannel.Create((int)PwmChip, 0, Frequency, DutyCycle);
        Frequency = 400;
        DutyCycle = 0.5;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        StopPwm();
        _pwmChannel.Dispose();
        GC.SuppressFinalize(this);
    }
}

/// <summary>
/// Represents the PWM channel type.
/// </summary>
public enum PwmChip
{
    /// <summary>
    /// Represents the first PWM chip.
    /// </summary>
    Chip0 = 0,

    /// <summary>
    /// Represents the second PWM chip.
    /// </summary>
    Chip1 = 1,
}
