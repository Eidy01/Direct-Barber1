﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Direct_Barber.Models;

public partial class Solicitud
{
    public int IdSolicitud { get; set; }

    public int? IdCliente { get; set; }

    public int? IdBarbero { get; set; }

    public string Dirección { get; set; }

    public DateTime? Fecha { get; set; }

    public string Descripcion { get; set; }

    public decimal? Precio { get; set; }
    [Display(Name = "Nombre del Barbero")]
    public virtual Usuario IdBarberoNavigation { get; set; }
    [Display(Name = "Nombre del cliente")]
    public virtual Usuario IdClienteNavigation { get; set; }
}
