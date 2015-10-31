using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace HierarchicalProfiler.Tests
{
    public class HierarchicalProfilerTests
    {
        [Fact]
        public void BasicUsage()
        {
            var profiler = new HierarchicalProfiler();
            basicUsage(profiler);
            basicUsage(profiler);
        }

        private void basicUsage(HierarchicalProfiler profiler)
        {
            profiler.StartFrame("root");
            Thread.Sleep(10);

            profiler.Enter("depth1 1");
            Thread.Sleep(10);
            profiler.Leave();

            profiler.Enter("depth1 2");
            Thread.Sleep(10);
            profiler.Enter("depth2 1");
            Thread.Sleep(10);
            profiler.Leave();
            Thread.Sleep(10);
            profiler.Leave();

            Thread.Sleep(10);
            profiler.EndFrame();

            var nodes = profiler.Nodes.ToList();
            Assert.Equal(4, nodes.Count);
            Assert.Equal("root", nodes[0].Name);
            var startTime = nodes[0].StartTicks;
            var endTime = nodes[0].EndTicks;
            Assert.True(endTime > startTime);
            Assert.True(endTime - startTime > TimeSpan.FromMilliseconds(59).Ticks);

            Assert.Equal("depth1 1", nodes[1].Name);
            Assert.Equal("depth2 1", nodes[3].Name);
            Assert.True(nodes[3].EndTicks > nodes[2].StartTicks);
            Assert.True(nodes[3].StartTicks > nodes[2].StartTicks);
            Assert.True(nodes[2].EndTicks > nodes[3].EndTicks);
        }
    }
}
