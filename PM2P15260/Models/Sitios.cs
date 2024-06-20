using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace PM2P15260.Models
{
    [Table("Sitios")]
    public class Sitios
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Descripcion { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Latitude { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Longitude { get; set; } = string.Empty;

        public string Foto { get; set; } = string.Empty;

    }
}
