using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
    public class SaleData
    {

        public void SaveSale(SaleModel sale)
        {
            //TODO: Make this SOLID/DRY/BETTER
            
            //start filling in the  sale detail models we need to save for the database
            //fill in the available information
            //creat the sale model
            //save the sale model
            //get the id from the sale model
            //finish filling in the sale details models
            //save the sale detail model
        }

    //    public List<ProductModel> GetProducts()
    //    {
    //        SqlDataAccess sql = new SqlDataAccess();

    //        var output = sql.LoadData<ProductModel, dynamic>("[dbo].[spProduct_GetAll]", new { }, "TRMData");
    //        return output;
    //    }
    }
}
