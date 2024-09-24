namespace POSApi.Model
{
    public class POSmodel
    {
        public string itemId {get;set;}
        public string itemName {get;set;}
        public decimal price {get;set;}
        public string category { get; set; }
        public decimal qty {get;set;}
        public decimal totalPrice {get;set;}
        public DateTime date {get;set;}
        public string username{get;set;}
    }
}
