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
        private IUserSession _userSession;
        private User _defaultUser;
        private TripService _tripService;
        private ITripDAO _tripDao;

        [SetUp]
        public void SetUp() {
            _userSession = Substitute.For<IUserSession>();
            _tripDao = Substitute.For<ITripDAO>();
            _defaultUser = new User();
            _userSession.GetLoggedUser().Returns(_defaultUser);
            _tripService = new TripService(_userSession, _tripDao);
        }
        
        [Test]
        public void RetrieveEmptyListIfUserIsNotAFriend() {
            var anotherUser = new User();

            var tripList = _tripService.GetTripsByUser(anotherUser);

            tripList.Should().BeEmpty();
        }
        
        [Test]
        public void RetrieveNonEmptyListIfUserIsAFriend() {
            var anotherUser = new User();
            anotherUser.AddFriend(_defaultUser);
            _tripDao.FindTripsByUser(anotherUser).Returns(new List<Trip>(new[] { new Trip() }));

            var tripList = _tripService.GetTripsByUser(anotherUser);

            tripList.Should().NotBeEmpty();
        }

        [Test]
        public void ThrowExceptionIfUserIsNotLogged() {
            _userSession.GetLoggedUser().Returns((User) null);

            Action result = () => _tripService.GetTripsByUser(_defaultUser);

            result.Should().Throw<UserNotLoggedInException>();
        }
    }
}