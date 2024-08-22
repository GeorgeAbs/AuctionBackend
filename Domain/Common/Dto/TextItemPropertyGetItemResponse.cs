using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Dto
{
    public class TextItemPropertyGetItemResponse
    {
        public string Value { get; }

        public string PropertyName { get; }

        public TextItemPropertyGetItemResponse(string value, string propertyName)
        {
            Value = value;
            PropertyName = propertyName;
        }
    }
}
