// Models/Jugador.cs
namespace Proyecto_2_Movil.Models
{
    public partial class Jugador
    {
        public string Nombre { get; set; } = "";
        public Raza? Raza { get; set; }
        public int VidaActual { get; set; } = 100;
        public int PartidasGanadas { get; set; }
        public int PartidasPerdidas { get; set; }
        public int Empates { get; set; }
    }
}