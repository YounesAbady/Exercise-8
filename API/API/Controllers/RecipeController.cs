using API.DTO;
using LLBLGen.Linq.Prefetch;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SD.LLBLGen.Pro.DQE.PostgreSql;
using SD.LLBLGen.Pro.LinqSupportClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using YumCity_Migrations.DatabaseSpecific;
using YumCity_Migrations.EntityClasses;
using YumCity_Migrations.Linq;

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
                    var recipes = await metaData.Recipe.Where(p => p.IsActive).With(p => p.Ingredients.Where(x => x.IsActive), p => p.Instructions.Where(x => x.IsActive), p => p.RecipeCategories.Where(x => x.IsActive)).OrderBy(p => p.Title).ToListAsync();
                    if (recipes.Count() == 0)
                        throw new InvalidOperationException("Cant be empty");
                    else
                    {
                        return Ok(recipes);
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
        public async Task<ActionResult> AddRecipe([FromBody] RecipeDto recipeDto)
        {
            try
            {
                await GlobalAntiforgery.ValidateRequestAsync(HttpContext);
                RecipeEntity recipe = Convert(recipeDto);
                RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c => c.AddDbProviderFactory(typeof(Npgsql.NpgsqlFactory)));
                using (var adapter = new DataAccessAdapter(_configuration.GetConnectionString("YumCityDb")))
                {
                    var metaData = new LinqMetaData(adapter);
                    if (recipe is null)
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

        [HttpPut]
        [Route("api/update-recipe/{id}"), Authorize]
        public async Task<ActionResult> UpdateRecipe([FromBody] RecipeDto newRecipe, Guid id)
        {
            try
            {
                await GlobalAntiforgery.ValidateRequestAsync(HttpContext);
                RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c => c.AddDbProviderFactory(typeof(Npgsql.NpgsqlFactory)));
                using (var adapter = new DataAccessAdapter(_configuration.GetConnectionString("YumCityDb")))
                {
                    var metaData = new LinqMetaData(adapter);
                    RecipeEntity oldRecipe = await metaData.Recipe.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
                    newRecipe.Ingredients = newRecipe.Ingredients.Where(r => !string.IsNullOrWhiteSpace(r)).ToList();
                    newRecipe.Instructions = newRecipe.Instructions.Where(r => !string.IsNullOrWhiteSpace(r)).ToList();
                    if (newRecipe.Ingredients.Count == 0 || newRecipe.Instructions.Count == 0 || newRecipe.Categories.Count == 0 || string.IsNullOrWhiteSpace(newRecipe.Title) || id == Guid.Empty)
                        throw new InvalidOperationException("Cant be empty");
                    else
                    {
                        oldRecipe.Title = newRecipe.Title;
                        RecipeEntity recipe = Convert(newRecipe);
                        await adapter.DeleteEntityAsync(oldRecipe);
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
        [Route("api/delete-recipe"), Authorize]
        public async Task<ActionResult> DeleteRecipe([FromBody] Guid id)
        {
            try
            {
                await GlobalAntiforgery.ValidateRequestAsync(HttpContext);
                if (id == Guid.Empty)
                    throw new InvalidOperationException("Cant be empty");
                else
                {
                    RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c => c.AddDbProviderFactory(typeof(Npgsql.NpgsqlFactory)));
                    using (var adapter = new DataAccessAdapter(_configuration.GetConnectionString("YumCityDb")))
                    {
                        var metaData = new LinqMetaData(adapter);
                        RecipeEntity recipe = await metaData.Recipe.FirstOrDefaultAsync(x => x.Id == id);
                        recipe.IsActive = false;
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

        [HttpGet]
        [Route("api/antiforgery"), Authorize]
        public void GetAntiforgery()
        {
            GlobalAntiforgery = _antiforgery;
            var tokens = GlobalAntiforgery.GetAndStoreTokens(HttpContext);
            HttpContext.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken!, new CookieOptions { HttpOnly = false, SameSite = SameSiteMode.None, Secure = true });
        }

        private RecipeEntity Convert(RecipeDto recipeDto)
        {
            RecipeEntity recipe = new();
            recipe.IsActive = true;
            if (string.IsNullOrEmpty(recipeDto.Id.ToString()) || Guid.Equals(recipeDto.Id, Guid.Empty))
                recipe.Id = Guid.NewGuid();
            else
                recipe.Id = recipeDto.Id;
            recipe.Title = recipeDto.Title;
            recipeDto.Ingredients = recipeDto.Ingredients.Where(r => !string.IsNullOrWhiteSpace(r)).ToList();
            recipeDto.Instructions = recipeDto.Instructions.Where(r => !string.IsNullOrWhiteSpace(r)).ToList();
            if (recipeDto.Ingredients.Count == 0 || recipeDto.Instructions.Count == 0 || recipeDto.Categories.Count == 0 || string.IsNullOrWhiteSpace(recipeDto.Title))
                return null;
            else
            {
                foreach (var item in recipeDto.Ingredients)
                {
                    IngredientEntity ingredient = new()
                    {
                        Id = Guid.NewGuid(),
                        Data = item,
                        RecipeId = recipe.Id,
                        IsActive = true
                    };
                    recipe.Ingredients.Add(ingredient);
                }
                foreach (var item in recipeDto.Instructions)
                {
                    InstructionEntity instruction = new()
                    {
                        Id = Guid.NewGuid(),
                        Data = item,
                        RecipeId = recipe.Id,
                        IsActive = true
                    };
                    recipe.Instructions.Add(instruction);
                }
                foreach (var item in recipeDto.Categories)
                {
                    RecipeCategoryEntity recipeCategory = new()
                    {
                        Id = Guid.NewGuid(),
                        Data = item,
                        RecipeId = recipe.Id,
                        IsActive = true
                    };
                    recipe.RecipeCategories.Add(recipeCategory);
                }
                recipe.UserId = recipeDto.UserId;
                return recipe;
            }
        }
    }
}
