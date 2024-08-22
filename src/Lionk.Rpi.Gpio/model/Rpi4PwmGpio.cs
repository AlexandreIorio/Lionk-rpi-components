// Copyright © 2024 Lionk Project

using System.Device.Pwm;
using Lionk.Core;

namespace Lionk.Rpi.Gpio;

/// <summary>
/// This class represents a PWM GPIO component.
/// </summary>
[NamedElement("PWM Gpio generator Rpi4", "This component represent a PWM Gpio generator from the Raspberry Pi 4")]
public class Rpi4PwmGpio : Gpio, IPwmChannel
{
    private PwmChannel? _pwmChannel;

    private Rpi4Gpio _pin = Rpi4Gpio.None;

    /// <summary>
    /// Gets or sets a value indicating whether the PWM signal is on. If <see langword="true"/>, the PWM signal is on; otherwise, it is off.
    /// </summary>
    public bool PwmOn { get; set; } = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="Rpi4PwmGpio"/> class.
    /// </summary>
    public Rpi4PwmGpio()
    {
        _pwmChannel = PwmChannel.Create(0, 0, Frequency, DutyCycle);
        Frequency = 400;
        DutyCycle = 0.5;
    }

    /// <inheritdoc/>
    public override bool CanExecute => _pwmChannel is not null
            && DutyCycle >= 0
            && DutyCycle <= 1
            && Frequency > 0;

    /// <summary>
    /// Gets or sets the duty cycle of the PWM signal (0.0 to 1.0), -1 if the PWM signal is not initialized.
    /// </summary>
    public double DutyCycle
    {
        get
        {
            if (_pwmChannel is null) return -1;
            return _pwmChannel.DutyCycle;
        }

        set
        {
            if (_pwmChannel is null) throw new InvalidOperationException("The PWM signal is not initialized.");
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
    public int Frequency
    {
        get => _pwmChannel?.Frequency ?? -1;
        set
        {
            if (_pwmChannel is null) throw new InvalidOperationException("The PWM signal is not initialized.");
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value), "The frequency must be a positive value.");
            _pwmChannel.Frequency = value;
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
    public void Start() => _pwmChannel?.Start();

    /// <summary>
    /// Stops the PWM signal on the GPIO pin.
    /// </summary>
    public void Stop() => _pwmChannel?.Stop();
}
