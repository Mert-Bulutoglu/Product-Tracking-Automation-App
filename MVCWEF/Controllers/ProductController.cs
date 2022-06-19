using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using MVCWEF.Models;

namespace MVCWEF.Controllers
{
    public class ProductController : Controller
    {
        string connectionString = @"Data Source=DESKTOP-B9HDJIB\SQLEXPRESS;Initial Catalog = MvcCrudDb; Integrated Security = True";
        public ActionResult Index()
        {
            DataTable dtblProduct = new DataTable();
            using(SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * From Product", sqlCon);
                sqlDa.Fill(dtblProduct);
            }
            return View(dtblProduct);
        }

      

        [HttpGet]
        public ActionResult Create()
        {
           
            return View();

        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(ProductModel productModel)
        {

            if (productModel.Price != 0 && productModel.Count != 0 && productModel.ProductName != "")
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {

                    sqlCon.Open();
                    string query = "INSERT INTO Product VALUES (@ProductName,@Price,@Count)";
                    SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                    sqlcmd.Parameters.AddWithValue("@ProductName", productModel.ProductName);
                    sqlcmd.Parameters.AddWithValue("@Price", productModel.Price);
                    sqlcmd.Parameters.AddWithValue("@Count", productModel.Count);
                    sqlcmd.ExecuteNonQuery();
                }
            }
            else
                TempData["hataMesaj"] = "Alanlar Boş Bırakılamaz";

                
            
            return View();
        }

        
        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            ProductModel productModel = new ProductModel();
            DataTable dtblProduct = new DataTable();
            using(SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM Product Where ProductID = @ProductID";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@ProductID",id);
                sqlDa.Fill(dtblProduct);
            }
            if (dtblProduct.Rows.Count == 1)
            {
                productModel.ProductId = Convert.ToInt32(dtblProduct.Rows[0][0].ToString());
                productModel.ProductName = dtblProduct.Rows[0][1].ToString();
                productModel.Price= Convert.ToDecimal(dtblProduct.Rows[0][2].ToString());
                productModel.Count= Convert.ToInt32(dtblProduct.Rows[0][3].ToString());
                return View(productModel);
            }
            else
                return RedirectToAction("Index");

 
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(ProductModel productModel)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "UPDATE Product SET ProductName = @ProductName, Price = @Price, Count = @Count Where ProductId=@ProductId";
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                sqlcmd.Parameters.AddWithValue("@ProductId", productModel.ProductId);
                sqlcmd.Parameters.AddWithValue("@ProductName", productModel.ProductName);
                sqlcmd.Parameters.AddWithValue("@Price", productModel.Price);
                sqlcmd.Parameters.AddWithValue("@Count", productModel.Count);
               
                sqlcmd.ExecuteNonQuery();

            }
            return RedirectToAction("Index");
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "DELETE FROM Product Where ProductId=@ProductId";
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                sqlcmd.Parameters.AddWithValue("@ProductId", id);
                sqlcmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }      
    }
}
