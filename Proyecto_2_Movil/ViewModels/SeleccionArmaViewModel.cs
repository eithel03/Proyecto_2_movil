using System.Collections.ObjectModel;
using System.Windows.Input;
using Proyecto_2_Movil.Models;
using Proyecto_2_Movil.Services;
using Proyecto_2_Movil.Views;

namespace Proyecto_2_Movil.ViewModels
{
    public partial class SeleccionArmaViewModel : BaseViewModel
    {
        private readonly PersistenciaService _servicio;
        private Jugador? _jugadorActual;

        public ObservableCollection<Arma> Armas { get; }
        public ICommand SeleccionarArmaCommand { get; }

        public string Titulo => _jugadorActual != null
            ? $"Selecciona arma para {_jugadorActual.Nombre}"
            : "Error: no hay jugadores";

        
        public SeleccionArmaViewModel()
            : this(App.Current.Handler.MauiContext.Services.GetService<PersistenciaService>())
        {
        }

        public SeleccionArmaViewModel(PersistenciaService servicio)
        {
            _servicio = servicio;

            Armas = new ObservableCollection<Arma>
            {
                new() { Nombre="Espada", Descripcion="Daño equilibrado.", AtaqueBase=25 },
                new() { Nombre="Hacha", Descripcion="Más daño pero menos precisión.", AtaqueBase=35 },
                new() { Nombre="Arco", Descripcion="Daño a distancia, crítico mayor.", AtaqueBase=20 },
                new() { Nombre="Dagas", Descripcion="Golpes dobles.", AtaqueBase=15 }
            };

            SeleccionarArmaCommand = new Command<Arma>(SeleccionarArma);

            Actualizar();
        }

        private void Actualizar()
        {
            _jugadorActual = _servicio.ObtenerJugadorSinArma();
            OnPropertyChanged(nameof(Titulo));
        }

        private async void SeleccionarArma(Arma arma)
        {
            if (_jugadorActual == null || arma == null)
                return;

            _jugadorActual.Arma = arma;

            if (_servicio.TodosTienenArma())
            {
                await Shell.Current.GoToAsync(nameof(CombatePage));
            }
            else
            {
                Actualizar();
            }
        }
    }
}
