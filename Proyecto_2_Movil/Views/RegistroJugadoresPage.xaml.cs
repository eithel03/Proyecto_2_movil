using Proyecto_2_Movil.Services;
using Proyecto_2_Movil.ViewModels;

namespace Proyecto_2_Movil.Views;

public partial class RegistroJugadoresPage : ContentPage
{
    public RegistroJugadoresPage()
    {
        InitializeComponent();

        var persistencia = new PersistenciaService();
        BindingContext = new RegistroJugadoresViewModel(persistencia);
    }
}
