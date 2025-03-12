using Microsoft.AspNetCore.Mvc;
using PartyInvitationManager.Models.Entities;
using PartyInvitationManager.Models.ViewModels;
using PartyInvitationManager.Services;
using System.Threading.Tasks;

namespace PartyInvitationManager.Controllers
{
    public class InvitationController : Controller
    {
        private readonly IPartyService _partyService;

        public InvitationController(IPartyService partyService)
        {
            _partyService = partyService;
        }

        // GET: Invitation/Respond/5
        [Route("Invitation/Respond/{id:int}")]
        public async Task<IActionResult> Respond(int id)
        {
            var model = await _partyService.GetInvitationResponseViewModelAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Invitation/Respond
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Respond(InvitationResponseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var status = model.WillAttend.Value ? InvitationStatus.RespondedYes : InvitationStatus.RespondedNo;
                await _partyService.UpdateInvitationStatusAsync(model.InvitationId, status);
                return View("Thanks", model);
            }

            // If validation fails, reload the response form
            var updatedModel = await _partyService.GetInvitationResponseViewModelAsync(model.InvitationId);
            updatedModel.WillAttend = model.WillAttend;
            return View(updatedModel);
        }

        [HttpGet]
        [Route("Invitation/AcceptInvitation/{id}")]
        public async Task<IActionResult> AcceptInvitation(int id)
        {
            await _partyService.UpdateInvitationStatusAsync(id, InvitationStatus.RespondedYes);
            var model = await _partyService.GetInvitationResponseViewModelAsync(id);
            model.WillAttend = true;
            return View("Thanks", model);
        }

        [HttpGet]
        [Route("Invitation/DeclineInvitation/{id}")]
        public async Task<IActionResult> DeclineInvitation(int id)
        {
            await _partyService.UpdateInvitationStatusAsync(id, InvitationStatus.RespondedNo);
            var model = await _partyService.GetInvitationResponseViewModelAsync(id);
            model.WillAttend = false;
            return View("Thanks", model);
        }

        // GET: Invitation/Thanks
        public IActionResult Thanks(InvitationResponseViewModel model)
        {
            return View(model);
        }
    }
}