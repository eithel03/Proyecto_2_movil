using System.Threading.Tasks;
using Proyecto_2_Movil.Models;

namespace Proyecto_2_Movil.Services
{
    public class PersistenciaService
    {
        public Jugador? Jugador1 { get; private set; }
        public Jugador? Jugador2 { get; private set; }

        public Task GuardarJugadoresAsync(Jugador jugador1, Jugador jugador2)
        {
            Jugador1 = jugador1;
            Jugador2 = jugador2;
            return Task.CompletedTask;
        }
    }
}
