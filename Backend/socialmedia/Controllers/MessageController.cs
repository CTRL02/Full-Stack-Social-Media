using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using socialmedia.DTOs;
using socialmedia.Entities;
using socialmedia.Repositories.MessageService;
using System.Security.Claims;

namespace socialmedia.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class MessageController:ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService, IUserService userService, IMapper mapper)
        {
            _messageService = messageService;
            _userService = userService;
            _mapper = mapper;
        }
        [HttpPost("sendmessage")]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value!;
            if (userName == createMessageDto.RecipientUsername)
            {
                return BadRequest("You cannot send a message to yourself.");
            }
            var sender = await _userService.GetUserByNameDuplicate(userName);
            var recipient = await _userService.GetUserByNameDuplicate(createMessageDto.RecipientUsername);
            if (sender == null || recipient == null)
                {
                    return NotFound("Sender or recipient not found.");
                }
            
            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content

            };

            _messageService.AddMessage(message);
            if(await _messageService.SaveAllAsync())
                return Ok(_mapper.Map<MessageDto>(message));
        
            return BadRequest("Failed to send message.");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] paramsForMessage paramsForMessage)
        {
            paramsForMessage.username = User.FindFirst(ClaimTypes.Name)?.Value!;
            var messages = await _messageService.GetMessagesForUser(paramsForMessage);


            return Ok(messages);
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
        {
            var currentUsername = User.FindFirst(ClaimTypes.Name)?.Value!;
            return Ok(await _messageService.GetMessagesThread(currentUsername, username));
        }

    }
}
