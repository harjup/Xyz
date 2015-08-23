using System.Collections.Generic;
using System.Linq;
using Assets.Xyz.Scripts;
using Xunit;

namespace Xyz.Tests
{
    public class SpawnScheduleGeneratorTests
    {
        [Theory]
        [InlineData(0, 10)]
        [InlineData(1, 10)]
        [InlineData(3, 10)]
        [InlineData(5, 10)]
        [InlineData(9, 10)]
        [InlineData(15, 10)]
        [InlineData(20, 10)]
        [InlineData(100, 10)]
        public void GenerateSpawnTimes_ReturnsCountSpawnTimes(int count, int duration)
        {
            var gen = new SpawnScheduleGenerator();
            const int start = 5;
            var actual = gen.GenerateSpawnTimes(start, duration + start, count).Count;
            Assert.Equal(count, actual);
        }

        [Fact]
        public void GenerateSpawnTimes_FirstIsStartValue()
        {
            var gen = new SpawnScheduleGenerator();
            const int start = 5;
            const int duration = 10;
            const int count = 5;
            var actual = gen.GenerateSpawnTimes(start, duration + start, count).First();
            Assert.Equal(start, actual);
        }
    }
}