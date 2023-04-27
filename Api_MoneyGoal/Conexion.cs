using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace webApi_MoneyGoal
{
    public class Conexion
    {
        private string Base;
        private string Servidor;
        private string Usuario;
        private string Clave;
        MySqlConnection cadenaConexion = new MySqlConnection();

        //public string CadenaConexion()
        //{
        //    this.Base = "MoneyGoal";
        //    this.Servidor = "DESKTOP-AGP9RA3\\SQLEXPRESS";
        //    this.Usuario = "sa";
        //    this.Clave = "contrasenia";            

        //    cadenaConexion.ConnectionString = "Server=" + this.Servidor +
        //                                          "; Database=" + this.Base +
        //                                          "; User Id=" + this.Usuario +
        //                                          "; Password=" + this.Clave;

        //    return cadenaConexion.ConnectionString.ToString();
        //}

        public string CadenaConexion()
        {
            //this.Base = "MoneyGoal";
            //this.Servidor = "DESKTOP-AGP9RA3\\SQLEXPRESS";
            //this.Usuario = "sa";
            //this.Clave = "contrasenia";

            //cadenaConexion.ConnectionString = "Data Source=MoneyGoalPruebas.mssql.somee.com;Initial Catalog=MoneyGoalPruebas;user id=Pcat_SQLLogin_1;pwd=lupdsta2dy; TrustServerCertificate=True;";
            //cadenaConexion.ConnectionString = "Data Source=DESKTOP-AGP9RA3\\SQLEXPRESS;Initial Catalog=MoneyGoal;user id=sa;pwd=contrasenia; TrustServerCertificate=True;";
            cadenaConexion.ConnectionString = "Data Source=127.0.0.1;Initial Catalog=moneygoal;User ID=root;Password=admin01;Convert Zero Datetime=True;Allow Zero Datetime=True";

            return cadenaConexion.ConnectionString.ToString();
        }

        public void abrirConexion()
        {
            try
            {
                cadenaConexion.Open();
                Console.WriteLine("Conexion abierta");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al abrir la BD " + ex.Message);
            }
        }

        public void cerrarConexion()
        {
            cadenaConexion.Close();
        }
    }
}
