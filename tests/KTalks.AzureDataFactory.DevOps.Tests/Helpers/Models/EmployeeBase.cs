using CsvHelper.Configuration.Attributes;

namespace KTalks.AzureDataFactory.DevOps.Tests.Helpers.Models
{
    internal class EmployeeBase
    {
        [Index(0)]
        [Name("Employee Id")]
        public string EmployeeId { get; set; } = default!;
    }
}
