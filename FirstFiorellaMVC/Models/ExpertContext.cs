namespace FirstFiorellaMVC.Models
{
    public class ExpertContext : IContext
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }
    }
}
