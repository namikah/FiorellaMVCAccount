using FirstFiorellaMVC.Models;
using System.Collections.Generic;

namespace FirstFiorellaMVC.ViewModels
{
    public class HomeViewModel
    {
        public List<SliderImage> SliderImages { get; set; }

        public Slider Slider { get; set; }

        public List<Product> Products { get; set; }

        public List<Category> Categories { get; set; }

        public AboutImage AboutImage { get; set; }

        public IContext AboutContext { get; set; }

        public  List<AboutUnstyledList> AboutUnstyledLists { get; set; }

        public List<Expert> Experts { get; set; }

        public List<Position> Positions { get; set; }

        public IContext ExpertContext { get; set; }

        public Subcribe Subcribe { get; set; }

        public List<Blog> Blogs { get; set; }

        public IContext BlogContext { get; set; }

        public List<Author> Authors { get; set; }

        public List<Instagram> Instagrams { get; set; }

        public List<Social> Socials { get; set; }

        public List<ProductImage> ProductImages { get; set; }

        public List<ProductImage> ImagesByProductId { get; set; }

        public List<Campaign> Campaigns { get; set; }

    }
}
