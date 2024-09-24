using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POSApi.Model;
using POSApi.Services;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace POSApi.Controllers
{
    [Route("api/POSda")]
    [ApiController]
    public class POS : Controller
    {
        private readonly HttpClient _http;
        private readonly string _conString;
        private readonly string _AudiencePOS;
        private readonly string _IssuerPOS;
        private readonly string _SigningKeyPOS;


        POSService pOSService = new POSService();

        public POS(HttpClient http, IConfiguration config)
        {
            _http = http;
            _conString = config.GetValue<string>("ConnectionStrings:POS");
            _AudiencePOS = config.GetValue<string>("AuthPOS:Audience");
            _IssuerPOS = config.GetValue<string>("AuthPOS:Issuer");
            _SigningKeyPOS = config.GetValue<string>("AuthPOS:IssuerSigningKey");
        }

        #region LOGIN
        [HttpPost("Login")]
        public async Task<IActionResult> Login(reqLog file)
        {
            IActionResult actionResult = null;
            DataTable dt = new DataTable();
            ResultModel<respLogin> res = new ResultModel<respLogin>();
            res.Data = new respLogin();

            try
            {
                dt = pOSService.Login(file,_conString);
                if (dt.Rows.Count == 1)
                {
                    foreach (DataRow d in dt.Rows)
                    {
                        res.Data.Names = d["Name"].ToString();
                        res.Data.Usernames = d["Username"].ToString();
                    }

                    res.Data.Token = pOSService.CreateJwtToken(_AudiencePOS,_IssuerPOS,_SigningKeyPOS,res.Data.Names,res.Data.Usernames, file);
                }

                res.Data = res.Data;
                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }
        #endregion LOGIN


        [HttpGet("test")]
        public async Task<string> gettest()
        {
            string a = "Berhasil";

            return a;
        }

        #region Item Management
        [HttpPost("PostItem")]
        [Authorize(AuthenticationSchemes = "BetaClient")]
        public async Task<IActionResult> PostItem(List<respItem> item)
        {
            IActionResult actionResult = null;
            ResultModel<List<respItem>> res = new ResultModel<List<respItem>>();
            DateTime dt = DateTime.Now;
            try
            {
                bool val = pOSService.PostItem(POSService.ListToDataTable<respItem>(item, "DataItem"), _conString);

                res.Data = item;
                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);
            }
            catch(Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpGet("GetItem/{Username}")]
        [Authorize(AuthenticationSchemes = "BetaClient")]
        public async Task<IActionResult> GetItem(string Username)
        {
            IActionResult actionResult = null;
            DataTable dt = new DataTable();
            ResultModel<List<respItem>> res = new ResultModel<List<respItem>>();
            List<respItem> itemList = new List<respItem>();
            try
            {
                dt = pOSService.GetItem(Username,_conString);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow d in dt.Rows)
                    {
                        respItem item = new respItem();
                        item.Id = Convert.ToInt32(d["id"]);
                        item.ItemName = d["ItemName"].ToString();
                        item.Category = Convert.ToInt32(d["Category"]);
                        item.Price = Convert.ToDecimal(d["Price"]);
                        item.Qty = Convert.ToDecimal(d["Qty"]);
                        item.Path = d["Path"].ToString();
                        item.Mime = d["Mime"].ToString();
                        itemList.Add(item);
                    }
                }
                res.Data = itemList;
                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);
            }
            catch(Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpPut("PutItem")]
        [Authorize(AuthenticationSchemes = "BetaClient")]
        public async Task<IActionResult> UpdateItem([FromBody] respItem updateItem)
        {
            IActionResult actionResult = null;
            ResultModel<bool> res = new ResultModel<bool>();
            try
            {
                // Validate the incoming item
                if (updateItem == null || updateItem.Id <= 0)
                {
                    res.Data = false;
                    res.isSuccess = false;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Invalid item data.";
                    return BadRequest(res);
                }

                // Call the service to update the item
                bool isUpdated = await pOSService.UpdateItem(updateItem, _conString);

                if (isUpdated)
                {
                    res.Data = true;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";
                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = false;
                    res.isSuccess = false;
                    res.ErrorCode = "02";
                    res.ErrorMessage = "Item update failed.";
                    actionResult = NotFound(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = false;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpDelete("DeleteItem/{id}")]
        [Authorize(AuthenticationSchemes = "BetaClient")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            IActionResult actionResult = null;
            ResultModel<bool> res = new ResultModel<bool>();

            try
            {
                // Validate the incoming id
                if (id <= 0)
                {
                    res.Data = false;
                    res.isSuccess = false;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Invalid item id.";
                    return BadRequest(res);
                }

                // Call the service to delete the item
                bool isDeleted = await pOSService.DeleteItem(id, _conString);

                if (isDeleted)
                {
                    res.Data = true;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";
                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = false;
                    res.isSuccess = false;
                    res.ErrorCode = "02";
                    res.ErrorMessage = "Item not found or deletion failed.";
                    actionResult = NotFound(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = false;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("GetCategory/{Username}")]
        [Authorize(AuthenticationSchemes = "BetaClient")]
        public async Task<IActionResult> GetCategory(string Username)
        {
            IActionResult actionResult = null;
            DataTable dt = new DataTable();
            ResultModel<List<Category>> res = new ResultModel<List<Category>>();
            List<Category> itemList = new List<Category>();
            try
            {
                dt = pOSService.GetCategory(Username, _conString);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow d in dt.Rows)
                    {
                        Category item = new Category();
                        item.Id = Convert.ToInt32(d["Id"]);
                        item.CatName = d["CategoryName"].ToString();
                        itemList.Add(item);
                    }
                }
                res.Data = itemList;
                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);
            }
            catch(Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        #endregion 


        [HttpPost("InputSales")]
        [Authorize(AuthenticationSchemes = "BetaClient")]
        public async Task<IActionResult> InputSales(POSmodel data)
        {
            IActionResult actionResult = null;
            DataTable dt = new DataTable();
            string resault = "";
            ResultModel<string> res = new ResultModel<string>();
            try
            {
                dt = pOSService.InputSales(data,_conString);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow d in dt.Rows)
                    {
                        resault = d["MESSAGE"].ToString();
                    }
                }
                res.Data = resault;
                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

            }
            return actionResult;
        }
        
        [HttpGet("GetSales/{Username}")]
        [Authorize(AuthenticationSchemes = "BetaClient")]
        public async Task<IActionResult> GetSales(string Username)
        {
            IActionResult actionResult = null;
            DataTable dt = new DataTable();
            List<POSmodel> resault = new List<POSmodel>();
            ResultModel<List<POSmodel>> res = new ResultModel<List<POSmodel>>();
            try
            {
                dt = pOSService.getSales(Username, _conString);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow d in dt.Rows)
                    {
                        POSmodel x = new POSmodel();
                        x.itemId = d["ItemId"].ToString();
                        x.itemName = d["ItemName"].ToString();
                        x.price = Convert.ToDecimal(d["Price"]);
                        x.category = d["CategoryName"].ToString();
                        x.qty = Convert.ToDecimal(d["Qty"]);
                        x.totalPrice = Convert.ToDecimal(d["TotalPrice"]);
                        x.date = Convert.ToDateTime(d["Date"]);
                        x.username = d["Username"].ToString();
                        resault.Add(x);
                    }
                }
                res.Data = resault;
                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

            }
            return actionResult;
        }

        [HttpGet("GetReportSales/{Username}")]
        [Authorize(AuthenticationSchemes = "BetaClient")]
        public async Task<IActionResult> GetReportSales(string Username)
        {
            IActionResult actionResult = null;
            DataTable dt = new DataTable();
            List<ReportSales> resault = new List<ReportSales>();
            ResultModel<List<ReportSales>> res = new ResultModel<List<ReportSales>>();
            try
            {
                dt = pOSService.getreportsales(Username, _conString);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow d in dt.Rows)
                    {
                        ReportSales x = new ReportSales();
                        x.Date = Convert.ToDateTime(d["FormattedDate"]);
                        x.ItemName = d["ItemName"].ToString();
                        x.CategoryName = d["CategoryName"].ToString();
                        x.TotalTransaction = d["TotalTransaction"].ToString();
                        resault.Add(x);
                    }
                }
                res.Data = resault;
                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

            }
            return actionResult;
        }

        [HttpGet("GetReportStock/{Username}")]
        public async Task<IActionResult> GetReportStock(string Username)
        {
            IActionResult actionResult = null;
            DataTable dt = new DataTable();
            List<ReportStock> resault = new List<ReportStock>();
            ResultModel<List<ReportStock>> res = new ResultModel<List<ReportStock>>();
            try
            {
                dt = pOSService.getReportStock(Username, _conString);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow d in dt.Rows)
                    {
                        ReportStock x = new ReportStock();
                        x.ItemName = d["ItemName"].ToString();
                        x.JumlahStock = Convert.ToDecimal(d["StockOnHand"]);
                        x.Category = d["CategoryName"].ToString();
                        x.Harga = Convert.ToDecimal(d["Price"]);
                        resault.Add(x);
                    }
                }
                res.Data = resault;
                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

            }
            return actionResult;
        }
    }
}
