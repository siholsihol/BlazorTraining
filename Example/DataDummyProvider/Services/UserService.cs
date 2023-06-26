using Bogus;
using DataDummyProvider.DTOs;
using System;
using System.Collections.Generic;

namespace DataDummyProvider.Services
{
    public static class UserService
    {
        private static List<UserDTO> _users = new List<UserDTO>();

        public static List<UserDTO> GetUsers()
        {
            if (_users.Count != 0)
                return _users;

            var loFake = new Faker<UserDTO>()
                .CustomInstantiator(x => new UserDTO())
                .RuleFor(x => x.FirstName, x => x.Name.FirstName())
                .RuleFor(x => x.GenderId, x => x.PickRandom(new[] { "F", "M" }))
                .RuleFor(x => x.Id, x => Guid.NewGuid().ToString());

            _users = loFake.Generate(30);

            return _users;
        }
    }
}
