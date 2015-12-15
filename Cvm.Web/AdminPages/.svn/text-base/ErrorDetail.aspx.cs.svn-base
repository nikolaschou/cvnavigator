using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cvm.Web.Code;
using Cvm.Web.Navigation;
using Napp.Web.Navigation;

namespace Cvm.Web.AdminPages
{
    public partial class ErrorDetail : System.Web.UI.Page
    {
        private const int NumberOfLines = 100;
        private string[] _files;
        private string logPath;
        private QueryStringHelper q = QueryStringHelper.Instance;
        protected String currentFile = null;

        public ErrorDetail()
        {
            logPath = Server.MapPath("~/Logs");
        }

        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            


        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (q.HasParm(QueryParmCvm.search))
            {
                string searchTxt = q.GetParmOrFail(QueryParmCvm.search);
                this.TextLiteral.Text = this.SearchErrorFile(searchTxt);

            }
            else if (q.HasParm(QueryParmCvm.fileName))
            {
                currentFile = q.GetParmOrFail(QueryParmCvm.fileName);
                this.TextLiteral.Text = GetContentOfFile(currentFile);

            }
            else
            {
                this.TextLiteral.Text = "";
                currentFile = "";
            }

            this.FileNameLit.Text = currentFile;
            Rep1.DataSource = FileNames;
            Rep1.DataBind();
        }

        private string GetContentOfFile(String _fileName)
        {
            String content;
            using(FileStream fs=File.Open(logPath + "/" + _fileName, FileMode.Open, FileAccess.Read,FileShare.ReadWrite))
            {
                StreamReader r = new StreamReader(fs);
                content= r.ReadToEnd();
                fs.Close();
            }
            return content;
        }

        protected String[] FileNames
        {
            get
            {
                if (_files==null)
                {
                    List<String> fs= new List<string>(Directory.GetFiles(logPath));
                    //Change sort order
                    fs.Sort((a,b)=>b.CompareTo(a));
                    //Last file (todays log file) should go first
                    fs.Insert(0,fs[fs.Count - 1]);
                    _files = fs.Select(f=>Path.GetFileName(f)).ToArray();
                }
                return _files;
            }
        }



        protected void OnChangeSearchContent(object sender, EventArgs e)
        {
            PageNavigation.GetCurrentLink().SetParm(QueryParmCvm.search, this.SearchContentTxtBox.Text).Redirect();
        }

        /// <summary>
        /// Finds the first log file containing the given search text and return the actual
        /// line and the following 40 lines. The private field 'matchedFile' is set with the
        /// name of the file containing the error.
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        private string SearchErrorFile(string searchText)
        {
            String match = null;
            currentFile = null;
            foreach (var f in FileNames)
            {
                String filePath = logPath + "/" + f;
                using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var reader = new StreamReader(fs);
                    while (!reader.EndOfStream)
                    {
                        String line = reader.ReadLine();
                        if (line.Contains(searchText))
                        {
                            currentFile = f;
                            match = GetPendingLines(line, reader);
                            break;
                        }
                    }
                    reader.Close();
                    if (match != null) break;
                }
            }
            return "...\n"+match+"...\n";
        }

        private string GetPendingLines(string line, StreamReader reader)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(line);
            for (int i=0;i<NumberOfLines;i++)
            {
                if (!reader.EndOfStream)
                {
                    sb.AppendLine(reader.ReadLine());
                }
            }
            return sb.ToString();
        }
    }
}