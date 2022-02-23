namespace FirstFiorellaMVC.Models
{
    public class Author
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public int PositionId { get; set; }

        public Position Position { get; set; }
    }
}
