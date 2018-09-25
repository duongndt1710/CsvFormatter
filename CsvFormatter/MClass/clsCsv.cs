using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Microsoft.VisualBasic.FileIO; // You have to add into reference the Microsoft.VisualBasic
using CsvFormatter.MClass;
    
namespace CsvFormatter.MClass
{
    /// <summary>
    /// A class include properties and methods dealing with csv file
    /// </summary>
    public class clsCsv
    {

        #region Members
        /// <summary>
        /// The Csv File Intput Path
        /// </summary>
        private String m_strFileInPath;
        /// <summary>
        /// The Csv File Output Path
        /// </summary>
        private String m_strFileOutPath;
        /// <summary>
        /// The readed input Datatable
        /// </summary>
        private DataTable m_dtDataIn;
        /// <summary>
        /// The exporting Datatable
        /// </summary>
        private DataTable m_dtDataOut;


        #endregion

        #region Properties

        /// <summary>
        /// Get or set the file path for the input file
        /// </summary>
        public String FileInPath
        {
            get { return this.m_strFileInPath; }
            set { this.m_strFileInPath = value; }
        }

        /// <summary>
        /// Get or set the file path for the output file
        /// </summary>
        public String FileOutPath
        {
            get { return this.m_strFileOutPath; }
            set { this.m_strFileOutPath = value; }
        }

        /// <summary>
        /// Get the DataTable of the input file
        /// </summary>
        public DataTable DataTableIn
        {
            get {
                if (m_dtDataIn != null)
                {
                    return this.m_dtDataIn;
                } else
                {
                    return null;
                }
            }
            
        }

        /// <summary>
        /// Get the DataTable of the Output file
        /// </summary>
        public DataTable DataTableOut
        {
            get
            {
                if (m_dtDataOut != null)
                {
                    return this.m_dtDataOut;
                }
                else
                {
                    return null;
                }
            }

        }


        #endregion


        #region Methods

        #region Init

        /// <summary>
        /// Constructor for new instance
        /// </summary>
        public clsCsv()
        {
            m_strFileInPath = "";
            m_strFileOutPath = "";
            m_dtDataOut = new DataTable();
            m_dtDataIn = new DataTable();

        }

        #endregion

        #region Read the Csv file

        /// <summary>
        /// Read the Csv File from the path argument
        /// </summary>
        /// <param name="inStrFilePath"></param>
        public bool fncReadFileCsv()
        {

            try
            {
                // Check if the file exist. If true then save the file path
                if (File.Exists(this.m_strFileInPath))
                {

                    this.subGetTableCsv();

                    return true;
                }
                else
                {

                    // Exit sub and return false
                    AppState.gLog.subWriteLog("Csv file do not exist!" + "\r\n" + this.m_strFileInPath);
                    return false;
                }

            } catch (Exception ex)
            {
                AppState.gLog.subWriteLogErr(ex.ToString());
                return false;
            }


        }


        /// <summary>
        /// Procesing and get the table
        /// </summary>
        /// <param name="inStrFilePath">File path</param>
        private void subGetTableCsv()
        {
            AppState.gLog.subWriteLog("Reading the Csv File = " + this.m_strFileInPath);

            

                using (TextFieldParser tfp = new TextFieldParser(this.m_strFileInPath, System.Text.Encoding.GetEncoding(932)))
                {
                    // Indicates that the fields are delimited
                    tfp.TextFieldType = FieldType.Delimited;

                    // using comma as seperator
                    tfp.SetDelimiters(",");

                    // Allow double quotes for some fields
                    tfp.HasFieldsEnclosedInQuotes = true;

                    // Trim white space
                    tfp.TrimWhiteSpace = true;

                    // Adding columns for the table
                    bool blnAddingColumn = true;

                    // Running loop for each line of the csv file
                    while (tfp.EndOfData == false)
                    {
                        // Read a line from the file and put to the array
                        String[] fields = tfp.ReadFields();

                        // Processing for adding columns 
                        if (blnAddingColumn)
                        {

                            // Re-initialize the datatable
                            this.m_dtDataIn = new DataTable();

                            // Adding Columns
                            for (int idx=0; idx <= fields.Length -1; idx++)
                            {
                                this.m_dtDataIn.Columns.Add("Col" + idx.ToString("00"), Type.GetType("System.String"));
                            }

                            // Make sure the no more columns add
                            blnAddingColumn = false;                            
                        }

                        // Put the dataRow into the datatable
                        DataRow dRow = this.m_dtDataIn.NewRow();
                        for (int i=0; i < fields.Length; i++)
                        {
                            dRow[i] = fields[i];
                        }

                        // Add the datarow
                        this.m_dtDataIn.Rows.Add(dRow);
                    }


                }

                        
        }

