using PartyInvitationManager.Models.Entities;
using System;
using System.Collections.Generic;

namespace PartyInvitationManager.Models.ViewModels
{
    public class PartyDetailsViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; }

        public List<InvitationViewModel> Invitations { get; set; } = new List<InvitationViewModel>();

        // Statistics
        public int TotalInvitations => Invitations.Count;
        public int NotSentCount => Invitations.Count(i => i.Status == InvitationStatus.InviteNotSent);
        public int SentCount => Invitations.Count(i => i.Status == InvitationStatus.InviteSent);
        public int RespondedYesCount => Invitations.Count(i => i.Status == InvitationStatus.RespondedYes);
        public int RespondedNoCount => Invitations.Count(i => i.Status == InvitationStatus.RespondedNo);
    }
}