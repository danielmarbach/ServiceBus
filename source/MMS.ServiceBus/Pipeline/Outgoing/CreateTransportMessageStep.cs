﻿//-------------------------------------------------------------------------------
// <copyright file="CreateTransportMessageStep.cs" company="MMS AG">
//   Copyright (c) MMS AG, 2008-2015
// </copyright>
//-------------------------------------------------------------------------------

namespace MMS.ServiceBus.Pipeline.Outgoing
{
    using System;
    using System.Threading.Tasks;

    public class CreateTransportMessageStep : IOutgoingLogicalStep
    {
        public Task Invoke(OutgoingLogicalContext context, Func<Task> next)
        {
            DeliveryOptions options = context.Options;

            var toSend = new TransportMessage { MessageIntent = MessageIntent.Publish };
            var sendOptions = options as SendOptions;

            if (sendOptions != null)
            {
                toSend.MessageIntent = sendOptions is ReplyOptions ? MessageIntent.Reply : MessageIntent.Send;
                toSend.ReplyTo = sendOptions.ReplyToAddress;

                if (sendOptions.CorrelationId != null)
                {
                    toSend.CorrelationId = sendOptions.CorrelationId;
                }
            }

            foreach (var headerEntry in context.LogicalMessage.Headers)
            {
                toSend.Headers[headerEntry.Key] = headerEntry.Value;
            }

            context.Set(toSend);

            return next();
        }
    }
}