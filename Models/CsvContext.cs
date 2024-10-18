using Microsoft.EntityFrameworkCore;

namespace CsvUpload.Models
{
    public class CsvContext : DbContext
    {
        public CsvContext(DbContextOptions<CsvContext> options) : base(options)
        {
        }

        public DbSet<CsvRecord> CsvRecords { get; set; }

    }
}