        #endregion

        #region Processing_The_Data_Table

        /// <summary>
        /// Re-format the datatable according to the specific 
        /// </summary>
        /// <returns></returns>
        public bool fncReFormatTheDataTable()
        {
            AppState.gLog.subWriteLog("Processing the DataTable");

            try
            {

                if (this.m_dtDataIn.Columns.Count <= 0)
                {
                    // There is no data to be processed
                    AppState.gLog.subWriteLog("There is no data to be processed");
                    return false;
                }
                else
                {
                    // Adding the columns to the output table 
                    this.m_dtDataOut = new DataTable(); // Declare new table

                    // 20170701 Duong CHG START >>>>>>>>>>>>>>>>>>>>>>>>>
                    //for (int idx = 0; idx <= m_dtDataIn.Columns.Count - 1; idx++ )
                    //{
                    //    // Adding columns into dtDataOut with same DataIn 
                    //    this.m_dtDataOut.Columns.Add(this.m_dtDataIn.Columns[idx].ColumnName, Type.GetType("System.String"));
                    //}

                    this.m_dtDataOut.Columns.Add("CurrPair", Type.GetType("System.String"));
                    this.m_dtDataOut.Columns.Add("OpenDate", Type.GetType("System.String"));
                    this.m_dtDataOut.Columns.Add("OpenValue", Type.GetType("System.String"));
                    this.m_dtDataOut.Columns.Add("CloseDate", Type.GetType("System.String"));
                    this.m_dtDataOut.Columns.Add("CloseValue", Type.GetType("System.String"));

                    // 20170701 Duong CHG E N D <<<<<<<<<<<<<<<<<<<<<<<<<

                    
                    // Processing for the data of the input table into the output table                    
                    for (int r_idx = 0; r_idx < m_dtDataIn.Rows.Count; r_idx++) // For each row in the table
                    { 

                        // Adding new row to the Output Table
                        DataRow dRow = this.m_dtDataOut.NewRow();
                        String strOpenVal = "";
                        String strCloseVal = "";

                        for (int c_idx = 0; c_idx < m_dtDataIn.Columns.Count; c_idx++) // For each column in the row
                        {

                            if (r_idx < 3)  // The first three rows are just normal copy
                            {
                                //dRow[c_idx] = this.m_dtDataIn.Rows[r_idx].ItemArray[c_idx].ToString();
                                continue;
                            }
                            else
                            {
                                switch (this.m_dtDataIn.Columns[c_idx].ColumnName) // Duong is coding here
                                {

                                    case "Col00": // format currency-pair
                                        dRow["CurrPair"] = GlbModule.fncGetStringByRemove(m_dtDataIn.Rows[r_idx].ItemArray[c_idx].ToString(), "/");
                                        break;

                                    case "Col01": // format date open
                                        dRow["OpenDate"] = GlbModule.fncCnvDateOutputByFormat(this.m_dtDataIn.Rows[r_idx].ItemArray[c_idx].ToString(), "yyyy-MM-dd HH:mm:ss");
                                        break;

                                    case "Col02": // the open value
                                        strOpenVal = GlbModule.fncSetDecimalDisp( this.m_dtDataIn.Rows[r_idx].ItemArray[c_idx].ToString(), "#0.00000");
                                        break;

                                    case "Col03": // format date close
                                        dRow["CloseDate"] = GlbModule.fncCnvDateOutputByFormat(this.m_dtDataIn.Rows[r_idx].ItemArray[c_idx].ToString(), "yyyy-MM-dd HH:mm:ss");
                                        break;

                                    case "Col04": // the close value
                                        strCloseVal = GlbModule.fncSetDecimalDisp( this.m_dtDataIn.Rows[r_idx].ItemArray[c_idx].ToString(), "#0.00000");
                                        break;

                                    case "Col05":

                                        // Processing the Open and Close value
                                        switch (this.m_dtDataIn.Rows[r_idx].ItemArray[c_idx].ToString())
                                        {
                                            case "売":
                                                strOpenVal = "-" + strOpenVal;
                                                break;

                                            case "買":
                                                strCloseVal = "-" + strCloseVal;
                                                break;
                                        }

                                        // Set on the value at OpenVal and Close Val
                                        dRow["OpenValue"] = strOpenVal;
                                        dRow["CloseValue"] = strCloseVal;

                                        break;

                                    default:      // The other normal fields
                                        //dRow[c_idx] = this.m_dtDataIn.Rows[r_idx].ItemArray[c_idx].ToString();
                                        break;
                                }
                            }

                            

                        }

                        // Add the DataRow
                        this.m_dtDataOut.Rows.Add(dRow);

                    }

                    // the first two time column in the 2nd row
                    this.m_dtDataOut.Rows[1].ItemArray[1] = GlbModule.fncCnvDateOutputByFormat(this.m_dtDataIn.Rows[1].ItemArray[1].ToString(), "yyyy-MM-dd HH:mm:ss");
                    this.m_dtDataOut.Rows[1].ItemArray[1] = GlbModule.fncCnvDateOutputByFormat(this.m_dtDataIn.Rows[1].ItemArray[1].ToString(), "yyyy-MM-dd HH:mm:ss");
                    
                }


                return true;
            }
            catch (Exception ex)
            {
                AppState.gLog.subWriteLogErr(ex.ToString());
                return false;
            }
        }

