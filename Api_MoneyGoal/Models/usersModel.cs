namespace Api_MoneyGoal.Models
{
    public class usersModel
    {
        public int id { get; set; }
        public string nombre_usuario { get; set; }
        public string apellidoPaterno_usuario { get; set; }
        public string apellidoMaterno_usuario { get; set; }
        public string direccion_usuario { get; set; }
        public string email_usuario { get; set; }
        public string telefono_usuario { get; set; }
        public string contrasenia_usuario { get; set; }
        public string cardName { get; set; }
        public string cardNumber { get; set; }
        public string expiration { get; set; }
        public string cvv { get; set; }
        public bool termino1 { get; set; } 
        public bool termino2 { get; set; }
        public bool activo { get; set; }
        public int rol { get; set; }
        public string user_created { get; set; }
    }
}
