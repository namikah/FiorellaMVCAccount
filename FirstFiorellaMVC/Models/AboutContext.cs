using System.Collections.Generic;

namespace FirstFiorellaMVC.Models
{
    public class AboutContext : IContext
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public string SubTitle { get ; set; }
    }
}
