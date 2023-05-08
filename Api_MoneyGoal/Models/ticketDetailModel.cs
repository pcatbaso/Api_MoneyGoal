namespace Api_MoneyGoal.Models
{
    public class ticketDetailModel
    {
        public int idTicketBet { get; set; }
        public int numGame { get; set; }
        public int idLocalTeam { get; set; }
        public int idVisitingTeam { get; set; }
        public string result { get; set; }
        public string? createdDate { get; set; }
        public string? updateDate { get; set; }
    }
}
