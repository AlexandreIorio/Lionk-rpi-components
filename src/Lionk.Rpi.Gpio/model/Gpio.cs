﻿// Copyright © 2024 Lionk Project

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
    protected bool IsRpi => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

    /// <summary>
    /// Gets a value indicating whether the current plateform is Windows.
    /// </summary>
    protected bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    /// <summary>
    /// Gets or sets the pin number of the GPIO component.
    /// </summary>
    public abstract Rpi4Gpio Pin { get; set; }
}
