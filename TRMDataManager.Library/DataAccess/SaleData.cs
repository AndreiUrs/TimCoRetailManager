using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
    public class SaleData
    {

        public void SaveSale(SaleModel saleInfo,string cashierId)
        {
            //TODO: Make this SOLID/DRY/BETTER
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            ProductData products = new ProductData();
            var taxRate = ConfigHelper.GetTaxRate()/100;

            //fill in the available information
            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                //Get info about this product
                var productInfo = products.GetProductById(detail.ProductId);

                if (productInfo == null)
                {
                    throw new Exception($"The product id of {detail.ProductId} could not be found in the database");
                }

                detail.PurchasePrice = (productInfo.RetailPrice * detail.Quantity);

                if (productInfo.IsTaxable)
                {
                    detail.Tax = (detail.PurchasePrice * taxRate);
                }
                details.Add(detail);
            }

            //Create Sale model
            SaleDBModel sale = new SaleDBModel {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x=>x.Tax),
                CashierId = cashierId
            };
            sale.Total = sale.SubTotal + sale.Tax;

            //Save the sale model
            SqlDataAccess sql = new SqlDataAccess();
            sql.SaveData("dbo.spSale_Insert", sale, "TRMData");

            //Get the ID from sale model
            sale.Id = sql.LoadData<int, dynamic>("dbo.spSale_Lookup", new { sale.CashierId, sale.SaleDate }, "TRMData").FirstOrDefault();

            //Finish filling in the sale detail model
            foreach (var item in details)
            {
                item.SaleId = sale.Id;
             
                //Save the sale detail model
                sql.SaveData("dbo.spSaleDetail_Insert", item, "TRMData");
            }
        }
    }
}
