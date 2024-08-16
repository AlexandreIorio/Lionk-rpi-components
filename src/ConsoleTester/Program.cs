// Copyright © 2024 Lionk Project

using Lionk.Rpi.Gpio;

PwmGpio pwmGpio = new();
pwmGpio.DutyCycle = 0;
pwmGpio.Execute();
CancellationTokenSource cts = new();
int i = 0;
while (true)
{
    i++;
    double value = Math.Abs(Math.Sin((double)i / 50));
    pwmGpio.DutyCycle = value;
    Thread.Sleep(10);
}
