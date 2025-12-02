using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_Movil.Models
{
    public enum TipoRaza { Humano, Elfo, Orco, Bestia }

    public class Raza
    {
        public TipoRaza Tipo { get; set; }
        public string Descripcion { get; set; }
    }
}
