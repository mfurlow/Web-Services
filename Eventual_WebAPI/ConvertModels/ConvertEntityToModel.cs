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

        //public static class ConvertEntityToModel
        //{
        //    public static DM.MenuItem MenuToModel(DAL.MenuItem entity)
        //    {
        //        DM.MenuItem result = new DM.MenuItem(entity.Name, entity.Price, entity.SizeType.Size);
        //        return result;
        //    }
        //}

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
                EventRegistrations = EventRegistrationsEntityToEventRegistrationsModel(event1.EventRegistrations),
                SavedEvents = SavedEventsEntityToSavedEventsModel(event1.SavedEvents),
                EventTypes = EventTypesEntityToEventTypesModel(event1.EventTypes)
            };

            return null;
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
        public static Eventual.Model.SavedEvent SavedEventEntityToSavedEventModel(Eventual.DAL.SavedEvent SavedEvent)
        {
            return null;
        }

        //converts EventType to EventType Model
        public static Eventual.Model.EventType EventTypeEntityToEventTypeModel(Eventual.DAL.EventType EventType)
        {
            return null;
        }


        //converts EventType to EventType Model
        public static ICollection<Eventual.Model.EventType> EventTypesEntityToEventTypesModel(ICollection<Eventual.DAL.EventType> eventType)
        {
            return null;
        }

        //converts Location to Location Model
        public static Eventual.Model.Location EventTypeEntityToEventTypeModel(Eventual.DAL.Location location)
        {
            return null;
        }

        //converts SavedEvent to SavedEvent Model
        public static ICollection<Eventual.Model.SavedEvent> SavedEventsEntityToSavedEventsModel(ICollection<Eventual.DAL.SavedEvent> savedEvent)
        {
            return null;
        }

        //converts State to State Model
        public static Eventual.Model.State StateEntityToStateModel(Eventual.DAL.State state)
        {
            return null;
        }

        //converts User to User Model
        public static Eventual.Model.User UserEntityToUserModel(Eventual.DAL.User user)
        {
            return null;
        }


        //converts User to UserRole Model
        public static Eventual.Model.UserRole UserRoleEntityToUserRoleModel(Eventual.DAL.UserRole userRole)
        {
            return null;
        }


        public static Eventual.Model.Location LocationEntityToLocationModel(Eventual.DAL.Location location)
        {
            return null;
        }
    }
}