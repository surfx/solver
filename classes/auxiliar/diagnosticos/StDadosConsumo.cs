namespace classes.auxiliar.diagnosticos
{
    public struct StDadosConsumo
    {

        public System.Diagnostics.Stopwatch? StopwatchProp { get; set; }

        public double ProcessorTimeMs { get; set; }
        public long AllocatedMemorySize { get; set; }
        public long SurvivedMemorySize { get; set; }

        override public string ToString()
        {
            return string.Format(
                "Tempo: {0} ms, {1:hh\\:mm\\:ss}\n" +
                "Memória: [CPU={2}, Allocated MemorySize={3:N0}, Survived = {4:N0}]",
                StopwatchProp?.ElapsedMilliseconds, StopwatchProp?.Elapsed,
                ProcessorTimeMs,
                AllocatedMemorySize,
                SurvivedMemorySize
            );
        }

    }
}