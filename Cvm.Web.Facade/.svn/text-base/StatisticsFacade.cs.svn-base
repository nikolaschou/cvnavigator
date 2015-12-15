using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using Napp.Backend.Business.Meta;
using Napp.Backend.DataAccess;
using Napp.Backend.Hibernate;

namespace Cvm.Web.Facade
{
    public class StatisticsFacade
    {
        private const int SIZE = 62;
        /// <summary>
        /// The lastModifiedLimit will be the size minus one negative
        /// </summary>
        private DateTime LastModifiedLimit = DateTime.Now.AddDays(-1*(SIZE-1));
        private const string LastModifiedTs = "LastModifiedTs";
        private const string DATE_FORMAT = "yyyy-MM-dd";
        

        public TableCountResult GetTableCounts(IList<TableObj> tables)
        {
            var mgr = new DbManager();
            //We use a two-dimensional array:
            // First index: table1 | table2 | table3 counted succeeding
            // Second index: date1 | date2 | date3 counted as 0 for today, 1 for yesterday etc.
            // Value: The number of updates records in the given table for the given date
            int[,] data = new int[tables.Count, SIZE];
            List<String> tableNames = new List<string>();
            //Maps from dates on the form yyyy-mm-dd to an integer where 0 means today, 1 yesterday etc.

            using (var conn = HibernateMgr.Current.GetDirectSqlConnection())
            {
                conn.Open();
                int counter = 0;
                foreach (var t in tables)
                {
                    string sql = "select * from " + t.GetTableName() + " where 1=0";
                    var tb = mgr.QueryToTable(conn, sql, "temp");
                    if (tb.Columns.Contains(LastModifiedTs))
                    {
                        DateTime lastMonth = LastModifiedLimit;
                        //As we need the day part of the DateTime, we must use an ms sql function
                        string convertDateLastmodifiedts = "CONVERT(DATE, " + LastModifiedTs + ")";
                        sql =
                            String.Format(
                                "select count(*) as CNT, {0} as {3} from {1} where {3} >= '{2}' group by {0} order by {0}",
                                convertDateLastmodifiedts, t.GetTableName(), lastMonth.ToString(DATE_FORMAT), LastModifiedTs);
                        tb = mgr.QueryToTable(conn, sql, "temp");
                        CopyValuesToArray(data, counter, tb);
                        tableNames.Add(t.GetTableName());
                        counter++;
                    }
                }
                conn.Close();
            }
            return new StatisticsFacade.TableCountResult(tableNames, data);
        }

        public class TableCountResult
        {
            private List<string> tableNames;
            private int[,] data;

            public TableCountResult(List<string> tableNameses, int[,] data)
            {
                this.tableNames = tableNameses;
                this.data = data;
            }

            /// <summary>
            /// Returns the number of updates rows in the given table for the given day
            /// </summary>
            /// <param name="tableIndex">The table index, can be translated to a table name using GetTableName</param>
            /// <param name="noOfDays">Number of days since today, 0 is today, 1 is yesterday etc.</param>
            /// <returns></returns>
            public int GetCount(int tableIndex, int noOfDays)
            {
                return data[tableIndex, noOfDays];
            }

            public String GetTableName(int tableIndex)
            {
                return tableNames[tableIndex];
            }
            public String GetDateStr(int noOfDays)
            {
                return Int2DateStr(noOfDays);
            }
            /// <summary>
            /// Returns the number of tables in the 
            /// As the data-array is initialized to the length of all tables in the database but
            /// only populated with tables which LastModifiedTs column, the actual size
            /// of populated is found from the tableNames length.
            /// </summary>
            /// <returns></returns>
            public int GetNoTables()
            {
                return tableNames.Count;
            }
            public int GetNoDays()
            {
                return data.GetUpperBound(1);
            }

            public string GetDateStrDayOnly(int noOfDays)
            {
                int day = DateTime.Now.AddDays(-noOfDays).Day;
                if (day >= 10) return day.ToString();
                else return "0" + day;
            }
        }

        private void CopyValuesToArray(int[,] data, int counter, DataTable tb)
        {
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                //   table  , map to number of days since today                              //number of rows-count
                data[counter, this.MapStringDate2Int((DateTime)tb.Rows[i][LastModifiedTs])] = (int)tb.Rows[i]["CNT"];
            }
        }

        private Dictionary<string, int> _date2int;
        /// <summary>
        /// Maps a date string on the form yyyy-mm-dd into an integer such that 
        /// 0 is today, 1 is yesterday, e.g. it returns the number of days since today (zero-based).
        /// </summary>
        /// <param name="dateStr"></param>
        /// <returns></returns>
        private int MapStringDate2Int(String dateStr)
        {
            if (_date2int == null) _date2int = GetDateMap();
            if (_date2int.ContainsKey(dateStr)) return _date2int[dateStr];
            //Otherwise, try parse
            string[] dateParts = dateStr.Split('-');
            int year = int.Parse(dateParts[0]);
            int month = int.Parse(dateParts[1]);
            int day = int.Parse(dateParts[2]);
            DateTime date = new DateTime(year, month, day);
            string key = date.ToString(DATE_FORMAT);
            if(_date2int.ContainsKey(key)) return _date2int[key];
            else throw new Exception("Could not find key "+key);
        }
        
        private int MapStringDate2Int(DateTime date)
        {
            return MapStringDate2Int(date.ToString(DATE_FORMAT));
        }
        private Dictionary<string, int> GetDateMap()
        {
            var map = new Dictionary<string, int>();
            
            for (int i = 0; i < SIZE; i++)
            {
                map[Int2DateStr(i)] = i;
            }
            return map;
        }

        protected static string Int2DateStr(int noOfDays)
        {
            DateTime now = DateTime.Now;
            return now.AddDays(-noOfDays).ToString(DATE_FORMAT);
        }
    }
}
