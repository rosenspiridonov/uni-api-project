using CsvHelper.Configuration.Attributes;

namespace DataReaderConsole
{
    public class Organization
    {
        public int Index { get; set; }

        [Name("Organization Id")]
        public string OrganizationId { get; set; }

        public string Name { get; set; }

        public string Website { get; set; }

        public string Country { get; set; }

        public string Description { get; set; }

        public int Founded { get; set; }

        public string Industry { get; set; }

        [Name("Number of employees")]
        public int NumberOfEmployees { get; set; }
    }
}
