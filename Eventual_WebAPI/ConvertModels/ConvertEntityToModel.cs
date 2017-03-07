using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eventual.DAL;
using Eventual.Model;

namespace Eventual_WebAPI.ConvertModels
{
    //converts all entities to models objects for json serialization
    public static class ConvertEntityToModel
    {

        //converts event to event model
        public static Eventual.Model.Event EventEntityToEventModel(Eventual.DAL.Event e)
        {
            Eventual.Model.Event result = new Eventual.Model.Event
            {
                EventID = e.EventID,
                EventStartTime = e.EventStartTime,
                EventEndTime = e.EventEndTime,
                EventTitle = e.EventTitle,
                EventPrice = e.EventPrice,
                EventDescription = e.EventDescription,
                LocationID = e.LocationID,
                EventImageURL = e.EventImageURL,
                Location = LocationEntityToLocationModel(e.Location),
                EventRegistrations = EventRegistrationsEntityToEventRegistrationsModel(e.EventRegistrations),
                SavedEvents = SavedEventsEntityToSavedEventsModel(e.SavedEvents),
                EventTypes = EventTypesEntityToEventTypesModel(e.EventTypes)
            };

            return result;
        }


        //converts country to country model
        public static Eventual.Model.Country CountryEntityToCountryModel(Eventual.DAL.Country country)
        {
            Eventual.Model.Country result = new Eventual.Model.Country
            {
                CountryAbbreviation = country.CountryAbbreviation,
                CountryID = country.CountryID,
                CountryLongName = country.CountryLongName
            }; 

            return result;
        }

        //converts EventRegistration to EventRegistration model
        public static ICollection<Eventual.Model.EventRegistration> EventRegistrationsEntityToEventRegistrationsModel(ICollection<Eventual.DAL.EventRegistration>
            eventRegistration)
        {
            ICollection<Eventual.Model.EventRegistration> result = new List<Eventual.Model.EventRegistration>();

            foreach (Eventual.DAL.EventRegistration er in eventRegistration)
            {
                result.Add(EventRegistrationEntityToEventRegistrationModel(er));
            }

            return result;
        }

        //converts EventRegistration to EventRegistration Model
        public static Eventual.Model.EventRegistration EventRegistrationEntityToEventRegistrationModel(Eventual.DAL.EventRegistration eventRegistration)
        {
            Eventual.Model.EventRegistration result = new Eventual.Model.EventRegistration
            {
                UserID = eventRegistration.UserID,
                EventID = eventRegistration.EventID,
                EventRegistrationDate = eventRegistration.EventRegistrationDate,
                Event = EventEntityToEventModel(eventRegistration.Event),
                User = UserEntityToUserModel(eventRegistration.User)
            };

            return result;
        }

        //converts SavedEvents to SavedEvents Model
        public static Eventual.Model.SavedEvent SavedEventEntityToSavedEventModel(Eventual.DAL.SavedEvent savedEvent)
        {
            Eventual.Model.SavedEvent result = new Eventual.Model.SavedEvent
            {
                UserID = savedEvent.UserID,
                EventID = savedEvent.EventID,
                DateOfSavingEvent = savedEvent.DateOfSavingEvent,
                Event = EventEntityToEventModel(savedEvent.Event),
                User = UserEntityToUserModel(savedEvent.User)
            };

            return result;
        }

        //converts EventType to EventType Model
        public static Eventual.Model.EventType EventTypeEntityToEventTypeModel(Eventual.DAL.EventType eventType)
        {
            Eventual.Model.EventType result = new Eventual.Model.EventType
            {
                EventTypeID = eventType.EventTypeID,
                EventTypeName = eventType.EventType1
            };

            return result;
        }


