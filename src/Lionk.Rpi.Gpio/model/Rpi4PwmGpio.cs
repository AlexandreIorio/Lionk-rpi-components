// Copyright © 2024 Lionk Project

using System.Device.Pwm;
using Lionk.Core;

namespace Lionk.Rpi.Gpio;

/// <summary>
/// This class represents a PWM GPIO component.
/// </summary>
[NamedElement("PWM Gpio generator Rpi4", "This component represent a PWM Gpio generator from the Raspberry Pi 4")]
public class Rpi4PwmGpio : StandardPwmGpio
{
    private PwmChannel? _pwmChannel;

    private Rpi4Gpio _pin = Rpi4Gpio.None;

    /// <inheritdoc/>
    public override bool CanExecute => base.CanExecute && _pwmChannel is not null;

    /// <summary>
    /// Gets or sets the duty cycle of the PWM signal (0.0 to 1.0), -1 if the PWM signal is not initialized.
    /// </summary>
    public new double DutyCycle
    {
        get
        {
            if (_pwmChannel is null) return -1;
            return _pwmChannel.DutyCycle;
        }

        set
        {
            base.DutyCycle = value;
            if (_pwmChannel is null) throw new InvalidOperationException("The PWM signal is not initialized.");
            _pwmChannel.DutyCycle = base.DutyCycle;
        }
    }

    /// <summary>
    /// Gets or sets the frequency of the PWM signal in Hertz.
    /// </summary>
    public new int Frequency
    {
        get => _pwmChannel?.Frequency ?? -1;
        set
        {
            base.Frequency = value;
            if (_pwmChannel is null) throw new InvalidOperationException("The PWM signal is not initialized.");
            _pwmChannel.Frequency = base.Frequency;
        }
    }

    /// <inheritdoc/>
    public override Rpi4Gpio Pin
    {
        get => _pin;
        set
        {
            if (!value.Is(GpioType.PWM)) throw new ArgumentException("The pin must be a PWM pin.");
            _pin = value;
            _pwmChannel = PwmChannel.Create(_pin.PwmChip(), _pin.PwmChannel(), Frequency, DutyCycle);
        }
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        Stop();
        _pwmChannel?.Dispose();
        _pwmChannel = null;
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Executes the component.
    /// </summary>
    public new void Execute()
    {
        base.Execute();
        if (PwmOn) Start();
        else Stop();
    }

    /// <summary>
    /// Starts the PWM signal on the GPIO pin.
    /// </summary>
    public override void Start()
    {
        base.Start();
        _pwmChannel?.Start();
    }

    /// <summary>
    /// Stops the PWM signal on the GPIO pin.
    /// </summary>
    public override void Stop()
    {
        base.Stop();
        _pwmChannel?.Stop();
    }
}
