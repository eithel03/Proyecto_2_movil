using System.Threading.Tasks;
// Services/PersistenciaService.cs
using System.Collections.Generic;
using Proyecto_2_Movil.Models;

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

        public bool TodosTienenRaza() => Jugadores.All(j => j.Raza != null);
    }
}

