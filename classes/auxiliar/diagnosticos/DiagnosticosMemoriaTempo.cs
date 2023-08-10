namespace classes.auxiliar.diagnosticos
{
    public class DiagnosticosMemoriaTempo
    {

        #region memória
        private readonly AppDomain _appdomain;

        private TimeSpan InitialProcessorTimeField;
        private long InitialAllocatedMemorySizeField;
        private long InitialSurvivedMemorySize;

        public DiagnosticosMemoriaTempo()
        {
            _appdomain = AppDomain.CurrentDomain;
            this.Reset();
        }

        public void Reset()
        {
            this.InitialProcessorTimeField = this._appdomain.MonitoringTotalProcessorTime;
            this.InitialAllocatedMemorySizeField = this._appdomain.MonitoringTotalAllocatedMemorySize;
            this.InitialSurvivedMemorySize = this._appdomain.MonitoringSurvivedMemorySize;
        }
        #endregion

        public StDadosConsumo? MesurarConsumo(Action act)
        {
            if (act == null) { return null; }
            System.Diagnostics.Stopwatch sw = new();
            sw.Start();
            act.Invoke();
            sw.Stop();

            GC.Collect();

            return new StDadosConsumo
            {
                StopwatchProp = sw,
                ProcessorTimeMs = (this._appdomain.MonitoringTotalProcessorTime - InitialProcessorTimeField).TotalMilliseconds,
                AllocatedMemorySize = this._appdomain.MonitoringTotalAllocatedMemorySize - InitialAllocatedMemorySizeField,
                SurvivedMemorySize = this._appdomain.MonitoringSurvivedMemorySize - InitialSurvivedMemorySize
            };
        }


    }
}