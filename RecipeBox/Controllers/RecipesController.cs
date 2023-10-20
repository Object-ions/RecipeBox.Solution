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
  // [Authorize]
  public class RecipesController : Controller
  {
    private readonly RecipeBoxContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public RecipesController(UserManager<ApplicationUser> userManager, RecipeBoxContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    // public async Task<ActionResult> Index() //new
    // {
    //   string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //new
    //   ApplicationUser currentUser = await _userManager.FindByIdAsync(userId); //new
    //   List<Recipe> userRecipes = _db.Recipes
    //                                 .Where(entry => entry.User.Id == currentUser.Id) //new
    //                                 .Include(recipe => recipe.Author)
    //                                 .ToList();
    //   return View(userRecipes);
    // }

    public ActionResult Index()
    {
      List<Recipe> recipeModel = _db.Recipes
                             .Include(recipe => recipe.Author)
                             .ToList();
      return View(recipeModel);
    }

    [Authorize]
    public ActionResult Create()
    {
      ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
      return View();
    }

    [Authorize]
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

    ////////////////////////
    //OLD
    ////////////////////
    // [Authorize]
    // public ActionResult Edit(int id)
    // {
    //   string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //new
    //   ApplicationUser currentUser = await _userManager.FindByIdAsync(userId); //new
    //   .Where(entry => entry.User.Id == currentUser.Id) //new

    //   Recipe thisRecipe = _db.Recipes.FirstOrDefault(re => re.RecipeId == id);
    //   ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
    //   return View(thisRecipe);
    // }

    [Authorize]
    public async Task<ActionResult> Edit(int id)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //new
      if (userId == null)
      {
        return Unauthorized(); // Ensure the userId is found. Otherwise, return an unauthorized result.
      }
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      Recipe thisRecipe = _db.Recipes
                            .Where(entry => entry.User.Id == currentUser.Id)
                            .FirstOrDefault(re => re.RecipeId == id);
      if (thisRecipe == null)
      {
        return NotFound(); // Return not found if no recipe matches the provided id for the current user.
      }
      ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
      return View(thisRecipe);
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    // NEW
    ////////////

    // [Authorize]
    // public async Task<ActionResult> Edit(int id)
    // {
    //   string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //   if (userId == null)
    //   {
    //       return Unauthorized(); // Ensure the userId is found. Otherwise, return an unauthorized result.
    //   }
    //   ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
    //   // Ensure the recipe belongs to the current user
    //   Recipe thisRecipe = _db.Recipes
    //                         .Where(entry => entry.User.Id == currentUser.Id)
    //                         .FirstOrDefault(re => re.RecipeId == id);
    //   if (thisRecipe == null)
    //   {
    //       return NotFound(); // Return not found if no recipe matches the provided id for the current user.
    //   }
    //   ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
    //   return View(thisRecipe);
    // }

    ////////////////////////////////////////////////////////////////////////////////////////

    [Authorize] //new
    [HttpPost]
    public async Task<ActionResult> Edit(Recipe recipe) //new
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //new
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId); //new
      recipe.User = currentUser; //new

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
    public async Task<ActionResult> DeleteConfirm(Recipe recipe, int id)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //new
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId); //new
      recipe.User = currentUser; //new
      
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