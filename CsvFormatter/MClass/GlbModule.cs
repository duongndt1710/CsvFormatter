using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsvFormatter.MClass
{
    /// <summary>
    /// Common methods and components using globally
    /// </summary>
    /// <remarks>
    /// 2017/05/17 Duong Created
    /// </remarks>
    public static class GlbModule
    {

        /// <summary>
        /// Convert the datetime string with the desire format
        /// </summary>
        /// <param name="inStrDate">Datetime string</param>
        /// <param name="inStrFormat">Desire format</param>
        /// <returns>
        /// 2017/05/17 Duong Created.
        /// </returns>
        public static String fncCnvDateOutputByFormat(String inStrDate, String inStrFormat)
        {
            // Logging
            //AppState.gLog.subWriteLog("fncCnvDateOutputByFormat -> '" + inStrDate + "' , '" + inStrFormat + "'");

            // Get the date from the input string
            DateTime dtmTemp = Convert.ToDateTime(inStrDate);

            // return the result with the desire format
            return dtmTemp.ToString(inStrFormat);

        }

        /// <summary>
        /// Returning the numeric value with the desired decimal format
        /// </summary>
        /// <param name="inStrNum"></param>
        /// <param name="strDecFormat"></param>
        /// <returns></returns>
        public static String fncSetDecimalDisp(String inStrNum, String strDecFormat)
        {
            // Convert the number to decimal
            Decimal decNum = Convert.ToDecimal(inStrNum);

            return decNum.ToString(strDecFormat);
        }

        /// <summary>
        /// Remove the first found Removing letter in the object string
        /// </summary>
        /// <param name="inStrObject"></param>
        /// <param name="inStrRemoveString"></param>
        /// <returns></returns>
        public static String fncGetStringByRemove(String inStrObject, String inStrRemoveString)
        {
            // Logging
            //AppState.gLog.subWriteLog("fncGetStringByRemove -> '" + inStrObject + "' , '" + inStrRemoveString + "'");

            // Finding the removing letter
            int intIdxOfRemoveLetter;
            intIdxOfRemoveLetter = inStrObject.IndexOf(inStrRemoveString);

            // Check if the removing letter is found
            if (intIdxOfRemoveLetter >= 0)
            {
                return inStrObject.Remove(intIdxOfRemoveLetter, 1);
            }
            else
            {
                return inStrObject;
            }



        }

    }
}
