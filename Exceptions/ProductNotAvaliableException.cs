using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkConnect.Exceptions
{
    public class ProductNotAvaliableException : Exception
    {
        const string DefaultMsg = "Not enough products in store.";

        public ProductNotAvaliableException(String message) 
        :base(message)
        {
            
        }

        public ProductNotAvaliableException() : base(DefaultMsg)
        {
            
        }
    }
}