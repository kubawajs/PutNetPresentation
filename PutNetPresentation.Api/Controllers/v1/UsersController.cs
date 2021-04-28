using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PutNetPresentation.Api.Filters;
using PutNetPresentation.Infrastructure.Dto;
using PutNetPresentation.Infrastructure.Services.Abstractions;

namespace PutNetPresentation.Api.Controllers.v1
{
    [ApiController]
    [Route("api/{version:apiVersion}/users")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        private readonly IMemoryCache _memoryCache;

        public UsersController(IUserService userService, ILogger<UsersController> logger, IMemoryCache memoryCache)
        {
            _userService = userService;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>Returns collection of UserDto objects</returns>
        [HttpGet]
        [ResponseCache(Duration = 10)]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                _logger.LogInformation("Successfully retrieved users object from repository.");
                return Ok(new { ResponseTime = DateTime.Now.ToLongTimeString(), users });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception occurred while retrieving users from database. Message: {e.Message}");
                return BadRequest();
            }
        }

        // Pagination
        //[HttpGet]
        //[ResponseCache(Duration = 10)]
        //[ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        //public async Task<IActionResult> Get([FromQuery] PaginationFilter paginationFilter)
        //{
        //    var key = paginationFilter.CacheKey;

        //    // Try get from memory cache
        //    if (_memoryCache.TryGetValue(key, out var users))
        //    {
        //        _logger.LogInformation($"Returned from memory cache. Key {key}");
        //        return Ok(users);
        //    }

        //    try
        //    {
        //        users = await _userService.GetAllAsync(paginationFilter.PageNumber, paginationFilter.PageSize);
        //        _logger.LogInformation("Successfully retrieved users object from repository.");

        //        _memoryCache.Set(key, users);
        //        _logger.LogInformation($"Results saved to memory cache. Key: {key}");

        //        return Ok(users);
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError($"Exception occurred while retrieving users from database. Message: {e.Message}");
        //        return BadRequest();
        //    }
        //}

        [HttpGet("{username}")]
        public async Task<IActionResult> Get(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                _logger.LogWarning("Username cannot be null or empty.");
                return NotFound();
            }

            var user = await _userService.GetAsync(username);
            if (user != null)
            {
                _logger.LogInformation("Successfully ");
                return Ok(user);
            }

            _logger.LogWarning($"Cannot retrieve user with username: {username}");
            return NotFound();

        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="registerUserDto"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /users
        ///     {
        ///         "username": "user_123",
        ///         "email": "user@mail.com",
        ///         "password": "Ex4mplePas$"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item model is invalid</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model is invalid.");
                return BadRequest();
            }

            try
            {
                await _userService.CreateUserAsync(registerUserDto);
                _logger.LogInformation($"User uccessfully created: {registerUserDto.Username}");

                return Created($"api/users/{registerUserDto.Username}", null);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception occurred while retrieving users from database. Message: {e.Message}");
                return BadRequest();
            }
        }
    }
}
