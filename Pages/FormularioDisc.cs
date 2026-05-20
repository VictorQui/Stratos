namespace ProyectoWebIDP.Models
{
    public class FormularioDisc
    {
        // Totales de la columna MÁS (Izquierda)
        public int ZMas { get; set; }
        public int EstrellaMas { get; set; }
        public int CuadradoMas { get; set; }
        public int TrianguloMas { get; set; }

        // Totales de la columna MENOS (Derecha)
        public int ZMenos { get; set; }
        public int EstrellaMenos { get; set; }
        public int CuadradoMenos { get; set; }
        public int TrianguloMenos { get; set; }
    }
}