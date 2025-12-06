using System;
using System.Windows.Input;
using Proyecto_2_Movil.Models;
using Proyecto_2_Movil.Services;

namespace Proyecto_2_Movil.ViewModels
{
    public class CombateViewModel : BaseViewModel
    {
        private readonly PersistenciaService _servicio;
        private readonly Random _random = new();

        public enum TurnoJugador
        {
            Jugador1,
            Jugador2
        }

        public CombateViewModel()
            : this(App.Current.Handler.MauiContext.Services.GetService<PersistenciaService>()!)
        {
        }

        public CombateViewModel(PersistenciaService servicio)
        {
            _servicio = servicio ?? throw new ArgumentNullException(nameof(servicio));

            Jugador1 = _servicio.Jugadores[0];
            Jugador2 = _servicio.Jugadores[1];

            // Vida inicial según raza
            Jugador1.VidaActual = CalcularVidaInicial(Jugador1);
            Jugador2.VidaActual = CalcularVidaInicial(Jugador2);

            VidaJ1 = Jugador1.VidaActual;
            VidaJ2 = Jugador2.VidaActual;

            Distancia = 2; // empiezan alejados
            CombateTerminado = false;
            Ganador = string.Empty;

            Turno = TurnoJugador.Jugador1;
            TurnoTexto = $"Turno de {NombreTurnoActual}";
            Mensaje = "¡Empieza el combate!";

            AvanzarCommand = new Command(Avanzar);
            RetrocederCommand = new Command(Retroceder);
            AtacarCommand = new Command(Atacar);
            SanarCommand = new Command(Sanar);
        }

        // -------- Jugadores --------

        public Jugador Jugador1 { get; }
        public Jugador Jugador2 { get; }

        public string NombreJ1 => Jugador1.Nombre;
        public string NombreJ2 => Jugador2.Nombre;

        // -------- Estado del combate --------

        private TurnoJugador _turno;
        public TurnoJugador Turno
        {
            get => _turno;
            set => SetProperty(ref _turno, value);
        }

        private int _vidaJ1;
        public int VidaJ1
        {
            get => _vidaJ1;
            set => SetProperty(ref _vidaJ1, value);
        }

        private int _vidaJ2;
        public int VidaJ2
        {
            get => _vidaJ2;
            set => SetProperty(ref _vidaJ2, value);
        }

        private int _distancia;
        public int Distancia
        {
            get => _distancia;
            set => SetProperty(ref _distancia, value);
        }

        private bool _combateTerminado;
        public bool CombateTerminado
        {
            get => _combateTerminado;
            set => SetProperty(ref _combateTerminado, value);
        }

        private string _ganador = string.Empty;
        public string Ganador
        {
            get => _ganador;
            set => SetProperty(ref _ganador, value);
        }

        private string _turnoTexto = string.Empty;
        public string TurnoTexto
        {
            get => _turnoTexto;
            set => SetProperty(ref _turnoTexto, value);
        }

        private string _mensaje = string.Empty;
        public string Mensaje
        {
            get => _mensaje;
            set => SetProperty(ref _mensaje, value);
        }

        private string NombreTurnoActual =>
            Turno == TurnoJugador.Jugador1 ? Jugador1.Nombre : Jugador2.Nombre;

        private string NombreTurnoOpuesto =>
            Turno == TurnoJugador.Jugador1 ? Jugador2.Nombre : Jugador1.Nombre;

        private Jugador JugadorEnTurno =>
            Turno == TurnoJugador.Jugador1 ? Jugador1 : Jugador2;

        private Jugador JugadorRival =>
            Turno == TurnoJugador.Jugador1 ? Jugador2 : Jugador1;

        // -------- Helpers de texto / coincidencias --------

