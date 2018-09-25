using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsvFormatter.MClass;

namespace CsvFormatter.MClass
{
    public static class AppState
    {


        #region Logging

        /// <summary>
        /// Logging function
        /// </summary>
        /// <remarks>
        /// 2017/5/12 Duong Created
        /// </remarks>
        public static clsLog gLog = new clsLog();

        #endregion

        #region Csv Operating

        /// <summary>
        /// Csv file operating
        /// </summary>
        /// <remarks>
        /// 2017/5/15 Duong Created
        /// </remarks>
        public static clsCsv gCsv = new clsCsv();

        #endregion
    }
}
