using AutoMapper;
using ConsoleMessengerServer.Entities;
using ConsoleMessengerServer.Entities.Mapping;
using ConsoleMessengerServer.Net;
using ConsoleMessengerServer.Requests;
using ConsoleMessengerServer.Responses;
using DtoLib;
using DtoLib.Dto.Requests;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.DataBase
{
    /// <summary>
    /// Сервис для работы с базой данных
    /// </summary>
    public class DbService
    {
        /// <summary>
        /// Маппер для мапинга ентити на DTO и обратно
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public DbService()
        {
            DataBaseMapper mapper = DataBaseMapper.GetInstance();
            _mapper = mapper.CreateIMapper();
        }

        /// <summary>
        /// Добавить нового пользователя в базу данных, 
        /// вернуть пользователя, при успешном добавлении, ноль - при неудаче
        /// </summary>
        /// <param name="registrationDto">DTO, который содержит данные о регистрации</param>
        /// <returns></returns>
        public User? AddNewUser(SignUpRequestDto registrationDto)
        {
            User? user = null;

            using (var dbContext = new MessengerDbContext())
            {
                // ищем есть ли аккаунт с таким номером уже в бд
                //var res = dbContext.Users.FirstOrDefault(user => user.PhoneNumber == registrationDto.PhoneNumber);

                var res = FindUserByPhoneNumber(registrationDto.PhoneNumber, dbContext);

                // если вернули false значит аккаунта под таким номером еще нет
                if (res == null)
                {
                    user = _mapper.Map<User>(registrationDto);
                    dbContext.Users.Add(user);

                    try
                    {
                        dbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }

                }//if                    

            }//using

            return user;
        }

        /// <summary>
        /// ПРобует найти пользователя по номеру телефона
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public User? FindUserByPhoneNumber(string phoneNumber)
        {
            User? user = null;

            using(var dbContext = new MessengerDbContext())
            {
                user = dbContext.Users.FirstOrDefault(user => user.PhoneNumber == phoneNumber);

                return user;
            }
        }

        /// <summary>
        /// ПРобует найти пользователя по номеру телефона
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public User? FindUserByPhoneNumber(string phoneNumber, MessengerDbContext dbContext)
        {
            User? user = null;

            user = dbContext.Users.FirstOrDefault(user => user.PhoneNumber == phoneNumber);

            return user;
        }

        /// <summary>
        /// Поиск пользователя в базе данных с использованием информации о запросе на вход в мессенджер
        /// </summary>
        /// <param name="signInRequestDto">Dto на запрос о входе в мессенджер</param>
        /// <returns></returns>
        public User? FindUser(SignInRequestDto signInRequestDto)
        {
            User? user = null;

            using (var dbContext = new MessengerDbContext())
            {
                user = dbContext.Users.Include(us => us.Dialogs).FirstOrDefault(user => user.PhoneNumber == signInRequestDto.PhoneNumber && user.Password == signInRequestDto.Password);              
            }

            return user;
        }

        public List<Dialog> FindDialogsByUser(User user)
        {
            List<Dialog> dialogs = new List<Dialog>();

            using(var dbContext = new MessengerDbContext())
            {
                dialogs = dbContext.Dialogs.Include(d => d.Users).Include(d => d.Messages).ThenInclude(m => m.UserSender).Where(d => d.Users.Contains(user)).ToList();
            }

            return dialogs;
        }

        /// <summary>
        /// Пробует найти пользователей удовлетворяющих поиску, при удаче возвращает пустой список
        /// </summary>
        /// <param name="searchRequestDto">Dto поискового запроса</param>
        /// <returns></returns>
        public List<User>? FindListOfUsers(UserSearchRequestDto searchRequestDto)
        {
            List<User>? res = null;

            using (var dbContext = new MessengerDbContext())
            {
                List<User> users = new List<User>(); 

                users = dbContext.Users.Where(u => u.PhoneNumber == searchRequestDto.PhoneNumber || (searchRequestDto.Name != "" && u.Name.ToLower().Contains(searchRequestDto.Name.ToLower()))).ToList();

                if(users.Count > 0)
                    res = users;
            }

            return res;
        }

        public Dialog CreateDialog(CreateDialogRequestDto dto)
        {   
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public Dialog? FindDialog(DeleteDialogRequestDto deleteDialogRequestDto)
        {
            try
            {
                using (var dbContext = new MessengerDbContext())
                {
                    return dbContext.Dialogs.Include(d => d.Users).FirstOrDefault(dial => dial.Id == deleteDialogRequestDto.DialogId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Создает сообщение и помещает его в базу данных
        /// </summary>
        /// <param name="request">запрос на отправку сообщения</param>
        /// <returns></returns>
        public Message AddMessage(/*SendMessageRequest request*/SendMessageRequestDto sendMessageRequestDto)
        {
            try
            {
                using (var dbContext = new MessengerDbContext())
                {
                    Message message = _mapper.Map<Message>(sendMessageRequestDto.Message);

                    var user = dbContext.Users.Include(user => user.Dialogs).First(user => user.Id == message.UserSender.Id);
                    var dialog = dbContext.Dialogs.Include(dial => dial.Users).First(dialog => dialog.Id == sendMessageRequestDto.DialogId);

                    message.UserSender = user;
                    message.Dialog = dialog;

                    //
                    //message = new Message() { Text = "dfsfdsf" };

                    dbContext.Messages.Add(message);
                    dbContext.SaveChanges();

                    return message;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Исключение: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Возвращает идентификатор пользователя, которому отправлено сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <returns></returns>
        public int GetRecipientUserId(Message message)
        {
            try
            {
                int userId;

                using (var dbContext = new MessengerDbContext())
                {
                    userId = dbContext.Messages.Include(m => m.Dialog).ThenInclude(d => d.Users).First(mes => mes.Id == message.Id).Dialog.Users.First(user => user.Id != message.UserSenderId).Id;

                    return userId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Попробовать найти сообщение по Id
        /// </summary>
        /// <param name="messageId">Id сообщения</param>
        /// <returns></returns>
        public Message? FindMessage(DeleteMessageRequestDto deleteMessageRequestDto)
        {
            try
            {
                using(var dbContext = new MessengerDbContext())
                {
                    return dbContext.Messages.Include(mes => mes.Dialog).Include(mes => mes.UserSender).FirstOrDefault(mes => mes.Id == deleteMessageRequestDto.MessageId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }          
        }

        /// <summary>
        /// Удаляет сообщение из базы данных
        /// </summary>
        /// <param name="messageRequest">Запрос, содержащий в себе сообщение</param>
        public void DeleteMessage(Message message)
        {
            //Message message1 = new Message() { Id = 5 };

            try
            {
                using (var dbContext = new MessengerDbContext())
                {
                    var mes = dbContext.Messages.Remove(message);

                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void DeleteDialog(Dialog dialog)
        {
            try
            {
                using(var dbContext = new MessengerDbContext())
                {
                    var dial = dbContext.Dialogs.Remove(dialog);

                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
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
            int interlocutorId;

            try
            {
                using(var dbContext = new MessengerDbContext())
                {
                    interlocutorId = dbContext.Dialogs.Include(d => d.Users).First(d => d.Id == dialogId).Users.First(user => user.Id != userId).Id;

                    return interlocutorId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


    }
}
