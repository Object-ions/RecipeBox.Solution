using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecipeBox.Controllers
{
  public class TagsController : Controller
  {
    private readonly RecipeBoxContext _db;

    public TagsController(RecipeBoxContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Tags.ToList());
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Tag tag)
    {
      if (!ModelState.IsValid)
      {
        return View(tag);
      }
      
      {
        _db.Tags.Add(tag);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

    public ActionResult Details(int id)
    {
      Tag thisTag = _db.Tags.Include(t => t.JoinEntities)
                                        .ThenInclude(join => join.Recipe)
                                        .FirstOrDefault(t => t.TagId == id);
      return View(thisTag);
    }
    
    public ActionResult Edit(int id)
    {
      Tag thisTag = _db.Tags.FirstOrDefault(p => p.TagId == id);
      return View(thisTag);
    }

    [HttpPost]
    public ActionResult Edit(Tag tag)
    {
      _db.Tags.Update(tag);
      _db.SaveChanges();
      return RedirectToAction ("Index");
    }
    

    public ActionResult Delete(int id)
    {
      Tag thisTag = _db.Tags.FirstOrDefault(p => p.TagId == id);
      return View(thisTag);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Tag thisTag = _db.Tags.FirstOrDefault(p => p.TagId == id);
      _db.Tags.Remove(thisTag);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddRecipe(int id)
    {
      Tag thisTag = _db.Tags.FirstOrDefault(p => p.TagId == id);
      ViewBag.RecipeId = new SelectList(_db.Recipes, "RecipeId", "Name");
      return View(thisTag);
    }

    [HttpPost]
    public ActionResult AddRecipe(Tag tag, int recipeId)
    {
      #nullable enable
      RecipeTag? joinEntity = _db.RecipeTags.FirstOrDefault(join => (join.RecipeId == recipeId && join.TagId == tag.TagId));
      #nullable disable
      if (joinEntity == null && recipeId != 0)
      {
        _db.RecipeTags.Add(new RecipeTag() {
          RecipeId = recipeId, TagId = tag.TagId
        });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = tag.TagId });
    }
  }
}