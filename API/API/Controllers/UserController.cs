using API.Controllers;
using Backend.DTO;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using SD.LLBLGen.Pro.DQE.PostgreSql;
using SD.LLBLGen.Pro.LinqSupportClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using YumCity_Migrations.DatabaseSpecific;
using YumCity_Migrations.EntityClasses;
using YumCity_Migrations.Linq;

namespace Backend.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration config)
        {
            _configuration = config;
        }

        [HttpPost]
        [Route("api/create-user")]
        public async Task<ActionResult> Register([FromBody] UserDto newUser)
        {
            RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c => c.AddDbProviderFactory(typeof(NpgsqlFactory)));
            using (var adapter = new DataAccessAdapter(_configuration.GetConnectionString("YumCityDb")))
            {
                var metaData = new LinqMetaData(adapter);
                UserEntity user = await metaData.User.FirstOrDefaultAsync(x => x.Name == newUser.Username);
                if (string.IsNullOrEmpty(newUser.Username) || string.IsNullOrEmpty(newUser.Password))
                    return BadRequest("Cant be empty");
                else if (user is not null)
                {
                    return BadRequest("Username already taken!");
                }
                else
                {
                    user = new();
                    user.Id = Guid.NewGuid();
                    user.Name = newUser.Username;
                    var hasher = new PasswordHasher<UserEntity>();
                    user.PasswordHash = hasher.HashPassword(user, newUser.Password);
                    user.IsActive = true;
                    await adapter.SaveEntityAsync(user);
                    return Ok();
                }
            }
        }

        [HttpPost]
        [Route("api/login")]
        public async Task<ActionResult> Login([FromBody] UserDto user)
        {
            RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c => c.AddDbProviderFactory(typeof(NpgsqlFactory)));
            using (var adapter = new DataAccessAdapter(_configuration.GetConnectionString("YumCityDb")))
            {
                var metaData = new LinqMetaData(adapter);
                UserEntity loggedUser = await metaData.User.FirstOrDefaultAsync(x => x.Name == user.Username && x.IsActive == true);
                var hasher = new PasswordHasher<UserEntity>();
                if (loggedUser == null)
                    return BadRequest("User not found!");
                if (hasher.VerifyHashedPassword(loggedUser, loggedUser.PasswordHash, user.Password).Equals(PasswordVerificationResult.Success))
                {
                    string token = CreateToken(loggedUser.Id);
                    var refreshToken = CreateRefreshToken();
                    SetRefreshToken(refreshToken, loggedUser.Id);
                    return Ok(token);
                }
                else
                    return BadRequest("Wrong password!");
            }
        }

        [HttpPost]
        [Route("api/refresh-token/{id}"), Authorize]
        public async Task<ActionResult<string>> RefreshToken([FromBody] string rT, Guid id)
        {
            try
            {
                await RecipeController.GlobalAntiforgery.ValidateRequestAsync(HttpContext);
                RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c => c.AddDbProviderFactory(typeof(NpgsqlFactory)));
                using (var adapter = new DataAccessAdapter(_configuration.GetConnectionString("YumCityDb")))
                {
                    var metaData = new LinqMetaData(adapter);
                    RefreshTokenEntity refreshToken = await metaData.RefreshToken.FirstOrDefaultAsync(x => x.Token.Equals(rT) && x.IsActive == true);
                    UserEntity loggedUser = await metaData.User.FirstOrDefaultAsync(user => user.Id == id && user.IsActive == true);
                    RefreshTokenEntity userRefreshToken = await metaData.RefreshToken.FirstOrDefaultAsync(x => x.Id == loggedUser.RefreshTokenId && x.IsActive == true);
                    if (!loggedUser.RefreshTokenId.Equals(refreshToken.Id))
                        return Unauthorized("Invalid refresh token");
                    else if (userRefreshToken.TimeExpires < DateTime.Now)
                        return Unauthorized("Token expired");
                    else
                    {
                        string token = CreateToken(loggedUser.Id);
                        var newRT = CreateRefreshToken();
                        SetRefreshToken(newRT, loggedUser.Id);
                        return Ok(token);
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("api/get-rt"), Authorize]
        public async Task<ActionResult<RefreshTokenEntity>> GetRefreshToken([FromBody] UserDto user)
        {
            try
            {
                await RecipeController.GlobalAntiforgery.ValidateRequestAsync(HttpContext);
                RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c => c.AddDbProviderFactory(typeof(NpgsqlFactory)));
                using (var adapter = new DataAccessAdapter(_configuration.GetConnectionString("YumCityDb")))
                {
                    var metaData = new LinqMetaData(adapter);
                    var hasher = new PasswordHasher<UserEntity>();
                    UserEntity loggedUser = await metaData.User.FirstOrDefaultAsync(x => x.Name == user.Username && x.IsActive == true);
                    if (loggedUser == null)
                        return BadRequest("User not found!");
                    if (hasher.VerifyHashedPassword(loggedUser, loggedUser.PasswordHash, user.Password).Equals(PasswordVerificationResult.Success))
                    {
                        RefreshTokenEntity refreshToken = await metaData.RefreshToken.FirstOrDefaultAsync(x => x.Id == loggedUser.RefreshTokenId && x.IsActive == true);
                        return Ok(refreshToken.Token);
                    }
                    else
                        return BadRequest("Invalid user data!");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("api/get-id"), Authorize]
        public async Task<ActionResult<RefreshTokenEntity>> GetUserId([FromBody] UserDto user)
        {
            try
            {
                await RecipeController.GlobalAntiforgery.ValidateRequestAsync(HttpContext);
                RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c => c.AddDbProviderFactory(typeof(NpgsqlFactory)));
                using (var adapter = new DataAccessAdapter(_configuration.GetConnectionString("YumCityDb")))
                {
                    var metaData = new LinqMetaData(adapter);
                    var hasher = new PasswordHasher<UserEntity>();
                    UserEntity loggedUser = await metaData.User.FirstOrDefaultAsync(x => x.Name == user.Username && x.IsActive == true);
                    if (loggedUser == null)
                        return BadRequest("User not found!");
                    if (hasher.VerifyHashedPassword(loggedUser, loggedUser.PasswordHash, user.Password).Equals(PasswordVerificationResult.Success))
                    {
                        return Ok(loggedUser.Id);
                    }
                    else
                        return BadRequest("Invalid user data!");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private RefreshTokenEntity CreateRefreshToken()
        {
            var refreshToken = new RefreshTokenEntity
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                TimeExpires = DateTime.Now.AddDays(1),
                TimeCreated = DateTime.Now,
                Id = Guid.NewGuid(),
                IsActive = true
            };
            return refreshToken;
        }

        async private void SetRefreshToken(RefreshTokenEntity newRT, Guid id)
        {
            RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c => c.AddDbProviderFactory(typeof(NpgsqlFactory)));
            using (var adapter = new DataAccessAdapter(_configuration.GetConnectionString("YumCityDb")))
            {
                var metaData = new LinqMetaData(adapter);
                var cookiesOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = newRT.TimeExpires
                };
                UserEntity loggedUser = await metaData.User.FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true);
                RefreshTokenEntity oldRefreshToken = await metaData.RefreshToken.FirstOrDefaultAsync(x => x.Id == loggedUser.RefreshTokenId);
                if (oldRefreshToken != null)
                {
                    oldRefreshToken.IsActive = false;
                    await adapter.SaveEntityAsync(oldRefreshToken);
                }
                loggedUser.RefreshTokenId = newRT.Id;
                loggedUser.RefreshToken = newRT;
                await adapter.SaveEntityAsync(newRT);
                await adapter.SaveEntityAsync(loggedUser);
            }
        }

        private string CreateToken(Guid id)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,id.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(3),
                signingCredentials: cred,
                issuer: "Younes Abady",
                audience: "https://localhost:7024/"
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
