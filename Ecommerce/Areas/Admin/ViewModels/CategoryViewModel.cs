using Ecommerce.Models;

namespace Ecommerce.Areas.Admin.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public List<ProductCategory>? productCategories { get; set; }
    }
}
