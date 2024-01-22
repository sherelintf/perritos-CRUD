using Microsoft.AspNetCore.Mvc;
using CRUD.Models;
using CRUD.Repositories.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace CRUD.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class OwnersController : ControllerBase
    {
        private readonly IOwnerRepository _ownerRepository;

        public OwnersController(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }

        /// <summary>
        /// Get all owners
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(200, "Get a list of owners", typeof(List<Owner>))]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<ActionResult<List<Owner>>> GetOwners()
        {
            try
            {
                var owners = await _ownerRepository.GetOwners();
                return Ok(owners);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get an owner by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("{id}")]
        [SwaggerResponse(200, "Get an owner by id", typeof(Owner))]
        [SwaggerResponse(404, "The owner was not found")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<ActionResult<Owner>> GetOwner(Guid id)
        {
            try
            {
                var owner = await _ownerRepository.GetOwner(id);
                if (owner == null)
                {
                    return NotFound("Owner not found");
                }
                return Ok(owner);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Create a new owner
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>

        [HttpPost]
        [SwaggerResponse(200, "Create a new owner", typeof(Owner))]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<ActionResult<Owner>> CreateOwner(Owner owner)
        {
            try
            {
                var newOwner = await _ownerRepository.InsertOwner(owner);
                return Ok(newOwner);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Update an owner by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="owner"></param>
        /// <returns></returns>

        [HttpPut("{id}")]
        [SwaggerResponse(200, "Update an owner by id", typeof(Owner))]
        [SwaggerResponse(404, "The owner was not found")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> UpdateOwner(Guid id, Owner owner)
        {
            try
            {
                var ownerToUpdate = await _ownerRepository.GetOwner(id);

                if (ownerToUpdate == null)
                {
                    return NotFound("Owner not found");
                }

                ownerToUpdate.Name = owner.Name;
                ownerToUpdate.Address = owner.Address;
                ownerToUpdate.PhoneNumber = owner.PhoneNumber;
                ownerToUpdate.Email = owner.Email;

                await _ownerRepository.UpdateOwner(ownerToUpdate);

                return Ok(ownerToUpdate);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        ///  Delete an owner by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpDelete("{id}")]
        [SwaggerResponse(204, "Delete an owner by id")]
        [SwaggerResponse(404, "The owner was not found")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> DeleteOwner(Guid id)
        {
            try
            {
                var ownerToDelete = await _ownerRepository.GetOwner(id);

                if (ownerToDelete == null)
                {
                    return NotFound("Owner not found");
                }

                await _ownerRepository.DeleteOwner(id);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
