using Backend.DTO;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using SD.LLBLGen.Pro.DQE.PostgreSql;
using SD.LLBLGen.Pro.ORMSupportClasses;
using YumCity.DatabaseSpecific;
using YumCity.EntityClasses;
using YumCity.Linq;

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
            RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c => c.AddDbProviderFactory(typeof(Npgsql.NpgsqlFactory)));
            using (var adapter = new DataAccessAdapter(_configuration.GetConnectionString("YumCityDb")))
            {
                var metaData = new LinqMetaData(adapter);
                UserEntity user = metaData.User.FirstOrDefault(x => x.Name == newUser.Username);
                if (string.IsNullOrEmpty(newUser.Username) || string.IsNullOrEmpty(newUser.Password))
                    return BadRequest("Cant be empty");
                else if (user != null)
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
                    await adapter.SaveEntityAsync(user);
                    //await SaveData();
                    return Ok();
                }
            }
        }
    }
}
