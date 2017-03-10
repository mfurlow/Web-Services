using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eventual.DAL;
using Eventual.Model;
using System.Data.Objects;
using System.Security.Cryptography;
using System.Text;

namespace Eventual_WebAPI.ConvertModels
{
    //converts all entities to models objects for json serialization
    public static class ConvertEntityToModel
    {

        //converts event to event model
        public static Eventual.Model.Event EventEntityToEventModel(Eventual.DAL.Event event1)
        {
            Eventual.Model.Event result = new Eventual.Model.Event
            {
                EventID = event1.EventID,
                EventStartTime = event1.EventStartTime,
                EventEndTime = event1.EventEndTime,
                EventTitle = event1.EventTitle,
                EventPrice = event1.EventPrice,
                EventDescription = event1.EventDescription,
                LocationID = event1.LocationID,
                EventImageURL = event1.EventImageURL,
                Location = LocationEntityToLocationModel(event1.Location),
                //EventRegistrations = EventRegistrationsEntityToEventRegistrationsModel(event1.EventRegistrations),
                //SavedEvents = SavedEventsEntityToSavedEventsModel(event1.SavedEvents),
                EventTypes = EventTypesEntityToEventTypesModel(event1.EventTypes)
            };

            return result;
        }

        //public static List<Eventual.Model.SearchResult> (List<Eventual.DAL.spSearchEvents>)


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

            foreach (Eventual.DAL.EventRegistration item in eventRegistration)
            {
                result.Add(EventRegistrationEntityToEventRegistrationModel(item));
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
           
            };

            return result;
        }

        //converts SavedEvents to SavedEvents Model
        public static Eventual.Model.SavedEvent SavedEventEntityToSavedEventModel(Eventual.DAL.SavedEvent savedEvent)
        {
            Eventual.Model.SavedEvent result = new Eventual.Model.SavedEvent
            {
                EventID = savedEvent.EventID,
                UserID  = savedEvent.UserID,
                User    = UserEntityToUserModel(savedEvent.User),
                Event   = EventEntityToEventModel(savedEvent.Event)
            };

            return result;
        }

        //converts EventType to EventType Model
        public static Eventual.Model.EventType EventTypeEntityToEventTypeModel(Eventual.DAL.EventType eventType)
        {
            Eventual.Model.EventType result = new Eventual.Model.EventType
            {
                EventTypeID   = eventType.EventTypeID, 
                EventTypeName = eventType.EventType1
            };

            return result;
        }


        //converts EventType to EventType Model
        public static ICollection<Eventual.Model.EventType> EventTypesEntityToEventTypesModel(ICollection<Eventual.DAL.EventType> eventType)
        {
            ICollection < Eventual.Model.EventType > result = new List<Eventual.Model.EventType>();

            foreach (var item in eventType)
            {
                result.Add(EventTypeEntityToEventTypeModel(item));

            }
        
            return result;
        }

        //converts Location to Location Model
        public static Eventual.Model.Location LocationEntityToLocationModel(Eventual.DAL.Location location)
        {
            Eventual.Model.Location result = new Eventual.Model.Location
            {
                LocationID = location.LocationID,
                LocationBuildingName = location.LocationBuildingName,
                LocationStreet1 = location.LocationStreet1,
                LocationStreet2 = location.LocationStreet2,
                LocationCity    = location.LocationCity,
                LocationZipcode = location.LocationZipcode,
                StateID         = location.StateID,
                CountryID       = location.CountryID,
                State           = StateEntityToStateModel(location.State),               
                Country         = CountryEntityToCountryModel(location.Country) 
            };

            return result;
        }

        //converts SavedEvent to SavedEvent Model
        public static ICollection<Eventual.Model.SavedEvent> SavedEventsEntityToSavedEventsModel(ICollection<Eventual.DAL.SavedEvent> savedEvent)
        {
            List<Eventual.Model.SavedEvent> result = new List<Eventual.Model.SavedEvent>();
            
            foreach(var item in savedEvent)
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
                StateID = state.StateID,
                StateAbbreviation = state.StateAbbreviation,
                StateLongName = state.StateLongName
            };

