using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TripServiceKata.Entity;
using TripServiceKata.Exception;
using TripServiceKata.Service;

namespace TripServiceKata.Tests {
    public class TripServiceShould {
        [Test]
        public void FailIfUserIsNotLoggedIn() {
            var userSession = Substitute.For<IUserSession>();
            var tripService = new TripService(userSession);
            userSession.GetLoggedUser().Returns((User) null);
            var user = new User();

            Action result = () => tripService.GetTripsByUser(user);

            result.Should().Throw<UserNotLoggedInException>();
        }  
        
        [Test]
        public void ReturnEmptyListIfLoggedUserIsNotCurrentUser() {
            var userSession = Substitute.For<IUserSession>();
            var tripService = new TripService(userSession);
            var returnedUser = new User();
            userSession.GetLoggedUser().Returns(returnedUser);
            var user = new User();

            var result = tripService.GetTripsByUser(user);

            result.Should().BeEmpty();
        }
        
        [Test]
        public void ReturnEmptyListIfLoggedUserIsNotAFriend() {
            var user = new User();
            var userSession = Substitute.For<IUserSession>();
            userSession.GetLoggedUser().Returns(user);
            var tripService = new TripService(userSession);
            var friend = new User();
            user.AddFriend(friend);

            var result = tripService.GetTripsByUser(user);

            result.Should().BeEmpty();
        }
    }
}