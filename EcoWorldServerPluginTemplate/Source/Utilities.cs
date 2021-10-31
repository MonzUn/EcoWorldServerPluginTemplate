using Eco.Gameplay.Systems.TextLinks;
using Eco.Shared.Math;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

namespace Eco.Plugins.EcoWorldServerPluginTemplate
{
    public static class Utilities
    {
        // Eco tag matching regex: Match all characters that are used to create HTML style tags
        private static readonly Regex HTMLTagRegex = new Regex("<[^>]*>");

        public static string StripTags(string toStrip)
        {
            if (toStrip == null)
                return string.Empty;

            return HTMLTagRegex.Replace(toStrip, string.Empty);
        }

        public static Tuple<string, string> MakeTuple(string key, string value)
        {
            return new Tuple<string, string>(key, value);
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            time = time.AddSeconds(unixTimeStamp).ToLocalTime();
            return time;
        }

        public static void StopAndDestroyTimer(ref Timer timer)
        {
            if (timer != null)
            {
                timer.Change(Timeout.Infinite, Timeout.Infinite);
                timer = null;
            }
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static string GetMarkedUpPosition(Vector3i postition) { return new Vector3Tooltip(postition).UILink(); }
    }
}
