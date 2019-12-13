﻿namespace TourOfHeroesServices.Tests
{
    using System.Linq;
    using System.Collections.Generic;

    using TourOfHeroesData.Models;
    using TourOfHeroesDTOs.UserDtos;
    using TourOfHeroesData.Common.Contracts;

    using Moq;
    using Xunit;
    using Contracts;

    public class UserServiceTests
    {
        public List<ApplicationUser> GetTestData()
        {
            return new List<ApplicationUser>
            {
                new ApplicationUser {
                    Id = "1",
                    UserName = "Pesho",
                    Email = "test@gmail.com",
                    FullName = "Martin Asenov"
                },
                new ApplicationUser {
                    Id = "2",
                    UserName = "Gosho",
                    Email = "test1@gmail.com",
                    FullName = "Gosho Goshev"
                },
                new ApplicationUser {
                    Id = "3",
                    UserName = "ko4inata",
                    Email = "test2@gmail.com",
                    FullName = "Koce Boce"
                },
            };
        }

        [Fact]
        public void RepositoryShouldCallAllMethodOnce()
        {
            var repo = new Mock<IRepository<ApplicationUser>>();

            repo.Setup(r => r.All())
                .Returns(GetTestData().AsQueryable());

            IUserService service = new UserService(repo.Object, null);

            service.UpdateUser("1", new UpdateUserDTO()
            {
                Role = "Admin"
            });

            repo.Verify(x => x.All(), Times.Once);
        }

        //[Fact]
        //public void UserRepositoryShouldReturnCorrectUserCount()
        //{
        //    var repo = new Mock<IRepository<ApplicationUser>>();

        //    repo.Setup(r => r.All())
        //        .Returns(GetTestData().AsQueryable());

        //    IUserService service = new UserService(repo.Object, null);
        //    //service.UpdateUser("1", new UpdateUserDTO()
        //    //{
        //    //    Role = "Admin"
        //    //});

        //    Assert.Equal(3, service.GetAllUsers().Count());
        //}
    }
}