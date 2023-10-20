using System.ComponentModel.DataAnnotations;

namespace RecipeBox.ViewModels
{
  public class RegisterViewModel
  {
    [Required] //remove Author Name since Name exists in Author Model. Then in the create Author, somehow tie to User.
    [Display(Name = "Author Name")] //see above
    public string AuthorName { get; set; } //see above

    [Required]
    [EmailAddress]
    [Display(Name = "Email Address")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
  }
}

    //[Required]
    // [DataType(DataType.Password)]
    // [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{6,}$", ErrorMessage = "Your password must contain at least six characters, a capital letter, a lowercase letter, a number, and a special character.")]
    // public string Password { get; set; }  