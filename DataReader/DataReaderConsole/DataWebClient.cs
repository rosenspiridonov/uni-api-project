using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

using CsvHelper;
using CsvHelper.Configuration;

using Newtonsoft.Json;

namespace DataReaderConsole
{
    public static class DataWebClient
    {
        private const string _baseApiUrl = "https://localhost:44346";
        private static string _jwtToken;
        private static readonly HttpClient _client = new HttpClient();

        public static async Task<Organization> GetOrganizationAsync(string organizationId)
        {
            var url = _baseApiUrl + "/organization/" + organizationId;

            try
            {
                var organization = await _client.GetFromJsonAsync<Organization>(url);
                return organization;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<bool> UploadRecords()
        {
            var filePath = Path.Combine("data", "data.csv");
            var url = _baseApiUrl + "/organization/upload";

            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    var records = csv.GetRecords<Organization>().ToList();
                    var response = await _client.PostAsJsonAsync(url, records);
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading records: {ex.Message}");
                return false;
            }
        }

        public static async Task DeleteOrganizationAsync(string organizationId)
        {
            var url = _baseApiUrl + "/organization/" + organizationId;

            try
            {
                var response = await _client.DeleteAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Organization not found or deletion failed");
                }
                else
                {
                    Console.WriteLine("Organization deleted");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error in deletion: {ex.Message}");
            }
        }

        public static async Task<bool> AuthenticateAsync(string username, string password)
        {
            var authUrl = _baseApiUrl + "/auth/login";
            try
            {
                var response = await _client.PostAsJsonAsync(authUrl, new { username, password });

                if (response.IsSuccessStatusCode)
                {
                    var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
                    _jwtToken = authResponse?.Token ?? throw new InvalidOperationException("Failed to retrieve token");
                    SetAuthorizationHeader();
                    return true;
                }

                return false;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Authentication failed: {ex.Message}");
                return false;
            }
        }

        private static void SetAuthorizationHeader()
        {
            if (!string.IsNullOrEmpty(_jwtToken))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
            }
        }
    }
}
