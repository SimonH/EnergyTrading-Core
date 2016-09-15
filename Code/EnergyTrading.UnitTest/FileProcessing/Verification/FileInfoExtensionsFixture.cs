namespace EnergyTrading.UnitTest.FileProcessing.Verification
{
    using System.Collections.Specialized;
    using System.IO;

    using EnergyTrading.Configuration;
    using EnergyTrading.FileProcessing.Verification;

    using NUnit.Framework;

    using Moq;

    [TestFixture]
    public class FileInfoExtensionsFixture
    {
        private readonly string prefix = "testPrefix";
        private Mock<IConfigurationManager> mockConfigManager;

        [SetUp]
        public void TestInitialize()
        {
            this.mockConfigManager = new Mock<IConfigurationManager>();
            this.mockConfigManager.Setup(x => x.AppSettings).Returns(new NameValueCollection { { ConfigurationManagerExtensions.VerificationPrefixAppSetting, this.prefix }});
        }

        [Test]
        public void IsTestFileReturnsFalseIfFileInfoIsNull()
        {
            var candidate = ((FileInfo)null).IsTestFile(this.mockConfigManager.Object);
            Assert.IsFalse(candidate);
        }

        [Test]
        public void IsTestFileReturnsFalseIfFileNameDoesNotStartWithPrefix()
        {
            var candidate = new FileInfo("someOtherFileName.txt").IsTestFile(this.mockConfigManager.Object);
            Assert.IsFalse(candidate);
        }

        [Test]
        public void IsTestFileReturnsTrueIfFileNameStartsWithPrefix()
        {
            var candidate = new FileInfo(this.prefix + "someOtherFileName.txt").IsTestFile(this.mockConfigManager.Object);
            Assert.IsTrue(candidate);
        }
        [Test]
        public void IsTestFileReturnsFalseIfStringIsNull()
        {
            var candidate = ((string)null).IsTestFile(this.mockConfigManager.Object);
            Assert.IsFalse(candidate);
        }

        [Test]
        public void IsTestFileReturnsFalseIfStringDoesNotStartWithPrefix()
        {
            var candidate = "someOtherFileName.txt".IsTestFile(this.mockConfigManager.Object);
            Assert.IsFalse(candidate);
        }

        [Test]
        public void IsTestFileReturnsTrueIfStringStartsWithPrefix()
        {
            var candidate = (this.prefix + "someOtherFileName.txt").IsTestFile(this.mockConfigManager.Object);
            Assert.IsTrue(candidate);
        }

        [Test]
        [TestCase(null, null)]
        [TestCase("", null)]
        [TestCase("    ", null)]
        [TestCase("filenamewithoutId.txt", null)]
        [TestCase("filenamewithId_uniqueId.txt", "uniqueId")]
        public void GetTestIdFromFileName(string fileName, string expected)
        {
            var candidate = fileName.GetTestIdFromFileName();
            Assert.That(candidate, Is.EqualTo(expected));
        }
    }
}