using CsvHelper.Configuration.Attributes;

namespace KTalks.AzureDataFactory.DevOps.Tests.Helpers.Models
{
    internal class EmployeeCurated : EmployeeBase
    {
        [Index(1)]
        [Name("Full Name")]
        public string FullName { get; set; } = default!;

        [Index(2)]
        [Name("Department")]
        public string Department { get; set; } = default!;
    }
}
