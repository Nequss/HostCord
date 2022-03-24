using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostCord.Models
{
    public class ComboBoxAction
    {
        private int _id;
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _text;
        public string text
        {
            get { return _text; }
            set { _text = value; }
        }
        public ComboBoxAction(int id, string text)
        {
            this.id = id;
            this.text = text;
        }
    }
}
