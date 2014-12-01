//-------------------------------------------------------------------------------
// <copyright file="LoadMessageHandlersStep.cs" company="MMS AG">
//   Copyright (c) MMS AG, 2008-2015
// </copyright>
//-------------------------------------------------------------------------------

namespace MMS.ServiceBus.Pipeline.Incoming
{
    using System;
    using System.Threading.Tasks;

    public class LoadMessageHandlersStep : IIncomingLogicalStep
    {
        private readonly HandlerRegistry registry;

        public LoadMessageHandlersStep(HandlerRegistry registry)
        {
            this.registry = registry;
        }

        public async Task Invoke(IncomingLogicalContext context, IBus bus, Func<Task> next)
        {
            var messageType = context.LogicalMessage.Instance.GetType();

            var handlers = this.registry.GetHandlers(messageType);

            foreach (var handler in handlers)
            {
                using (context.CreateSnapshot())
                {
                    var messageHandler = new MessageHandler
                        {
                            Instance = handler,
                            Invocation =
                                async (handlerInstance, message) =>
                                await this.registry.InvokeHandle(handlerInstance, message, bus)
                        };

                    context.Handler = messageHandler;

                    await next();

                    if (context.HandlerInvocationAborted)
                    {
                        break;
                    } 
                }
            }
        }
    }
}