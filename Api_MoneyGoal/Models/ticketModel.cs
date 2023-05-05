namespace Api_MoneyGoal.Models
{
    public class ticketModel
    {
        public int id { get; set; }
        public int idTicketBet { get; set; }
        public int numGame { get; set; }
        public int idLocalTeam { get; set; }
        public int idVisitingTeam { get; set; }
        public int active { get; set; }
        public string createdDate { get; set; }
        public string updateDate { get; set; }
    }
}

