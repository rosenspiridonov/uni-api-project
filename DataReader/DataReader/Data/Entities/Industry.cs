using System.ComponentModel.DataAnnotations;

namespace DataReader.Data.Entities
{
    public class Industry
    {
        public Industry()
        {
            Organizations = new HashSet<Organization>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Organization> Organizations { get; set; }
    }
}
