using CsvHelper.Configuration;
using CsvHelper;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;

namespace DataReaderConsole
{
    public static class DataWebClient
    {
        private const string _baseApiUrl = "http://localhost:23451";

        public static async Task<bool> UploadRecords()
        {
            string filePath = Path.Combine("data", "data.csv");
            string url = _baseApiUrl + "/upload-records";

            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    var records = csv.GetRecords<Organization>().ToList();
                    using (var client = new HttpClient())
                    {
                        var json = JsonConvert.SerializeObject(records);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        var response = await client.PostAsync(url, content);

                        return response.IsSuccessStatusCode;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
