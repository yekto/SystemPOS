namespace POSApi.Model
{
    public class ReportSales
    {
        public DateTime Date {get;set;}
        public string ItemName {get;set;}
        public string CategoryName {get;set;}
        public string TotalTransaction {get;set;}
    }

    public class ReportStock
    {
        public string ItemName { get; set; }
        public decimal JumlahStock { get; set; }
        public string Category { get; set; }
        public decimal Harga { get; set; }
    }
}
