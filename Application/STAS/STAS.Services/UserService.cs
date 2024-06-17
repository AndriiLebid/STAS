﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STAS.Model;
using STAS.Repo;

namespace STAS.Services
{
    public class UserService
    {

        #region Private Fields

        private readonly UserRepo repo = new();

        #endregion

        public async Task<List<User>> GetUserList() {

            List<User> users = await repo.GetAllUserAsync();

            return users;
        }

        public async Task<User>? GetUserById(int id)
        {

            User? users = await repo.GetUserByIdAsync(id);

            return users;
        }


        public async Task<int> DeleteUserAsync(int id)
        {

            int result = await repo.DeleteUserAsync(id);

            return result;
        }
    }
}
