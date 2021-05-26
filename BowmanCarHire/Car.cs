using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowmanCarHire
{
    class Car
    {
        public string vehicleRegNo { get; set; }
        public string make { get; set; }
        public string engineSize { get; set; }
        public DateTime dateRegistered { get; set; }
        public double rentalCost { get; set; }
        public int available { get; set; }

        public Car()
        {
        }

        public Car(string vehicleRegNo, string make, string engineSize, DateTime dateRegistered, double rentalCost, int available)
        {
            this.vehicleRegNo = vehicleRegNo;
            this.make = make;
            this.engineSize = engineSize;
            this.dateRegistered = dateRegistered;
            this.rentalCost = rentalCost;
            this.available = available;
        }

        internal string AvailableToString(int available)
        {
            string availableString;
            if (available == 1)
                availableString = "yes";
            else
                availableString = "No";
            return availableString;
        }
        public override string ToString()
        {
            string desc = this.vehicleRegNo.PadRight(11);
            desc += this.make.PadRight(12);
            desc += this.engineSize.PadRight(11);
            desc += $"{this.dateRegistered:yyyy-MM-dd}".PadRight(14);
            desc += $"€{this.rentalCost:n2}".PadRight(10);
            if (this.available == 1)
            {
                desc += "Yes \r\n".PadLeft(10);
            }
            else
            {
                desc += "No \r\n".PadLeft(10);
            }
            
            return desc;
        }

    }


}
