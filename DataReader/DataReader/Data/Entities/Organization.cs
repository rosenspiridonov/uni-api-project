using System.ComponentModel.DataAnnotations;

namespace DataReader.Data.Entities
{
    public class Organization
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Website { get; set; }

        public Country Country { get; set; }
        public int CountryId { get; set; }

        [Required]
        public string Description { get; set; }

        public int Founded { get; set; }

        public Industry Industry { get; set; }
        public int IndustryId { get; set; }

        public int NumberOfEmployees { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
