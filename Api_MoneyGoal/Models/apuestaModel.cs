using Org.BouncyCastle.Bcpg.OpenPgp;

namespace Api_MoneyGoal.Models
{
    public class apuestaModel
    {
        public int id { get; set; }
        public int idUser { get; set; }
        public int idTicket { get; set; }
        public string numGame { get; set; }
        public bool local { get; set; }
        public bool draw { get; set; }
        public bool visitor { get; set; }
        public double cost { get; set; }
        public string createdDate { get; set; }
        public string updateDate { get; set; }
    }
}
