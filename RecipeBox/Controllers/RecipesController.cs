using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace RecipeBox.Controllers
{
  [Authorize]
  public class RecipesController : Controller
  {
    private readonly RecipeBoxContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public RecipesController(UserManager<ApplicationUser> userManager, RecipeBoxContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public async Task<ActionResult> Index() //new
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //new
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId); //new
      List<Recipe> userRecipes = _db.Recipes
                                    .Where(entry => entry.User.Id == currentUser.Id) //new
                                    .Include(recipe => recipe.Author)
                                    .ToList();
      return View(userRecipes);
    }

    // public ActionResult Index()
    // {
    //   List<Recipe> recipeModel = _db.Recipes
    //                          .Include(recipe => recipe.Author)
    //                          .ToList();
    //   return View(recipeModel);
    // }

    public ActionResult Create()
    {
      ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Recipe recipe, int AuthorId) //new
    {
      if (!ModelState.IsValid)
      {
        ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
        return View(recipe);
      }
      else
      {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //new
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId); //new
        recipe.User = currentUser; //new
        _db.Recipes.Add(recipe);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

    // [HttpPost]
    // public ActionResult Create(Recipe recipe)
    // {
    //   if (!ModelState.IsValid)
    //   {
    //     ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
    //     return View(recipe);
    //   }
    //   else
    //   {
    //     _db.Recipes.Add(recipe);
    //     _db.SaveChanges();
    //     return RedirectToAction("Index");
    //   }
    // }

    public ActionResult Edit(int id)
    {
      Recipe thisRecipe = _db.Recipes.FirstOrDefault(re => re.RecipeId == id);
      ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
      return View(thisRecipe);
    }

    [HttpPost]
    public ActionResult Edit(Recipe recipe)
    {
      _db.Recipes.Update(recipe);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Recipe thisRecipe = _db.Recipes
                                .Include(re => re.Author)
                                .Include(je => je.JoinEntities)
                                .ThenInclude(tag => tag.Tag)
                                .FirstOrDefault(re => re.RecipeId == id);
      return View(thisRecipe);
    }

    public ActionResult Delete(int id)
    {
      Recipe thisRecipe = _db.Recipes.FirstOrDefault(re => re.RecipeId == id);
      return View(thisRecipe);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirm(int id)
    {
      Recipe thisRecipe = _db.Recipes.FirstOrDefault(re => re.RecipeId == id);
      _db.Recipes.Remove(thisRecipe);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddTag(int id)
    {
      Recipe thisRecipe = _db.Recipes.FirstOrDefault(re => re.RecipeId == id);
      ViewBag.TagId = new SelectList(_db.Tags, "TagId", "Name");
      return View(thisRecipe);
    }

    [HttpPost]
    public ActionResult AddTag(Recipe recipe, int tagId)
    {
      #nullable enable
      RecipeTag? joinEntity = _db.RecipeTags.FirstOrDefault(join => (join.TagId == tagId && join.RecipeId == recipe.RecipeId));
      #nullable disable
      if (joinEntity == null && tagId != 0)
      {
        _db.RecipeTags.Add(new RecipeTag() {TagId = tagId, RecipeId = recipe.RecipeId});
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = recipe.RecipeId});
    }

    public ActionResult DeleteTag(int id)
    {
      Recipe thisRecipe = _db.Recipes
                                .Include(re => re.Author)
                                .Include(je => je.JoinEntities)
                                .ThenInclude(tag => tag.Tag)
                                .FirstOrDefault(re => re.RecipeId == id);
      return View(thisRecipe);
    }

    [HttpPost, ActionName("DeleteTag")]
    public ActionResult DeleteTagConfirm(int joinId)
    {
      RecipeTag joinEntry = _db.RecipeTags.FirstOrDefault(entry => entry.RecipeTagId == joinId);
      _db.RecipeTags.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}