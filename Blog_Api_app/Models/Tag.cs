namespace Blog_Api_app.Models
{
    public class Tag
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public ICollection<BlogPost> Blogs { get; set; }

    }
}
