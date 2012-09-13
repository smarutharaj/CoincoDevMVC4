using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Coinco.SMS.Website.Models
{
    public enum TransactionType : int
    {
            [Description("Item")]
            Item = 1,
            [Description("Hour")]
            Hour = 2,
            [Description("Expense")]
            Expense = 3,
            [Description("Fee")]
            Fee = 4
    }

    
      
}