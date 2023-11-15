namespace classes.auxiliar.inputs
{

    public struct ParametrosEntrada
    {

        public string fileImagem { get; set; }
        public string fileFormulas { get; set; }
        public string fileEstatisticas { get; set; }

        public override string ToString()
        {
            return string.Format("fileImagem: '{0}', fileFormulas: '{1}', fileEstatisticas: '{2}'", fileImagem, fileFormulas, fileEstatisticas);
        }

    }

    public class AvaliarParametrosEntrada
    {

        /*
            aceitos:
            file_formulas, file_img, file_estatisticas
        */

        public static ParametrosEntrada? avaliar(string[] args)
        {
            if (args == null || args.Length <= 0) { return null; }

            ParametrosEntrada rt = new ParametrosEntrada();

            foreach (string argumento in args)
            {
                if (argumento == null || argumento.Length <= 0) { continue; }
                if (argumento.ToLower().StartsWith("file_img"))
                {
                    rt.fileImagem = getValor(argumento);
                    continue;
                }
                if (argumento.ToLower().StartsWith("file_formulas"))
                {
                    rt.fileFormulas = getValor(argumento);
                    continue;
                }
                if (argumento.ToLower().StartsWith("file_estatisticas"))
                {
                    rt.fileEstatisticas = getValor(argumento);
                    continue;
                }
            }

            return rt;
        }

        public static bool isParametrosOk(ParametrosEntrada? pe)
        {
            if (pe == null) { return false; }
            if (string.IsNullOrEmpty(pe.Value.fileFormulas))
            {
                return false;
            }
            return true;
        }

        private static string getValor(string argumento)
        {
            if (argumento == null || argumento.Length <= 0) { return ""; }
            if (!argumento.Contains('=')) { return argumento; }
            return argumento[(argumento.IndexOf("=") + 1)..];
        }

    }



}