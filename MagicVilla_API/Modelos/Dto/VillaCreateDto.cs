﻿using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Modelos.Dtos
{
    public class VillaCreateDto
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public string Detalle { get; set; }
        [Required]
        public double Tarifa { get; set; }
        public int Ocupantes { get; set; }
        public int MetrosCuadrados { get; set; }
        public string ImagenUrl { get; set; }
        public string Amenidad { get; set; }
    }
}
