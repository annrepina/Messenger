using AutoMapper;
using Common.Dto.Requests;
using ConsoleMessengerServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConsoleMessengerServer.DataBaseServices
{
    /// <summary>
    /// Сервис для работы с базой данных
    /// </summary>
    public class DbService
    {
        /// <summary>
        /// Маппер для мапинга DTO
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public DbService(IMapper mapper)
        {
            _mapper = mapper;
        }

        #region Методы взаимодействия с базой данных

        /// <summary>
        /// Добавить нового пользователя в базу данных, 
        /// вернуть пользователя, при успешном добавлении, null - при неудаче
        /// </summary>
        /// <param name="registrationDto">DTO, который содержит данные о регистрации</param>
        /// <returns>Entity пользователя</returns>
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

        /// <summary>
        /// Прочитать сообщения - поменять статус IsRead у сообщений на true
        /// </summary>
        /// <param name="readMessagesRequest">Запрос на прочтение сообщений</param>
        public void ReadMessages(ExtendedReadMessagesRequestDto readMessagesRequest)
        {
            using (var dbContext = new MessengerDbContext())
            {
                var messagesAreReadId = readMessagesRequest.MessagesId;

                List<Message> messagesList = dbContext.Dialogs.Include(d => d.Messages).First(d => d.Id == readMessagesRequest.DialogId).Messages.ToList();

                foreach (int id in messagesAreReadId)
                {
                    messagesList.First(mes => mes.Id == id).IsRead = true;
                }

                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Найти пользователя по номеру телефона
        /// </summary>
        /// <param name="phoneNumber">Номер телефона</param>
        /// <returns>Найденный пользователь при удачном поиске и null - при неудачном</returns>
        public User? FindUserByPhoneNumber(string phoneNumber)
        {
            using (var dbContext = new MessengerDbContext())
            {
                return dbContext.Users.FirstOrDefault(user => user.PhoneNumber == phoneNumber);
            }
        }

        /// <summary>
        /// Найти пользователя по номеру телефона - перегрузка метода
        /// </summary>
        /// <param name="phoneNumber">Номер телефона</param>
        /// <returns>Найденный пользователь при удачном поиске и null - при неудачном</returns>
        public User? FindUserByPhoneNumber(string phoneNumber, MessengerDbContext dbContext)
        {
            return dbContext.Users.FirstOrDefault(user => user.PhoneNumber == phoneNumber);
        }

        /// <summary>
        /// Поиск пользователя в базе данных с использованием информации о запросе на вход в мессенджер
        /// </summary>
        /// <param name="signInRequestDto">Dto -  запрос о входе в мессенджер</param>
        /// <returns>Найденный пользователь при удачном поиске и null - при неудачном</returns>
        public User? FindUser(SignInRequestDto signInRequestDto)
        {
            using (var dbContext = new MessengerDbContext())
            {
                return dbContext.Users.Include(us => us.Dialogs).FirstOrDefault(user => user.PhoneNumber == signInRequestDto.PhoneNumber && user.Password == signInRequestDto.Password);
            }
        }

        /// <summary>
        /// Найти все диалоги определенного пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Список диалогов</returns>
        public List<Dialog> FindDialogsByUser(User user)
        {
            using (var dbContext = new MessengerDbContext())
            {
                return dbContext.Dialogs.Include(d => d.Users).Include(d => d.Messages).ThenInclude(m => m.UserSender).Where(d => d.Users.Contains(user)).ToList();
            }
        }

        /// <summary>
        /// Найти пользователей удовлетворяющих поиску, при неудаче возвращает пустой список
        /// </summary>
        /// <param name="searchRequestDto">Dto поискового запроса</param>
        /// <returns>Список пользователей удовлетворяюзщих поисковому запросу</returns>
        public List<User> FindListOfUsers(SearchRequestDto searchRequestDto)
        {
            using (var dbContext = new MessengerDbContext())
            {
                return dbContext.Users.Where(u => u.PhoneNumber == searchRequestDto.PhoneNumber || (searchRequestDto.Name != "" && u.Name.ToLower().Contains(searchRequestDto.Name.ToLower()))).ToList();
            }
        }

        /// <summary>
        /// Создает диалог в базе данных
        /// </summary>
        /// <param name="dto">Dto - запрос на создание диалога</param>
        /// <returns>Созданный в базе данных диалог</returns>
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

        /// <summary>
        /// Найти диалог в базе данных
        /// </summary>
        /// <param name="deleteDialogRequestDto">Dto - запрос на удаление диалога</param>
        /// <returns>Найденный диалог, если он существует в базе данных, иначе null</returns>
        public Dialog? FindDialog(ExtendedDeleteDialogRequestDto deleteDialogRequestDto)
        {
            using (var dbContext = new MessengerDbContext())
            {
                return dbContext.Dialogs.Include(d => d.Users).FirstOrDefault(dial => dial.Id == deleteDialogRequestDto.DialogId);
            }
        }

        /// <summary>
        /// Создает сообщение и помещает его в базу данных
        /// </summary>
        /// <param name="sendMessageRequestDto">Dto - запрос на отправку сообщения</param>
        /// <returns>Созданное сообщение</returns>
        public Message AddMessage(SendMessageRequestDto sendMessageRequestDto)
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
        /// <returns>Id пользователя</returns>
        public int GetRecipientUserId(Message message)
        {
            using (var dbContext = new MessengerDbContext())
            {
                return dbContext.Messages.Include(m => m.Dialog).ThenInclude(d => d.Users).First(mes => mes.Id == message.Id).Dialog.Users.First(user => user.Id != message.UserSenderId).Id;
            }
        }

        /// <summary>
        /// Найти сообщение по Id
        /// </summary>
        /// <param name="deleteMessageRequestDto">Dto запрос на удаление сообщения</param>
        /// <returns>В случае удачного поиска возвращает сообщение, иначе - null</returns>
        public Message? FindMessage(ExtendedDeleteMessageRequestDto deleteMessageRequestDto)
        {
            using (var dbContext = new MessengerDbContext())
            {
                return dbContext.Messages.Include(mes => mes.Dialog).Include(mes => mes.UserSender).FirstOrDefault(mes => mes.Id == deleteMessageRequestDto.MessageId);
            }
        }

        /// <summary>
        /// Удаляет сообщение из базы данных
        /// </summary>
        /// <param name="message">Сообщение</param>
        public void DeleteMessage(Message message)
        {
            using (var dbContext = new MessengerDbContext())
            {
                var mes = dbContext.Messages.Remove(message);

                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Удаляет диалог из базы данных
        /// </summary>
        /// <param name="dialog">Диалог</param>
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
        /// <returns>Id собеседника</returns>
        public int GetInterlocutorId(int dialogId, int userId)
        {
            using (var dbContext = new MessengerDbContext())
            {
                return dbContext.Dialogs.Include(d => d.Users).First(d => d.Id == dialogId).Users.First(user => user.Id != userId).Id;
            }
        }

        #endregion Методы взаимодействия с базой данных
    }
}