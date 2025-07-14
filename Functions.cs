using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FFXIVMobile_Companion
{
    internal class Functions
    {
		public static void DownloadFile(string address, string filename)
		{
			using var client = new HttpClient();
			using var s = client.GetStreamAsync(address);
			using var fs = new FileStream(filename, FileMode.Create);
			s.Result.CopyTo(fs);
		}

        public static async Task<Status> GetRemoteStatus()
        {
            try
            {
				using var client = new HttpClient();
				var data = await client.GetStringAsync("http://aida.moe/ffxiv_mobile/status.json");

				var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                };

				var statusContext = new StatusContext(options);

                return JsonSerializer.Deserialize(data, statusContext.Status);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static bool ValidateIPAndPort(string IPAndPort)
        {
            if (string.IsNullOrWhiteSpace(IPAndPort)) { return true; }

            string[] splitValues = IPAndPort.Split('.');
            if (splitValues.Length != 4)
            {
                Console.WriteLine(Color.Red + "Please enter a valid IP address and port." + Color.Default);
                return false;
            }

            if (!IPAndPort.Contains(":")) 
            {
                Console.WriteLine(Color.Red + "Please enter a valid IP address and port." + Color.Default); 
                return false; 
            }

            return true;
        }
        public static string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToUpper();
                }
            }
        }
        public static string TerminalURL(string caption, string url) => $"\u001B]8;;{url}\a{caption}\u001B]8;;\a";
    }
}