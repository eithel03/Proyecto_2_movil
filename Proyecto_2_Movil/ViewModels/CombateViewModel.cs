using Proyecto_2_Movil.Models;
using Proyecto_2_Movil.Services;

namespace Proyecto_2_Movil.ViewModels
{
    public partial class CombateViewModel : BaseViewModel
    {
        private readonly PersistenciaService _servicio;

        public CombateViewModel()
            : this(App.Current.Handler.MauiContext.Services.GetService<PersistenciaService>())
        {
        }

        public CombateViewModel(PersistenciaService servicio)
        {
            _servicio = servicio;

            Jugador1 = _servicio.Jugadores[0];
            Jugador2 = _servicio.Jugadores[1];
        }

        public Jugador Jugador1 { get; }
        public Jugador Jugador2 { get; }
    }
}
