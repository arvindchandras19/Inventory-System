using System;
using System.Linq;
using System.Web;
using System.Net;

namespace Inventory.Class
{
    public static class GetIPAddress
    {
        // RSuresh
        public static string GetDeviceAddress()
        {
            try
            {
                var userHostAddress = HttpContext.Current.Request.UserHostAddress;

                // Attempt to parse.  If it fails, we catch below and return "0.0.0.0"
                // Could use TryParse instead, but I wanted to catch all exceptions
                IPAddress.Parse(userHostAddress);

                var xForwardedFor = HttpContext.Current.Request.ServerVariables["X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(xForwardedFor))
                    return userHostAddress;

                // Get a list of public ip addresses in the X_FORWARDED_FOR variable
                var publicForwardingIps = xForwardedFor.Split(',').Where(ip => !IsPrivateIpAddress(ip)).ToList();

                // If we found any, return the last one, otherwise return the user host address
                return publicForwardingIps.Any() ? publicForwardingIps.Last() : userHostAddress;
            }
            catch (Exception)
            {
                // Always return all zeroes for any failure (my calling code expects it)
                return "0.0.0.0";
            }
        }
        public static string GetMachineAddress()
        {
            try
            {
                var userHostAddress = HttpContext.Current.Request.UserHostAddress;

                // Attempt to parse.  If it fails, we catch below and return "0.0.0.0"
                // Could use TryParse instead, but I wanted to catch all exceptions
                IPAddress.Parse(userHostAddress);

                var xForwardedFor = HttpContext.Current.Request.ServerVariables["X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(xForwardedFor))
                    return userHostAddress;

                // Get a list of public ip addresses in the X_FORWARDED_FOR variable
                var publicForwardingIps = xForwardedFor.Split(',').Where(ip => !IsPrivateIpAddress(ip)).ToList();

                // If we found any, return the last one, otherwise return the user host address
                return publicForwardingIps.Any() ? publicForwardingIps.Last() : userHostAddress;
            }
            catch (Exception)
            {
                // Always return all zeroes for any failure (my calling code expects it)
                return "0.0.0.0";
            }
        }

        private static bool IsPrivateIpAddress(string ipAddress)
        {
            // http://en.wikipedia.org/wiki/Private_network
            // Private IP Addresses are: 
            //  24-bit block: 10.0.0.0 through 10.255.255.255
            //  20-bit block: 172.16.0.0 through 172.31.255.255
            //  16-bit block: 192.168.0.0 through 192.168.255.255
            //  Link-local addresses: 169.254.0.0 through 169.254.255.255 (http://en.wikipedia.org/wiki/Link-local_address)

            var ip = IPAddress.Parse(ipAddress);
            var octets = ip.GetAddressBytes();

            var is24BitBlock = octets[0] == 10;
            if (is24BitBlock) return true; // Return to prevent further processing

            var is20BitBlock = octets[0] == 172 && octets[1] >= 16 && octets[1] <= 31;
            if (is20BitBlock) return true; // Return to prevent further processing

            var is16BitBlock = octets[0] == 192 && octets[1] == 168;
            if (is16BitBlock) return true; // Return to prevent further processing

            var isLinkLocalAddress = octets[0] == 169 && octets[1] == 254;
            return isLinkLocalAddress;
        }


        /// <summary>
        /// Resolves the public IP address and returns it as a string.
        /// </summary>
        /// <param name="useHttps">
        /// Specifies whether to use HTTPS to talk to ipify.org (defaults to
        /// <b>false</b> if omitted).
        /// </param>
        /// <returns>
        /// A string containing the IP address, or an empty string if an error is
        /// encountered.
        /// </returns>
        public static string GetPublicAddress(bool useHttps = false)
        {
            var endpoint = useHttps ? "https://api.ipify.org" : "http://api.ipify.org";
            WebClient client = new WebClient();
            try
            {
                return client.DownloadString(endpoint);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Resolves the public IP address and returns it as an instance of
        /// <see cref="IPAddress" />.
        /// </summary>
        /// <param name="useHttps">
        /// Specifies whether to use HTTPS to talk to ipify.org (defaults to
        /// <b>false</b> if omitted).
        /// </param>
        /// <returns>
        /// An instance of <see cref="IPAddress" />. If an error occures, then
        /// <see cref="IPAddress.None" /> is returned.
        /// encountered.
        /// </returns>
        public static IPAddress GetPublicIPAddress(bool useHttps = false)
        {
            string address = GetPublicAddress(useHttps);
            IPAddress ipAddress;
            if (!IPAddress.TryParse(address, out ipAddress))
            {
                return IPAddress.None;
            }
            return ipAddress;
        }
    }
}