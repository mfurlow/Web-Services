using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eventual.DAL;
using Eventual.Model;

namespace Eventual_WebAPI.ConvertModels
{

    public static class ConvertModelToEntity
    {
        //coverts all models to entities
        public static Eventual.DAL.Event EventModelToEventEntity(Eventual.Model.Event e)
        {
            Eventual.DAL.Event result = new Eventual.DAL.Event
            {
                EventID = e.EventID,
                EventStartTime = e.EventStartTime,
                EventEndTime = e.EventEndTime,
                EventTitle = e.EventTitle,
                EventPrice = e.EventPrice,
                EventDescription = e.EventDescription,
                LocationID = e.LocationID,
                EventImageURL = e.EventImageURL,
                Location = LocationModelToLocationEntity(e.Location),
                EventRegistrations = EventRegistrationsModelToEventRegistrationsEntity(e.EventRegistrations),
                SavedEvents = SavedEventsModelToSavedEventsEntity(e.SavedEvents),
                EventTypes = EventTypesModelToEventTypesEntity(e.EventTypes)
            };

            return result;
        }


        //converts country to country model
        public static Eventual.DAL.Country CountryModelToCountryEntity(Eventual.Model.Country country)
        {
            Eventual.DAL.Country result = new Eventual.DAL.Country
            {
                CountryAbbreviation = country.CountryAbbreviation,
                CountryID = country.CountryID,
                CountryLongName = country.CountryLongName
            };

            return result;
        }

        //converts EventRegistration to EventRegistration model
        public static ICollection<Eventual.DAL.EventRegistration> EventRegistrationsModelToEventRegistrationsEntity(ICollection<Eventual.Model.EventRegistration>
            eventRegistration)
        {
            ICollection<Eventual.DAL.EventRegistration> result = new List<Eventual.DAL.EventRegistration>();

            foreach (Eventual.Model.EventRegistration er in eventRegistration)
            {
                result.Add(EventRegistrationModelToEventRegistrationEntity(er));
            }

            return result;
        }

        //converts EventRegistration to EventRegistration Model
        public static Eventual.DAL.EventRegistration EventRegistrationModelToEventRegistrationEntity(Eventual.Model.EventRegistration eventRegistration)
        {
            Eventual.DAL.EventRegistration result = new Eventual.DAL.EventRegistration
            {
                UserID = eventRegistration.UserID,
                EventID = eventRegistration.EventID,
                EventRegistrationDate = eventRegistration.EventRegistrationDate,
                Event = EventModelToEventEntity(eventRegistration.Event),
                User = UserModelToUserEntity(eventRegistration.User)
            };

            return result;
        }

        //converts SavedEvents to SavedEvents Model
        public static Eventual.DAL.SavedEvent SavedEventModelToSavedEventEntity(Eventual.Model.SavedEvent savedEvent)
        {
            Eventual.DAL.SavedEvent result = new Eventual.DAL.SavedEvent
            {
                UserID = savedEvent.UserID,
                EventID = savedEvent.EventID,
                DateOfSavingEvent = savedEvent.DateOfSavingEvent,
                Event = EventModelToEventEntity(savedEvent.Event),
                User = UserModelToUserEntity(savedEvent.User)
            };

            return result;
        }

        //converts EventType to EventType Model
        public static Eventual.DAL.EventType EventTypeModelToEventTypeEntity(Eventual.Model.EventType eventType)
        {
            Eventual.DAL.EventType result = new Eventual.DAL.EventType
            {
                EventTypeID = eventType.EventTypeID,
                EventType1 = eventType.EventTypeName
            };

            return result;
        }


        //converts EventType to EventType Model
        public static ICollection<Eventual.DAL.EventType> EventTypesModelToEventTypesEntity(ICollection<Eventual.Model.EventType> eventType)
        {
            ICollection<Eventual.DAL.EventType> result = new List<Eventual.DAL.EventType>();

            foreach (Eventual.Model.EventType et in eventType)
            {
                result.Add(EventTypeModelToEventTypeEntity(et));
            }

            return result;
        }

        //converts Location to Location Model
        public static Eventual.DAL.Location EventTypeModelToEventTypeEntity(Eventual.Model.Location location)
        {
            Eventual.DAL.Location result = new Eventual.DAL.Location
            {
                LocationID = location.LocationID,
                LocationStreet1 = location.LocationStreet1,
                LocationStreet2 = location.LocationStreet2,
                LocationCity = location.LocationCity,
                LocationZipcode = location.LocationZipcode,
                LocationBuildingName = location.LocationBuildingName,
                StateID = location.StateID,
                CountryID = location.CountryID,
                State = StateModelToStateEntity(location.State),
                Country = CountryModelToCountryEntity(location.Country)
            };

            return result;
        }

        //converts SavedEvent to SavedEvent Model
        public static ICollection<Eventual.DAL.SavedEvent> SavedEventsModelToSavedEventsEntity(ICollection<Eventual.Model.SavedEvent> savedEvents)
        {
            ICollection<Eventual.DAL.SavedEvent> result = new List<Eventual.DAL.SavedEvent>();

            foreach (Eventual.Model.SavedEvent item in savedEvents)
            {
                result.Add(SavedEventModelToSavedEventEntity(item));
            }

            return result;
        }

        //converts State to State Model
        public static Eventual.DAL.State StateModelToStateEntity(Eventual.Model.State state)
        {
            Eventual.DAL.State result = new Eventual.DAL.State
            {
                StateAbbreviation = state.StateAbbreviation,
                StateID = state.StateID,
                StateLongName = state.StateLongName
            };

            return result;
        }

        //converts User to User Model
        public static Eventual.DAL.User UserModelToUserEntity(Eventual.Model.User user)
        {
            Eventual.DAL.User result = new Eventual.DAL.User
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
                UserRole = UserRoleModelToUserRoleEntity(user.UserRole),
                EventRegistrations = EventRegistrationsModelToEventRegistrationsEntity(user.EventRegistrations),
                SavedEvents = SavedEventsModelToSavedEventsEntity(user.SavedEvents)
            };

            return result;
        }


        //converts User to UserRole Model
        public static Eventual.DAL.UserRole UserRoleModelToUserRoleEntity(Eventual.Model.UserRole userRole)
        {
            Eventual.DAL.UserRole result = null;
            if (userRole != null)
            {

                result = new Eventual.DAL.UserRole
                {
                    UserRoleID = userRole.UserRoleID,
                    UserRoleType = userRole.UserRoleType
                };
            }
            
            return result;
        }


        public static Eventual.DAL.Location LocationModelToLocationEntity(Eventual.Model.Location location)
        {
            Eventual.DAL.Location result = new Eventual.DAL.Location
            {
                LocationID = location.LocationID,
                LocationBuildingName = location.LocationBuildingName,
                LocationCity = location.LocationCity,
                LocationStreet1 = location.LocationStreet1,
                LocationStreet2 = location.LocationStreet2,
                LocationZipcode = location.LocationZipcode,
                CountryID = location.CountryID,
                StateID = location.StateID,
                Country = CountryModelToCountryEntity(location.Country),
                State = StateModelToStateEntity(location.State),
            };

            return result;
        }

    }
}

