﻿//-------------------------------------------------------------------------------
// <copyright file="SendOptions.cs" company="MMS AG">
//   Copyright (c) MMS AG, 2008-2015
// </copyright>
//-------------------------------------------------------------------------------

namespace MMS.ServiceBus
{
    public class SendOptions : DeliveryOptions
    {
        public Queue Queue { get; set; }

        public string CorrelationId { get; set; }
    }
}