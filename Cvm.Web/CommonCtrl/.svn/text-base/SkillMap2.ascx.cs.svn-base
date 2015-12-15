using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Web.Navigation;
using Iesi.Collections.Generic;
using Napp.Backend.DataAccess;
using Napp.Backend.Hibernate;
using Napp.Web.Navigation;

namespace Cvm.Web.CommonCtrl
{
    public partial class SkillMap2 : System.Web.UI.UserControl
    {
        protected const string RELATION_WEIGHT = "relationWeight";
        protected const string FIRST_SKILL_NAME = "firstSkillName";
        protected const string SECOND_SKILL_NAME = "secondSkillName";




        private HashSet<String> appendedNodes = new HashSet<string>();
        /// <summary>
        /// Returns true if the skill was already appended as a node.
        /// </summary>
        /// <param name="skillName"></param>
        /// <returns></returns>
        protected bool DidAppendNode(String skillName)
        {
            if (appendedNodes.Contains(skillName))
            {
                return true;
            }
            else
            {
                appendedNodes.Add(skillName);
                return false;
            }
        }


        protected string GetColorForNode(string skillName, string firstSkillName, string color)
        {
            return (firstSkillName.Equals(skillName) ? "black" : color);
        }


        private HashedSet<String> skillNames = new HashedSet<string>();
        private DataTable _relatedSkills;
        private String _skillName;

        /// <summary>
        /// Returns the skills related to a given skillName. The result is cached pr. request.
        /// </summary>
        /// <param name="skillName"></param>
        /// <returns></returns>
        protected DataTable GetRelatedSkills(string skillName)
        {
            if (!String.Equals(_skillName, skillName))
            {
                this._skillName = skillName;
                this._relatedSkills = GetRelatedSkillsUtil(skillName);
            }
            return this._relatedSkills;
        }

        protected DataTable GetRelatedSkillsUtil(string skillName)
        {
            DataTable tb;
            using (var conn = HibernateMgr.Current.GetDirectSqlConnection())
            {
                conn.Open();
                //
                String sql =
                    @"
  select top 20 a.firstSkillName, a.secondSkillName, a.relationWeight  from SkillRelation a
  where (a.firstSkillName='{0}' or a.secondSkillName='{0}') and a.distance=1 
  order by a.relationWeight desc";
                tb = new DbManager().QueryToTable(conn, String.Format(sql, skillName), "temp");
                conn.Close();
            }
            return tb;
        }

  

        protected String RenderSkill(string skillName)
        {
            if (String.IsNullOrEmpty(skillName))
            {
                Utl.Msg.PostMessage("SkillMap.MustEnterSkillName");
                return "";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                RenderSkill(skillName, 0, sb);
                return sb.ToString();
            }

        }
        protected void RenderSkill(string skillName, int level, StringBuilder sb)
        {
            DataRowCollection result = GetRelatedSkills(skillName).Rows;
            Dictionary<int, int> colorMap = GenerateColorMap(result);
            foreach (DataRow row in result)
            {
                string color = MakeRgbColor(colorMap[(int)(decimal)row["relationWeight"]]);
                string firstSkillName = (string)row[FIRST_SKILL_NAME];
                string secondSkillName = (string)row[SECOND_SKILL_NAME];
                EnsureNodeIsAppended(firstSkillName, sb, GetColorForNode(skillName, firstSkillName, color));
                EnsureNodeIsAppended(secondSkillName, sb, GetColorForNode(skillName, secondSkillName, color));
                sb.AppendLine(String.Format("g.addEdge('{0}', '{1}',  {{ color:'{2}',label : 'Label'}});", row[FIRST_SKILL_NAME], row[SECOND_SKILL_NAME], color));
                this.skillNames.Add(firstSkillName);
                this.skillNames.Add(secondSkillName);
                if (level > 0)
                {
                    //Recurse
                    RenderSkill(secondSkillName, level - 1, sb);
                }
            }
        }

        protected void EnsureNodeIsAppended(String skillName, StringBuilder sb, string color)
        {
            if (!DidAppendNode(skillName))
            {

                String size, href;
                if (MakeHrefLinks) href = "'"+CvmPages.SkillGraphLink(skillName).GetLinkAsHref()+"'";
                else href = "null";
                size = "20";
                sb.AppendLine(String.Format("g.addNode('{0}',{{color:'{1}',size:'{2}',href:{3},label:'{4}'}});", skillName, color, size, href, skillName));
            }
        }





        protected string MakeRgbColor(int color)
        {
            return "rgb(" + color + "," + color + "," + color + ")";
        }

        /// <summary>
        /// Returns map from relationWeight ranging from 0 to infinity to a color code
        /// ranging from 0 to 255.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected Dictionary<int, int> GenerateColorMap(DataRowCollection data)
        {

            Dictionary<int, int> result = new Dictionary<int, int>();
            foreach (DataRow row in data)
            {
                int key = (int)(decimal)row[RELATION_WEIGHT];
                result[key] = 0;
            }
            if (result.Count == 0)
            {
                Utl.Msg.PostMessage("SkillMap.FoundNoSkills");
                return result;
            }
            Dictionary<int, int>.KeyCollection keys = result.Keys;

            int minVal = keys.Min();
            int maxVal = keys.Max();
            int range = (maxVal - minVal);
            if (range > 0)
            {
                foreach (DataRow row in data)
                {
                    int key = (int)(decimal)row[RELATION_WEIGHT];

                    int intDistance = key - minVal;
                    int colorCode = 255 - (255 * intDistance) / range;
                    result[key] = colorCode;
                }
            }
            return result;
        }

        private string _currentSkillName;
        /// <summary>
        /// The name of the skill in context of the skill map
        /// </summary>
        public string CurrentSkillName
        {
            get { return _currentSkillName; }
            set { 
                this._currentSkillName = value;
                this.GraphLit.Text=RenderSkill(this._currentSkillName);
            }
        }

        public bool MakeHrefLinks = true;
    }
}