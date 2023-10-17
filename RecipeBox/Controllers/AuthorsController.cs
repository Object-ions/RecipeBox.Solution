using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecipeBox.Controllers
{
  public class AuthorsController : Controller
  {
    private readonly RecipeBoxContext _db;

    public AuthorsController(RecipeBoxContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Author> model = _db.Authors.ToList();
      return View(model);
    }

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