        //converts EventType to EventType Model
        public static ICollection<Eventual.Model.EventType> EventTypesEntityToEventTypesModel(ICollection<Eventual.DAL.EventType> eventType)
        {
            ICollection<Eventual.Model.EventType> result = new List<Eventual.Model.EventType>();

            foreach (Eventual.DAL.EventType et in eventType)
            {
                result.Add(EventTypeEntityToEventTypeModel(et));
            }

            return result;
        }

        //converts Location to Location Model
        public static Eventual.Model.Location EventTypeEntityToEventTypeModel(Eventual.DAL.Location location)
        {
            Eventual.Model.Location result = new Eventual.Model.Location
            {
                LocationID           = location.LocationID,
                LocationStreet1      = location.LocationStreet1,
                LocationStreet2      = location.LocationStreet2,
                LocationCity         = location.LocationCity,
                LocationZipcode      = location.LocationZipcode,
                LocationBuildingName = location.LocationBuildingName,
                StateID              = location.StateID,
                CountryID            = location.CountryID,
                State                = StateEntityToStateModel(location.State),
                Country              = CountryEntityToCountryModel(location.Country)                
            };

            return result;
        }

        //converts SavedEvent to SavedEvent Model
        public static ICollection<Eventual.Model.SavedEvent> SavedEventsEntityToSavedEventsModel(ICollection<Eventual.DAL.SavedEvent> savedEvents)
        {
            ICollection<Eventual.Model.SavedEvent> result = new List<Eventual.Model.SavedEvent>();

            foreach (Eventual.DAL.SavedEvent item in savedEvents )
            {
                result.Add(SavedEventEntityToSavedEventModel(item));
            }

            return result;
        }

        //converts State to State Model
        public static Eventual.Model.State StateEntityToStateModel(Eventual.DAL.State state)
        {
            Eventual.Model.State result = new Eventual.Model.State
            {
                StateAbbreviation = state.StateAbbreviation,
                StateID = state.StateID,
                StateLongName = state.StateLongName
            };

            return result;
        }

        //converts User to User Model
        public static Eventual.Model.User UserEntityToUserModel(Eventual.DAL.User user)
        {
            Eventual.Model.User result = new Eventual.Model.User
            {
                UserID = user.UserID,
                UserBirthDate = user.UserBirthDate,
                UserStartDate = user.UserStartDate,
                UserEmail = user.UserEmail,
                UserEndDate = user.UserEndDate,
                UserFirstName = user.UserFirstName,
                UserLastName = user.UserLastName,
                UserHashedPassword = user.UserHashedPassword,
                UserImageURL = user.UserImageURL,
                UserPhoneNumber = user.UserPhoneNumber,
                UserRoleID = user.UserRoleID,
                UserRole = UserRoleEntityToUserRoleModel(user.UserRole),
                EventRegistrations = EventRegistrationsEntityToEventRegistrationsModel(user.EventRegistrations),
                SavedEvents = SavedEventsEntityToSavedEventsModel(user.SavedEvents)
            };

            return result;
        }


        //converts User to UserRole Model
        public static Eventual.Model.UserRole UserRoleEntityToUserRoleModel(Eventual.DAL.UserRole userRole)
        {
            Eventual.Model.UserRole result = new Eventual.Model.UserRole
            {
                UserRoleID = userRole.UserRoleID,
                UserRoleType = userRole.UserRoleType
            };

            return result;
        }


        public static Eventual.Model.Location LocationEntityToLocationModel(Eventual.DAL.Location location)
        {
            Eventual.Model.Location result = new Eventual.Model.Location
            {
                LocationID = location.LocationID,
                LocationBuildingName = location.LocationBuildingName,
                LocationCity = location.LocationCity,
                LocationStreet1 = location.LocationStreet1,
                LocationStreet2 = location.LocationStreet2,
                LocationZipcode = location.LocationZipcode,
                CountryID = location.CountryID,
                StateID = location.StateID,
                Country = CountryEntityToCountryModel(location.Country),
                State = StateEntityToStateModel(location.State),
            };

            return result;
        }
    }
}