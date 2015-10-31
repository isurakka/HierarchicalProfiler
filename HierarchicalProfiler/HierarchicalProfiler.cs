using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HierarchicalProfiler
{
    public class HierarchicalProfiler
    {
        public IEnumerable<Node> Nodes => nodes; 

        private readonly Stopwatch sw;
        private readonly List<Node> nodes;
        private readonly Stack<int> depthStartIndices;
        private bool frameStarted;

        public HierarchicalProfiler()
        {
            sw = new Stopwatch();
            nodes = new List<Node>();
            depthStartIndices = new Stack<int>();
        }

        public void StartFrame(string name)
        {
            if (frameStarted) throw new InvalidOperationException("Frame has already been started");

            nodes.Clear();
            sw.Reset();

            frameStarted = true;

            Enter(name);
            sw.Start();
        }

        public void EndFrame()
        {
            if (!frameStarted) throw new InvalidOperationException("Frame hasn't been started");

            sw.Stop();
            Leave();

            frameStarted = false;
        }

        public void Enter(string name)
        {
            if (!frameStarted) throw new InvalidOperationException("Frame hasn't been started");

            nodes.Add(new Node(name, depthStartIndices.Count, sw.Elapsed.Ticks));
            depthStartIndices.Push(nodes.Count - 1);
        }

        public void Leave()
        {
            if (!frameStarted) throw new InvalidOperationException("Frame hasn't been started");
            if (!depthStartIndices.Any()) throw new InvalidOperationException("Can't leave anymore!");

            nodes[depthStartIndices.Pop()].EndTicks = sw.Elapsed.Ticks;
        }

        public class Node
        {
            public string Name { get; }
            public int Depth { get; }
            public long StartTicks { get; }
            public long EndTicks { get; internal set; }

            public Node(string name, int depth, long startTicks)
            {
                Name = name;
                Depth = depth;
                StartTicks = startTicks;
            }
        }
    }
}
