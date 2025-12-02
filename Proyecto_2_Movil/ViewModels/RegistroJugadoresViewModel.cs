using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Proyecto_2_Movil.Services;
using Proyecto_2_Movil.Models;

namespace Proyecto_2_Movil.ViewModels
{
    public class RegistroJugadoresViewModel : BaseViewModel
    {
        private readonly PersistenciaService _persistenciaService;

        public RegistroJugadoresViewModel(PersistenciaService persistenciaService)
        {
            _persistenciaService = persistenciaService;
            ContinuarCommand = new Command(async () => await ContinuarAsync());
        }

        private string _nombreJugador1 = string.Empty;
        public string NombreJugador1
        {
            get => _nombreJugador1;
            set => SetProperty(ref _nombreJugador1, value);
        }

        private string _nombreJugador2 = string.Empty;
        public string NombreJugador2
        {
            get => _nombreJugador2;
            set => SetProperty(ref _nombreJugador2, value);
        }

        private string _mensajeError = string.Empty;
        public string MensajeError
        {
            get => _mensajeError;
            set => SetProperty(ref _mensajeError, value);
        }

        private bool _hayError;
        public bool HayError
        {
            get => _hayError;
            set => SetProperty(ref _hayError, value);
        }

        public ICommand ContinuarCommand { get; }

        private async Task ContinuarAsync()
        {
            HayError = false;
            MensajeError = string.Empty;

            var j1 = NombreJugador1?.Trim();
            var j2 = NombreJugador2?.Trim();

            if (string.IsNullOrWhiteSpace(j1) || string.IsNullOrWhiteSpace(j2))
            {
                MostrarError("Ambos nombres son obligatorios.");
                return;
            }

            if (j1.Length < 3 || j2.Length < 3)
            {
                MostrarError("Cada nombre debe tener al menos 3 caracteres.");
                return;
            }

            if (j1.Equals(j2, StringComparison.OrdinalIgnoreCase))
            {
                MostrarError("Los nombres de los jugadores deben ser distintos.");
                return;
            }

            // Crear jugadores
            var jugador1 = new Jugador { Nombre = j1 };
            var jugador2 = new Jugador { Nombre = j2 };

            // Guardar en el servicio
            await _persistenciaService.GuardarJugadoresAsync(jugador1, jugador2);

            // Navegar a la siguiente pantalla (Selección de Raza)
            await Shell.Current.GoToAsync("SeleccionRazaPage");

        }

        private void MostrarError(string mensaje)
        {
            MensajeError = mensaje;
            HayError = true;
        }
    }
}
