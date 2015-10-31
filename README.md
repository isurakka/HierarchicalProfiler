# HierarchicalProfiler
Simple hierarchical profiler. Single threaded usage only. Inspired by [hprof](https://github.com/cmr/hprof).

# Example usage
```
var profiler = new HierarchicalProfiler();
profiler.StartFrame("root");

profiler.Enter("update");

profiler.Enter("input");
// update input
profiler.Leave();

profiler.Enter("physics");
// update physics
profiler.Leave();

profiler.Leave();

profiler.Enter("render");
// render everything
profiler.Leave();

profiler.EndFrame();
```

Inspect `profiler.Nodes` for profiling results. Nodes are laid linearly.