        #endregion

        #region Outpu_The_CsvFile

        /// <summary>
        /// Export the file in csv format. Automatically create the file if the file doesn't exist.
        /// </summary>
        /// <param name="inStrFile"></param>
        /// <returns></returns>
        public bool fncOutputFile(String inStrFile)
        {

            AppState.gLog.subWriteLog("fncOutputFile -> " + inStrFile);

            try
            {

                // Checking the File 
                if (File.Exists(inStrFile) == false)
                {

                    // Create the file
                    File.CreateText(inStrFile).Close();

                }

                using (FileStream fs = new FileStream(inStrFile,FileMode.Append,FileAccess.Write)) {

                    using (StreamWriter ws = new StreamWriter(fs, System.Text.Encoding.GetEncoding("Shift_JIS")))
                    {

                        // writing the content
                        // 20170701 Duong Chg START >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                        //for (int r_idx = 0; r_idx < this.m_dtDataOut.Rows.Count; r_idx++)
                        for (int r_idx = 3; r_idx < this.m_dtDataOut.Rows.Count; r_idx++) // starting from third row
                        // 20170701 Duong Chg E N D <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                        {

                            // first column
                            // 20170701 duong chg START >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                            // Reasons: the double-quote signs are not necessary anymore
                            //ws.Write("\"" + this.m_dtDataOut.Rows[r_idx].ItemArray[0] + "\"");

                            //// the rest
                            //for (int c_idx = 1; c_idx < this.m_dtDataOut.Columns.Count; c_idx++)
                            //{
                            //    ws.Write(",\"" + this.m_dtDataOut.Rows[r_idx].ItemArray[c_idx] +"\"");
                            //}
                            ws.Write("" + this.m_dtDataOut.Rows[r_idx].ItemArray[0] + "");

                            // the rest
                            for (int c_idx = 1; c_idx < this.m_dtDataOut.Columns.Count; c_idx++)
                            {
                                ws.Write(", " + this.m_dtDataOut.Rows[r_idx].ItemArray[c_idx] + "");
                            }
                            // 20170701 duong chg E N D >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                            // next line
                            ws.WriteLine();
                        }


                            // Closing
                            ws.Close();

                    }

                    // Closing
                    fs.Close();

                }


                // Successfully output the file
                return true; 

            } 
            catch (Exception ex) {

                AppState.gLog.subWriteLog("fncOutputFile -> Fail");
                AppState.gLog.subWriteLogErr(ex.ToString());

                return false;

            } 
        }



        #endregion 

        #endregion

    }
}
