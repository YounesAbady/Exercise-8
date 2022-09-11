using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SD.LLBLGen.Pro.DQE.PostgreSql;
using SD.LLBLGen.Pro.ORMSupportClasses;
using YumCity_Migrations.DatabaseSpecific;
using YumCity_Migrations.EntityClasses;
using YumCity_Migrations.Linq;

namespace API.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IConfiguration _configuration;

        public CategoryController(IConfiguration config)
        {
            _configuration = config;
        }

        [HttpGet]
        [Route("api/list-categories"), Authorize]
        public async Task<ActionResult<string>> ListCategories()
        {
            try
            {
                await RecipeController.GlobalAntiforgery.ValidateRequestAsync(HttpContext);
                RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c => c.AddDbProviderFactory(typeof(Npgsql.NpgsqlFactory)));
                using (var adapter = new DataAccessAdapter(_configuration.GetConnectionString("YumCityDb")))
                {
                    var metaData = new LinqMetaData(adapter);
                    var categories = metaData.Category.OrderBy(x => x.Data).ToList();
                    if (categories.Count() == 0)
                        throw new InvalidOperationException("Cant be empty");
                    else
                        return Ok(categories);
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
                await RecipeController.GlobalAntiforgery.ValidateRequestAsync(HttpContext);
                RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c => c.AddDbProviderFactory(typeof(Npgsql.NpgsqlFactory)));
                using (var adapter = new DataAccessAdapter(_configuration.GetConnectionString("YumCityDb")))
                {
                    var metaData = new LinqMetaData(adapter);
                    var categories = metaData.Category.ToList();
                    if (string.IsNullOrEmpty(category))
                        return BadRequest("Cant be empty");
                    else
                    {
                        foreach (var item in categories)
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
                await RecipeController.GlobalAntiforgery.ValidateRequestAsync(HttpContext);
                RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c => c.AddDbProviderFactory(typeof(Npgsql.NpgsqlFactory)));
                using (var adapter = new DataAccessAdapter(_configuration.GetConnectionString("YumCityDb")))
                {
                    var metaData = new LinqMetaData(adapter);
                    if (string.IsNullOrEmpty(id.ToString()))
                        throw new InvalidOperationException("Cant be empty");
                    else
                    {
                        CategoryEntity category = metaData.Category.FirstOrDefault(c => c.Id == id);
                        var recipeCategories = metaData.RecipeCategory;
                        foreach (RecipeCategoryEntity recipeCategory in recipeCategories)
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
                await RecipeController.GlobalAntiforgery.ValidateRequestAsync(HttpContext);
                RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c => c.AddDbProviderFactory(typeof(Npgsql.NpgsqlFactory)));
                using (var adapter = new DataAccessAdapter(_configuration.GetConnectionString("YumCityDb")))
                {
                    var metaData = new LinqMetaData(adapter);
                    CategoryEntity category = metaData.Category.FirstOrDefault(c => c.Id == id);
                    if (string.IsNullOrEmpty(newCategory))
                        throw new InvalidOperationException("Cant be empty");
                    else
                    {
                        var recipeCategories = metaData.RecipeCategory;
                        foreach (RecipeCategoryEntity recipeCategory in recipeCategories)
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
    }
}
