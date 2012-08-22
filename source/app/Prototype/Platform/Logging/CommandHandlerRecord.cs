﻿using System;
using MongoDB.Bson;
using Prototype.Platform.Mongo;

namespace Prototype.Platform.Logging
{
    public class CommandHandlerRecord
    {
        /// <summary>
        /// Handler unique id
        /// </summary>
        public string HandlerId { get; set; }

        /// <summary>
        /// Command unique id
        /// </summary>
        public string CommandId { get; set; }

        /// <summary>
        /// Date of start of handling
        /// </summary>
        public DateTime StartedDate { get; set; }

        /// <summary>
        /// Date of end of handling
        /// </summary>
        public DateTime EndedDate { get; set; }

        /// <summary>
        /// CLR full type name of command handler
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Error Message (if exists, "" otherwise)
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Error stack trace (if exists, "" otherwise)
        /// </summary>
        public string ErrorStackTrace { get; set; }

        /// <summary>
        /// To Bson
        /// </summary>
        public static BsonDocument ToBson(CommandHandlerRecord record)
        {
            return new BsonDocument()
            {
                { "HandlerId", record.HandlerId ?? "" },
                { "CommandId", record.CommandId ?? "" },
                { "StartedDate", record.StartedDate },
                { "EndedDate", record.EndedDate },
                { "TypeName", record.TypeName ?? "" },
                { "ErrorMessage", record.ErrorMessage ?? "" },
                { "ErrorStackTrace", record.ErrorStackTrace ?? ""},
            };
        }

        public static CommandHandlerRecord FromBson(BsonDocument doc)
        {
            var handler = new CommandHandlerRecord()
            {
                HandlerId = doc.GetString("HandlerId"),
                CommandId = doc.GetString("CommandId"),
                StartedDate = doc.GetDateTime("StartedDate"),
                EndedDate = doc.GetDateTime("EndedDate"),
                ErrorMessage = doc.GetString("ErrorMessage"),
                ErrorStackTrace = doc.GetString("ErrorStackTrace"),
                TypeName = doc.GetString("TypeName"),
            };

            return handler;
        }
    }
}
