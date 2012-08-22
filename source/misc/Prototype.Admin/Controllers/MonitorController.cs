using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Prototype.Databases;
using Prototype.Platform.Logging;

namespace Prototype.Admin.Controllers
{
    public class MonitorController : Controller
    {
        [Dependency]
        public MongoLogsDatabase MongoLogs { get; set; }

        [Dependency]
        public LogManager LogManager { get; set; }

        public ActionResult Index(String mode)
        {
            MongoCursor<BsonDocument> cursor = null;

            switch (mode)
            {
                case "errors":
                {
                    var query = Query.GT("Errors", 0);
                    cursor = MongoLogs.Logs.FindAs<BsonDocument>(query);
                    break;
                }

                default:
                    cursor = MongoLogs.Logs.FindAllAs<BsonDocument>();
                    break;
            }


            cursor.SetLimit(100);
            cursor.SetSortOrder(SortBy.Descending("Command.Metadata.CreatedDate"));

            var result = LogManager.BsonToRecords(cursor.ToList());

            ViewData["mode"] = mode ?? "";
            return View("Index", result);
        }
    }

    public static class TimeSpanEntenstion
    {
        public static string ToReadableString(this TimeSpan span)
        {
            string formatted = string.Format("{0}{1}{2}{3}",
                span.Days > 0 ? string.Format("{0:0} days ", span.Days) : string.Empty,
                span.Hours > 0 ? string.Format("{0:0} hours ", span.Hours) : string.Empty,
                span.Minutes > 0 ? string.Format("{0:0} minutes ", span.Minutes) : string.Empty,
                span.Seconds > 0 ? string.Format("{0:0} seconds ", span.Seconds) : string.Empty);

            return formatted + "ago";
        }
    }
}