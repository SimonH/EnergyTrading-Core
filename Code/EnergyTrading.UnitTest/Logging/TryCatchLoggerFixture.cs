using System;
using EnergyTrading.Logging;
using Moq;
using NUnit.Framework;

namespace EnergyTrading.UnitTest.Logging
{
    [TestFixture]
    public class TryCatchLoggerFixture
    {
        private Mock<ILogger> calledLogger;
        private Mock<ILogger> exceptionLogger;

        [SetUp]
        public void SetUp()
        {
            calledLogger = new Mock<ILogger>();
            exceptionLogger = new Mock<ILogger>();
        }

        private TryCatchLogger CreateSut()
        {
            return new TryCatchLogger(calledLogger.Object, exceptionLogger.Object);
        }

        private void WhenLoggerThrows(Mock<ILogger> mockLogger)
        {
            mockLogger.Setup(l => l.Debug(It.IsAny<string>())).Throws(new InvalidOperationException());
        }


        [Test]
        public void Construction()
        {
            Assert.That(() => new TryCatchLogger(null, exceptionLogger.Object), Throws.TypeOf<ArgumentNullException>());
            Assert.That(new TryCatchLogger(calledLogger.Object, null), Is.Not.Null);
            Assert.That(CreateSut(), Is.Not.Null);
        }

        [Test]
        public void NormalLoggingDoesNotCallExceptionLogger()
        {
            CreateSut().Debug("Test");
            exceptionLogger.Verify(l => l.Debug(It.IsAny<string>()), Times.Never());
            calledLogger.Verify(l => l.Debug("Test"), Times.Once());
        }

        [Test]
        public void ExceptionInNormalLoggingCallsExceptionLogger()
        {
            WhenLoggerThrows(calledLogger);
            CreateSut().Debug("Test");
            exceptionLogger.Verify(l => l.Debug("Exception trying to call normal Logger process", It.IsAny<Exception>()), Times.Once());
            exceptionLogger.Verify(l => l.Debug("Test"), Times.Once());
        }

        [Test]
        public void NothingHappensWhenExceptionLoggerIsNull()
        {
            WhenLoggerThrows(calledLogger);
            new TryCatchLogger(calledLogger.Object, null).Debug("Test");
        }

        [Test]
        public void ExceptionsFromExceptionLoggerAreIgnored()
        {
            WhenLoggerThrows(calledLogger);
            WhenLoggerThrows(exceptionLogger);
            CreateSut().Debug("Test");
        }
    }
}