using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace DailyTravelManagement.Models
{
	public class Travel
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TravelId { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string Unique_Id { get; set; } =" ";


        //Foreignkey
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; } 

        public double Kilometer { get; set; }

        public double Amount { get; set; }

        public double FuelCharge { get; set; }

        public double MaintenanceCharge { get; set; }

        public double DriverCommission { get; set; }

        public DateTime Date { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Pickup { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Drop { get; set; } = "";

        [Column(TypeName = "nvarchar(100)")]
        public string? Note { get; set; } 

        [Column(TypeName = "nvarchar(10)")]
        public string Create_By { get; set; } = "admin";

        public DateTime? Create_Date { get; set; } = DateTime.Now;

        
        [Column(TypeName = "nvarchar(10)")]
        public string Update_By { get; set; } = "NA";


        public DateTime? Update_Date { get; set; }

        public int Status { get; set; } = 1;

        public int Is_Active { get; set; } = 1;

        [NotMapped]
        public string? VehicleTitleWithIcon {
            get
            {
                return Vehicle == null ? "" : Vehicle.Icon +" "+ Vehicle.VehicleModelName;
            }
        }
       
        
        [NotMapped]
        public string? FormattedDate
        {
            get
            {
                var DateOnlyValue = DateOnly.FromDateTime(Date);
                return DateOnlyValue.ToString();
            }
        }

        [NotMapped]
        public string? FormattedAmount
        {
            get
            {

                return "₹" + Amount;
            }
        }


        [NotMapped]
        public string? ExpenseCharges
        {
            get
            {
                var expence = FuelCharge + MaintenanceCharge + DriverCommission;
                return "₹" + "-" + expence;
            }
        }



    }
}

