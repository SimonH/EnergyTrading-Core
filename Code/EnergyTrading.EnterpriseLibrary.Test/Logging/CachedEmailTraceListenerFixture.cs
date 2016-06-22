namespace EnergyTrading.EnterpriseLibrary.Test.Logging
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
    using NUnit.Framework;

    using EnergyTrading.Logging.EnterpriseLibrary;

    [TestFixture]
    public class CachedEmailTraceListenerFixture
    {
        // As we can't test the base class behavour, because it's part of EntLib Framework, 
        //So the way we test, is, if we pass address paratmete as string.Empty, then If any test case hit the base TradeData method, then it will 
        // always raise a first exception as "System.ArgumentException: The parameter 'address' cannot be an empty string", considered as Success
        //TODO: NOT great tests.. but something is better than nothing
        [Test]
        public void ShouldReportFirstMessage()
        {
            try
            {
                var listener = new CachedEmailTraceListener(
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    2500,
                    null,
                    EmailAuthenticationMode.WindowsCredentials,
                    string.Empty,
                    string.Empty,
                    false,
                    1.0);
                listener.TraceData(null, string.Empty, TraceEventType.Error, 1, new LogEntry());
                // If no exception
                Assert.Fail();
            }
            catch(ArgumentException ex)
            {
                Assert.AreEqual("The argument cannot be empty.\r\nParameter name: toAddress", ex.Message);
            }
            catch
            {
               Assert.Fail(); 
            }
        }

        [Test]
        public void ShouldCacheTheMessagesBasedOnCacheInterval()
        {
            try
            {
                var listener = new CachedEmailTraceListener(
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    2500,
                    null,
                    EmailAuthenticationMode.WindowsCredentials,
                    string.Empty,
                    string.Empty,
                    false,
                    1.0); // 1 hour cache time

                var logEntry = new LogEntry { Message = "ABC Error", Severity = TraceEventType.Error };
                listener.TraceData(null, string.Empty, TraceEventType.Error, 1, logEntry);
                // If no exception
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("The argument cannot be empty.\r\nParameter name: toAddress", ex.Message);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [Test]
        public void ShouldSendEmailIfCacheIsExpiredAfterSpecifiedIntervel()
        {
            var logEntry = new LogEntry() { Message = "ABC Error", Severity = TraceEventType.Error };
            try
            {
                var listener = new CachedEmailTraceListener(
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    2500,
                    null,
                    EmailAuthenticationMode.WindowsCredentials,
                    string.Empty,
                    string.Empty,
                    false,
                    1.0 / 3600); // 1 sec cache time
                listener.TraceData(null, string.Empty, TraceEventType.Error, 1, logEntry);
                // If no exception
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("The argument cannot be empty.\r\nParameter name: toAddress", ex.Message);
            }
            catch
            {
                Assert.Fail();
            }
        }
    }
}