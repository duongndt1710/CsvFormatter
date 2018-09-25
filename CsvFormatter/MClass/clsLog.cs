using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows.Forms;

namespace CsvFormatter.MClass
{
    /// <summary>
    /// Log Class for loggin
    /// </summary>
    public class clsLog
    {

        #region Members

        private string m_strLogFilePath;
        private const string cStrFileLogNm = "Log";
        private const string cStrFileLogErrNm = "ErrLog";

        #endregion




        #region Subs And Functions

        #region Initialization

        /// <summary>
        /// Initialization
        /// </summary>
        public clsLog()
        {

            // Init the log path
            m_strLogFilePath = Application.StartupPath + "\\Log";

        }

        #endregion


        #region Output Log File

        /// <summary>
        /// Normal Log file
        /// </summary>
        /// <param name="_strConntent">Log Content</param>
        public void subWriteLog(string _strConntent)
        {

            string _strFile = m_strLogFilePath + "\\" + cStrFileLogNm + DateTime.Now.ToString("MMdd") + ".txt";

            try
            {

                // Checking File Path
                if (Directory.Exists(m_strLogFilePath + "\\" ) == false ) {

                    // Create the path
                    Directory.CreateDirectory(m_strLogFilePath + "\\");

                }


                // Checking the File 
                if (File.Exists(_strFile) == false)
                {

                    // Create the file
                    File.CreateText(_strFile).Close();

                }


                // Writing the text content
                this.subWriteFile(_strConntent, _strFile);

            } catch (Exception ex) {
                MessageBox.Show(ex.ToString(),"Error Message",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

        }


        /// <summary>
        /// Error Log file
        /// </summary>
        /// <param name="_strConntent">Error Log Content</param>
        public void subWriteLogErr(string _strConntent)
        {

            string _strFile = m_strLogFilePath + "\\Err\\" + cStrFileLogErrNm + DateTime.Now.ToString("MMdd") + ".txt";

            try
            {

                // Checking File Path
                if (Directory.Exists(m_strLogFilePath + "\\Err\\") == false)
                {

                    // Create the path
                    Directory.CreateDirectory(m_strLogFilePath + "\\Err\\");

                }


                // Checking the File 
                if (File.Exists(_strFile) == false)
                {

                    // Create the file
                    File.CreateText(_strFile).Close();

                }


                // Writing the text content
                this.subWriteFile(_strConntent, _strFile);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        /// <summary>
        /// WRiting the content to the appointed path
        /// </summary>
        /// <param name="_strContent">The content to be written</param>
        /// <param name="_strFile">The File to be written</param>
        private void subWriteFile(string _strContent, string _strFile)
        {

            // Checking if the file exist for over a year
            DateTime dtmCreated = Directory.GetCreationTime(_strFile);
            if (dtmCreated.Year < DateTime.Now.Year)
            {
                // Delete the file so that later the stream write will make a new one
                File.Delete(_strFile);

            }


            using (FileStream fs = new FileStream(_strFile,FileMode.Append,FileAccess.Write)) {

                using (StreamWriter ws = new StreamWriter(fs, Encoding.GetEncoding("Shift_JIS")))
                {

                    // writing the content
                    ws.Write(DateTime.Now.ToString("HH:mm:ssss: "));
                    ws.WriteLine(_strContent);

                    // Closing
                    ws.Close();

                }

                // Closing
                fs.Close();

            }

        }




        #endregion

        #endregion





    }
}
