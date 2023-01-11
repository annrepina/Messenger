using AutoMapper;
using ConsoleMessengerServer.Entities;
using ConsoleMessengerServer.Entities.Mapping;
using ConsoleMessengerServer.Net;
using DtoLib;
using DtoLib.Dto;
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
        /// Попытаться добавить нового пользователя в базу данных, 
        /// вернуть id пользователя, при успешном добавлении, ноль - при неудаче
        /// </summary>
        /// <param name="registrationDto">DTO, который содержит данные о регистрации</param>
        /// <returns></returns>
        public User? TryAddNewUser(RegistrationDto registrationDto)
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
        /// Пробует найти пользователей удовлетворяющих поиску, при удаче возвращает список, при неудаче null
        /// </summary>
        /// <param name="searchRequestDto">Dto поискового запроса</param>
        /// <returns></returns>
        public List<User> TrySearchUsers(UserSearchRequestDto searchRequestDto)
        {
            List<User> users = new List<User>();

            using (var dbContext = new MessengerDbContext())
            {
                users = dbContext.Users.Where(u => u.PhoneNumber == searchRequestDto.PhoneNumber || (searchRequestDto.Name != "" && u.Name.ToLower().Contains(searchRequestDto.Name.ToLower()))).ToList();
            }

            return users;
        }
    }
}
