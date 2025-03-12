using PartyInvitationManager.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace PartyInvitationManager.Models.ViewModels
{
    public class InvitationViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the guest's name")]
        [Display(Name = "Guest Name")]
        public string GuestName { get; set; }

        [Required(ErrorMessage = "Please enter the guest's email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Guest Email")]
        public string GuestEmail { get; set; }

        public InvitationStatus Status { get; set; }

        public int PartyId { get; set; }
    }
}