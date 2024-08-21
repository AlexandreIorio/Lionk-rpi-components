// Copyright © 2024 Lionk Project

using System.Runtime.InteropServices;

using Lionk.Core.Component;

namespace Lionk.Rpi.Gpio;

/// <summary>
/// This interface is used to mark a class as a GPIO component.
/// </summary>
public abstract class Gpio : BaseExecutableComponent
{
    /// <summary>
    /// Gets a value indicating whether the current plateform is a Raspberry Pi.
    /// </summary>
    private readonly bool _isRpi;

    /// <summary>
    /// Gets a value indicating whether the current plateform is a Raspberry Pi.
    /// </summary>
    protected bool IsRpi => _isRpi;

    /// <summary>
    /// Initializes a new instance of the <see cref="Gpio"/> class.
    /// </summary>
    protected Gpio() => _isRpi = RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
        && RuntimeInformation.OSDescription.Contains("raspberry", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets or sets the pin number of the GPIO component.
    /// </summary>
    public abstract Rpi4Gpio Pin { get; set; }
}
