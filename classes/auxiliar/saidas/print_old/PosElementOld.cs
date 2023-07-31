namespace classes.auxiliar.saidas.print_old
{
    [Obsolete]
    public class PosElementOld<T>
    {
        public T Elemento { get; set; }
        public int Posicao { get; set; }
        public int Height { get; set; }

        public PosElementOld(T treeProp, int posicao, int height)
        {
            Elemento = treeProp;
            Posicao = posicao;
            Height = height;
        }
        public override string? ToString()
        {
            return string.Format("{0} {1} {2}", Elemento, Posicao, Height);
        }
    }
}