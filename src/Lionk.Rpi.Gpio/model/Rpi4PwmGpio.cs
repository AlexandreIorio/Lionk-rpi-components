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
            if (_pwmChannel is null) return;
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
        get => base.Pin;
        set
        {
            base.Pin = value;
            _pwmChannel = PwmChannel.Create(base.Pin.PwmChip(), base.Pin.PwmChannel(), base.Frequency, base.DutyCycle);
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

    /// <inheritdoc/>
    protected override void OnExecute(CancellationToken ct)
    {
        base.OnExecute(ct);
        if (PwmOn) Start();
        else Stop();
    }

    /// <summary>
    /// Starts the PWM signal on the GPIO pin.
    /// </summary>
    public override void Start()
    {
        base.Start();
        if (_pwmChannel is null) return;
        _pwmChannel.Frequency = base.Frequency;
        _pwmChannel.DutyCycle = base.DutyCycle;
        _pwmChannel.Start();
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
