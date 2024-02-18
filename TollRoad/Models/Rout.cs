﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace TollRoad.Models;

public partial class Rout
{
    public int Id { get; set; }

    public int IdFirstCheckpoint { get; set; }

    public int IdSecondCheckpoint { get; set; }

    public decimal DistanceInKm { get; set; }

    public decimal Fare { get; set; }

    public virtual Checkpoint IdFirstCheckpointNavigation { get; set; }

    public virtual Checkpoint IdSecondCheckpointNavigation { get; set; }

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}