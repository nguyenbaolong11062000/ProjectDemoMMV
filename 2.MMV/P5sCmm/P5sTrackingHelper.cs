using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace P5sCmm
{

 public static class P5sTrackingHelper
{
    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }

    public static double CalculateDistance(P5sLocation location1, P5sLocation location2)
    {

        try
        {
            double circumference = 40000.0; // Earth's circumference at the equator in km
            double distance = 0.0;

            //Calculate radians
            double latitude1Rad = DegreesToRadians(location1.Latitude);
            double longitude1Rad = DegreesToRadians(location1.Longitude);
            double latititude2Rad = DegreesToRadians(location2.Latitude);
            double longitude2Rad = DegreesToRadians(location2.Longitude);

            double logitudeDiff = Math.Abs(longitude1Rad - longitude2Rad);

            if (logitudeDiff > Math.PI)
            {
                logitudeDiff = 2.0 * Math.PI - logitudeDiff;
            }

            double angleCalculation =
                Math.Acos(
                  Math.Sin(latititude2Rad) * Math.Sin(latitude1Rad) +
                  Math.Cos(latititude2Rad) * Math.Cos(latitude1Rad) * Math.Cos(logitudeDiff));

            if (double.IsNaN(angleCalculation))
                return 0.0;

            distance = circumference * angleCalculation / (2.0 * Math.PI);

            return distance;
        }
        catch (Exception)
        {
            return 0.0; 
        }
      
    }

    public static double CalculateDistance(params P5sLocation[] locations)
    {

        double totalDistance = 0.0;

        try
        {
            for (int i = 0; i < locations.Length - 1; i++)
            {
                P5sLocation current = locations[i];
                P5sLocation next = locations[i + 1];

                totalDistance += CalculateDistance(current, next);
            }

            return totalDistance;
        }
        catch (Exception)
        {
            return totalDistance;
        }
       
    }

    
    }
    public class P5sLocation
    {
        public double Latitude = 0;
        public double Longitude = 0;
        public P5sLocation(String str)
        {
            try
            {

                String[] tmp = str.Split(new char[] { ',' });

                this.Latitude = double.Parse(tmp[0].ToString());
                this.Longitude = double.Parse(tmp[1].ToString());
            }
            catch (Exception)
            {
                
            }

        }
    }
}
