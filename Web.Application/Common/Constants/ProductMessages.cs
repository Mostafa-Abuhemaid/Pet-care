using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.Common.Constants
{
    public static class ProductMessages
    {
        public const string CategoryNotFound = "The specified category does not exist.";
        public const string ProductNotFound = "Product with ID was not found.";
        public const string NoProductsFound = "No products match the provided filters.";
        public const string ProductsRetrieved = "Products retrieved successfully.";
        public const string InvalidSortColumn = "Invalid sorting column provided.";
    }

}
