using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CharlieBackend.TelegramBotApi.Models
{
    public class MessageHeader : IEquatable<MessageHeader>
    {
        public long ChatId { get; set; }
        public int MessageId { get; set; }

        public bool Equals([AllowNull] MessageHeader other)
        {
            if(ChatId == other?.ChatId
                && MessageId == other?.MessageId)
            {
                return true;
            }
            return false;
        }
    }
}
