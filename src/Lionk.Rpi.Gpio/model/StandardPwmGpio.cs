// Copyright © 2024 Lionk Project

using Newtonsoft.Json;

namespace Lionk.Rpi.Gpio;

/// <summary>
/// This class represents a standard PWM GPIO component.
/// </summary>
public abstract class StandardPwmGpio : Gpio, IPwmChannel
{
    #region Private Fields

    private double _dutyCycle;
    private int _frequency = 400;
    private Rpi4Gpio _pin = Rpi4Gpio.None;
    private bool _pwmOn;

    #endregion Private Fields

    #region Public Properties

    /// <inheritdoc/>
    public override bool CanExecute => Pin is not Rpi4Gpio.None
                                       && DutyCycle >= 0
                                       && DutyCycle <= 1
                                       && Frequency > 0;

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
            if (value <= 0) value = 1;
            SetField(ref _frequency, value);
        }
    }

    /// <inheritdoc/>
    public override Rpi4Gpio Pin
    {
        get => _pin;
        set
        {
            if (!value.Is(GpioType.PWM)) return;
            SetField(ref _pin, value);
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the PWM signal is on. If <see langword="true"/>, the PWM signal is on; otherwise, it is off.
    /// </summary>
    public bool PwmOn
    {
        get => _pwmOn;
        set => SetField(ref _pwmOn, value);
    }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// Method to start the PWM signal.
    /// </summary>
    public virtual void Start() => PwmOn = true;

    /// <summary>
    /// Method to stop the PWM signal.
    /// </summary>
    public virtual void Stop() => PwmOn = false;

    #endregion Public Methods
}
