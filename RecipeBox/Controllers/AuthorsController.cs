using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RecipeBox.Controllers
{
  public class AuthorsController : Controller
  {
    private readonly RecipeBoxContext _db;

    public AuthorsController(RecipeBoxContext db)
    {
      _db = db;
    }
  }
}