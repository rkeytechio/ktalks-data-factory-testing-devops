using CsvHelper.Configuration.Attributes;

namespace KTalks.AzureDataFactory.DevOps.Tests.Helpers.Models
{
    internal class EmployeeRaw : EmployeeBase
    {
        [Index(1)]
        [Name("First Name")]
        public string FirstName { get; set; } = default!;

        [Index(2)]
        [Name("Last Name")]
        public string LastName { get; set; } = default!;

        [Index(3)]
        [Name("Department Code")]
        public string DepartmentCode { get; set; } = default!;
    }
}
