﻿using System.ComponentModel.DataAnnotations;

namespace PartyInvitationManager.Models.ViewModels
{
    public class InvitationResponseViewModel
    {
        public int InvitationId { get; set; }
        public string GuestName { get; set; }
        public string PartyDescription { get; set; }
        public DateTime PartyDate { get; set; }
        public string PartyLocation { get; set; }

        [Required]
        [Display(Name = "Will you attend?")]
        public bool? WillAttend { get; set; }
    }
}