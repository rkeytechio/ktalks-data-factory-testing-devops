namespace KTalks.AzureDataFactory.DevOps.Tests.Helpers
{
    internal class Constants
    {
        // Data Lake Containers
        public const string SourceContainerName = "source";
        public const string RawContainerName = "raw";
        public const string StagingContainerName = "staging";
        

        // Data Lake Directories
        public const string LandingDirectoryName = "landing";

        // Pipeline Names
        public const string PipelineSourceToRaw = "PL_Source_To_Raw";
        public const string PipelineRawToStaging = "PL_Raw_To_Staging";

        // Pipeline Status
        public const string PipelineStatusSucceeded = "Succeeded";

        public const string PipelineStatusFailed = "Failed";
    }
}
