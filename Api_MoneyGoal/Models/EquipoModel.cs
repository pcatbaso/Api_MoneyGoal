namespace Api_MoneyGoal.Models
{
    public class EquipoModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string fecha_creacion { get; set; }
        public string fecha_actualizacion { get; set; }

        public bool activo { get; set; }
    }
}
