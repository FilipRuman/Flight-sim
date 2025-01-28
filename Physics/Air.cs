using Godot;
using System;

public static class Air
{
    // https://en.wikipedia.org/wiki/Density_of_air
    private const double seaLevelPressure = 101325;/*p0           sea level standard atmospheric pressure in Pa */
    private const double seaLevelTemperature = 288.15f; /*T0      sea level standard temperature in K */
    private const double temperatureLapseRate = 0.0065f;/*L        https://en.wikipedia.org/wiki/Lapse_rate  in K/m */
    private const double idealGasConstant = 8.31446f; /*R          in  J/(mol·K) */
    private const double molarMassOfDryAir = 0.0289652f; /*M       in kg/mol */

    public static Vector3 wind = new(0, 0, 5);

    public static float GetLocalAirDensity(float altitude/* h */)
    {
        // Calculate temperature at altitude
        double temperatureAtAltitude = seaLevelTemperature - (temperatureLapseRate * altitude);

        // Calculate pressure at altitude
        double powX = 1 - (temperatureLapseRate * altitude / seaLevelTemperature);
        double powY = ConstantsManager.gravity * molarMassOfDryAir / (idealGasConstant * temperatureLapseRate);
        double pressureAtAltitude = seaLevelPressure * Math.Pow(powX, powY);

        // Calculate air density using the ideal gas law
        double airDensity = pressureAtAltitude * molarMassOfDryAir / (idealGasConstant * temperatureAtAltitude);

        return (float)airDensity;
    }
}
