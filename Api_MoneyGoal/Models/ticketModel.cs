namespace Api_MoneyGoal.Models
{
    public class ticketModel
    {
        public int id { get; set; }
        public int idTicketBet { get; set; }
        public int active { get; set; }
        public string dateActive { get; set; }
        public string dateRange { get; set; }    
        public string dateDeactive { get; set; }        
        public string? createdDate { get; set; }
        public string? updateDate { get; set; }

        public List<ticketDetailModel> listTicketDetail { get; set; }
    }
}

