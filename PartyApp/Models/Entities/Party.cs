using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PartyInvitationManager.Models.Entities
{
    public class Party
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a description for the party")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter the event date")]
        [Display(Name = "Event Date")]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Please enter the location")]
        public string Location { get; set; }

        public ICollection<Invitation> Invitations { get; set; }
    }
}