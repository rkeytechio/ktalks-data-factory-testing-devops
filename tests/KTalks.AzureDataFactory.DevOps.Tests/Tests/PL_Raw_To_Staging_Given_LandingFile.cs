using KTalks.AzureDataFactory.DevOps.Tests.Helpers;
using KTalks.AzureDataFactory.DevOps.Tests.Helpers.Models;

namespace KTalks.AzureDataFactory.DevOps.Tests.Tests
{
    [Parallelizable]
    internal class PL_Raw_To_Staging_Given_LandingFile : TestBase
    {
        [Test]
        public async Task NotPresent_Then_Pipeline_Should_End_Failed()
        {
            var tempFileName = DataHelpers.GetTempCsvFileName();
            var outcome = await _adfHelper.RunPipelineRunOutcomeWithFile(Constants.PipelineRawToStaging, tempFileName);
            Assert.True(outcome == Constants.PipelineStatusFailed);
        }

        [Test]
        public async Task IsValid_Then_Pipeline_Should_End_Succeeded()
        {
            var tempFileName = DataHelpers.GetTempCsvFileName();
            await _blobStorageHelper.UploadCsvRecordsToBlob(DataHelpers.SingleValidEmployeeRawData(), Constants.RawContainerName, DataHelpers.GetLandingFilePath(tempFileName));

            var outcome = await _adfHelper.RunPipelineRunOutcomeWithFile(Constants.PipelineRawToStaging, tempFileName);
            Assert.True(outcome == Constants.PipelineStatusSucceeded);
        }

        [Test]
        public async Task IsValid_Then_Pipeline_Should_CopyFileTo_Staging_Landing()
        {
            var tempFileName = DataHelpers.GetTempCsvFileName();
            var employeeRawData = DataHelpers.SingleValidEmployeeRawData();
            await _blobStorageHelper.UploadCsvRecordsToBlob(employeeRawData, Constants.RawContainerName, DataHelpers.GetLandingFilePath(tempFileName));

            var outcome = await _adfHelper.RunPipelineRunOutcomeWithFile(Constants.PipelineRawToStaging, tempFileName);

            Assert.True(outcome == Constants.PipelineStatusSucceeded);

            var rawContainerEmployeeData = await _blobStorageHelper.GetCsvRecordsFromBlob<EmployeeRaw>(Constants.StagingContainerName, DataHelpers.GetLandingFilePath(tempFileName));

            Assert.IsTrue(
                rawContainerEmployeeData.Select(x => x.EmployeeId)
                    .SequenceEqual(
                        employeeRawData.Select(x => x.EmployeeId)));
        }

        [Test]
        public async Task HasDepartmentMissing_Then_Pipeline_Should_RemoveRecord_From_Staging_Landing()
        {
            var tempFileName = DataHelpers.GetTempCsvFileName();
            var employeeRawData = DataHelpers.SingleEmployeeRawDataNoDepartment();
            await _blobStorageHelper.UploadCsvRecordsToBlob(employeeRawData, Constants.RawContainerName, DataHelpers.GetLandingFilePath(tempFileName));

            var outcome = await _adfHelper.RunPipelineRunOutcomeWithFile(Constants.PipelineRawToStaging, tempFileName);

            Assert.True(outcome == Constants.PipelineStatusSucceeded);

            var rawContainerEmployeeData = await _blobStorageHelper.GetCsvRecordsFromBlob<EmployeeRaw>(Constants.StagingContainerName, DataHelpers.GetLandingFilePath(tempFileName));

            Assert.IsTrue(!rawContainerEmployeeData.Any());
        }

        [Test]
        public async Task HasFirstNameMissingRecord_Then_Pipeline_Should_CopyFileTo_Staging_Landing()
        {
            var tempFileName = DataHelpers.GetTempCsvFileName();
            var employeeRawData = DataHelpers.SingleEmployeeRawDataNoFirstName();
            await _blobStorageHelper.UploadCsvRecordsToBlob(employeeRawData, Constants.RawContainerName, DataHelpers.GetLandingFilePath(tempFileName));

            var outcome = await _adfHelper.RunPipelineRunOutcomeWithFile(Constants.PipelineRawToStaging, tempFileName);

            Assert.True(outcome == Constants.PipelineStatusSucceeded);

            var rawContainerEmployeeData = await _blobStorageHelper.GetCsvRecordsFromBlob<EmployeeRaw>(Constants.StagingContainerName, DataHelpers.GetLandingFilePath(tempFileName));

            Assert.IsTrue(
                rawContainerEmployeeData.Select(x => x.EmployeeId)
                    .SequenceEqual(
                        employeeRawData.Select(x => x.EmployeeId)));
        }

        [Test]
        public async Task HasLastNameMissingRecord_Then_Pipeline_Should_CopyFileTo_Staging_Landing()
        {
            var tempFileName = DataHelpers.GetTempCsvFileName();
            var employeeRawData = DataHelpers.SingleEmployeeRawDataNoLastName();
            await _blobStorageHelper.UploadCsvRecordsToBlob(employeeRawData, Constants.RawContainerName, DataHelpers.GetLandingFilePath(tempFileName));

            var outcome = await _adfHelper.RunPipelineRunOutcomeWithFile(Constants.PipelineRawToStaging, tempFileName);

            Assert.True(outcome == Constants.PipelineStatusSucceeded);

            var rawContainerEmployeeData = await _blobStorageHelper.GetCsvRecordsFromBlob<EmployeeRaw>(Constants.StagingContainerName, DataHelpers.GetLandingFilePath(tempFileName));

            Assert.IsTrue(
                rawContainerEmployeeData.Select(x => x.EmployeeId)
                    .SequenceEqual(
                        employeeRawData.Select(x => x.EmployeeId)));
        }

        [Test]
        public async Task HasNameMissingRecord_Then_Pipeline_Should_RemoveRecord_From_Staging_Landing()
        {
            var tempFileName = DataHelpers.GetTempCsvFileName();
            var employeeRawData = DataHelpers.SingleEmployeeRawDataNoNames();
            await _blobStorageHelper.UploadCsvRecordsToBlob(employeeRawData, Constants.RawContainerName, DataHelpers.GetLandingFilePath(tempFileName));

            var outcome = await _adfHelper.RunPipelineRunOutcomeWithFile(Constants.PipelineRawToStaging, tempFileName);

            Assert.True(outcome == Constants.PipelineStatusSucceeded);

            var rawContainerEmployeeData = await _blobStorageHelper.GetCsvRecordsFromBlob<EmployeeRaw>(Constants.StagingContainerName, DataHelpers.GetLandingFilePath(tempFileName));

            Assert.IsTrue(!rawContainerEmployeeData.Any());
        }
    }
}
