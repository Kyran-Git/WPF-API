﻿using API.DTO.Users;
using API.Models;
using System.Runtime.CompilerServices;

namespace API.Mappers
{
    public static class UserMapper
    {
        public static UserDTO ToUserDTO(this Users userModel)
        {
            return new UserDTO
            {
                Id = userModel.Id,
                UserName = userModel.UserName,
                Password = userModel.Password,
                Role = userModel.Role,
            };
        }

        public static Users ToUserFromCreate(this CreateUserDTO userDTO)
        {
            return new Users
            {
                UserName = userDTO.UserName,
                Password = userDTO.Password,
                Role = userDTO.Role,
            };
        }

        public static Users ToUserFromUpdate(this UpdateUserDTO userDTO)
        {
            return new Users
            {
                UserName = userDTO.UserName,
                Password = userDTO.Password,
                Role = userDTO.Role,
            };
        }
    }
}
