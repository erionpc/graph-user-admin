using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.Extensions.Logging;
using GraphUserAdmin.API.Abstractions;
using GraphUserAdmin.Shared.Users;
using GraphUserAdmin.API.Exceptions;
using GraphUserAdmin.Shared.Paging;
using Swashbuckle.AspNetCore.Annotations;
using GraphUserAdmin.API.Extensions;

namespace GraphUserAdmin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService, ILogger<UsersController> logger) : ControllerBase
    {
        private readonly IUserService UserService = userService;
        private readonly ILogger<UsersController> Logger = logger;

        [HttpGet]
        [SwaggerOperation(summary: "Return paginated users list.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedResponse<IEnumerable<UserViewModel>>>> GetUsersAsync([FromQuery] UserSearchRequestModel? searchRequest, CancellationToken cancellationToken)
        {
            try
            {
                var graphUsers = await UserService.SearchAsync(searchRequest, cancellationToken);
                if (graphUsers is null) 
                    return NotFound();

                var data = graphUsers.Data.Select(u => u.MapToUserViewModel());
                var paginatedResponse = graphUsers.CloneFromThis(data);

                return Ok(paginatedResponse);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while getting users");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserViewModel>> GetUser([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var user = await UserService.GetByObjectIdAsync(id, cancellationToken);
                if (user is null)
                    return NotFound();

                return Ok(user.MapToUserViewModel());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while getting the user");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserViewModel>> NewUser(UserViewModel user, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await UserService.CreateAsync(user, cancellationToken));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while creating user");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutUser([FromRoute] Guid id, UserViewModel user, CancellationToken cancellationToken)
        {
            try
            {
                await UserService.UpdateAsync(id, user, cancellationToken);

                return NoContent();
            }
            catch (UserNotFoundException ex)
            {
                Logger.LogError(ex, $"User not found");
                return NotFound();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while updating user");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser([FromRoute]Guid id, CancellationToken cancellationToken)
        {
            try
            {
                await UserService.DeleteAsync(id, cancellationToken);
                return NoContent();
            }
            catch (UserNotFoundException ex)
            {
                Logger.LogError(ex, $"User not found");
                return NotFound();
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error occurred while deleting user");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
