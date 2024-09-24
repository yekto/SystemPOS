using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net.Http.Json;
using SystemPOS.Client.Services;
using SystemPOS.Shared.POS;
using static System.Net.WebRequestMethods;

namespace SystemPOS.Server.Controllers
{
    [Route("api/ToPOSapi")]
    [ApiController]
    public class POSController : Controller
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;

        public POSController(HttpClient http, IConfiguration config)
        {
            _http = http;
            _configuration = config;
            _http.BaseAddress = new Uri(_configuration.GetValue<string>("BaseURL"));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(reqLog data)
        {
            IActionResult actionResult = null;
            ResultModel<respLogin> res = new ResultModel<respLogin>();
            try
            {
                //string jsons = JsonConvert.SerializeObject(data);
                var resault = await _http.PostAsJsonAsync<reqLog>("api/POSda/Login", data);
                var result = await resault.Content.ReadFromJsonAsync<ResultModel<respLogin>>();
                //var resultget = await _http.GetFromJsonAsync<string>($"api/POSda/test");

                if (result.isSuccess)
                {
                    res.Data = result.Data;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = "Bad request ToPOSapi"+ ex.Message.ToString();
                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        #region Item Management
        [HttpPost("PostItem")]
        public async Task<IActionResult> PostItem([FromHeader(Name = "Authorization")] string token, List<respItem> data)
        {
            IActionResult actionResult = null;
            ResultModel<List<respItem>> res = new ResultModel<List<respItem>>();
            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");
                var resault = await _http.PostAsJsonAsync<List<respItem>>("api/POSda/PostItem", data);
                var result = await resault.Content.ReadFromJsonAsync<ResultModel<List<respItem>>>();

                if (result.isSuccess)
                {
                    res.Data = result.Data;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
            }
            catch  (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = "Bad request ToPOSapi"+ex.Message.ToString();
                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpGet("GetItem/{Username}")]
        public async Task<IActionResult> GetItem([FromHeader(Name = "Authorization")] string token, string Username)
        {
            IActionResult actionResult = null;
            ResultModel<List<respItem>> res = new ResultModel<List<respItem>>();
            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");
                var result = await _http.GetFromJsonAsync<ResultModel<List<respItem>>>($"api/POSda/GetItem/{Username}");

                if (result.isSuccess)
                {
                    res.Data = result.Data;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
            }
            catch(Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = "Bad request ToPOSapi"+ ex.Message.ToString();
                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpDelete("DeleteItem/{id}")]
        public async Task<IActionResult> DeleteItem([FromHeader(Name = "Authorization")] string token, int id)
        {
            IActionResult actionResult = null;
            ResultModel<bool> res = new ResultModel<bool>();
            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");

                var request = new HttpRequestMessage(HttpMethod.Delete, $"api/POSda/DeleteItem/{id}");
                var response = await _http.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResultModel<bool>>();

                    res.Data = result.Data;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = false;
                    res.isSuccess = true;
                    res.ErrorCode = response.StatusCode.ToString();
                    res.ErrorMessage = response.RequestMessage.ToString();
                    actionResult = Ok(res);
                }
            }
            catch(Exception ex)
            {
                res.Data = false;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = "Bad request ToPOSapi" + ex.Message.ToString();
                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpGet("GetCategory/{Username}")]
        public async Task<IActionResult> GetCategory([FromHeader(Name = "Authorization")] string token, string Username)
        {
            IActionResult actionResult = null;
            ResultModel<List<Category>> res = new ResultModel<List<Category>>();
            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");
                var result = await _http.GetFromJsonAsync<ResultModel<List<Category>>>($"api/POSda/GetCategory/{Username}");

                if (result.isSuccess)
                {
                    res.Data = result.Data;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = "Bad request ToPOSapi" + ex.Message.ToString();
                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpPut("PutItem")]
        public async Task<IActionResult> UpdateItem([FromHeader(Name = "Authorization")] string token, [FromBody] respItem updateItem)
        {
            IActionResult actionResult = null;
            ResultModel<bool> res = new ResultModel<bool>();
            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");
                var response = await _http.PutAsJsonAsync("api/POSda/PutItem", updateItem);
                var result = await response.Content.ReadFromJsonAsync<ResultModel<bool>>();

                if (result.isSuccess)
                {
                    res.Data = result.Data;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = false;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
            }
            catch(Exception ex)
            {
                res.Data = false;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = "Bad request ToPOSapi" + ex.Message.ToString();
                actionResult = BadRequest(res);
            }
            return actionResult;
        }
        #endregion

        #region POS
        [HttpPost("InputSales")]
        public async Task<IActionResult> InputSales([FromHeader(Name = "Authorization")] string token, POSmodel data)
        {
            IActionResult actionResult = null;
            ResultModel<string> res = new ResultModel<string>();
            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");
                var resault = await _http.PostAsJsonAsync<POSmodel>("api/POSda/InputSales", data);
                var result = await resault.Content.ReadFromJsonAsync<ResultModel<string>>();

                if (result.isSuccess)
                {
                    res.Data = result.Data;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = "Bad request ToPOSapi" + ex.Message.ToString();
                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        #endregion
    }
}
