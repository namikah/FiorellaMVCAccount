using FirstFiorellaMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FirstFiorellaMVC.DataAccessLayer
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {

        }

        public DbSet<Slider> Sliders { get; set; }

        public DbSet<SliderImage> SliderImages { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<AboutImage> AboutImages { get; set; }

        public DbSet<AboutContext> AboutContexts { get; set; }

        public DbSet<AboutUnstyledList> AboutUnstyledLists { get; set; }

        public DbSet<Expert> Experts { get; set; }

        public DbSet<ExpertContext> ExpertContexts { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Subcribe> Subcribes { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<BlogContext> BlogContexts { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Instagram> Instagrams { get; set; }

        public DbSet<Social> Socials { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<Campaign> Campaigns { get; set; }

        public DbSet<Bio> Bios { get; set; }
    }
}
