using System;
using System.Linq;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using Prototype.Admin.Models;
using Prototype.Platform.Domain;
using Prototype.Platform.Domain.Transitions.Interfaces;
using Prototype.Platform.Domain.Utilities;

namespace Abe.Admin.Controllers
{
    public class AggregateController : Controller
    {
        private readonly ITransitionRepository _transitionRepository;

        public AggregateController(ITransitionRepository transitionRepository)
        {
            _transitionRepository = transitionRepository;
        }

        public ActionResult Show(String id)
        {
            if (String.IsNullOrEmpty(id))
                return View(new AggregatePage(id, null, null));

            try
            {
                var transitions = _transitionRepository.GetTransitions(id, 0, int.MaxValue);

                if (transitions.Count == 0)
                    return View(new AggregatePage(id, null, null));
                
                if (String.IsNullOrEmpty(transitions[0].AggregateTypeId))
                    return View(new AggregatePage(id, transitions, null));

                var aggregateType = Type.GetType(transitions[0].AggregateTypeId);

                var aggregate = AggregateCreator.CreateAggregateRoot(aggregateType);
                var state = AggregateCreator.CreateAggregateState(aggregateType);
                StateSpooler.Spool(state, transitions);
                aggregate.Setup(state, transitions.Last().Id.Version);

                var bson = state.ToBsonDocument();
                return View(new AggregatePage(id, transitions, bson));
            }
            catch (Exception)
            {
                return View(new AggregatePage(id, null, null));
            }
        }
    }
}