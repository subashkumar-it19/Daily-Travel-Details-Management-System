using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyTravelManagement.Models
{
	public class Vehicle
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int  VehicleId { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string Unique_Id { get; set; } = "";

        [Column(TypeName = "nvarchar(50)")]
        public string VehicleName { get; set; } = "Bike";

        [Column(TypeName = "nvarchar(100)")]
        public string VehicleModelName { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string Icon { get; set; } = "";

        [NotMapped]
        public string? TitleWithIcon {
            get
            {
                return this.Icon + " " + this.VehicleName;
            }
        }

        [NotMapped]
        public string? TitleWithModel
        {
            get
            {
                return this.Icon + " " + this.VehicleModelName;
            }
        }

        [Column(TypeName = "nvarchar(50)")]
        public string VehicleType { get; set; } = "Two Wheeler";

        [Column(TypeName = "nvarchar(10)")]
        public string Create_By { get; set; } = "admin";

		public DateTime? Create_Date { get; set; } = DateTime.Now;

        [Column(TypeName = "nvarchar(10)")]
        public string Update_By { get; set; } = "NA";

        public DateTime? Update_Date { get; set; } 

		public int Status { get; set; } = 1;

		public int Is_Active { get; set; } = 1;

    }
}

