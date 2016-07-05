using System.Collections;
using System.Linq;
using EnergyTrading.Logging;
using NUnit.Framework;

namespace EnergyTrading.UnitTest.Logging
{
    [TestFixture]
    public class FilteredLoggerFixture
    {
        [Test]
        public void Construction()
        {
            Assert.That(() => new FilteredLogger(null, LogFilterLevel.None), Throws.ArgumentNullException);
            var candidate = new FilteredLogger(new NullLogger(), LogFilterLevel.None);
            Assert.That(candidate, Is.Not.Null);
        }

        public static IEnumerable DebugEnabledCases
        {
            get
            {
                yield return new TestCaseData(LogFilterLevel.None).Returns(true).SetName("DebugEnabledFilterNone");
                yield return new TestCaseData(LogFilterLevel.All).Returns(false).SetName("DebugEnabledFilterAll");
                yield return new TestCaseData(LogFilterLevel.Debug).Returns(false).SetName("DebugEnabledFilterDebugOnly");
                yield return new TestCaseData(LogFilterLevel.Warn).Returns(true).SetName("DebugEnabledFilterWarnOnly");
                yield return new TestCaseData(LogFilterLevel.Warn | LogFilterLevel.Debug).Returns(false).SetName("DebugEnabledFilterDebugAndWarn");
                yield return new TestCaseData(LogFilterLevel.BelowWarn).Returns(false).SetName("DebugEnabledFilterBelowWarn");
                yield return new TestCaseData(LogFilterLevel.BelowError).Returns(false).SetName("DebugEnabledFilterBelowError");
                yield return new TestCaseData(LogFilterLevel.BelowFatal).Returns(false).SetName("DebugEnabledFilterBelowFatal");
            }
        }

        [Test]
        [TestCaseSource("DebugEnabledCases")]
        public bool DebugEnabled(LogFilterLevel level)
        {
            return new FilteredLogger(new NullLogger(), level).IsDebugEnabled;
        }

        public static IEnumerable DebugCases
        {
            get
            {
                yield return new TestCaseData(LogFilterLevel.None, "text", 1).SetName("DebugFilterNone");
                yield return new TestCaseData(LogFilterLevel.All, "text", 0).SetName("DebugFilterAll");
                yield return new TestCaseData(LogFilterLevel.Debug, "text", 0).SetName("DebugFilterDebugOnly");
                yield return new TestCaseData(LogFilterLevel.Warn, "text", 1).SetName("DebugFilterWarnOnly");
                yield return new TestCaseData(LogFilterLevel.Warn | LogFilterLevel.Debug, "text", 0).SetName("DebugFilterDebugAndWarn");
                yield return new TestCaseData(LogFilterLevel.BelowWarn, "text", 0).SetName("DebugFilterBelowWarn");
                yield return new TestCaseData(LogFilterLevel.BelowError, "text", 0).SetName("DebugFilterBelowError");
                yield return new TestCaseData(LogFilterLevel.BelowFatal, "text", 0).SetName("DebugFilterBelowFatal");
            }
        }

        [Test]
        [TestCaseSource("DebugCases")]
        public void Debug(LogFilterLevel level, string message, int expectedMessageCount)
        {
            var testLogger = new testLogger();
            var logger = new FilteredLogger(testLogger, level);
            logger.Debug(message);
            Assert.That(testLogger.Messages.Count(), Is.EqualTo(expectedMessageCount));
            if (expectedMessageCount > 0)
            {
                Assert.That(testLogger.Messages.First(), Is.EqualTo(message));
            }
        }
    }
}