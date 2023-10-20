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
  public class AuthorsController : Controller
  {
    private readonly RecipeBoxContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthorsController(UserManager<ApplicationUser> userManager, RecipeBoxContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    // public async Task<ActionResult> Index()
    //   string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //   ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
    //   string currentAuthor = _db.Authors
    //                             .

    public ActionResult Index()
    {
      List<Author> model = _db.Authors.ToList();
      return View(model);
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

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Author author)
    {
      _db.Authors.Add(author);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Author thisAuthor = _db.Authors
                            .Include(author => author.Recipes)
                            .ThenInclude(recipe => recipe.JoinEntities)
                            .ThenInclude(join => join.Tag)
                            .FirstOrDefault(author => author.AuthorId == id);
      return View(thisAuthor);
    }
    
    public ActionResult Edit (int id)
    {
      Author thisAuthor = _db.Authors.FirstOrDefault(au => au.AuthorId == id);
      return View(thisAuthor);
    }

    [HttpPost]
    public ActionResult Edit (Author author)
    {
      _db.Authors.Update(author);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Author thisAuthor = _db.Authors.FirstOrDefault(a => a.AuthorId == id);
      return View(thisAuthor);
    }

    [HttpPost, ActionName ("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Author thisAuthor = _db.Authors.FirstOrDefault(a => a.AuthorId == id);
      _db.Authors.Remove(thisAuthor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}