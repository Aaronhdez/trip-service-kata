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
        private Trip defautlTrip;

        [SetUp]
        public void SetUp() {
            userSession = Substitute.For<IUserSession>();
            tripDao = Substitute.For<ITripDAO>();
            tripService = new TripService(userSession, tripDao);
            defaultUser = new User();
            defautlTrip = new Trip();
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
        public void ReturnsAListWIthOneTripWhenATripIsAdded()
        {
            defaultUser.AddFriend(defaultUser);
            userSession.GetLoggedUser().Returns(defaultUser);
            tripDao.FindTripsByUser(defaultUser).Returns(new List<Trip>(new[] { defautlTrip }));

            var result = tripService.GetTripsByUser(defaultUser);

            result.Should().ContainEquivalentOf(defautlTrip);
        }

        [Test]
        public void ReturnsAListWithOneTripWhenATripIsAddedToAFriend()
        {
            var friend = new User();
            friend.AddFriend(defaultUser);
            userSession.GetLoggedUser().Returns(defaultUser);
            tripDao.FindTripsByUser(friend).Returns(new List<Trip>(new[] { defautlTrip }));

            var result = tripService.GetTripsByUser(friend);

            result.Should().ContainEquivalentOf(defautlTrip);
        }
    }
}