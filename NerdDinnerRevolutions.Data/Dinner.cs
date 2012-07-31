using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace NerdDinnerRevolutions.Data
{
    public class Dinner
    {
        public int DinnerID { get; set; }

        [Required(ErrorMessage = "Please enter a Dinner Title")]
        [StringLength(20, ErrorMessage = "Title is too long")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter the Date of the Dinner")]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Please enter the location of the Dinner")]
        [StringLength(30, ErrorMessage = "Address is too long")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter your email address")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Please enter a valid email address")]
        public string HostedBy { get; set; }

        public virtual ICollection<RSVP> RSVPs { get; set; }
    }

    public class RSVP
    {
        public int RsvpID { get; set; }
        public int DinnerID { get; set; }
        public string AttendeeEmail { get; set; }
    }

    // 
    // Our EF Code First Context class.  This class handles persistence and
    // interactions with the database.  This could have also been implemented
    // in a separate class library.

    public class NerdDinners : DbContext
    {
        public DbSet<Dinner> Dinners { get; set; }
        public DbSet<RSVP> RSVPs { get; set; }
    }
}
