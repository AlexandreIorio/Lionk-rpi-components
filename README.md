# Lionk Plugin - DS18B20 for Raspberry Pi

## Overview

This repository contains a plugin for the **Lionk** framework designed to interface with DS18B20 temperature sensors on a Raspberry Pi. The plugin allows for seamless integration of DS18B20 sensors into the Lionk ecosystem, enabling real-time temperature monitoring and data processing within your Lionk-based application.

## Features

- **Real-time Temperature Monitoring**: Capture temperature data from DS18B20 sensors connected to a Raspberry Pi.
- **Seamless Integration**: Easily integrate with the Lionk core framework to extend functionality.
- **Configurable Settings**: Adjust sensor reading intervals and thresholds within the Lionk web interface.
## Requirements

- **Hardware**: Raspberry Pi (any model) with DS18B20 temperature sensors.
- **Software**: 
  - .NET 8.0 SDK
  - Lionk Application (ensure it's correctly installed and configured)

## Installation

1. **Connect the DS18B20 Sensor** to your Raspberry Pi's GPIO pins according to the sensor's datasheet.
2. **Clone this repository** to your Raspberry Pi:
   ```bash
   git clone https://github.com/Lionk-Framework/Lionk-plugin-rpi-ds18b20.git
   ```
3. **Build the plugin**:
   ```bash
   cd Lionk-plugin-rpi-ds18b20
   dotnet build
   ```
4. **Deploy the Plugin**:
   - Copy the generated DLL to the plugins directory of your Lionk installation.
   - Restart the Lionk service to load the new plugin.

## Configuration

1. Access the Lionk web interface.
2. Navigate to the Plugins section and locate the DS18B20 plugin.
3. Configure the sensor reading intervals.
4. Save the configuration and start monitoring.

## Usage

Once installed and configured, the DS18B20 plugin will begin capturing temperature data according to your settings. Data can be viewed and managed through the Lionk web dashboard, and alerts will be triggered based on the thresholds you've set.

## Contributing

Contributions are welcome! Please fork this repository and submit a pull request for any enhancements or bug fixes.

## Support

For issues or questions, please open an issue on the GitHub repository or contact the maintainers directly.