using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using ConexionBD;
namespace WebAplicacionConexiconSQL
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        ClassAccesoSQL objacc = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                objacc = new ClassAccesoSQL("Data Source = ROMANISIDOR; Initial Catalog = BDTIENDA; Integrated Security = true");
                Session["objacc"] = objacc;
            }
            else
            {
                objacc = (ClassAccesoSQL)Session["objacc"];
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SqlConnection temp = null;
            string mensaje = "";
            temp = objacc.AbrirConexion(ref mensaje);
            //TextBox1.Text = mensaje;
            if (temp != null)
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), 
                    "msgbox01", "verAlerta('Conexión correcta ', '"+ mensaje +"', 'success');", true);
                temp.Close(); //Cerrar la conexión
                temp.Dispose(); //Destruir el objeto de conexión

            }
            else
            {
                this.ClientScript.RegisterStartupScript(this.GetType(),
                    "msgbox01", "verAlerta(`Conexión incorrecta `, `" + mensaje + "`, `error`);", true);
            }
           
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            SqlDataReader caja = null;

            SqlConnection cnab = null;

            string m = "";

            cnab = objacc.AbrirConexion(ref m);

            if (cnab != null)
            {
                this.ClientScript.RegisterStartupScript(this.GetType(),
                    "123", "verAlerta('Correcto', '" + m + "', 'success')", true);

                caja = objacc.ConsultarReader("select * from EMPLEADO", cnab, ref m);

                if (caja != null)
                //La consuta es correcta y se leen los datos
                {
                    ListBox1.Items.Clear();
                    while (caja.Read())
                    {
                        ListBox1.Items.Add(caja[0] + " " + caja["NOMBRE"]);

                    }

                }

            }
            else
            {
                this.ClientScript.RegisterStartupScript(this.GetType(),
                   "1234", "verAlerta('Incorrecto', '" + m + "', 'error')", true);

            }

            cnab.Close();
            cnab.Dispose();
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            DataSet contenedor = null;

            SqlConnection cnab = null;

            string m = "";

            cnab = objacc.AbrirConexion(ref m);

            if (cnab != null)
            {
                this.ClientScript.RegisterStartupScript(this.GetType(),
                    "msg123", "verAlerta(`Correcto`, `" + m + "`, `success`)", true);

                contenedor = objacc.ConsultaDS("select * from EMPLEADO", cnab, ref m);

                cnab.Close();
                cnab.Dispose();
                if (contenedor != null)
                //La consuta es correcta y se leen los datos
                {

                    GridView1.DataSource = contenedor.Tables[0];
                    GridView1.DataBind();
                    Session["Tabla1"] = contenedor;

                }

            }
            else
            {
                this.ClientScript.RegisterStartupScript(this.GetType(),
                   "msg1234", "verAlerta('Incorrecto', '" + m + "', 'error')", true);

            }



        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            DataSet temp = Session["Tabla1"] as DataSet;
            ListBox1.Items.Clear();

            ListBox1.Items.Add("Datos recuperados del dataTable 0");



            foreach (DataRow registro in temp.Tables[0].Rows)
            {
                ListBox1.Items.Add(registro[0] + " -- " + registro[1]);
            }
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            //Declaración de parametros 
            SqlParameter first = new SqlParameter("id", SqlDbType.Int);
            SqlParameter second = new SqlParameter("nombre", SqlDbType.NVarChar, 50);

            //Dando valores a los parámetros
            first.Value = txtId.Text;
            second.Value = txtNombre.Text;

            //Establecer la dirección de los parámetros
            first.Direction = ParameterDirection.Input;
            second.Direction = ParameterDirection.Input;

            string sentencia = "Insert Into empleado values(@id, @nombre);";
            SqlConnection conexion = null;
            string mensaje = "";
            Boolean resp = false;
            conexion = objacc.AbrirConexion(ref mensaje);

            resp = objacc.InsertaEmpleadosconPar(sentencia, conexion, ref mensaje, first, second);

            //resp = objacc.ModificaBDinsegura(sentencia, conexion, ref mensaje);

            if (resp)
            {

                this.ClientScript.RegisterStartupScript(this.GetType(),
                   "msgModificacion1", "verAlerta(`Correcto`, `" + mensaje + "`, `success`)", true);
            }
            else
            {
                this.ClientScript.RegisterStartupScript(this.GetType(),
                    "msgModificacion2", "verAlerta(`Error`, `" + mensaje + "`, `error`)", true);
            }
        }

        protected void Button6_Click(object sender, EventArgs e)
        {

          

            string sentencia = "Insert Into empleado values("+ txtId.Text +", '"
                + txtNombre.Text + "');";
            SqlConnection conexion = null;
            string mensaje = "";
            Boolean resp = false;
            conexion = objacc.AbrirConexion(ref mensaje);
            TextBox1.Text = sentencia;
         

            resp = objacc.ModificaBDinsegura(sentencia, conexion, ref mensaje);

            if (resp)
            {

                this.ClientScript.RegisterStartupScript(this.GetType(),
                   "msgModificacion1", "verAlerta(`Correcto`, `" + mensaje + "`, `success`)", true);
            }
            else
            {
                this.ClientScript.RegisterStartupScript(this.GetType(),
                    "msgModificacion2", "verAlerta(`Error`, `" + mensaje + "`, `error`)", true);
            }

        }

        protected void Button7_Click(object sender, EventArgs e)
        {


            //Declaración de parametros 
            SqlParameter[] misparametros = new SqlParameter[4];

            //Dando valores a los parámetros
            misparametros[0] = new SqlParameter("idprod", SqlDbType.Int);
            misparametros[0].Value = txtIdProd.Text;
            misparametros[0].Direction = ParameterDirection.Input;


            //Dando valores a los parámetros con formato JSON
            misparametros[1] = new SqlParameter
            {
                ParameterName = "descripcion",
                SqlDbType = SqlDbType.NVarChar,
                Size = 50,
                Direction = ParameterDirection.Input,
                Value = txtDesc.Text
            };

            

            misparametros[2] = new SqlParameter
            {
                ParameterName = "categoria",
                SqlDbType = SqlDbType.NVarChar,
                Size = 15,
                Direction = ParameterDirection.Input,
                Value = TxtCat.Text
            };

           
            misparametros[3] = new SqlParameter
            {
                ParameterName = "precio",
                SqlDbType = SqlDbType.Float,
                Direction = ParameterDirection.Input,
                Value = txtPrec.Text
            };


            

            string sentencia = "Insert Into productos values(@idprod, @descripcion, @categoria, @precio);";
            SqlConnection conexion = null;
            string mensaje = "";
            Boolean resp = false;
            conexion = objacc.AbrirConexion(ref mensaje);

            resp = objacc.ModificaBDUnPocoMasSegura(sentencia, conexion, ref mensaje, misparametros);

            //resp = objacc.ModificaBDinsegura(sentencia, conexion, ref mensaje);

            if (resp)
            {

                this.ClientScript.RegisterStartupScript(this.GetType(),
                   "msgModificacion1", "verAlerta(`Correcto`, `" + mensaje + "`, `success`)", true);
            }
            else
            {
                this.ClientScript.RegisterStartupScript(this.GetType(),
                    "msgModificacion2", "verAlerta(`Error`, `" + mensaje + "`, `error`)", true);
            }

        }
    }
}