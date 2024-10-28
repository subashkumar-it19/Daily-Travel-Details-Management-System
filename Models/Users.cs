using System;
using System.ComponentModel.DataAnnotations;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DailyTravelManagement.Models
{
	public class Users
	{
		[Key]
		public int UserId { get; set; }
		[Required(ErrorMessage ="Enter Username")]
		public string Username { get; set; }
		[Required(ErrorMessage ="Enter Password")]
		public string Password { get; set; }
		[Required(ErrorMessage = "Enter Email Id")]
		public string EmailId { get; set; } = "admin@gmail.com";
	}
}

