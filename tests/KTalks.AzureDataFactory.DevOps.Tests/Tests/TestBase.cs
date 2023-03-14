using KTalks.AzureDataFactory.DevOps.Tests.Helpers;

namespace KTalks.AzureDataFactory.DevOps.Tests.Tests
{
    public class TestBase
    {
        protected readonly AdfHelper _adfHelper;
        protected readonly BlobStorageHelper _blobStorageHelper;

        public TestBase()
        {
            _adfHelper = new AdfHelper();
            _blobStorageHelper = new BlobStorageHelper();
        }
    }
}
