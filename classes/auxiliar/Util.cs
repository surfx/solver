namespace classes.auxiliar
{

    public class Util
    {

        public static System.Diagnostics.Stopwatch? CountTimer(Action act)
        {
            if (act == null) { return null; }
            System.Diagnostics.Stopwatch sw = new();
            sw.Start();
            act.Invoke();
            sw.Stop();
            return sw;
        }

        public static void print(System.Diagnostics.Stopwatch? sw)
        {
            if (sw == null) { return; }
            Console.WriteLine(string.Format("Tempo: {0} ms, {1:hh\\:mm\\:ss}", sw.ElapsedMilliseconds, sw.Elapsed));
        }

    }

}