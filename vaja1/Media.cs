using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vaja1
{
    public class Media
    {
        private string s;
        public String author
        {
            get { return s; }
            set { s = value; }
        }

        private string t;
        public String title
        {
            get { return t; }
            set { t = value; }
        }

        private string d;
        public String description
        {
            get { return d; }
            set { d = value; }
        }

        private string p;
        public String path
        {
            get { return p; }
            set { p = value; }
        }

        private string ip;
        public String imgPath
        {
            get { return ip; }
            set { ip = value; }
        }

        private string g;
        public String genre
        {
            get { return g; }
            set { g = value; }
        }

        public Media()
        {
        }

        public override string ToString()
        {
            return author + "  " + title + "  " + description + "  " + path + "  " + imgPath + "  " + genre + "\n";
        }
    }
}
