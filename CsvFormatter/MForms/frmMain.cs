using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CsvFormatter.MClass;

namespace CsvFormatter.MForms
{
    public partial class frmMain : Form
    {

        #region Events

        #region Events-Forms

        /// <summary>
        /// Form's Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void subEvent_FrmMain_Load(object sender, EventArgs e)
        {

            // log
            MClass.AppState.gLog.subWriteLog("Form Load Start");

            // Invisible the form
            this.Visible = false;
            this.SuspendLayout();

            // Displaying in the center of the primary screen
            this.Top = (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2;
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;

            // Form re-display
            this.ResumeLayout(false);
            this.Visible = true;


        }

        /// <summary>
        /// Form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void subEvent_FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            AppState.gLog.subWriteLog("Form is closing");
        }

        #endregion

        #region Events Buttons

        private void subEvent_BtnSetting_Click(object sender, EventArgs e)
        {
            AppState.gLog.subWriteLog("BtnSetting Clicked");
        }

        /// <summary>
        /// Save file click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void subEvent_BtnSaveOutput_Click(object sender, EventArgs e)
        {
            AppState.gLog.subWriteLog("BtnSaveOutput Clicked");

            try
            {
                // Generating the output file
                AppState.gCsv.fncReFormatTheDataTable();

                // Save file dialog
                using (SaveFileDialog sfd = new SaveFileDialog()) 
                {
                    // Setting for the component
                    sfd.Filter = "CSV files (*.csv)|*.csv"; //File extension
                    sfd.RestoreDirectory = true;
                    sfd.InitialDirectory = this.txtOutput.Text;
                    sfd.FileName = DateTime.Now.ToString("yyyyMMddHHmmss");

                    // check the result
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                          
                        // Export the processed csv file with the input path from the SaveFiledialog
                        if (AppState.gCsv.fncOutputFile(sfd.FileName) == false)
                        {
                            MessageBox.Show("CSV出力失敗しました！\n本日のログファイルを設計者に送信してください。");
                        }
                        else
                        {
                            // Display the text into the Output text
                            this.txtOutput.Text = sfd.FileName;

                            MessageBox.Show("CSV出力が完了しました！ \n" + sfd.FileName);
                        }

                        

                    }

                }



            } catch (Exception ex) {
                AppState.gLog.subWriteLogErr(ex.ToString());
            }
        }

        /// <summary>
        /// Open Csv file button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void subEvent_BtnOpenInput_Click(object sender, EventArgs e)
        {
            AppState.gLog.subWriteLog("BtnOpenInput Clicked");

            bool blnOpenSuccess = false; // The file to check if the opening process is correctly handling after dispose the openfiledialog

            try
            {

                // Using open file dialog component
                using (OpenFileDialog ofdCsvFile = new OpenFileDialog()) {

                    // Set the opening folder
                    if (System.IO.Directory.Exists(this.txtInputFile.Text) == false) {               

                        this.txtInputFile.Text = Application.StartupPath; // Set as exe file startup path
                    
                    }
                    // asign the startup path
                    ofdCsvFile.InitialDirectory = this.txtInputFile.Text;
                       
                    // Dis-abled multiselect
                    ofdCsvFile.Multiselect = false;

                    // Set for extension
                    ofdCsvFile.Filter = "CSV files (*.csv)|*.csv";

                    // Show the file dialog
                    ofdCsvFile.ShowDialog();

                    if (ofdCsvFile.FileName != String.Empty)
                    {

                        // Get the file path
                        AppState.gCsv.FileInPath = ofdCsvFile.FileName;

                        // Set the file path for the opened file
                        this.txtInputFile.Text = ofdCsvFile.FileName;

                        // Check on open success
                        blnOpenSuccess = true;

                    }
                    else
                    {

                        // Discard the component
                        ofdCsvFile.Dispose();

                        // Log
                        AppState.gLog.subWriteLog("Cancel opening file");

                        return;
                    }

                    // Discard the component
                    ofdCsvFile.Dispose();

                }

                // Check if the file is selected normally
                if (blnOpenSuccess) {

                    // process the selected csv file
                    AppState.gCsv.fncReadFileCsv();

                }


            }
            catch (Exception ex)
            {
                AppState.gLog.subWriteLogErr(ex.ToString());
            }


        }


        #endregion



        #endregion


        #region Functions

        #region Initialization

        public frmMain()
        {
            InitializeComponent();

            // Init setting
            subInitSetting();
            subEventSetting();
        }

        /// <summary>
        /// Init setting of the form
        /// </summary>
        private void subInitSetting()
        {

            // Form style
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;


        }

        /// <summary>
        /// Event setting related
        /// </summary>
        private void subEventSetting()
        {
            // Form
            this.Load += subEvent_FrmMain_Load; // Form Load
            this.FormClosing += subEvent_FrmMain_FormClosing;

            // Buttons
            this.btnOpenInput.Click += subEvent_BtnOpenInput_Click;
            this.btnSaveOutput.Click += subEvent_BtnSaveOutput_Click;
            this.btnSetting.Click += subEvent_BtnSetting_Click;

        }






        #endregion


        #endregion


    }
}
