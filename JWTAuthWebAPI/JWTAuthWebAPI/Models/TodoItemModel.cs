using System.ComponentModel.DataAnnotations;

namespace JWTAuthWebAPI.Models
{
    public class TodoItemModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required (ErrorMessage ="Completion Status is Required")]
        public bool IsComplete { get; set; }

       public User User { get; set; }

    }

    public class User {
       
        [Required(ErrorMessage = "User is Required")]
        public string UserId { get; set; }
        public string FullName { get; set; }

    }
}
