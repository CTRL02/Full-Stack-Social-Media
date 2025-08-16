using socialmedia.DTOs;
using socialmedia.Entities;
using System.Collections.Generic;

namespace socialmedia.Repositories.MessageService
{
    public interface IMessageService
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<IEnumerable<MessageDto>> GetMessagesThread(string currentUserName, string recipientUserName);
        Task<IEnumerable<MessageDto>> GetMessagesForUser(paramsForMessage messageParams);
        Task<bool> SaveAllAsync();
    }


}
