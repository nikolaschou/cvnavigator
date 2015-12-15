using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Napp.Backend.DataAccess;
using Napp.Backend.Hibernate;

namespace Cvm.Web.CommonCtrl
{
    public partial class SkillMap1 : System.Web.UI.UserControl
    {
        
        private DataTable _skillMap;

        /// <summary>
        /// The number of top skills to be shown
        /// </summary>
        public int NumberOfSkills=40;

        /// <summary>
        /// The width of the map
        /// </summary>
        public int DivWidth=800;

        protected DataTable MySkillMap
        {
            get
            {
                if (_skillMap == null) _skillMap = LoadSkillMap();
                return _skillMap;
            }
        }

        public String Top1SkillName
        {
            get
            {
                DataRowCollection rows = MySkillMap.Rows;
                if (rows.Count == 0) return null;
                else return (String)rows[0]["SkillName"];
            }
        }
        private DataTable LoadSkillMap()
        {
            using(var con = HibernateMgr.Current.GetDirectSqlConnection())
            {
                System.Data.DataTable dbMan;

                con.Open();
                string query = @"select top " + this.NumberOfSkills + @" SUM(r.usedInTotal) as Total, SkillName from ResourceSkill r join Skill s on r.skillId=s.skillId
                               group by SkillName order by Total desc";
                
                dbMan = new DbManager().QueryToTable(con, query, "temp");

                con.Close();

                return dbMan;
            }
        }
    }
    
}