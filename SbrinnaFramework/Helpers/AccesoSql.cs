namespace SbrinnaCoreFramework.Sdk.Helpers
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// Clase para acceder a una base de datos de SQL
    /// </summary>
    public class AccesoSql
    {
        /// <summary>
        /// Ruta del fichero de configuración donde 
        /// se encuentran todas las cadenas de conexión.
        /// </summary>
        private static string rutaConfiguracion = "configuration/CadenasConexion";

        /// <summary>
        /// Conexión a la base de datos de SQL Server
        /// </summary>
        private SqlConnection conexionSql;

        /// <summary>
        /// Comando para realizar las operaciones sobre la base de datos.
        /// </summary>
        private SqlCommand comandoSql;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public AccesoSql()
        {
            this.conexionSql = new SqlConnection();
            this.comandoSql = new SqlCommand();
        }

        /// <summary>
        /// Abre la conexión a la base de datos usando el origen de datos
        /// por defecto.
        /// </summary>
        public void Abrir()
        {
            this.Abrir(TipoOrigenDatos.MicrosoftCrm);
        }

        /// <summary>
        /// Abre la conexión a la base de datos según el tipo de origen indicado
        /// </summary>
        /// <param name="tipoOrigenDatos">Tipo de origen de datos a abrir</param>
        public void Abrir(TipoOrigenDatos tipoOrigenDatos)
        {
            // instanciamos el objeto de conexión
            if (this.conexionSql == null)
            {
                this.conexionSql = new SqlConnection();
            }

            // si la conexión estaba abierta la cerramos
            if (this.conexionSql.State == ConnectionState.Open)
            {
                this.conexionSql.Close();
            }

            // Cargamos la cadena de conexión del fichero de configuración
            switch (tipoOrigenDatos)
            {
                case TipoOrigenDatos.EntryCrm:
                    this.conexionSql.ConnectionString = Configuracion.GetSetting("EntrySqlServer");
                    break;
                case TipoOrigenDatos.MicrosoftCrm:
                    this.conexionSql.ConnectionString = Configuracion.GetSetting("CrmSqlServer");
                    break;
            }

            // Abrimos la conexión
            this.conexionSql.Open();
        }

        /// <summary>
        /// Cierra la conexión de la base de datos de CRM
        /// </summary>
        public void Cerrar()
        {
            if (this.conexionSql != null)
            {
                this.conexionSql.Close();
            }
        }

        /// <summary>
        /// Añade un parámetro al comando.
        /// </summary>
        /// <param name="idParametro">nombre del parámetro</param>
        /// <param name="tipoSql">tipo de dato de sql del parámetro</param>
        /// <param name="valor">valor del parámetro</param>
        public void AñadirParametro(string idParametro, SqlDbType tipoSql, object valor)
        {
            this.AñadirParametro(idParametro, tipoSql, 0, ParameterDirection.Input, valor);
        }

        /// <summary>
        /// Añade un parámetro al comando.
        /// </summary>
        /// <param name="idParametro">nombre del parámetro</param>
        /// <param name="tipoSql">tipo de dato de sql del parámetro</param>
        /// <param name="direction">Si la dirección es de entrada o salida</param>
        /// <param name="value">valor del parámetro</param>
        public void AñadirParametro(string idParametro, SqlDbType tipoSql, ParameterDirection direction, object value)
        {
            this.AñadirParametro(idParametro, tipoSql, 0, direction, value);
        }

        /// <summary>
        /// Añade un parámetro al comando.
        /// </summary>
        /// <param name="idParametro">nombre del parámetro</param>
        /// <param name="tipoSql">tipo de dato de sql del parámetro</param>
        /// <param name="tamaño">Tamaño que tiene el tipo de dato del parámetro</param>
        /// <param name="valor">valor del parámetro</param>
        public void AñadirParametro(string idParametro, SqlDbType tipoSql, int tamaño, object valor)
        {
            this.AñadirParametro(idParametro, tipoSql, tamaño, ParameterDirection.Input, valor);
        }

        /// <summary>
        /// Añade un parámetro al comando.
        /// </summary>
        /// <param name="idParametro">nombre del parámetro</param>
        /// <param name="tipoSql">tipo de dato de sql del parámetro</param>
        /// <param name="tamaño">Tamaño que tiene el tipo de dato del parámetro</param>
        /// <param name="dieccion">Si la dirección es de entrada o salida</param>
        /// <param name="valor">valor del parámetro</param>
        public void AñadirParametro(string idParametro, SqlDbType tipoSql, int tamaño, ParameterDirection dieccion, object valor)
        {
            if (idParametro == null || string.IsNullOrEmpty(idParametro))
            {
                throw new ArgumentNullException("idParametro");
            }

            if (this.comandoSql == null)
            {
                this.comandoSql = new SqlCommand();
            }

            // Añadimos el parámetro al comando
            SqlParameter newSqlParam = new SqlParameter();
            newSqlParam.ParameterName = idParametro;
            newSqlParam.SqlDbType = tipoSql;
            newSqlParam.Direction = dieccion;

            if (tamaño > 0)
            {
                newSqlParam.Size = tamaño;
            }

            if (valor != null)
            {
                newSqlParam.Value = valor;
            }

            this.comandoSql.Parameters.Add(newSqlParam);
        }

        /// <summary>
        /// Limpia todos los parámetro asociado al comando.
        /// </summary>
        public void LimpiarParametros()
        {
            this.comandoSql.Parameters.Clear();
        }

        /// <summary>
        /// Ejecuta la sentence SQL y devuelve un data reader.
        /// </summary>
        /// <param name="sentence">Cadena con la sentence de SQL</param>
        /// <returns>DataReader para acceder a los resultados</returns>
        public SqlDataReader EjecutarConsulta(string sentence)
        {
            if (sentence == null || string.IsNullOrEmpty(sentence))
            {
                throw new ArgumentNullException("sentence");
            }

            this.PrepararComando(sentence);

            return this.comandoSql.ExecuteReader();
        }
        
        /// <summary>
        /// Ejecuta un procedure almacenado y devuelve un data reader
        /// </summary>
        /// <param name="procedure">Cadena con el procedure almacenado de SQL</param>
        /// <returns>DataReader para acceder a los resultados </returns>
        public SqlDataReader Ejecutarprocedure(string procedure)
        {
            if (procedure == null || string.IsNullOrEmpty(procedure))
            {
                throw new ArgumentNullException("procedure");
            }

            this.Prepararprocedure(procedure);

            return this.comandoSql.ExecuteReader();
        }

        /// <summary>
        /// Ejecuta una sentence escalar.
        /// </summary>
        /// <param name="sentence">Cadena con la sentecia SQL</param>
        /// <returns>Valor que devuelve la sentence</returns>
        public object EjecutarEscalar(string sentence)
        {
            if (sentence == null || string.IsNullOrEmpty(sentence))
            {
                throw new ArgumentNullException("sentence");
            }

            this.PrepararComando(sentence);

            return this.comandoSql.ExecuteScalar();
        }

        /// <summary>
        /// Ejecuta una sentence escalar.
        /// </summary>
        /// <param name="sentence">Cadena con la sentecia SQL</param>
        /// <returns>Valor que devuelve la sentence</returns>
        public object EjecutarEscalarprocedure(string procedure)
        {
            if (procedure == null || string.IsNullOrEmpty(procedure))
            {
                throw new ArgumentNullException("procedure");
            }

            this.Prepararprocedure(procedure);
            return this.comandoSql.ExecuteScalar();
        }
        
        public int EjecutarNonQueryprocedure(string procedure)
        {
            if (procedure == null || string.IsNullOrEmpty(procedure))
            {
                throw new ArgumentNullException("procedure");
            }

            this.Prepararprocedure(procedure);
            return this.comandoSql.ExecuteNonQuery();
        }
        
        /// <summary>
        /// Devuelve un DataTable con los resultados de la consulta.
        /// </summary>
        /// <returns>Tabla en memoria con los resultados de la consulta</returns>
        public DataTable ObtenerTabla(string sentence)
        {
            if (sentence == null)
            {
                throw new ArgumentNullException("sentence");
            }

            this.PrepararComando(sentence);

            using (DataTable dataTable = new DataTable())
            {
                using (SqlDataAdapter sda = new SqlDataAdapter(this.comandoSql))
                {
                    sda.Fill(dataTable);
                }

                return dataTable;
            }
        }

        /// <summary>
        /// Prepara el comando con la sentence indicada.
        /// </summary>
        /// <param name="sentence">La sentence a pasar al comando</param>
        private void PrepararComando(string sentence)
        {
            if (this.comandoSql == null)
            {
                this.comandoSql = new SqlCommand();
            }

            this.comandoSql.Connection = this.conexionSql;
            this.comandoSql.CommandText = sentence;
            this.comandoSql.CommandType = CommandType.Text;

            if (this.conexionSql.State == ConnectionState.Closed)
            {
                this.Abrir();
            }
        }

        /// <summary>
        /// Prepara el comando con la procedure almacenado.
        /// </summary>
        /// <param name="procedure">procedure almacenado</param>
        private void Prepararprocedure(string procedure)
        {
            if (this.comandoSql == null)
            {
                this.comandoSql = new SqlCommand();
            }

            this.comandoSql.Connection = this.conexionSql;
            this.comandoSql.CommandText = procedure;
            this.comandoSql.CommandType = CommandType.StoredProcedure;

            if (this.conexionSql.State == ConnectionState.Closed)
            {
                this.Abrir();
            }
        }
    }
}
