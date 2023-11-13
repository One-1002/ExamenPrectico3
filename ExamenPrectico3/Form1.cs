using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;


namespace ExamenPrectico3
{
    public partial class Form1 : Form
    {
        //Datos de conexion a MySQL (XAMPP)
        string conexionSQL = "Server=localhost;Port=3306;Database=examenpractico;Uid=root;Pwd=;";
        //Metodo de para insertar registros
        public SerialPort ArduinoPort
        {
            get;
        }
        public Form1()
        {
            InitializeComponent();
            //crear Serial Port
            ArduinoPort = new System.IO.Ports.SerialPort();
            ArduinoPort.PortName = "COM13";
            ArduinoPort.BaudRate = 9600;
            ArduinoPort.DataBits = 8;
            ArduinoPort.ReadTimeout = 500;
            ArduinoPort.WriteTimeout = 500;
            ArduinoPort.Open();
            // vincular eventos
            this.FormClosing += CerrandoForm1;
            this.button1.Click += button1_Click;
            this.button3.Click += button3_Click;
            this.button2.Click += button2_Click;
            this.button4.Click += button4_Click;
        }
        private void InsertarRegistros(string nombre, string Fecha, float TempC, float TempF, float RAWVAL)
        {
            using (MySqlConnection conection = new MySqlConnection(conexionSQL))
            {
                conection.Open();
                string insertQuery = "INSERT INTO registros (Nombre, Fecha, TempC, TempF, RAWVAL)" +
                    "VALUES (@Nombre, @Fecha, @TempC, @TempF, @RAWVAL)";

                using (MySqlCommand command = new MySqlCommand(insertQuery, conection))
                {
                    command.Parameters.AddWithValue("@Nombre", nombre);
                    command.Parameters.AddWithValue("@Fecha", Fecha );
                    command.Parameters.AddWithValue("@TempC", TempC);
                    command.Parameters.AddWithValue("@TempF", TempF);
                    command.Parameters.AddWithValue("@RAWVAL", RAWVAL);
                    command.ExecuteNonQuery();
                }
                conection.Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ArduinoPort.Write("b");
        }
        private void CerrandoForm1(object sender, EventArgs e)
        {
            //cerrar puerto
            if (ArduinoPort.IsOpen) ArduinoPort.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ArduinoPort.Write("a");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!ArduinoPort.IsOpen)
                ArduinoPort.Open();
            ArduinoPort.Write("b");
            button2.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ArduinoPort.Write("a");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //obtener los datos de los textbox
            string nombre = textBox1.Text;
            string fecha = textBox2.Text;

            //Guardar los datos en un archivo de texto
            string rutaArchivo = "C:\\Users\\danpa\\OneDrive\\Escritorio\\Semestre 3\\Ruta\\Archivooneida.txt";
            bool archivoExiste = File.Exists(rutaArchivo);
            if (archivoExiste == false)
            {
               // File.WriteAllText(rutaArchivo, datos);
            }
            else
            {
                //Verificar si el archivo ya existe
                using (StreamWriter writer = new StreamWriter(rutaArchivo, true))
                {
                    if (archivoExiste)
                    {
                        //si el archivo existe, añadir un separador antes del nuevo registro
                        writer.WriteLine();
                        //Programacion de funcionalidad de insert SQL
                        InsertarRegistros(nombre, fecha);
                        MessageBox.Show("Datos ingresados correctamente");
                    }
                    else
                    {
                        writer.Write(datos);
                        //Programacion de funcionalidad de insert SQL
                        InsertarRegistros(nombres, apellido, int.Parse(edad), decimal.Parse(estatura), telefono, genero);
                        MessageBox.Show("Datos ingresados correctamente");
                    }
                }
            }
            //Mostrar un mensaje con los datos capturados
            MessageBox.Show("Datos guardados con éxito:\n\n" + datos, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                MessageBox.Show("Por favor, ingrese datos validos en los campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
        
    

