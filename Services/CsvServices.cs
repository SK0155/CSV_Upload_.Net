using CsvHelper;
using CsvHelper.Configuration;
using CsvUpload.Models;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CsvUpload.Services
{
    public class CsvService
    {
        private readonly Models.CsvContext _dbContext;

        public CsvService(Models.CsvContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Method to handle CSV file upload and database insertion
        public async Task<(bool success, string message)> UploadCsvAsync(IFormFile file)
        {
            // Validate the file
            if (file == null || file.Length == 0)
                return (false, "No file provided.");

            // Validate file extension
            if (Path.GetExtension(file.FileName).ToLower() != ".csv")
                return (false, "Invalid file type. Please upload a CSV file.");

            try
            {
                // Reading the CSV file and parsing records
                using (var streamReader = new StreamReader(file.OpenReadStream()))
                using (var csvReader = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    // Parse the CSV file into a list of records matching CsvRecord model
                    List<CsvRecord> records = csvReader.GetRecords<CsvRecord>().ToList();

                    // Begin transaction for safe database insertion
                    using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            // Insert the records into the database
                            foreach (var record in records)
                            {
                                record.Id = 0; // Assuming Id is an integer. If nullable, set to null.
                            }
                            await _dbContext.CsvRecords.AddRangeAsync(records);
                            await _dbContext.SaveChangesAsync();

                            // Commit the transaction on successful insertion
                            await transaction.CommitAsync();
                            return (true, "CSV uploaded and processed successfully.");
                        }
                        catch (Exception ex)
                        {
                            // Rollback transaction in case of error
                            await transaction.RollbackAsync();

                            return (false, $"Error while saving data to database: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any file reading or CSV parsing errors
                return (false, $"Error processing CSV file: {ex.Message}");
            }
        }
    }
}