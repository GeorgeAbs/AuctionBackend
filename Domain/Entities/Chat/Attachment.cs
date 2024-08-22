using static Domain.CoreEnums.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Chat
{
    public class Attachment : EntityBase
    {
        public string FilePath { get; set; }

        public AttachmentTypes AttachmentType { get; set; }

        public Attachment(string filePath, AttachmentTypes attachmentType)
        {
            FilePath = filePath;
            AttachmentType = attachmentType;
        }
    }
}
