using Microsoft.EntityFrameworkCore;
using PartyInvitationManager.Models.ViewModels;
using PartyInvitationManager.Data;
using PartyInvitationManager.Models.Entities;
using PartyInvitationManager.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartyInvitationManager.Services
{
    public interface IPartyService
    {
        Task<List<Party>> GetAllPartiesAsync();
        Task<Party> GetPartyByIdAsync(int id);
        Task<PartyDetailsViewModel> GetPartyDetailsAsync(int id);
        Task CreatePartyAsync(PartyViewModel model);
        Task UpdatePartyAsync(PartyViewModel model);
        Task DeletePartyAsync(int id);
        Task AddInvitationAsync(InvitationViewModel model);
        Task<Invitation> GetInvitationByIdAsync(int id);
        Task<InvitationResponseViewModel> GetInvitationResponseViewModelAsync(int id);
        Task UpdateInvitationStatusAsync(int id, InvitationStatus status);
        Task SendInvitationAsync(int invitationId);
    }

    public class PartyService : IPartyService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public PartyService(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<List<Party>> GetAllPartiesAsync()
        {
            return await _context.Parties.ToListAsync();
        }

        public async Task<Party> GetPartyByIdAsync(int id)
        {
            return await _context.Parties.FindAsync(id);
        }

        public async Task<PartyDetailsViewModel> GetPartyDetailsAsync(int id)
        {
            var party = await _context.Parties
                .Include(p => p.Invitations)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (party == null)
                return null;

            var viewModel = new PartyDetailsViewModel
            {
                Id = party.Id,
                Description = party.Description,
                EventDate = party.EventDate,
                Location = party.Location,
                Invitations = party.Invitations.Select(i => new InvitationViewModel
                {
                    Id = i.Id,
                    GuestName = i.GuestName,
                    GuestEmail = i.GuestEmail,
                    Status = i.Status,
                    PartyId = i.PartyId
                }).ToList()
            };

            return viewModel;
        }

        public async Task CreatePartyAsync(PartyViewModel model)
        {
            var party = new Party
            {
                Description = model.Description,
                EventDate = model.EventDate,
                Location = model.Location,
                Invitations = new List<Invitation>()
            };

            _context.Parties.Add(party);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePartyAsync(PartyViewModel model)
        {
            var party = await _context.Parties.FindAsync(model.Id);
            if (party == null)
                throw new KeyNotFoundException($"Party with ID {model.Id} not found");

            party.Description = model.Description;
            party.EventDate = model.EventDate;
            party.Location = model.Location;

            _context.Parties.Update(party);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePartyAsync(int id)
        {
            var party = await _context.Parties.FindAsync(id);
            if (party == null)
                throw new KeyNotFoundException($"Party with ID {id} not found");

            _context.Parties.Remove(party);
            await _context.SaveChangesAsync();
        }

        public async Task AddInvitationAsync(InvitationViewModel model)
        {
            var invitation = new Invitation
            {
                GuestName = model.GuestName,
                GuestEmail = model.GuestEmail,
                Status = InvitationStatus.InviteNotSent,
                PartyId = model.PartyId
            };

            _context.Invitations.Add(invitation);
            await _context.SaveChangesAsync();
        }

        public async Task<Invitation> GetInvitationByIdAsync(int id)
        {
            return await _context.Invitations
                .Include(i => i.Party)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<InvitationResponseViewModel> GetInvitationResponseViewModelAsync(int id)
        {
            var invitation = await _context.Invitations
                .Include(i => i.Party)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invitation == null)
                return null;

            return new InvitationResponseViewModel
            {
                InvitationId = invitation.Id,
                GuestName = invitation.GuestName,
                PartyDescription = invitation.Party.Description,
                PartyDate = invitation.Party.EventDate,
                PartyLocation = invitation.Party.Location,
                WillAttend = null
            };
        }

        public async Task UpdateInvitationStatusAsync(int id, InvitationStatus status)
        {
            var invitation = await _context.Invitations.FindAsync(id);
            if (invitation == null)
                throw new KeyNotFoundException($"Invitation with ID {id} not found");

            invitation.Status = status;
            _context.Invitations.Update(invitation);
            await _context.SaveChangesAsync();
        }

        public async Task SendInvitationAsync(int invitationId)
        {
            var invitation = await _context.Invitations
                .Include(i => i.Party)
                .FirstOrDefaultAsync(i => i.Id == invitationId);

            if (invitation == null)
                throw new KeyNotFoundException($"Invitation with ID {invitationId} not found");

            await _emailService.SendInvitationEmailAsync(
                invitation.GuestEmail,
                invitation.GuestName,
                invitation.Party.Description,
                invitation.Party.EventDate,
                invitation.Party.Location,
                invitation.Id
            );

            invitation.Status = InvitationStatus.InviteSent;
            _context.Invitations.Update(invitation);
            await _context.SaveChangesAsync();
        }
    }
}