            return result;
        }

        //converts User to User Model
        public static Eventual.Model.User UserEntityToUserModel(Eventual.DAL.User user)
        {
            Eventual.Model.User result = new Eventual.Model.User
            {
                UserBirthDate = user.UserBirthDate,
                UserEmail = user.UserEmail,
                UserEndDate = user.UserEndDate,
                UserFirstName = user.UserFirstName,
                UserStartDate = user.UserStartDate,
                UserHashedPassword = user.UserHashedPassword,
                UserID = user.UserID,
                UserImageURL = user.UserImageURL,
                UserLastName = user.UserLastName,
                UserPhoneNumber = user.UserPhoneNumber,
                UserRole = UserRoleEntityToUserRoleModel(user.UserRole),
                UserRoleID = user.UserRoleID
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

        public static List<Eventual.Model.EventAPI> EventAPIEntityToEventAPIModel(List<spGetAllSavedEventsForSpecificUser_Result> savedEvents)
        {
            List<Eventual.Model.EventAPI> result = new List<Eventual.Model.EventAPI>();

            foreach (var item in savedEvents)
            {
                result.Add(EventAPIEntityToEventAPIModel(item));
            }
            return result;
        }

        public static List<Eventual.Model.EventAPI> EventAPIEntityToEventAPIModel(List<spGetAllCurrentRegisteredEventsForSpecificUser_Result> currentEvents)
        {
            List<Eventual.Model.EventAPI> result = new List<Eventual.Model.EventAPI>();

            foreach (var item in currentEvents)
            {
                result.Add(EventAPIEntityToEventAPIModel(item));
            }

            return result;
        }

        public static List<Eventual.Model.EventAPI> EventAPIEntityToEventAPIModel(List<spGetAllPastRegisteredEventsForSpecificUser_Result> pastEvents)
        {
            List<Eventual.Model.EventAPI> result = new List<Eventual.Model.EventAPI>();

            foreach (var item in pastEvents)
            {
                result.Add(EventAPIEntityToEventAPIModel(item));
            }

            return result;
        }
        //converts Event to spGetAllSavedEventsForSpecificUser_Result
        public static Eventual.Model.EventAPI EventAPIEntityToEventAPIModel(spGetAllSavedEventsForSpecificUser_Result saved)
        {
            EventAPI eventAPI = null;
            if (saved != null)
            {
                eventAPI = new EventAPI
                {
                    EventID = saved.EventID,
                    EventEndTime = saved.EventEndTime,
                    EventStartTime = saved.EventStartTime,
                    EventImageURL = saved.EventImageURL,
                    EventPrice = saved.EventPrice,
                    EventTitle = saved.EventTitle,
                    LocationCity = saved.LocationCity,
                    LocationStreet1 = saved.LocationStreet1,
                    StateAbbreviation = saved.StateAbbreviation,
                    UserID = saved.UserID
                };

            }

            return eventAPI;
        }


        //converts Event to spGetAllCurrentRegisteredEventsForSpecificUser_Result
        public static Eventual.Model.EventAPI EventAPIEntityToEventAPIModel(spGetAllCurrentRegisteredEventsForSpecificUser_Result current)
        {
            EventAPI eventAPI = null;

            if (current != null)
            {
                eventAPI = new EventAPI
                {
                    EventID = current.EventID,
                    EventEndTime = current.EventEndTime,
                    EventStartTime = current.EventStartTime,
                    EventImageURL = current.EventImageURL,
                    EventPrice = current.EventPrice,
                    EventTitle = current.EventTitle,
                    LocationCity = current.LocationCity,
                    LocationStreet1 = current.LocationStreet1,
                    StateAbbreviation = current.StateAbbreviation,
                    UserID = current.UserID
                };

            }

            return eventAPI;
        }


        //converts Event to spGetAllCurrentRegisteredEventsForSpecificUser_Result
        public static Eventual.Model.EventAPI EventAPIEntityToEventAPIModel(spGetAllPastRegisteredEventsForSpecificUser_Result past)
        {
            EventAPI eventAPI = null;

            if (past != null)
            {
                eventAPI = new EventAPI
                {
                    EventID = past.EventID,
                    EventEndTime = past.EventEndTime,
                    EventStartTime = past.EventStartTime,
                    EventImageURL = past.EventImageURL,
                    EventPrice = past.EventPrice,
                    EventTitle = past.EventTitle,
                    LocationCity = past.LocationCity,
                    LocationStreet1 = past.LocationStreet1,
                    StateAbbreviation = past.StateAbbreviation,
                    UserID = past.UserID
                };
            }

            return eventAPI;
        }
    }
}