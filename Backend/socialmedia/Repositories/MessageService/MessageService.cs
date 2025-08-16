using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using socialmedia.Data;
using socialmedia.DTOs;
using socialmedia.Entities;

namespace socialmedia.Repositories.MessageService
{
    public class MessageService : IMessageService
    {

private readonly IMapper _mapper;
        private readonly DBContext _context;
        public MessageService(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesThread(string currentUserName, string recipientUserName)
        {
          var messages = await _context.Messages.Include(u => u.Sender).Include(u => u.Recipient).Where(m => m.Recipient.UserName == currentUserName &&
          m.Sender.UserName == recipientUserName || 
          m.Sender.UserName == currentUserName 
          && m.Recipient.UserName == recipientUserName)
                .OrderBy(m => m.SentAt)
                .ToListAsync();

            var unreadMessages = messages.Where(m => !m.IsRead && m.RecipientUsername == currentUserName).ToList();
            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.IsRead = true;
                    message.ReadAt = DateTime.UtcNow;
                }
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesForUser(paramsForMessage paramsF)
        {
           var query = _context.Messages
                .OrderBy(m => m.SentAt)
                .AsQueryable();

            query = paramsF.container switch
            {
                "Inbox" => query = query.Where(m => m.Recipient.UserName == paramsF.username),
                "Outbox" => query = query.Where(m => m.Sender.UserName == paramsF.username),
                _ => query = query.Where(m => m.Recipient.UserName == paramsF.username && !m.IsRead)
            };

            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

            return await messages.ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
