using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Updater
{


    public partial class Form1 : Form
    {
        static string ConnStr = "Data Source=etrav-hack;Initial Catalog=images;Persist Security Info=True;User ID=Application;Password=yourpassword";

        

        public Form1()

        {
            InitializeComponent();
            //string CallingApp = null;
            //tester
            //updater.getupdateApp("QualityPlan", "QualityPlanNET.frx", null);

        }

        public class updater : IDisposable
        {
            //string CallingApp = null; //Fortesting
            string CallingFormfrx = null;
            FileStream DevEXlib = null; //Holds collection of DevExpress dll's
            string DVXName;
            public Process[] processes { get; private set; }
            public void Dispose()
            {
                if (DevEXlib != null)
                {
                    DevEXlib.Dispose();
                    DevEXlib = null;
                }
            }
            public void getupdateApp(string CallingAppN, string CallingformN, string CallingVerN) //Goes and gets Updated application and Fast Report frx files as well as Devexpress dll's
            {
                //textBox1.Text = CallingAppN + CallingformN + CallingVerN;
                string CallingApp = CallingAppN; CallingFormfrx = CallingformN;
                System.IO.FileStream fP;                          // Writes the BLOB to a file (*.bmp).
                BinaryWriter bw;                        // Streams the BLOB to the FileStream object.
                const int bufferSize = 1000;                   // Size of the BLOB buffer.
                byte[] outbyte = new byte[bufferSize];  // The BLOB byte[] buffer to be filled by GetBytes.
                long retval;                            // The bytes returned from GetBytes.
                long startIndex = 0;                    // The starting position in the BLOB output.
                closerunningapp(CallingApp);

                //This section updates the CallingApp if not null
                if (CallingApp == null) { MessageBox.Show("dll not called properly! App name missing!"); return; }
                using (System.Data.SqlClient.SqlConnection SqlConn = new SqlConnection(ConnStr))
                using (SqlCommand command = SqlConn.CreateCommand())
                {
                    //varPathToNewLocation = userRoot;
                    SqlConn.Open();
                    command.Parameters.AddWithValue("@varID", CallingApp);
                    command.CommandText = string.Format("SELECT VSApplication FROM tblVSapplications WHERE Applicationname='{0}'", CallingApp + ".exe");
                    using (SqlDataReader sqlQueryResult = command.ExecuteReader(CommandBehavior.SequentialAccess))

                        while (sqlQueryResult.Read())
                        {
                            //fP = new FileStream(sqlQueryResult.ToString(), FileMode.OpenOrCreate, FileAccess.Write);
                            try
                            {


                                fP = new FileStream(string.Format("C:\\Sql\\{0}.exe", CallingApp), FileMode.OpenOrCreate, FileAccess.Write);
                            }
                            catch (IOException)
                            {

                                return;
                            }
                            bw = new BinaryWriter(fP);
                            startIndex = 0;
                            retval = sqlQueryResult.GetBytes(0, startIndex, outbyte, 0, bufferSize);
                            // Continue reading and writing while there are bytes beyond the size of the buffer.
                            while (retval == bufferSize)
                            {
                                bw.Write(outbyte);
                                bw.Flush();

                                // Reposition the start index to the end of the last buffer and fill the buffer.
                                startIndex += bufferSize;
                                retval = sqlQueryResult.GetBytes(0, startIndex, outbyte, 0, bufferSize);
                            }

                            // Write the remaining buffer.
                            bw.Write(outbyte, 0, (int)retval);
                            bw.Flush();

                            // Close the output file.
                            bw.Close();
                            fP.Close();
                            //checkedListBox1.SetItemChecked(0, true);



                        }

                    reopenrunningapp(CallingApp);

                    //This section updates the CallingFormfrx if not null
                    if (CallingFormfrx == null) { MessageBox.Show("dll not called properly! frx name missing!"); return; }
                    using (System.Data.SqlClient.SqlConnection SqlConn2 = new SqlConnection(ConnStr))
                    using (SqlCommand command2 = SqlConn2.CreateCommand())
                    {
                        //varPathToNewLocation = userRoot;
                        SqlConn.Open();
                        command2.Parameters.AddWithValue("@varID", CallingFormfrx);
                        command2.CommandText = string.Format("SELECT VSApplication FROM tblVSapplications WHERE Applicationname='{0}'", CallingFormfrx);
                        using (SqlDataReader sqlQueryResult = command2.ExecuteReader(CommandBehavior.SequentialAccess))

                            while (sqlQueryResult.Read())
                            {
                                //fP = new FileStream(sqlQueryResult.ToString(), FileMode.OpenOrCreate, FileAccess.Write);
                                try
                                {


                                    fP = new FileStream("C:\\Sql\\" + CallingFormfrx, FileMode.OpenOrCreate, FileAccess.Write);
                                }
                                catch (IOException)
                                {

                                    return;
                                }
                                bw = new BinaryWriter(fP);
                                startIndex = 0;
                                retval = sqlQueryResult.GetBytes(0, startIndex, outbyte, 0, bufferSize);
                                // Continue reading and writing while there are bytes beyond the size of the buffer.
                                while (retval == bufferSize)
                                {
                                    bw.Write(outbyte);
                                    bw.Flush();

                                    // Reposition the start index to the end of the last buffer and fill the buffer.
                                    startIndex += bufferSize;
                                    retval = sqlQueryResult.GetBytes(0, startIndex, outbyte, 0, bufferSize);
                                }

                                // Write the remaining buffer.
                                bw.Write(outbyte, 0, (int)retval);
                                bw.Flush();

                                // Close the output file.
                                bw.Close();
                                fP.Close();
                                //checkedListBox1.SetItemChecked(1, true);


                            }
                    }

                    //This section updates the DevExpress Library if not null and requested by IT.
                    if (DevEXlib == null) { return; }
                    //Must Delete Old DevExpress Library
                    using (System.Data.SqlClient.SqlConnection SqlConn3 = new SqlConnection(ConnStr))
                    using (SqlCommand command3 = SqlConn3.CreateCommand())
                    {
                        //varPathToNewLocation = userRoot;
                        SqlConn.Open();
                        command3.Parameters.AddWithValue("@varID", CallingFormfrx);
                        command3.CommandText = string.Format("SELECT VSApplication FROM tblVSapplications WHERE Applicationname='{0}'", DVXName);
                        using (SqlDataReader sqlQueryResult = command3.ExecuteReader(CommandBehavior.SequentialAccess))

                            while (sqlQueryResult.Read())
                            {
                                
                                try
                                {


                                    fP = new FileStream("C:\\Sql\\" + DVXName, FileMode.OpenOrCreate, FileAccess.Write);
                                }
                                catch (IOException)
                                {

                                    return;
                                }
                                bw = new BinaryWriter(fP);
                                startIndex = 0;
                                retval = sqlQueryResult.GetBytes(0, startIndex, outbyte, 0, bufferSize);
                                // Continue reading and writing while there are bytes beyond the size of the buffer.
                                while (retval == bufferSize)
                                {
                                    bw.Write(outbyte);
                                    bw.Flush();

                                    // Reposition the start index to the end of the last buffer and fill the buffer.
                                    startIndex += bufferSize;
                                    retval = sqlQueryResult.GetBytes(0, startIndex, outbyte, 0, bufferSize);
                                }

                                // Write the remaining buffer.
                                bw.Write(outbyte, 0, (int)retval);
                                bw.Flush();

                                // Close the output file.
                                bw.Close();
                                fP.Close();
                                
                            }
                    }
                }
                //this.Close();
                return;
            }

            private void reopenrunningapp(string callingApp)
            {
                var p = new Process();
                p.StartInfo.FileName = callingApp + ".exe";
                p.Start();
            }

            public void closerunningapp(string app)
            {
                app = app.ToLower();
                
                Process[] processes = Process.GetProcesses();
                try
                {
                    foreach (Process proc in processes)
                    {
                        string ProcessName = proc.ProcessName;
                        ProcessName = ProcessName.ToLower();
                        if (ProcessName.CompareTo(app) == 0)
                            proc.Kill();
                        //proc.CloseMainWindow();
                        //proc.WaitForExit();
                    }
                    //buildList();
                }
                catch (System.NullReferenceException)
                {
                    MessageBox.Show("No instances of" + app + "running.");
                    //checkedListBox1.Items.Add(string.Format("No instances of{0}running.", app));
                    //checkedListBox1.Update();
                }
            }

            private Process GetaProcess(string processname)
            {
                Process[] aProc = Process.GetProcessesByName(processname);

                if (aProc.Length > 0)
                    return aProc[0];

                else return null;
            }

            void myprc_Exited(object sender, EventArgs e)
            {
                MessageBox.Show(((Process)sender).ProcessName + " process has exited!");
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }

}
