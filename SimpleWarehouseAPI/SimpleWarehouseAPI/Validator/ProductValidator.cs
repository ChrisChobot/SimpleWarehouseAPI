using SimpleWarehouseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWarehouseAPI.Validator
{
    public static class ProductValidator
    {
        public static bool ValidateData(Product product, out List<string> errorList)
        {
            errorList = new List<string>();

            bool isIdOk = CheckId(product.Id, ref errorList);
            bool isNameOk = CheckName(product.Name, ref errorList);
            bool isPriceOk = CheckPrice(product.Price, ref errorList);

            return isIdOk && isNameOk && isPriceOk;
        }

        public static bool ValidateData(ProductCreateInputModel productCreate, out List<string> errorList)
        {
            errorList = new List<string>();

            bool isNameOk = CheckName(productCreate.Name, ref errorList);
            bool isPriceOk = CheckPrice(productCreate.Price, ref errorList);

            return isNameOk && isPriceOk;
        }

        public static bool ValidateData(ProductUpdateInputModel product, MainDbContext context, out List<string> errorList)
        {
            errorList = new List<string>();

            bool isIdOk = CheckId(product.Id, ref errorList);
            bool idExistInDb = isIdOk && context.Products.FirstOrDefault((x) => x.Id == product.Id) != null;
            bool isNameOk = CheckName(product.Name, ref errorList);
            bool isPriceOk = CheckPrice(product.Price, ref errorList);

            return isIdOk && idExistInDb && isNameOk && isPriceOk;
        }

        public static bool IsIdOk(Guid? id)
        {
            List<string> errorList = new List<string>();
            return CheckId(id, ref errorList);
        }

        private static bool CheckId(Guid? id, ref List<string> errorList)
        {
            if (id != null && id != Guid.Empty)
            {
                return true;
            }
            else
            {
                errorList.Add("Invalid Id");
                return false;
            }
        }

        private static bool CheckName(string name, ref List<string> errorList)
        {
            if (name != null && !string.IsNullOrWhiteSpace(name))
            {
                if (name.Length <= 100)
                {
                    return true;
                }
                else
                {
                    errorList.Add("Name is too long");
                    return false;
                }
            }
            else
            {
                errorList.Add("Invalid Name");
                return false;
            }
        }

        private static bool CheckPrice(decimal price, ref List<string> errorList)
        {
            if (price >= 0)
            {
                return true;
            }
            else
            {
                errorList.Add("Invalid Price");
                return false;
            }
        }
    }
}
