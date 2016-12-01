using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAPBot
{

    // A wrapper for a DateTime struct so it can be shared across multiple classes consistently

    class DateTimeWrapper
    {
        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public DateTimeWrapper()
        {
            date = DateTime.Today;
        }

        public DateTimeWrapper(DateTime date)
        {
            this.date = date;
        }

    }
}
