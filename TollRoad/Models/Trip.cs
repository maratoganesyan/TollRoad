﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace TollRoad.Models;

public partial class Trip
{
    public int Id { get; set; }

    public int IdRoute { get; set; }

    public int IdVehicle { get; set; }

    public DateTime StartDateTimeOfTrip { get; set; }

    public DateTime EndDateTimeOfTrip { get; set; }

    public int StatusOfTrip { get; set; }

    public decimal TotalPriceOfTrip { get; set; }

    public virtual Rout IdRouteNavigation { get; set; }

    public virtual Vehicle IdVehicleNavigation { get; set; }

    public virtual StatusesOfTrip StatusOfTripNavigation { get; set; }

    public override string ToString()
    {
        return IdRoute + " " + IdVehicleNavigation.StateNumber + " " + StatusOfTripNavigation.ToString() + " " +
                TotalPriceOfTrip + " " + StartDateTimeOfTrip + " " + EndDateTimeOfTrip;
    }
}