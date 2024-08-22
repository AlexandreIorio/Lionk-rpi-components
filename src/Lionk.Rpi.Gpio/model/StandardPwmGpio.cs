// Copyright © 2024 Lionk Project

namespace Lionk.Rpi.Gpio;

/// <summary>
/// This class represents a standard PWM GPIO component.
/// </summary>
public abstract class StandardPwmGpio : Gpio, IPwmChannel
{
    private Rpi4Gpio _pin = Rpi4Gpio.None;
    private int _frequency;
    private double _dutyCycle;

    /// <inheritdoc/>
    public override Rpi4Gpio Pin
    {
        get => _pin;
        set
        {
            if (!value.Is(GpioType.PWM)) throw new ArgumentException("The pin must be a PWM pin.");
            _pin = value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the PWM signal is on. If <see langword="true"/>, the PWM signal is on; otherwise, it is off.
    /// </summary>
    public bool PwmOn { get; set; } = false;

    /// <summary>
    /// Gets or sets the duty cycle of the PWM signal as a value between 0.0 and 1.0.
    /// </summary>
    public virtual double DutyCycle
    {
        get => _dutyCycle;
        set
        {
            double dutyCycle;
            if (value > 1) dutyCycle = 1;
            else if (value < 0) dutyCycle = 0;
            else dutyCycle = value;
            _dutyCycle = dutyCycle;
        }
    }

    /// <summary>
    /// Gets or sets the frequency of the PWM signal in hertz.
    /// </summary>
    public virtual int Frequency
    {
        get => _frequency;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value), "The frequency must be a positive value.");
            _frequency = value;
        }
    }

    /// <summary>
    /// Method to start the PWM signal.
    /// </summary>
    public virtual void Start() => PwmOn = true;

    /// <summary>
    /// Method to stop the PWM signal.
    /// </summary>
    public virtual void Stop() => PwmOn = false;

    /// <inheritdoc/>
    public override bool CanExecute => Pin is not Rpi4Gpio.None
                                       && DutyCycle >= 0
                                       && DutyCycle <= 1
                                       && Frequency > 0;
}
