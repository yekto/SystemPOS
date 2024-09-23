namespace POSApi.Model
{
    public class reqItem
    {
        public string Username { get; set; }
    }
    public class respItem
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public int Category { get; set; }
        public decimal Price {get;set;}
        public decimal Qty {get;set;}
        public string Path {get;set;}
        public string Mime { get;set;} = string.Empty;
    }
}
