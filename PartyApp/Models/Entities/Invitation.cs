using System.ComponentModel.DataAnnotations;

namespace PartyInvitationManager.Models.Entities
{
    public class Invitation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the guest's name")]
        [Display(Name = "Guest Name")]
        public string GuestName { get; set; }

        [Required(ErrorMessage = "Please enter the guest's email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Guest Email")]
        public string GuestEmail { get; set; }

        [Required]
        public InvitationStatus Status { get; set; } = InvitationStatus.InviteNotSent;
        public int PartyId { get; set; }
        public Party Party { get; set; }
    }
}