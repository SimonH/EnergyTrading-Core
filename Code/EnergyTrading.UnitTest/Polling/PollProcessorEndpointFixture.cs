namespace EnergyTrading.UnitTest.Polling
{
    using System;

    using EnergyTrading.Polling;

    using NUnit.Framework;

    [TestFixture]
    public class PollProcessorEndpointFixture
    {
        [Test]
        public void ValidateNullName()
        {
            Assert.Throws<NotSupportedException>(() => new PollProcessorEndpoint().Validate());
        }

        [Test]
        public void ValidateEmptyName()
        {
            Assert.Throws<NotSupportedException>(() => new PollProcessorEndpoint { Name = string.Empty }.Validate());
        }

        [Test]
        public void ValidateZeroInterval()
        {
            Assert.Throws<NotSupportedException>(() => new PollProcessorEndpoint { Name = "test", IntervalSecs = 0 }.Validate());
        }

        [Test]
        public void ValidateNegativeInterval()
        {
            Assert.Throws<NotSupportedException>(() => new PollProcessorEndpoint { Name = "test", IntervalSecs = -1 }.Validate());
        }

        [Test]
        public void ValidateInvalidHandler()
        {
            Assert.Throws<NotSupportedException>(() => new PollProcessorEndpoint { Name = "test", IntervalSecs = -1 }.Validate());
        }

        [Test]
        public void ValidateValidEndpoint()
        {
            new PollProcessorEndpoint { Name = "test", IntervalSecs = 1, Handler = typeof(PollerImpl), Workers = 1 }.Validate();
        }

        [Test]
        public void ValidateNegativeWorkers()
        {
            Assert.Throws<NotSupportedException>(() => new PollProcessorEndpoint() {Name = "test", IntervalSecs = 1, Handler = typeof (PollerImpl), Workers = -1}
                .Validate());
        }
    }
}