        private static bool ContieneIgnoreCase(string? texto, string palabra)
        {
            if (string.IsNullOrWhiteSpace(texto)) return false;
            return texto.IndexOf(palabra, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private bool ArmaActualEsArco =>
            ContieneIgnoreCase(JugadorEnTurno.Arma?.Nombre, "Arco");

        private bool ArmaActualEsHacha =>
            ContieneIgnoreCase(JugadorEnTurno.Arma?.Nombre, "Hacha");

        private bool ArmaActualEsDagas =>
            ContieneIgnoreCase(JugadorEnTurno.Arma?.Nombre, "Daga");

        // -------- Vida y curación por raza --------

        private bool EsHumano(Jugador j) => ContieneIgnoreCase(j.Raza?.Nombre, "Humano");
        private bool EsElfo(Jugador j) => ContieneIgnoreCase(j.Raza?.Nombre, "Elfo") && !ContieneIgnoreCase(j.Raza?.Nombre, "Agua");
        private bool EsElfoAgua(Jugador j) => ContieneIgnoreCase(j.Raza?.Nombre, "Agua");
        private bool EsOrco(Jugador j) => ContieneIgnoreCase(j.Raza?.Nombre, "Orco");
        private bool EsBestia(Jugador j) => ContieneIgnoreCase(j.Raza?.Nombre, "Bestia");

        private int CalcularVidaInicial(Jugador j)
        {
            if (EsElfoAgua(j))
                return 115; // +15 vida inicial
            return 100;
        }

        private int ObtenerVidaMaxima(Jugador j)
        {
            if (EsElfoAgua(j))
                return 130;
            return 100;
        }

        private int CalcularCuracion(Jugador j)
        {
            // Basado en las descripciones de raza (porcentajes aproximados)
            if (EsHumano(j)) return _random.Next(35, 51);  // ~45%
            if (EsElfo(j)) return _random.Next(55, 71);  // ~65%
            if (EsElfoAgua(j)) return _random.Next(75, 91);  // 75–90%
            if (EsOrco(j)) return _random.Next(30, 46);  // poción
            if (EsBestia(j)) return _random.Next(40, 61);  // dormir (50%)
            return _random.Next(25, 41);
        }

        // -------- Comandos --------

        public ICommand AvanzarCommand { get; }
        public ICommand RetrocederCommand { get; }
        public ICommand AtacarCommand { get; }
        public ICommand SanarCommand { get; }

        // -------- Acciones --------

        private void Avanzar()
        {
            if (CombateTerminado) return;

            if (Distancia > 0)
            {
                Distancia--;
                ActualizarEstado($"{NombreTurnoActual} avanza hacia {NombreTurnoOpuesto}.");
                CambiarTurno();
            }
            else
            {
                Mensaje = "Ya están a distancia cuerpo a cuerpo.";
            }
        }

        private void Retroceder()
        {
            if (CombateTerminado) return;

            Distancia++;
            ActualizarEstado($"{NombreTurnoActual} se aleja de {NombreTurnoOpuesto}.");
            CambiarTurno();
        }

        private bool EstaEnRangoDeAtaque()
        {
            if (ArmaActualEsArco)
            {
                // Arco: puede atacar a distancia 2, 1 o 0
                return Distancia <= 2;
            }

            // Espada, Hacha, Dagas: cuerpo a cuerpo
            return Distancia == 0;
        }

        private void Atacar()
        {
            if (CombateTerminado) return;

            if (!EstaEnRangoDeAtaque())
            {
                Mensaje = ArmaActualEsArco
                    ? "Están demasiado lejos incluso para tu arco. Acércate un poco más."
                    : "Están muy lejos. Debes avanzar para atacar cuerpo a cuerpo.";
                return;
            }

            string armaTexto = JugadorEnTurno.Arma?.Nombre ?? "arma";

            if (ArmaActualEsHacha)
            {
                EjecutarAtaque("hacha", baseMin: 16, baseMax: 24, missChance: 0.25, critChance: 0.20, dobleGolpe: false);
            }
            else if (ArmaActualEsArco)
            {
                EjecutarAtaque("arco", baseMin: 12, baseMax: 20, missChance: 0.35, critChance: 0.35, dobleGolpe: false);
            }
            else if (ArmaActualEsDagas)
            {
                EjecutarAtaque("dagas", baseMin: 7, baseMax: 11, missChance: 0.10, critChance: 0.30, dobleGolpe: true);
            }
            else
            {
                // Espada o cualquier otra arma cuerpo a cuerpo
                EjecutarAtaque("espada", baseMin: 10, baseMax: 16, missChance: 0.05, critChance: 0.15, dobleGolpe: false);
            }
        }

        private void EjecutarAtaque(string tipoArma, int baseMin, int baseMax, double missChance, double critChance, bool dobleGolpe)
        {
            if (CombateTerminado) return;

            // ¿Falló todo el ataque?
            if (_random.NextDouble() < missChance)
            {
                Mensaje = $"{NombreTurnoActual} falló el ataque con {tipoArma}.";
                CambiarTurno();
                return;
            }

            int golpes = dobleGolpe ? 2 : 1;
            int totalDano = 0;
            int golpesCriticos = 0;

            for (int i = 0; i < golpes; i++)
            {
                int dano = _random.Next(baseMin, baseMax + 1);
                bool critico = _random.NextDouble() < critChance;

                if (critico)
                {
                    dano = (int)Math.Round(dano * 1.5);
                    golpesCriticos++;
                }

                totalDano += dano;

                if (Turno == TurnoJugador.Jugador1)
                {
                    VidaJ2 -= dano;
                    if (VidaJ2 <= 0)
                    {
                        VidaJ2 = 0;
                        SincronizarVidaConJugadores();
                        Mensaje = $"{Jugador1.Nombre} atacó con {tipoArma} e hizo {totalDano} de daño (críticos: {golpesCriticos}) y ganó la partida.";
                        FinalizarCombate(Jugador1.Nombre);
                        return;
                    }
                }
                else
                {
                    VidaJ1 -= dano;
                    if (VidaJ1 <= 0)
                    {
                        VidaJ1 = 0;
                        SincronizarVidaConJugadores();
                        Mensaje = $"{Jugador2.Nombre} atacó con {tipoArma} e hizo {totalDano} de daño (críticos: {golpesCriticos}) y ganó la partida.";
                        FinalizarCombate(Jugador2.Nombre);
                        return;
                    }
                }
            }

            SincronizarVidaConJugadores();

            string detalleGolpes = dobleGolpe
                ? $"{golpes} golpes, críticos: {golpesCriticos}"
                : (golpesCriticos > 0 ? "¡Golpe crítico!" : "Golpe normal.");

            Mensaje = $"{NombreTurnoActual} atacó con {tipoArma} e hizo {totalDano} de daño ({detalleGolpes}).";

            CambiarTurno();
        }

        private void Sanar()
        {
            if (CombateTerminado) return;

            var jugador = JugadorEnTurno;
            int curacion = CalcularCuracion(jugador);

            if (Turno == TurnoJugador.Jugador1)
            {
                VidaJ1 += curacion;
                int maxVida = ObtenerVidaMaxima(Jugador1);
                if (VidaJ1 > maxVida) VidaJ1 = maxVida;
            }
            else
            {
                VidaJ2 += curacion;
                int maxVida = ObtenerVidaMaxima(Jugador2);
                if (VidaJ2 > maxVida) VidaJ2 = maxVida;
            }

            SincronizarVidaConJugadores();

            Mensaje = $"{NombreTurnoActual} se curó {curacion} puntos de vida.";
            CambiarTurno();
        }

        // -------- Helpers internos --------

        private void CambiarTurno()
        {
            if (CombateTerminado) return;

            Turno = Turno == TurnoJugador.Jugador1
                ? TurnoJugador.Jugador2
                : TurnoJugador.Jugador1;

            TurnoTexto = $"Turno de {NombreTurnoActual}";
        }

        private void FinalizarCombate(string ganador)
        {
            CombateTerminado = true;
            Ganador = ganador;
            TurnoTexto = $"Ganador: {ganador}";
        }

        private void SincronizarVidaConJugadores()
        {
            Jugador1.VidaActual = VidaJ1;
            Jugador2.VidaActual = VidaJ2;
        }

        private void ActualizarEstado(string mensaje)
        {
            Mensaje = mensaje;

            if (!CombateTerminado)
                TurnoTexto = $"Turno de {NombreTurnoActual}";
            else
                TurnoTexto = $"Ganador: {Ganador}";
        }
    }
}
