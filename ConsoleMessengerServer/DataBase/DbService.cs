using AutoMapper;
using ConsoleMessengerServer.Entities;
using ConsoleMessengerServer.Entities.Mapping;
using ConsoleMessengerServer.Net;
using ConsoleMessengerServer.Requests;
using ConsoleMessengerServer.Responses;
using DtoLib;
using DtoLib.Dto;
using DtoLib.Dto.Requests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public User? AddNewUser(RegistrationDto registrationDto)
        {
            User? user = null;

            using (var dbContext = new MessengerDbContext())
            {
                // ищем есть ли аккаунт с таким номером уже в бд
                var res = dbContext.Users.FirstOrDefault(user => user.PhoneNumber == registrationDto.PhoneNumber);

                // если вернули null значит аккаунта под таким номером еще нет
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
        /// Пробует найти пользователей удовлетворяющих поиску, при удаче возвращает пустой список
        /// </summary>
        /// <param name="searchRequestDto">Dto поискового запроса</param>
        /// <returns></returns>
        public List<User> SearchUsers(UserSearchRequestDto searchRequestDto)
        {
            List<User> users = new List<User>();

            using (var dbContext = new MessengerDbContext())
            {
                users = dbContext.Users.Where(u => u.PhoneNumber == searchRequestDto.PhoneNumber || (searchRequestDto.Name != "" && u.Name.ToLower().Contains(searchRequestDto.Name.ToLower()))).ToList();
            }

            return users;
        }

        public Dialog CreateDialog(CreateDialogRequestDto dto)
        {
            CreateDialogResponse createDialogResponse;

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

        /// <summary>
        /// Создает сообщение и помещает его в базу данных
        /// </summary>
        /// <param name="sendMessageRequestDto">Dto для запроса на отправку сообщения</param>
        /// <returns></returns>
        public Message AddMessage(MessageRequest request)
        {
            Message message;

            try
            {
                using (var dbContext = new MessengerDbContext())
                {
                    message = request.Message;

                    var user = dbContext.Users.Include(user => user.Dialogs).First(user => user.Id == message.UserSender.Id);
                    var dialog = dbContext.Dialogs.Include(dial => dial.Users).First(dialog => dialog.Id == request.DialogId);

                    message.UserSender = user;
                    message.Dialog = dialog;

                    dbContext.Messages.Add(message);
                    dbContext.SaveChanges();

                    return message;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
        /// Удаляет сообщение из базы данных
        /// </summary>
        /// <param name="messageRequest">Запрос, содержащий в себе сообщение</param>
        public void DeleteMessage(Message message)
        {
            //Message message;

            try
            {
                using (var dbContext = new MessengerDbContext())
                {
                    //message = messageRequest.Message;

                    var a = dbContext.Messages.Remove(message);

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
