// Copyright © 2024 Lionk Project

using System.Device.Pwm;
using Lionk.Core;

namespace Lionk.Rpi.Gpio;

/// <summary>
/// This class represents a PWM GPIO component.
/// </summary>
[NamedElement("PWM Gpio generator Rpi4", "This component represent a PWM Gpio generator from the Raspberry Pi 4")]
public class PwmGpio : Gpio
{
    private readonly PwmChannel _pwmChannel;

    private Rpi4Gpio _pin = Rpi4Gpio.None;

    /// <summary>
    /// Gets or sets a value indicating whether the PWM signal is on. If <see langword="true"/>, the PWM signal is on; otherwise, it is off.
    /// </summary>
    public bool PwmOn { get; set; } = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="PwmGpio"/> class.
    /// </summary>
    public PwmGpio()
    {
        _pwmChannel = PwmChannel.Create(0, 0, Frequency, DutyCycle);
        Frequency = 400;
        DutyCycle = 0.5;
    }

    /// <inheritdoc/>
    public override bool CanExecute => Pin.Is(GpioType.PWM);

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
    public override Rpi4Gpio Pin
    {
        get => _pin;
        set
        {
            if (value.Is(GpioType.PWM)) _pin = value;
            else throw new ArgumentException("The pin must be a PWM pin.");
        }
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        StopPwm();
        _pwmChannel.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Executes the component.
    /// </summary>
    public new void Execute()
    {
        base.Execute();

        if (PwmOn) StartPwm();
        else StopPwm();
    }

    /// <summary>
    /// Starts the PWM signal on the GPIO pin.
    /// </summary>
    public void StartPwm() => _pwmChannel.Start();

    /// <summary>
    /// Stops the PWM signal on the GPIO pin.
    /// </summary>
    public void StopPwm() => _pwmChannel.Stop();
}
