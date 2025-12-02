using System;

namespace Proyecto_2_Movil.Models   // ← CORRECTO, con guion bajo
{
    public class Jugador           // ← CORRECTO, debe ser PUBLIC
    {
        public string Nombre { get; set; } = string.Empty;
        public int PartidasGanadas { get; set; }
        public int PartidasPerdidas { get; set; }
    }
}
