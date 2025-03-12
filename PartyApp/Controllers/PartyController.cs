using Microsoft.AspNetCore.Mvc;
using PartyInvitationManager.Models.ViewModels;
using PartyInvitationManager.Models.ViewModels;
using PartyInvitationManager.Services;
using System.Threading.Tasks;

namespace PartyInvitationManager.Controllers
{
    public class PartyController : Controller
    {
        private readonly IPartyService _partyService;

        public PartyController(IPartyService partyService)
        {
            _partyService = partyService;
        }

        // GET: Party
        public async Task<IActionResult> Index()
        {
            var parties = await _partyService.GetAllPartiesAsync();
            return View(parties);
        }

        // GET: Party/Details/5
        [Route("Party/Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var partyDetails = await _partyService.GetPartyDetailsAsync(id);
            if (partyDetails == null)
            {
                return NotFound();
            }

            return View(partyDetails);
        }

        // GET: Party/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Party/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PartyViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _partyService.CreatePartyAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Party/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var party = await _partyService.GetPartyByIdAsync(id);
            if (party == null)
            {
                return NotFound();
            }

            var viewModel = new PartyViewModel
            {
                Id = party.Id,
                Description = party.Description,
                EventDate = party.EventDate,
                Location = party.Location
            };

            return View(viewModel);
        }

        // POST: Party/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PartyViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _partyService.UpdatePartyAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // POST: Party/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _partyService.DeletePartyAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Party/AddInvitation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInvitation(InvitationViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _partyService.AddInvitationAsync(model);
                return RedirectToAction(nameof(Details), new { id = model.PartyId });
            }

            // If we get here, something failed, return to the party details
            var partyDetails = await _partyService.GetPartyDetailsAsync(model.PartyId);
            return View("Details", partyDetails);
        }

        // POST: Party/SendInvitation/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendInvitation(int id, int partyId)
        {
            await _partyService.SendInvitationAsync(id);
            return RedirectToAction(nameof(Details), new { id = partyId });
        }
    }
}