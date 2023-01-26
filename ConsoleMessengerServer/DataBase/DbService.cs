using AutoMapper;
using ConsoleMessengerServer.Entities;
using ConsoleMessengerServer.Entities.Mapping;
using DtoLib.Dto.Requests;
using Microsoft.EntityFrameworkCore;

namespace ConsoleMessengerServer.DataBase
{
    /// <summary>
    /// Сервис для работы с базой данных
    /// </summary>
    public class DbService
    {
        /// <summary>
        /// Маппер для мапинга Entity на DTO и обратно
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public DbService(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Добавить нового пользователя в базу данных, 
        /// вернуть пользователя, при успешном добавлении, null - при неудаче
        /// </summary>
        /// <param name="registrationDto">DTO, который содержит данные о регистрации</param>
        /// <returns></returns>
        public User? AddNewUser(SignUpRequestDto registrationDto)
        {
            using (var dbContext = new MessengerDbContext())
            {
                User? user = FindUserByPhoneNumber(registrationDto.PhoneNumber, dbContext);

                if (user == null)
                {
                    user = _mapper.Map<User>(registrationDto);

                    dbContext.Users.Add(user);

                    dbContext.SaveChanges();

                    return user;

                }//if                    

                return null;

            }//using
        }

        public void ReadMessages(MessagesAreReadRequestDto messagesAreReadRequest)
        {
            using (var dbContext = new MessengerDbContext())
            {
                var messagesAreReadId = messagesAreReadRequest.MessagesId;

                List<Message> messagesList = dbContext.Dialogs.Include(d => d.Messages).First(d => d.Id == messagesAreReadRequest.DialogId).Messages.ToList();

                foreach (int id in messagesAreReadId)
                {
                    messagesList.First(mes => mes.Id == id).IsRead = true;
                }

                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// ПРобует найти пользователя по номеру телефона
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public User? FindUserByPhoneNumber(string phoneNumber)
        {
            using (var dbContext = new MessengerDbContext())
            {
                return dbContext.Users.FirstOrDefault(user => user.PhoneNumber == phoneNumber);
            }
        }

        /// <summary>
        /// ПРобует найти пользователя по номеру телефона
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public User? FindUserByPhoneNumber(string phoneNumber, MessengerDbContext dbContext)
        {
            return dbContext.Users.FirstOrDefault(user => user.PhoneNumber == phoneNumber);
        }

        /// <summary>
        /// Поиск пользователя в базе данных с использованием информации о запросе на вход в мессенджер
        /// </summary>
        /// <param name="signInRequestDto">Dto на запрос о входе в мессенджер</param>
        /// <returns></returns>
        public User? FindUser(SignInRequestDto signInRequestDto)
        {
            using (var dbContext = new MessengerDbContext())
            {
                return dbContext.Users.Include(us => us.Dialogs).FirstOrDefault(user => user.PhoneNumber == signInRequestDto.PhoneNumber && user.Password == signInRequestDto.Password);
            }
        }
        public List<Dialog> FindDialogsByUser(User user)
        {
            using (var dbContext = new MessengerDbContext())
            {
                return dbContext.Dialogs.Include(d => d.Users).Include(d => d.Messages).ThenInclude(m => m.UserSender).Where(d => d.Users.Contains(user)).ToList();
            }
        }

        /// <summary>
        /// Пробует найти пользователей удовлетворяющих поиску, при удаче возвращает пустой список
        /// </summary>
        /// <param name="searchRequestDto">Dto поискового запроса</param>
        /// <returns></returns>
        public List<User> FindListOfUsers(UserSearchRequestDto searchRequestDto)
        {
            using (var dbContext = new MessengerDbContext())
            {
                return dbContext.Users.Where(u => u.PhoneNumber == searchRequestDto.PhoneNumber || (searchRequestDto.Name != "" && u.Name.ToLower().Contains(searchRequestDto.Name.ToLower()))).ToList();
            }
        }
        public Dialog CreateDialog(CreateDialogRequestDto dto)
        {
            using (var dbContext = new MessengerDbContext())
            {
                Dialog dialog = _mapper.Map<Dialog>(dto);
                int senderId = dialog.Messages.First().UserSenderId;

                foreach (var userId in dto.UsersId)
                {
                    User user = dbContext.Users.First(user => user.Id == userId);
                    dialog.Users.Add(user);
                }

                dialog.Messages.First().UserSender = dialog.Users.First(user => user.Id == senderId);

                dbContext.Add(dialog);
                dbContext.SaveChanges();

                return dialog;
            }
        }

        public Dialog? FindDialog(DeleteDialogRequestDto deleteDialogRequestDto)
        {
            using (var dbContext = new MessengerDbContext())
            {
                return dbContext.Dialogs.Include(d => d.Users).FirstOrDefault(dial => dial.Id == deleteDialogRequestDto.DialogId);
            }
        }

        /// <summary>
        /// Создает сообщение и помещает его в базу данных
        /// </summary>
        /// <param name="request">запрос на отправку сообщения</param>
        /// <returns></returns>
        public Message AddMessage(/*SendMessageRequest request*/SendMessageRequestDto sendMessageRequestDto)
        {
            using (var dbContext = new MessengerDbContext())
            {
                Message message = _mapper.Map<Message>(sendMessageRequestDto.Message);

                var user = dbContext.Users.Include(user => user.Dialogs).First(user => user.Id == message.UserSender.Id);
                var dialog = dbContext.Dialogs.Include(dial => dial.Users).First(dialog => dialog.Id == sendMessageRequestDto.DialogId);

                message.UserSender = user;
                message.Dialog = dialog;

                dbContext.Messages.Add(message);
                dbContext.SaveChanges();

                return message;
            }
        }

        /// <summary>
        /// Возвращает идентификатор пользователя, которому отправлено сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <returns></returns>
        public int GetRecipientUserId(Message message)
        {
            using (var dbContext = new MessengerDbContext())
            {
                return dbContext.Messages.Include(m => m.Dialog).ThenInclude(d => d.Users).First(mes => mes.Id == message.Id).Dialog.Users.First(user => user.Id != message.UserSenderId).Id;
            }
        }

        /// <summary>
        /// Попробовать найти сообщение по Id
        /// </summary>
        /// <param name="messageId">Id сообщения</param>
        /// <returns></returns>
        public Message? FindMessage(DeleteMessageRequestDto deleteMessageRequestDto)
        {
            using (var dbContext = new MessengerDbContext())
            {
                return dbContext.Messages.Include(mes => mes.Dialog).Include(mes => mes.UserSender).FirstOrDefault(mes => mes.Id == deleteMessageRequestDto.MessageId);
            }
        }

        /// <summary>
        /// Удаляет сообщение из базы данных
        /// </summary>
        /// <param name="messageRequest">Запрос, содержащий в себе сообщение</param>
        public void DeleteMessage(Message message)
        {
            using (var dbContext = new MessengerDbContext())
            {
                var mes = dbContext.Messages.Remove(message);

                dbContext.SaveChanges();
            }
        }

        public void DeleteDialog(Dialog dialog)
        {
            using (var dbContext = new MessengerDbContext())
            {
                var dial = dbContext.Dialogs.Remove(dialog);

                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Возвращает Id пользователя, который является собеседником определенного пользователя в определенном диалоге
        /// </summary>
        /// <param name="dialogId">Id диалога</param>
        /// <param name="userId">Id пользователя</param>
        /// <returns></returns>
        public int GetInterlocutorId(int dialogId, int userId)
        {
            using (var dbContext = new MessengerDbContext())
            {
                return dbContext.Dialogs.Include(d => d.Users).First(d => d.Id == dialogId).Users.First(user => user.Id != userId).Id;
            }
        }
    }
}