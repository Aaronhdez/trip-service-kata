using System;
using System.Collections.Generic;
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
        private ITripDAO tripDao;

        [SetUp]
        public void SetUp() {
            userSession = Substitute.For<IUserSession>();
            tripDao = Substitute.For<ITripDAO>();
            tripService = new TripService(userSession, tripDao);
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
        
        [Test]
        public void ReturnEmptyListIfLoggedUserIsAFriend()
        {
            defaultUser.AddFriend(defaultUser);
            userSession.GetLoggedUser().Returns(defaultUser);
            tripDao.FindTripsByUser(defaultUser).Returns(new List<Trip>());

            var result = tripService.GetTripsByUser(defaultUser);

            result.Should().BeEmpty();
        }
        
        
        [Test]
        public void ACharacterizationTest()
        {
            defaultUser.AddFriend(defaultUser);
            userSession.GetLoggedUser().Returns(defaultUser);
            var trip = new Trip();
            tripDao.FindTripsByUser(defaultUser).Returns(new List<Trip>(new[] { trip }));

            var result = tripService.GetTripsByUser(defaultUser);

            result.Should().ContainEquivalentOf(trip);
        }
    }
}