# Remote Debugging of a C# .NET 8 Application on Raspberry Pi 4

## Prerequisites

- A Raspberry Pi 4 running a Linux distribution (Raspbian recommended).
- SSH access enabled on the Raspberry Pi.
- Visual Studio Community 2022 installed on your development machine.
- .NET 8 SDK installed on the Raspberry Pi.
- A DS18B20 temperature sensor connected to the Raspberry Pi.

## Step 1: Install .NET 8 on the Raspberry Pi

1. Open an SSH connection to your Raspberry Pi.
2. Download and install the .NET 8 SDK by running the following commands:
   ```bash
   wget https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.sh
   chmod +x dotnet-install.sh
   ./dotnet-install.sh --channel 8.0
   ```
3. Add the SDK path to your shell profile:
   ```bash
   export DOTNET_ROOT=$HOME/.dotnet
   export PATH=$PATH:$HOME/.dotnet:$HOME/.dotnet/tools
   ```
4. Verify the installation by running:
   ```bash
   dotnet --version
   ```

## Step 2: Configure SSH on the Raspberry Pi

1. Ensure that the SSH service is enabled:
   ```bash
   sudo systemctl enable ssh
   sudo systemctl start ssh
   ```
2. Test the SSH connection from your development machine:
   ```bash
   ssh pi@<raspberry_pi_ip_address>
   ```

## Step 3: Install the Visual Studio Remote Debugger on the Raspberry Pi

1. Install the `vsdbg` tool on the Raspberry Pi by running the following command:
   ```bash
   curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l ~/vsdbg
   ```

## Step 4: Configure the Project in Visual Studio Community 2022

1. Open Visual Studio Community 2022 and create a new C# .NET 8 project.
2. Go to the **Properties** of your project.
3. Select the **Debug** section.
4. Add a new **SSH Deployment Profile**.
5. Configure the SSH details:
   - **IP Address**: The IP address of your Raspberry Pi.
   - **Username**: `pi` (or another configured user).
   - **Password**: The user’s password.
   - **Path to `vsdbg`**: Specify the path where `vsdbg` is installed (e.g., `/home/pi/vsdbg`).
6. Save the profile and select it as the debug target.

## Step 5: Remote Debugging

1. With the SSH profile configured, select it as the debug target.
2. Press `F5` to start debugging. Visual Studio will:
   - Deploy your application to the Raspberry Pi.
   - Launch the remote debugger.
   - Connect Visual Studio to the debugging process on the Raspberry Pi.

## Step 6: Communicating with the DS18B20 Sensor

1. Ensure the DS18B20 sensor is correctly connected to the Raspberry Pi (use a 4.7kΩ pull-up resistor between the data line and VCC).
2. In your C# project, add the following packages to interact with the sensor:
   ```bash
   dotnet add package System.Device.Gpio
   dotnet add package Iot.Device.Bindings
   ```
3. Use the following code to read the temperature from the DS18B20:

   ```csharp
   using System;
   using System.Device.Gpio;
   using Iot.Device.OneWire;
   using Iot.Device.Ds18b20;

   class Program
   {
       static void Main(string[] args)
       {
           int pin = 4; // GPIO pin number connected to DS18B20
           using (var bus = new OneWireBus(pin))
           {
               var sensor = new Ds18b20(bus, bus.EnumerateDeviceIds().First());
               double temperature = sensor.Temperature.Celsius;
               Console.WriteLine($"Temperature: {temperature}°C");
           }
       }
   }
   ```

4. Deploy and debug the application by following the previous steps.

## Conclusion

You have now configured remote debugging for a C# .NET 8 application on a Raspberry Pi 4 using Visual Studio Community 2022. This setup allows you to effectively test and debug your code that interacts with a DS18B20 sensor.