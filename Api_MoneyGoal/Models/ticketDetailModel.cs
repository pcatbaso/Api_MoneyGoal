namespace Api_MoneyGoal.Models
{
    public class ticketDetailModel
    {
        public int idTicketBet { get; set; }
        public int numGame { get; set; }
        public int idLocalTeam { get; set; }
        public string? nameLocal { get; set; }
        public int idVisitingTeam { get; set; }
        public string? nameVisitante { get; set; }
        public string startDate { get; set; }
        public string result { get; set; }
        public bool localApuesta { get; set; }
        public bool drawApuesta { get; set; }
        public int costo { get; set; }
        public bool visitApuesta { get; set; }
        public string? createdDate { get; set; }
        public string? updateDate { get; set; }
    }
}
