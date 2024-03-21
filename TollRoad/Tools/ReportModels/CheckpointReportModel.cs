using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollRoad.Models;

namespace TollRoad.Tools.ReportModels
{
    internal class CheckpointReportModel
    {
        public Checkpoint Checkpoint { get; set; }
        public List<Trip> Trips { get; set; }
        public DateTime StartGenerationDate { get; set; }
        public DateTime EndGenerationDate { get; set; }
        public decimal TotalCost { get; set; }
        public CheckpointReportModel(Checkpoint checkpoint, List<Trip> Trips, DateTime Start, DateTime End)
        {
            Checkpoint = checkpoint;
            this.Trips = Trips;
            this.StartGenerationDate = Start;
            this.EndGenerationDate = End;
            TotalCost = Trips.Sum(t => t.TotalPriceOfTrip);
        }
    }
}
