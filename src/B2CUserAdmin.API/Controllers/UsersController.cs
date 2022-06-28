using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.Extensions.Logging;
using B2CUserAdmin.API.Abstractions;
using B2CUserAdmin.Shared.Users;

namespace B2CUserAdmin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService UserService;
        private readonly ILogger<UsersController> Logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            UserService = userService;
            Logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetUsers(Guid? objectId = null, string emailSearch = null)
        {
            if (objectId.HasValue)
            {
                var user = await UserService.GetByObjectIdAsync(objectId.Value);
                if (user is null) 
                    return NotFound();

                return Ok(user);
            }
            else
            {
                return NotFound();
            }

            //if (emailSearch is null)
            //{
            //    var users = await UserService.GetAllAsync();

            //    return Ok(users);
            //}
            //else
            //{
            //    var users = await UserService.SearchByEmailAsync(emailSearch, includeClaims);

            //    return Ok(users);
            //}
        }

        //[HttpGet("{objectId}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult<IEnumerable<UserViewModel>>> GetUser(Guid objectId)
        //{
        //    var user = await UserService.GetByObjectIdAsync(objectId);
        //    if (user is null) return NotFound();
        //    else return Ok(user);
        //}

        //[HttpPost()]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status409Conflict)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<UserViewModel>> NewUser(UserViewModel user, Guid clientId, string lang, Guid brandId)
        //{
        //    try
        //    {
        //        return Ok(await UserService.CreateAsync(user, clientId, lang, brandId));
        //    }
        //    catch(ApplicationNotFoundException e)
        //    {
        //        return NotFound(e.Message);
        //    }
        //}

        //// PUT: api/Users/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> PutUser(Guid id, UserViewModel user)
        //{
        //    try
        //    {
        //        await UserService.PutAsync(id, user);
                
        //        return NoContent();
        //    }
        //    catch (UserNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}

        //// DELETE: api/Users/5
        //[HttpDelete("{objectId}")]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //public async Task<ActionResult<User>> DeleteUser(Guid objectId)
        //{
        //    try
        //    {
        //        var user = await UserService.GetByObjectIdAsync(objectId);
        //        await UserService.DeleteAsync(user.UserId);
        //        return NoContent();
        //    }
        //    catch (UserNotFoundException ex)
        //    {
        //        Logger.LogWarning(ex, $"User with objectId: {objectId} Not found during deletion");
        //        return NotFound(ex.Message);
        //    }
        //    catch(Exception e) {
        //        Logger.LogError(e, "Unkown Error Occured");
        //        return BadRequest();
        //    }
        //}
    }
}
