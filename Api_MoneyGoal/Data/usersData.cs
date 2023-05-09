using Api_MoneyGoal.Models;
using MySql.Data.MySqlClient;
using System.Data;
using webApi_MoneyGoal;

namespace Api_MoneyGoal.Data
{
    public class usersData
    {
        Conexion conexion = new Conexion();
        MySqlConnection conn;

        public async Task<bool> Insertar(usersModel user)
        {
            string cadenaConexion = conexion.CadenaConexion();
            conn = new MySqlConnection(cadenaConexion);
            
            MySqlCommand cmd = null;
            MySqlCommand cmdB = null;
            conn.Open();

            var transaction = conn.BeginTransaction();

            try
            {                
                cmd = new MySqlCommand("sp_insertUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("name_param", user.nombre_usuario));
                cmd.Parameters.Add(new MySqlParameter("lastNameP_param", user.apellidoPaterno_usuario));
                cmd.Parameters.Add(new MySqlParameter("lastNameM_param", user.apellidoMaterno_usuario));
                cmd.Parameters.Add(new MySqlParameter("address_param", user.direccion_usuario));
                cmd.Parameters.Add(new MySqlParameter("phoneNumber_param", user.telefono_usuario));
                cmd.Parameters.Add(new MySqlParameter("email_param", user.email_usuario));
                cmd.Parameters.Add(new MySqlParameter("password_param", user.contrasenia_usuario));
                cmd.Parameters.Add(new MySqlParameter("rol_param", user.rol));
                cmd.Parameters.Add(new MySqlParameter("active_param", user.activo));

                cmd.Parameters.Add(new MySqlParameter("@resultado", MySqlDbType.VarChar));
                cmd.Parameters["@resultado"].Direction = ParameterDirection.Output;

                var reader = cmd.ExecuteNonQuery();

                var t = cmd.Parameters["@resultado"].Value.ToString();                

                if (Convert.ToInt16(t) > 0)
                {                                        
                    cmdB = new MySqlCommand("sp_insertUserBank", conn);
                    cmdB.CommandType = CommandType.StoredProcedure;
                    cmdB.Parameters.Add(new MySqlParameter("idUser_param", Convert.ToInt16(t)));
                    cmdB.Parameters.Add(new MySqlParameter("cardName_param", user.cardName));
                    cmdB.Parameters.Add(new MySqlParameter("cardNumber_param", user.cardNumber));
                    cmdB.Parameters.Add(new MySqlParameter("expiration_param", user.expiration));
                    cmdB.Parameters.Add(new MySqlParameter("cvv_param", user.cvv));

                    cmdB.Parameters.Add(new MySqlParameter("@resultado", MySqlDbType.VarChar));
                    cmdB.Parameters["@resultado"].Direction = ParameterDirection.Output;

                    var reader1 = cmdB.ExecuteNonQuery();

                    var t1 = cmdB.Parameters["@resultado"].Value.ToString();

                    if(t1 == "Éxito")
                    {
                        transaction.Commit();
                        
                        conn.Close();
                        return true;
                    }
                    else
                    {
                        throw new Exception("Ocurrio un error al insertar la cuenta bancaria del usuario");
                    }                                            
                }
                else
                {
                    throw new Exception("Ocurrio un error");                    
                }
                
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<List<usersModel>> Consultar()
        {
            string cadenaConexion = conexion.CadenaConexion();
            MySqlCommand cmd = null;
            conn = new MySqlConnection(cadenaConexion);
            var dtUsers = new DataTable();

            var listaUsuarios = new List<usersModel>();

            try
            {
                conn.Open();
                cmd = new MySqlCommand("sp_getUsers", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add(new MySqlParameter("search_param", search_param));
                //cmd.Parameters.Add(new MySqlParameter("email_param", email_param));

                MySqlDataReader reader = cmd.ExecuteReader();

                dtUsers.Load(reader);

                if(dtUsers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtUsers.Rows)
                    {
                        usersModel user = new usersModel();
                        user.id = Convert.ToInt32(dr["id"]);
                        user.nombre_usuario = dr["name"].ToString();
                        user.apellidoPaterno_usuario = dr["lastNameP"].ToString();
                        user.apellidoMaterno_usuario = dr["lastNameM"].ToString();
                        user.direccion_usuario = dr["address"].ToString();
                        user.telefono_usuario = dr["phoneNumber"].ToString();
                        user.email_usuario = dr["email"].ToString();
                        user.contrasenia_usuario = dr["password"].ToString();
                        user.rol = dr["rol"].ToString();
                        user.activo = Convert.ToInt32(dr["active"]) == 1 ? true : false;

                        listaUsuarios.Add(user);
                    }
                }

                return listaUsuarios;
            }
            catch(Exception ex)
            {
                throw new Exception("Error catch: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<bool> Actualizar(usersModel user)
        {
            string cadenaConexion = conexion.CadenaConexion();
            MySqlCommand cmd = null;
            MySqlCommand cmdB = null;
            conn = new MySqlConnection(cadenaConexion);

            conn.Open();
            var transaction = conn.BeginTransaction();

            try
            {
                cmd = new MySqlCommand("sp_updateUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("name_param", user.nombre_usuario));
                cmd.Parameters.Add(new MySqlParameter("lastNameP_param", user.apellidoPaterno_usuario));
                cmd.Parameters.Add(new MySqlParameter("lastNameM_param", user.apellidoMaterno_usuario));
                cmd.Parameters.Add(new MySqlParameter("address_param", user.direccion_usuario));
                cmd.Parameters.Add(new MySqlParameter("phoneNumber_param", user.telefono_usuario));
                cmd.Parameters.Add(new MySqlParameter("email_param", user.email_usuario));
                cmd.Parameters.Add(new MySqlParameter("password_param", user.contrasenia_usuario));
                cmd.Parameters.Add(new MySqlParameter("id_param", user.id));

                cmd.Parameters.Add(new MySqlParameter("@resultado", MySqlDbType.VarChar));
                cmd.Parameters["@resultado"].Direction = ParameterDirection.Output;

                var reader = cmd.ExecuteNonQuery();

                if (cmd.Parameters["@resultado"].Value.ToString() == "1")
                {
                    cmdB = new MySqlCommand("sp_updateUserBank", conn);
                    cmdB.CommandType = CommandType.StoredProcedure;
                    
                    cmdB.Parameters.Add(new MySqlParameter("cardName_param", user.cardName));
                    cmdB.Parameters.Add(new MySqlParameter("cardNumber_param", user.cardNumber));
                    cmdB.Parameters.Add(new MySqlParameter("expiration_param", user.expiration));
                    cmdB.Parameters.Add(new MySqlParameter("cvv_param", user.cvv));
                    cmdB.Parameters.Add(new MySqlParameter("idUser_param", user.id));

                    cmdB.Parameters.Add(new MySqlParameter("@resultado", MySqlDbType.VarChar));
                    cmdB.Parameters["@resultado"].Direction = ParameterDirection.Output;

                    var reader1 = cmdB.ExecuteNonQuery();

                    var t1 = cmdB.Parameters["@resultado"].Value.ToString();

                    if(t1 == "1")
                    {
                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        throw new Exception("Ocurrio un error al actualizar datos bancarios.");
                    }
                }
                else
                {
                    throw new Exception("Ocurrio un error al actualizar datos del usuario.");
                }
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Ocurrio un error: " + ex.Message);
            }
        }

        public async Task<bool> Eliminar(string id)
        {
            string cadenaConexion = conexion.CadenaConexion();
            MySqlCommand cmd = null;
            MySqlCommand cmdB = null;
            conn = new MySqlConnection(cadenaConexion);

            conn.Open();

            try
            {
                cmd = new MySqlCommand("sp_disabledUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("id_param", id));

                cmd.Parameters.Add(new MySqlParameter("@resultado", MySqlDbType.VarChar));
                cmd.Parameters["@resultado"].Direction = ParameterDirection.Output;

                var reader = cmd.ExecuteNonQuery();

                if (cmd.Parameters["@resultado"].Value.ToString() == "1")
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error: " + ex.Message);
            }
        }
    }
}
