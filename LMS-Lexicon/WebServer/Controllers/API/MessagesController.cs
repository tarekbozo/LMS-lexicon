using System.Linq;
using System.Web.Http;
using WebServer.Models;
using WebServer.Models.LMS;
using Microsoft.AspNet.Identity;
using WebServer.Repository;
using WebServer.ViewModels;
using System.Collections.Generic;

namespace WebServer.Controllers.API
{
    [Authorize]
    public class MessagesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Messages
        public IQueryable<MessageVM> GetMessages()
        {
            List<MessageVM> _messages = new List<MessageVM>();

            foreach (Message m in new MailRepository().Messages(User.Identity.GetUserName()))
            {
                MessageVM message = new MessageVM();
                message.ID = m.ID;
                message.Content = m.MailContent;
                message.Description = m.Description;

                if(m.Reciever.UserName==User.Identity.GetUserName())
                {
                    message.Favorite = m.Favorite;
                    message.Important = m.Important;
                    message.PermantDelete = m.PermantDelete;
                    message.Trash = m.Trash;
                }
                else
                {
                    message.Favorite = m.Favorite2;
                    message.Important = m.Important2;
                    message.PermantDelete = m.PermantDelete2;
                    message.Trash = m.Trash2;
                }

                message.Readed = m.Readed;
                message.Sent = m.Sent;
                message.Sender = new User
                {
                    Id = m.Sender.Id,
                    BirthDate = m.Sender.BirthDate,
                    UserName=m.Sender.UserName,
                    Email=m.Sender.Email,
                    FirstName=m.Sender.FirstName,
                    LastName=m.Sender.LastName,
                    PhoneNumber=m.Sender.PhoneNumber
                };
                message.Reciever = new User
                {
                    Id = m.Reciever.Id,
                    BirthDate = m.Reciever.BirthDate,
                    UserName = m.Reciever.UserName,
                    Email = m.Reciever.Email,
                    FirstName = m.Reciever.FirstName,
                    LastName = m.Reciever.LastName,
                    PhoneNumber = m.Reciever.PhoneNumber
                };
                message.Owned = message.Sender.UserName == User.Identity.GetUserName();
                _messages.Add(message);
            }

            return _messages.AsQueryable();
        }
        public IHttpActionResult GetMessage(int ID)
        {
            Message m = new MailRepository().Message(ID) as Message;
            if(m==null){
                return BadRequest("Couldn't find Message with ID of "+ID);
            }
            return Ok(new Message
            {
                ID = m.ID,
                Sent = m.Sent,
                Readed = m.Readed,
                Favorite = m.Favorite,
                Favorite2 = m.Favorite2,
                Important = m.Important,
                Important2 = m.Important2,
                MailContent = m.MailContent,
                PermantDelete = m.PermantDelete,
                PermantDelete2 = m.PermantDelete2,
                Trash = m.Trash,
                Trash2 = m.Trash2,
                Reciever = new User { 
                    Email=m.Reciever.Email,
                    UserName=m.Reciever.UserName
                },
                Sender = new User{
                    Email=m.Sender.Email,
                    UserName=m.Sender.UserName
                },
                Description=m.Description
            });
        }
        [HttpPost]
        public IHttpActionResult Send(CreateMessageVM message)
        {
            if(message==null){
                return BadRequest();
            }
            if (new UsersRepository().UserByEmail(message.EmailFrom) == null || new UsersRepository().UserByEmail(message.EmailTo) == null){
                return BadRequest("Email Couldn't be found");
            }
            if (message.MessageContent == null || message.MessageContent == "")
            {
                return BadRequest("No Content to send.");
            }
            new MailRepository().Add(new Message { Description=message.Description, MailContent=message.MessageContent, RecieverId=new UsersRepository().UserByEmail(message.EmailTo).Id, SenderId=new UsersRepository().UserByEmail(message.EmailFrom).Id});
            return Ok();
        }
        [HttpDelete]
        public IHttpActionResult Delete(Message message)
        {
            if(message==null)
            {
                return BadRequest();
            }
            string email="";
            if(message.Sender.UserName==User.Identity.GetUserName())
            {
                email = message.Sender.Email;
            }
            else
            {
                email = message.Reciever.Email;
            }
            new MailRepository().Delete(message.ID,email);
            return Ok();
        }
        [HttpPut]
        public IHttpActionResult SetMailAsTrash([FromBody]MessageEditSettingsVM setting)
        {
            if (setting == null || setting.Email == null || setting.Email == "")
            {
                return BadRequest();
            }
            Message mT = new MailRepository().Message(setting.ID) as Message;
            Message m = new Message
            {
                ID = mT.ID,
                RecieverId = mT.RecieverId,
                SenderId = mT.SenderId,
                Sent = mT.Sent,
                Description = mT.Description,
                Favorite = mT.Favorite,
                Favorite2 = mT.Favorite2,
                Important = mT.Important,
                Important2 = mT.Important2,
                MailContent = mT.MailContent,
                PermantDelete = mT.PermantDelete,
                PermantDelete2 = mT.PermantDelete2,
                Readed = mT.Readed,
                Trash = mT.Trash,
                Trash2 = mT.Trash2,
                Reciever = mT.Reciever,
                Sender = mT.Sender
            };
            mT = null;
            if (m == null)
            {
                return BadRequest("a message with id of " + setting.ID + " doesn't exist");
            }
            if (m.Sender.Email.ToLower() != setting.Email.ToLower() && m.Reciever.Email.ToLower() != setting.Email.ToLower())
            {
                return BadRequest("Email not found");
            }
            if (m.Sender.Email.ToLower() == setting.Email.ToLower())
            {
                if (m.Trash2 == true)
                {
                    m.Trash2 = false;
                }
                else
                {
                    m.Trash2 = true;
                }
            }
            else if (m.Reciever.Email.ToLower() == setting.Email.ToLower())
            {
                if (m.Trash == true)
                {
                    m.Trash = false;
                }
                else
                {
                    m.Trash = true;
                }
            }
            m.Sender = null;
            m.Reciever = null;
            new MailRepository().Edit(m);
            return Ok();
        }
        [HttpPut]
        public IHttpActionResult SetMailAsFavorite([FromBody]MessageEditSettingsVM setting)
        {
            if (setting==null || setting.Email==null|| setting.Email=="") {
                return BadRequest();
            }
            Message mT = new MailRepository().Message(setting.ID) as Message;
            Message m = new Message {
                ID =mT.ID,
                RecieverId =mT.RecieverId,
                SenderId =mT.SenderId,
                Sent =mT.Sent,
                Description=mT.Description,
                Favorite =mT.Favorite,
                Favorite2 =mT.Favorite2,
                Important =mT.Important,
                Important2 =mT.Important2,
                MailContent=mT.MailContent,
                PermantDelete=mT.PermantDelete,
                PermantDelete2=mT.PermantDelete2,
                Readed=mT.Readed,
                Trash=mT.Trash,
                Trash2=mT.Trash2,
                Reciever=mT.Reciever,
                Sender=mT.Sender
            };
            mT = null;
            if (m == null)
            {
                return BadRequest("a message with id of " + setting.ID + " doesn't exist");
            }
            if (m.Sender.Email.ToLower() != setting.Email.ToLower() && m.Reciever.Email.ToLower() != setting.Email.ToLower())
            {
                return BadRequest("Email not found");
            }
            if (m.Sender.Email.ToLower() == setting.Email.ToLower())
            {
                if (m.Favorite2 == true) {
                    m.Favorite2 = false;
                }
                else
                {
                    m.Favorite2 = true;
                }
            }
            else if (m.Reciever.Email.ToLower() == setting.Email.ToLower())
            {
                if (m.Favorite == true)
                {
                    m.Favorite = false;
                }
                else
                {
                    m.Favorite = true;
                }
            }
            m.Sender = null;
            m.Reciever = null;
            new MailRepository().Edit(m);
            return Ok();
        }
        [HttpPut]
        public IHttpActionResult SetMailAsImportant([FromBody]MessageEditSettingsVM setting)
        {
            if (setting == null || setting.Email == null || setting.Email == "")
            {
                return BadRequest();
            }
            Message mT = new MailRepository().Message(setting.ID) as Message;
            Message m = new Message
            {
                ID = mT.ID,
                RecieverId = mT.RecieverId,
                SenderId = mT.SenderId,
                Sent = mT.Sent,
                Description = mT.Description,
                Favorite = mT.Favorite,
                Favorite2 = mT.Favorite2,
                Important = mT.Important,
                Important2 = mT.Important2,
                MailContent = mT.MailContent,
                PermantDelete = mT.PermantDelete,
                PermantDelete2 = mT.PermantDelete2,
                Readed = mT.Readed,
                Trash = mT.Trash,
                Trash2 = mT.Trash2,
                Reciever = mT.Reciever,
                Sender = mT.Sender
            };
            mT = null;
            if (m == null)
            {
                return BadRequest("a message with id of " + setting.ID + " doesn't exist");
            }
            if (m.Sender.Email.ToLower() != setting.Email.ToLower() && m.Reciever.Email.ToLower() != setting.Email.ToLower())
            {
                return BadRequest("Email not found");
            }
            if (m.Sender.Email.ToLower() == setting.Email.ToLower())
            {
                if (m.Important2 == true)
                {
                    m.Important2 = false;
                }
                else
                {
                    m.Important2 = true;
                }
            }
            else if (m.Reciever.Email.ToLower() == setting.Email.ToLower())
            {
                if (m.Important == true)
                {
                    m.Important = false;
                }
                else
                {
                    m.Important = true;
                }
            }
            m.Sender = null;
            m.Reciever = null;
            new MailRepository().Edit(m);
            return Ok();
        }
    }
}