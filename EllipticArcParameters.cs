using Esri.ArcGISRuntime.Geometry;
using System;

public class EllipticArcParameters
{
    public double SemiMajorAxis { get; set; }
    public double SemiMinorAxis { get; set; }
    public bool IsMinor { get; set; } = true;
    public bool IsCounterClockwise { get; set; } = true;
    public double RotationAngle { get; set; }
    public double CentralAngle { get; set; } = 45; // This is a simplified assumption

    public static EllipticArcParameters CalculateArcParameters(MapPoint startPoint, MapPoint endPoint)
    {
        var parameters = new EllipticArcParameters();

        // Calculate the geodesic distance between the points
        var distance = GeometryEngine.GeodesicDistance(startPoint, endPoint);

        // Set the semi-major axis as the geodesic distance
        parameters.SemiMajorAxis = distance / 2; // Using half the distance as an example

        // Set the semi-minor axis as a fixed ratio of the semi-major axis for simplicity
        parameters.SemiMinorAxis = parameters.SemiMajorAxis / 2;

        // Determine the initial bearing for the rotation angle
        parameters.RotationAngle = CalculateInitialBearing(startPoint, endPoint);

        // The isMinor and isCounterClockwise flags are set based on simplified assumptions
        // and might need adjustments based on the specific application or visual requirements

        return parameters;
    }

    private static double CalculateInitialBearing(MapPoint start, MapPoint end)
    {
        var lon1 = DegreesToRadians(start.X);
        var lon2 = DegreesToRadians(end.X);
        var lat1 = DegreesToRadians(start.Y);
        var lat2 = DegreesToRadians(end.Y);

        var dLon = lon2 - lon1;
        var y = Math.Sin(dLon) * Math.Cos(lat2);
        var x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
        var brng = Math.Atan2(y, x);

        // Convert radians to degrees (as bearing: 0 = north, +east), normalize to 0-360
        return (RadiansToDegrees(brng) + 360) % 360;
    }

    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }

    private static double RadiansToDegrees(double radians)
    {
        return radians * 180 / Math.PI;
    }
}
