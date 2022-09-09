using API.DTO;
using LLBLGen.Linq.Prefetch;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SD.LLBLGen.Pro.DQE.PostgreSql;
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
                    if (metaData.Recipe.Count() == 0)
                        throw new InvalidOperationException("Cant be empty");
                    else
                    {
                        List<RecipeEntity> recipes = new List<RecipeEntity>();
                        foreach (var item in metaData.Recipe)
                        {
                            //.Include() didnt work for me :(
                            foreach (var x in metaData.Ingredient)
                            {
                                if (item.Id == x.RecipeId)
                                    item.Ingredients.Add(x);
                            }
                            foreach (var x in metaData.Instruction)
                            {
                                if (item.Id == x.RecipeId)
                                    item.Instructions.Add(x);
                            }
                            foreach (var x in metaData.RecipeCategory)
                            {
                                if (item.Id == x.RecipeId)
                                    item.RecipeCategories.Add(x);
                            }
                            recipes.Add(item);
                        }
                        return Ok(recipes.ToList().OrderBy(x => x.Title));
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
                    if (recipe.Ingredients.Count == 0 || recipe.Instructions.Count == 0 || recipe.RecipeCategories.Count == 0 || string.IsNullOrWhiteSpace(recipe.Title))
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
                if (id == Guid.Empty || newRecipe.Ingredients.Count == 0 || newRecipe.Instructions.Count == 0 || newRecipe.Categories.Count == 0 || string.IsNullOrWhiteSpace(newRecipe.Title))
                    throw new InvalidOperationException("Cant be empty");
                else
                {
                    RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c => c.AddDbProviderFactory(typeof(Npgsql.NpgsqlFactory)));
                    using (var adapter = new DataAccessAdapter(_configuration.GetConnectionString("YumCityDb")))
                    {
                        var metaData = new LinqMetaData(adapter);
                        RecipeEntity oldRecipe = metaData.Recipe.FirstOrDefault(x => x.Id == id);
                        newRecipe.Ingredients = newRecipe.Ingredients.Where(r => !string.IsNullOrWhiteSpace(r)).ToList();
                        newRecipe.Instructions = newRecipe.Instructions.Where(r => !string.IsNullOrWhiteSpace(r)).ToList();
                        if (newRecipe.Ingredients.Count == 0 || newRecipe.Instructions.Count == 0 || newRecipe.Categories.Count == 0 || string.IsNullOrWhiteSpace(newRecipe.Title))
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
                        RecipeEntity recipe = metaData.Recipe.FirstOrDefault(x => x.Id == id);
                        await adapter.DeleteEntityAsync(recipe);
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
                    if (metaData.Category.Count() == 0)
                        throw new InvalidOperationException("Cant be empty");
                    else
                        return Ok(metaData.Category.ToList().OrderBy(x => x.Data));
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
                        foreach (var item in metaData.Category)
                        {
                            if (item.Data == category)
                            {
                                return BadRequest("Category already exists!");
                            }
                        }
                        CategoryEntity newCategory = new CategoryEntity
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
                        CategoryEntity category = metaData.Category.FirstOrDefault(c => c.Id == id);
                        foreach (RecipeCategoryEntity recipeCategory in metaData.RecipeCategory)
                        {
                            if (recipeCategory.Data == category.Data)
                            {
                                await adapter.DeleteEntityAsync(recipeCategory);
                            }
                        }
                        await adapter.DeleteEntityAsync(category);
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
                    CategoryEntity category = metaData.Category.FirstOrDefault(c => c.Id == id);
                    if (string.IsNullOrEmpty(newCategory))
                        throw new InvalidOperationException("Cant be empty");
                    else
                    {
                        foreach (RecipeCategoryEntity recipeCategory in metaData.RecipeCategory)
                        {
                            if (recipeCategory.Data == category.Data)
                            {
                                recipeCategory.Data = newCategory;
                                await adapter.SaveEntityAsync(recipeCategory);
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

        private RecipeEntity Convert(RecipeDto recipeDto)
        {
            RecipeEntity recipe = new();
            if (string.IsNullOrEmpty(recipeDto.Id.ToString()) || Guid.Equals(recipeDto.Id, Guid.Empty))
                recipe.Id = Guid.NewGuid();
            else
                recipe.Id = recipeDto.Id;
            recipe.Title = recipeDto.Title;
            recipeDto.Ingredients = recipeDto.Ingredients.Where(r => !string.IsNullOrWhiteSpace(r)).ToList();
            recipeDto.Instructions = recipeDto.Instructions.Where(r => !string.IsNullOrWhiteSpace(r)).ToList();
            foreach (var item in recipeDto.Ingredients)
            {
                IngredientEntity ingredient = new()
                {
                    Id = Guid.NewGuid(),
                    Data = item,
                    RecipeId = recipe.Id
                };
                recipe.Ingredients.Add(ingredient);
            }
            foreach (var item in recipeDto.Instructions)
            {
                InstructionEntity instruction = new()
                {
                    Id = Guid.NewGuid(),
                    Data = item,
                    RecipeId = recipe.Id
                };
                recipe.Instructions.Add(instruction);
            }
            foreach (var item in recipeDto.Categories)
            {
                RecipeCategoryEntity recipeCategory = new()
                {
                    Id = Guid.NewGuid(),
                    Data = item,
                    RecipeId = recipe.Id
                };
                recipe.RecipeCategories.Add(recipeCategory);
            }
            recipe.UserId = recipeDto.UserId;
            return recipe;
        }
    }
}
