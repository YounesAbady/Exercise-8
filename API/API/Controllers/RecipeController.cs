using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SD.LLBLGen.Pro.DQE.PostgreSql;
using SD.LLBLGen.Pro.ORMSupportClasses;
using YumCity.DatabaseSpecific;
using YumCity.EntityClasses;
using YumCity.Linq;
namespace API.Controllers
{
    public class RecipeController : Controller
    {
        private readonly IAntiforgery _antiforgery;
        public static IAntiforgery GlobalAntiforgery;
        private readonly IConfiguration _configuration;

        public RecipeController(IAntiforgery antiforgery, IConfiguration config)
        {
            _antiforgery = antiforgery;
            _configuration = config;
        }

        [HttpGet]
        [Route("api/list-recipes"), Authorize]
        public async Task<ActionResult<RecipeEntity>> ListRecipes()
        {
            try
            {
                await GlobalAntiforgery.ValidateRequestAsync(HttpContext);
                RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c => c.AddDbProviderFactory(typeof(Npgsql.NpgsqlFactory)));
                using (var adapter = new DataAccessAdapter(_configuration.GetConnectionString("YumCityDb")))
                {
                    var metaData = new LinqMetaData(adapter);
                    if (metaData.Recipe.Count() == 0)
                        throw new InvalidOperationException("Cant be empty");
                    else
                        return Ok(metaData.Recipe.ToList().OrderBy(x => x.Title));
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("api/list-categories"), Authorize]
        public async Task<ActionResult<string>> ListCategories()
        {
            try
            {
                await GlobalAntiforgery.ValidateRequestAsync(HttpContext);
                RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c => c.AddDbProviderFactory(typeof(Npgsql.NpgsqlFactory)));
                using (var adapter = new DataAccessAdapter(_configuration.GetConnectionString("YumCityDb")))
                {
                    var metaData = new LinqMetaData(adapter);
                    if (metaData.AllCategory.Count() == 0)
                        throw new InvalidOperationException("Cant be empty");
                    else
                        return Ok(metaData.AllCategory.ToList().OrderBy(x => x.Data));
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("api/add-category"), Authorize]
        public async Task<ActionResult> AddCategory([FromBody] string category)
        {
            try
            {
                await GlobalAntiforgery.ValidateRequestAsync(HttpContext);
                RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c => c.AddDbProviderFactory(typeof(Npgsql.NpgsqlFactory)));
                using (var adapter = new DataAccessAdapter(_configuration.GetConnectionString("YumCityDb")))
                {
                    var metaData = new LinqMetaData(adapter);
                    if (string.IsNullOrEmpty(category))
                        return BadRequest("Cant be empty");
                    else
                    {
                        AllCategoryEntity newCategory = new AllCategoryEntity
                        {
                            Id = Guid.NewGuid(),
                            Data = category
                        };
                        await adapter.SaveEntityAsync(newCategory);
                        return Ok();
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("api/add-recipe"), Authorize]
        public async Task<ActionResult> AddRecipe([FromBody] RecipeEntity recipe)
        {
            try
            {
                await GlobalAntiforgery.ValidateRequestAsync(HttpContext);
                RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c => c.AddDbProviderFactory(typeof(Npgsql.NpgsqlFactory)));
                using (var adapter = new DataAccessAdapter(_configuration.GetConnectionString("YumCityDb")))
                {
                    var metaData = new LinqMetaData(adapter);
                    //recipe.Ingredients = recipe.Ingredients.Where(r => !string.IsNullOrWhiteSpace(r)).ToList();
                    //recipe.Instructions = recipe.Instructions.Where(r => !string.IsNullOrWhiteSpace(r)).ToList();
                    if (recipe.Ingredients.Count == 0 || recipe.Instructions.Count == 0 || recipe.Categories.Count == 0 || string.IsNullOrWhiteSpace(recipe.Title))
                        return BadRequest("Cant be empty");
                    else
                    {
                        await adapter.SaveEntityAsync(recipe);
                        return Ok();
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("api/delete-category"), Authorize]
        public async Task<ActionResult> DeleteCategory([FromBody] Guid id)
        {
            try
            {
                await GlobalAntiforgery.ValidateRequestAsync(HttpContext);
                RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c => c.AddDbProviderFactory(typeof(Npgsql.NpgsqlFactory)));
                using (var adapter = new DataAccessAdapter(_configuration.GetConnectionString("YumCityDb")))
                {
                    var metaData = new LinqMetaData(adapter);
                    if (string.IsNullOrEmpty(id.ToString()))
                        throw new InvalidOperationException("Cant be empty");
                    else
                    {
                        AllCategoryEntity category = metaData.AllCategory.FirstOrDefault(c => c.Id == id);
                        CategoryEntity selectedCategory;
                        await adapter.DeleteEntityAsync(category);
                        foreach (RecipeEntity recipe in metaData.Recipe)
                        {
                            selectedCategory = recipe.Categories.FirstOrDefault(x => x.Id == id);
                            if (selectedCategory is not null)
                            {
                                recipe.Categories.Remove(selectedCategory);
                                await adapter.SaveEntityAsync(recipe);
                            }
                        }
                        return Ok();
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("api/update-category/{id}"), Authorize]
        public async Task<ActionResult> UpdateCategory(Guid id, [FromBody] string newCategory)
        {
            try
            {
                await GlobalAntiforgery.ValidateRequestAsync(HttpContext);
                RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c => c.AddDbProviderFactory(typeof(Npgsql.NpgsqlFactory)));
                using (var adapter = new DataAccessAdapter(_configuration.GetConnectionString("YumCityDb")))
                {
                    var metaData = new LinqMetaData(adapter);
                    AllCategoryEntity category = metaData.AllCategory.FirstOrDefault(c => c.Id == id);
                    if (string.IsNullOrEmpty(newCategory))
                        throw new InvalidOperationException("Cant be empty");
                    else
                    {
                        CategoryEntity selectedCategory;
                        foreach (RecipeEntity recipe in metaData.Recipe)
                        {
                            selectedCategory = recipe.Categories.FirstOrDefault(x => x.Data == category.Data);
                            if (selectedCategory is not null)
                            {
                                recipe.Categories.FirstOrDefault(selectedCategory).Data = newCategory;
                                await adapter.SaveEntityAsync(recipe);
                            }
                        }
                        category.Data = newCategory;
                        await adapter.SaveEntityAsync(category);
                        return Ok();
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("api/antiforgery"), Authorize]
        public void GetAntiforgery()
        {
            GlobalAntiforgery = _antiforgery;
            var tokens = GlobalAntiforgery.GetAndStoreTokens(HttpContext);
            HttpContext.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken!, new CookieOptions { HttpOnly = false, SameSite = SameSiteMode.None, Secure = true });
        }
    }
}
