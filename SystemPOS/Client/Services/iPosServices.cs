using SystemPOS.Shared;
using SystemPOS.Shared.POS;

namespace SystemPOS.Client.Services
{
    public interface iPosServices
    {
        respLogin activeUser { get; set; } 
        Task<ResultModel<respLogin>> Login(reqLog req);
        Task<ResultModel<List<respItem>>> PostItem(List<respItem> items, string token);
        Task<ResultModel<List<respItem>>> GetItem(string Username, string token);
        Task<ResultModel<bool>> DeleteItem( int id, string token);
        Task<ResultModel<List<Category>>> GetCategory(string Username, string token);
        Task<ResultModel<bool>> UpdateItem(respItem data, string token);
        Task<ResultModel<string>> InputSales(POSmodel data, string token);
        Task<ResultModel<List<POSmodel>>> GetSales(string data, string token);
        Task<ResultModel<List<ReportSales>>> GetReportSales(string Username, string token);
        Task<ResultModel<List<ReportStock>>> GetReportStock(string Username, string token);

    }
}
