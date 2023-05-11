﻿using System.ComponentModel.DataAnnotations.Schema;

namespace JatszohazBlazor.Shared.Models
{
    public class Rent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public RentStatus Status { get; set; }
        [ForeignKey("User")]
        public User Renter { get; set; }

        public enum RentStatus
        {
            Pending,
            Approved,
            GaveOut,
            InMyRoom,
            Back,
            Declined,
            Cancelled
        }
    }
}
