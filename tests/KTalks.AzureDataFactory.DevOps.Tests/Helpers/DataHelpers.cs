using KTalks.AzureDataFactory.DevOps.Tests.Helpers.Models;

namespace KTalks.AzureDataFactory.DevOps.Tests.Helpers
{
    internal static class DataHelpers
    {
        public static List<EmployeeRaw> EmptyEmployeeRawData()
        {
            return new List<EmployeeRaw>();
        }

        public static List<EmployeeRaw> SingleValidEmployeeRawData()
        {
            return new List<EmployeeRaw>()
            {
                new() { EmployeeId = "EMP-9999", FirstName = "John", LastName = "Smith", DepartmentCode = "MGMT" }
            };
        }

        public static List<EmployeeRaw> SingleEmployeeRawDataNoDepartment()
        {
            return new List<EmployeeRaw>()
            {
                new() { EmployeeId = "EMP-9999", FirstName = "John", LastName = "Smith" }
            };
        }

        public static List<EmployeeRaw> SingleEmployeeRawDataNoNames()
        {
            return new List<EmployeeRaw>()
            {
                new() { EmployeeId = "EMP-9999", DepartmentCode = "MGMT" }
            };
        }

        public static List<EmployeeRaw> SingleEmployeeRawDataNoFirstName()
        {
            return new List<EmployeeRaw>()
            {
                new() { EmployeeId = "EMP-9999", LastName = "Smith", DepartmentCode = "MGMT" }
            };
        }

        public static List<EmployeeRaw> SingleEmployeeRawDataNoLastName()
        {
            return new List<EmployeeRaw>()
            {
                new() { EmployeeId = "EMP-9999", FirstName = "John", DepartmentCode = "MGMT" }
            };
        }

        public static string GetTempCsvFileName()
        {
            return $"{Path.GetRandomFileName()}.csv";
        }

        public static string GetLandingFilePath(string fileName) =>
            $"{Constants.LandingDirectoryName}/{fileName}";
    }
}
