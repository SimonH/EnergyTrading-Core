﻿namespace EnergyTrading.UnitTest.FileProcessing
{
    using System;

    using EnergyTrading.FileProcessing;

    using NUnit.Framework;

    [TestFixture]
    public class FileProcessorTests
    {
        [Test]
        public void TestSuccessFileNameRetainsDirectoryStructure()
        {
            var helper = new FileProcessorHelper(new FileProcessorEndpoint { DropPath = @"c:\a", FailurePath = @"c:\b", ScavengeInterval= new TimeSpan(0, 0, 2), SuccessPath = @"c:\d" }, new FileHandler(), new NullPostProcessor());
            var result = helper.GenerateSuccessFileName(@"c:\a\e\f\g.txt");
            Assert.IsTrue(result.StartsWith(@"c:\d\e\f\g.txt"));
        }

        [Test]
        public void TestErrorFileNameRetainsDirectoryStructure()
        {
            var helper = new FileProcessorHelper(new FileProcessorEndpoint { DropPath = @"c:\a", FailurePath = @"c:\b", ScavengeInterval = new TimeSpan(0, 0, 2), SuccessPath = @"c:\d" }, new FileHandler(), new NullPostProcessor());
            var result = helper.GenerateErrorFileName(@"c:\a\e\f\g.txt");
            Assert.IsTrue(result.StartsWith(@"c:\b\e\f\g.txt"));
        }

        [Test]
        public void TestFilePathVariableInSuccessPath()
        {
            var helper = new FileProcessorHelper(new FileProcessorEndpoint { DropPath = @"c:\a", FailurePath = @"c:\b", ScavengeInterval = new TimeSpan(0, 0, 2), SuccessPath = @"%filepath%\d" }, new FileHandler(), new NullPostProcessor());
            var result = helper.GenerateSuccessFileName(@"c:\a\e\f\g.txt");
            Assert.IsTrue(result.StartsWith(@"c:\a\e\f\d\g.txt"));
        }

        [Test]
        public void TestFilePathVariableInFailurePath()
        {
            var helper = new FileProcessorHelper(new FileProcessorEndpoint { DropPath = @"c:\a", FailurePath = @"%filepath%\b", ScavengeInterval = new TimeSpan(0, 0, 2), SuccessPath = @"C:\d" }, new FileHandler(), new NullPostProcessor());
            var result = helper.GenerateErrorFileName(@"c:\a\e\f\g.txt");
            Assert.IsTrue(result.StartsWith(@"c:\a\e\f\b\g.txt"));
        }
    }
}
