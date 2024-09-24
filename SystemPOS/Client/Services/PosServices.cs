using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Globalization;
using System.Net.Http.Json;
using SystemPOS.Shared.POS;

namespace SystemPOS.Client.Services
{
    public class PosServices : iPosServices
    {
        private readonly HttpClient _http;

        public PosServices(HttpClient http)
        {
            _http = http;
        }

        public respLogin activeUser { get; set; }
        public async Task<ResultModel<respLogin>> Login(reqLog data)
        {
            ResultModel<respLogin> res = new ResultModel<respLogin>();
            try
            {
                _http.DefaultRequestHeaders.Clear();
                //_http.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");
                string jsong = JsonConvert.SerializeObject(data);
                var resault = await _http.PostAsJsonAsync<reqLog>("api/ToPOSapi/Login", data);
                var result = await resault.Content.ReadFromJsonAsync<ResultModel<respLogin>>();

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;
            }
            return res;
        }
        public async Task<ResultModel<List<respItem>>> PostItem (List<respItem> data, string token)
        {
            ResultModel<List<respItem>> res = new ResultModel<List<respItem>>();
            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                string jsong = JsonConvert.SerializeObject(data);
                var resault = await _http.PostAsJsonAsync<List<respItem>>("api/ToPOSapi/PostItem", data);
                var result = await resault.Content.ReadFromJsonAsync<ResultModel<List<respItem>>>();

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                }
            }
            catch(Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;
            }
            return res;
        }
        public async Task<ResultModel<List<respItem>>> GetItem(string Username, string token)
        {
            ResultModel<List<respItem>> res = new ResultModel<List<respItem>>();
            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var result = await _http.GetFromJsonAsync<ResultModel<List<respItem>>>($"api/ToPOSapi/GetItem/{Username}");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                }
            }
            catch(Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;
            }
            return res;
        }
        public async Task<ResultModel<bool>> DeleteItem(int id, string token)
        {
            ResultModel<bool> res = new ResultModel<bool>();
            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                //var result = await _http.GetFromJsonAsync<ResultModel<bool>>($"api/ToPOSapi/DeleteItem/{id}");
                var request = new HttpRequestMessage(HttpMethod.Delete, $"api/ToPOSapi/DeleteItem/{id}");
                var response = await _http.SendAsync(request);


                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResultModel<bool>>();
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                }
                else
                {
                    res.Data = false;
                    res.isSuccess = response.IsSuccessStatusCode;
                    res.ErrorCode = response.StatusCode.ToString();
                    res.ErrorMessage = response.RequestMessage.ToString();
                }
            }
            catch(Exception ex)
            {
                res.Data = false;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;
            }
            return res;
        }
        public async Task<ResultModel<List<Category>>> GetCategory(string Username,string token)
        {
            ResultModel<List<Category>> res = new ResultModel<List<Category>>();
            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var result = await _http.GetFromJsonAsync<ResultModel<List<Category>>>($"api/ToPOSapi/GetCategory/{Username}");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;
            }
            return res;
        }
        public async Task<ResultModel<bool>> UpdateItem(respItem data, string token)
        {
            ResultModel<bool> res = new ResultModel<bool>();
            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var response = await _http.PutAsJsonAsync("api/ToPOSapi/PutItem", data);
                var result = await response.Content.ReadFromJsonAsync<ResultModel<bool>>();

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                }
                else
                {
                    res.Data = false;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                res.Data = false;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;
            }
            return res;
        }
        public async Task<ResultModel<string>> InputSales(POSmodel data, string token)
        {
            ResultModel<string> res = new ResultModel<string>();
            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                string jsong = JsonConvert.SerializeObject(data);
                var resault = await _http.PostAsJsonAsync<POSmodel>("api/ToPOSapi/InputSales", data);
                var result = await resault.Content.ReadFromJsonAsync<ResultModel<string>>();

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;
            }
            return res;
        }
        public async Task<ResultModel<List<POSmodel>>> GetSales(string Username, string token)
        {
            ResultModel<List<POSmodel>> res = new ResultModel<List<POSmodel>>();
            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var result = await _http.GetFromJsonAsync<ResultModel<List<POSmodel>>>($"api/ToPOSapi/GetSales/{Username}");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;
            }
            return res;
        }
        public async Task<ResultModel<List<ReportSales>>> GetReportSales(string Username, string token)
        {
            ResultModel<List<ReportSales>> res = new ResultModel<List<ReportSales>>();
            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var result = await _http.GetFromJsonAsync<ResultModel<List<ReportSales>>>($"api/ToPOSapi/GetReportSales/{Username}");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                }
            }
            catch(Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;
            }
            return res;
        }
        public async Task<ResultModel<List<ReportStock>>> GetReportStock(string Username, string token)
        {
            ResultModel<List<ReportStock>> res = new ResultModel<List<ReportStock>>();
            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var result = await _http.GetFromJsonAsync<ResultModel<List<ReportStock>>>($"api/ToPOSapi/GetReportStock/{Username}");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;
            }
            return res;
        }
    }
}
