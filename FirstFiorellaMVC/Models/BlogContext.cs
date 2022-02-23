namespace FirstFiorellaMVC.Models
{
    public class BlogContext : IContext
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }
    }
}
