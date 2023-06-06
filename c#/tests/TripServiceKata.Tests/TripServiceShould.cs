using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TripServiceKata.Entity;
using TripServiceKata.Exception;
using TripServiceKata.Service;

namespace TripServiceKata.Tests {
    public class TripServiceShould {
        private IUserSession userSession;
        private TripService tripService;
        private User defaultUser;

        [SetUp]
        public void SetUp() {
            userSession = Substitute.For<IUserSession>();
            tripService = new TripService(userSession);
            defaultUser = new User();
        }

        [Test]
        public void FailIfUserIsNotLoggedIn() {
            userSession.GetLoggedUser().Returns((User) null);

            Action result = () => tripService.GetTripsByUser(defaultUser);

            result.Should().Throw<UserNotLoggedInException>();
        }  
        
        [Test]
        public void ReturnEmptyListIfLoggedUserIsNotCurrentUser() {
            var anotherUser = new User();
            userSession.GetLoggedUser().Returns(anotherUser);

            var result = tripService.GetTripsByUser(defaultUser);

            result.Should().BeEmpty();
        }
        
        [Test]
        public void ReturnEmptyListIfLoggedUserIsNotAFriend() {
            userSession.GetLoggedUser().Returns(defaultUser);
            var friend = new User();
            defaultUser.AddFriend(friend);

            var result = tripService.GetTripsByUser(defaultUser);

            result.Should().BeEmpty();
        }
    }
}