using KTalks.AzureDataFactory.DevOps.Tests.Helpers;
using KTalks.AzureDataFactory.DevOps.Tests.Helpers.Models;

namespace KTalks.AzureDataFactory.DevOps.Tests.Tests
{
    [Parallelizable]
    public class PL_Source_To_Raw_Given_SourceFile : TestBase
    {
        [Test]
        public async Task NotPresent_Then_Pipeline_Should_End_Failed()
        {
            var tempFileName = DataHelpers.GetTempCsvFileName();
            var outcome = await _adfHelper.RunPipelineRunOutcomeWithFile(Constants.PipelineSourceToRaw, tempFileName);
            Assert.True(outcome == Constants.PipelineStatusFailed);
        }

        [Test]
        public async Task IsValid_Then_Pipeline_Should_End_Succeeded()
        {
            var tempFileName = DataHelpers.GetTempCsvFileName();
            await _blobStorageHelper.UploadCsvRecordsToBlob(DataHelpers.SingleValidEmployeeRawData(), Constants.SourceContainerName, tempFileName);

            var outcome = await _adfHelper.RunPipelineRunOutcomeWithFile(Constants.PipelineSourceToRaw, tempFileName);
            Assert.True(outcome == Constants.PipelineStatusSucceeded);
        }

        [Test]
        public async Task IsValid_Then_Pipeline_Should_CopyFileTo_Raw_Landing()
        {
            var tempFileName = DataHelpers.GetTempCsvFileName();
            var employeeRawData = DataHelpers.SingleValidEmployeeRawData();
            await _blobStorageHelper.UploadCsvRecordsToBlob(employeeRawData, Constants.SourceContainerName, tempFileName);

            var outcome = await _adfHelper.RunPipelineRunOutcomeWithFile(Constants.PipelineSourceToRaw, tempFileName);

            Assert.True(outcome == Constants.PipelineStatusSucceeded);

            var rawContainerEmployeeData = await _blobStorageHelper.GetCsvRecordsFromBlob<EmployeeRaw>(Constants.RawContainerName, $"{Constants.LandingDirectoryName}/{tempFileName}");

            Assert.IsTrue(
                rawContainerEmployeeData.Select(x => x.EmployeeId)
                .SequenceEqual(
                    employeeRawData.Select(x => x.EmployeeId)));
        }
    }
}