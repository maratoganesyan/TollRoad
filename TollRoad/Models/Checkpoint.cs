﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace TollRoad.Models;

public partial class Checkpoint
{
    public int Id { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public string Address { get; set; }

    public int MaxWidthOfVehicleInMm { get; set; }

    public int MaxHeightOfVehicleInMm { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<PhotoOfCheckpoint> PhotoOfCheckpoints { get; set; } = new List<PhotoOfCheckpoint>();

    public virtual ICollection<Rout> RoutIdFirstCheckpointNavigations { get; set; } = new List<Rout>();

    public virtual ICollection<Rout> RoutIdSecondCheckpointNavigations { get; set; } = new List<Rout>();

    public override string ToString()
    {
        return Latitude.ToString() + " " +
                Longitude.ToString() + " " +
                Address + " " +
                MaxWidthOfVehicleInMm.ToString() + " " +
                MaxHeightOfVehicleInMm.ToString();

    }
}