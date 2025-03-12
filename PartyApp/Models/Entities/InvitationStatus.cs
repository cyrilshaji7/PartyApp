using System.ComponentModel.DataAnnotations;

namespace PartyInvitationManager.Models.Entities
{
    public enum InvitationStatus
    {
        InviteNotSent,
        InviteSent,
        RespondedYes,
        RespondedNo
    }
}