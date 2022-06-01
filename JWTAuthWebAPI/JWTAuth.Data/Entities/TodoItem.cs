namespace JWTAuth.Data.Entities
{ 
    public class TodoItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public string UserId { get; set; } 
        public ApplicationUser User { get; set; }
    }

}
