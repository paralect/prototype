﻿using System;
using Prototype.Databases;
using Prototype.Domain.Aggregates.Patient.Events;
using Prototype.Platform.Dispatching;
using Prototype.Platform.Dispatching.Attributes;
using Prototype.Views;
using Uniform;

namespace Prototype.Handlers.ViewHandlers
{
    [Priority(PriorityStages.ViewHandling)]
    public class PatientViewHandler : IMessageHandler
    {
        private readonly IDocumentCollection<PatientView> _patients;

        public PatientViewHandler(ViewDatabase database)
        {
            _patients = database.Patients;
        }

        public void Handle(PatientCreated e)
        {
            _patients.Save(patient =>
            {
                patient.PatientId = e.Id;
                patient.DateOfBirth = e.DateOfBirth;
                patient.Level = e.Level;
                patient.SiteId = e.SiteId;
                patient.UpdateName(e.Name, e.Initials);
            });
        }

        public void Handle(PatientUpdated e)
        {
            _patients.Update(e.Id, patient =>
            {
                patient.PatientId = e.Id;
                patient.DateOfBirth = e.DateOfBirth;
                patient.Level = e.Level;
                patient.SiteId = e.SiteId;
                patient.UpdateName(e.Name, e.Initials);
            });
        }

        public void Handle(PatientDeleted e)
        {
            _patients.Delete(e.Id);
        }
    }
}