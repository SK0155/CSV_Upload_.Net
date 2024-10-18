using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CsvUpload.Models
{
    public class CsvRecord
    {

        public int Id { get; set; } // Will be auto-generated
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }
}