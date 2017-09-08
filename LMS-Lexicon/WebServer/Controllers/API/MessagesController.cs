using System.Linq;
using System.Web.Http;
using WebServer.Models;
using WebServer.Models.LMS;
using Microsoft.AspNet.Identity;
using WebServer.Repository;
using WebServer.ViewModels;

namespace WebServer.Controllers.API
{
    [Authorize]
    public class MessagesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Messages
        public IQueryable<Message> GetMessages()
        {
            return new MailRepository().Messages(User.Identity.GetUserName());
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
                    Email=m.Reciever.Email
                },
                Sender = new User{
                    Email=m.Reciever.Email
                }
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
        public IHttpActionResult Delete(int id,string email)
        {

            new MailRepository().Delete(id,email);
            return Ok();
        }
        [HttpPut]
        public IHttpActionResult SetMailAsTrash(int id, string email, bool value)
        {
            Message m = new MailRepository().Message(id) as Message;
            if (m == null)
            {
                return BadRequest("a message with id of "+id+" doesn't exist");
            }
            if(m.Sender.Email.ToLower()!=email.ToLower() && m.Reciever.Email.ToLower()!=email.ToLower()){
                return BadRequest("Email not found");
            }
            if(m.Sender.Email.ToLower()==email.ToLower()){
                m.Trash2 = value;
            }
            else if (m.Reciever.Email.ToLower() == email.ToLower()){
                m.Trash = value;
            }
            m.Sender = null;
            m.Reciever = null;
            new MailRepository().Edit(m);
            return Ok();
        }
        [HttpPut]
        public IHttpActionResult SetMailAsFavorite(int id, string email, bool value)
        {
            Message m = new MailRepository().Message(id) as Message;
            if (m == null)
            {
                return BadRequest("a message with id of " + id + " doesn't exist");
            }
            if (m.Sender.Email.ToLower() != email.ToLower() && m.Reciever.Email.ToLower() != email.ToLower())
            {
                return BadRequest("Email not found");
            }
            if (m.Sender.Email.ToLower() == email.ToLower())
            {
                m.Favorite2 = value;
            }
            else if (m.Reciever.Email.ToLower() == email.ToLower())
            {
                m.Favorite = value;
            }
            m.Sender = null;
            m.Reciever = null;
            new MailRepository().Edit(m);
            return Ok();
        }
        [HttpPut]
        public IHttpActionResult SetMailAsImportant(int id, string email, bool value)
        {
            Message m = new MailRepository().Message(id) as Message;
            if (m == null)
            {
                return BadRequest("a message with id of " + id + " doesn't exist");
            }
            if (m.Sender.Email.ToLower() != email.ToLower() && m.Reciever.Email.ToLower() != email.ToLower())
            {
                return BadRequest("Email not found");
            }
            if (m.Sender.Email.ToLower() == email.ToLower())
            {
                m.Important2 = value;
            }
            else if (m.Reciever.Email.ToLower() == email.ToLower())
            {
                m.Important = value;
            }
            m.Sender = null;
            m.Reciever = null;
            new MailRepository().Edit(m);
            return Ok();
        }
    }
}