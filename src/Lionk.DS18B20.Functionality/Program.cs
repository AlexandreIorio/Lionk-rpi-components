using Lionk.DS18B20;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("DS18B20 Sensors - Starting");
        List<string> list = DS18B20.ConnectedSensors();
        List<DS18B20> sensors = new List<DS18B20>();

        int i = 0;
        foreach (string sensor in list)
        {
            Console.WriteLine($"Construct DS18B20 with: {sensor}");
            DS18B20 dS18B20 = new DS18B20($"Sensor {i}", sensor);
            sensors.Add(dS18B20);
        }

        foreach (DS18B20 sensor in sensors)
        {
            Console.WriteLine($"Reading sensor: {sensor.Address}");
            sensor.ReadSensor();
        }

        foreach (DS18B20 sensor in sensors)
        {
            Console.WriteLine(sensor.GetSensorStatus() + "\n");
        }

        Console.WriteLine("DS18B20 Sensors - Ending");
    }
}
