using Proyecto_2_Movil.Models;
using System.Collections.Generic;
using System.Linq;

namespace Proyecto_2_Movil.Services
{
    public partial class PersistenciaService
    {
        public List<Jugador> Jugadores { get; } = new(2);

        public void InicializarJugadores(string nombre1, string nombre2)
        {
            Jugadores.Clear();
            Jugadores.Add(new Jugador { Nombre = nombre1 });
            Jugadores.Add(new Jugador { Nombre = nombre2 });
        }

        public Jugador? ObtenerJugadorSinRaza() =>
            Jugadores.FirstOrDefault(j => j.Raza == null);

        public bool TodosTienenRaza() =>
            Jugadores.All(j => j.Raza != null);

        // -------------------------
        //  ARMAS
        // -------------------------

        public Jugador? ObtenerJugadorSinArma() =>
            Jugadores.FirstOrDefault(j => j.Arma == null);

        public bool TodosTienenArma() =>
            Jugadores.All(j => j.Arma != null);
    }
}
