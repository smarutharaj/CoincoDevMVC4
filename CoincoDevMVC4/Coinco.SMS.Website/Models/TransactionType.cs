using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Coinco.SMS.Website.Models
{
    public class TransactionType
    {
        public int TransactionTypeID { get; set; }
        public string TransactionTypeName{ get; set; }

        public static TransactionType Item
        {
            get
            {
                return new TransactionType { TransactionTypeID = 3, TransactionTypeName = "Item" };
            }
        }

        public static TransactionType Fee
        {
            get
            {
                return new TransactionType { TransactionTypeID = 2, TransactionTypeName = "Fee" };
            }
        }
        public static TransactionType Hour
        {
            get
            {
                return new TransactionType { TransactionTypeID = 1, TransactionTypeName = "Hour" };
            }
        }
        public static TransactionType Expense
        {
            get
            {
                return new TransactionType { TransactionTypeID = 4, TransactionTypeName = "Expense" };
            }
        }

        public static List<TransactionType> GetTransactionTypes()
        {
            List<TransactionType> transactionTypes = new List<TransactionType> { };
            transactionTypes.Add(TransactionType.Item);
            transactionTypes.Add(TransactionType.Hour);
            transactionTypes.Add(TransactionType.Fee);
            transactionTypes.Add(TransactionType.Expense);

            return transactionTypes;
        }
    }



     
}
      
