using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace WampSharp.Core
{
    public delegate Task AsyncEventHandler<in TEventArgs>(object sender, TEventArgs e) where TEventArgs : EventArgs;

    public static class AsyncEventHandlerExtensions
    {
        public static async Task InvokeAsync<TEventArgs>(this AsyncEventHandler<TEventArgs> handler, object sender,
                                                         TEventArgs e) where TEventArgs : EventArgs
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            Delegate[] invocationList = handler.GetInvocationList();

            foreach (AsyncEventHandler<TEventArgs> currentSubscriber in invocationList
                .Cast<AsyncEventHandler<TEventArgs>>())
            {
                await currentSubscriber(sender, e).ConfigureAwait(false);
            }
        }

        public static AsyncEventHandler<EventArgs> ConvertToAsync
            (this EventHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            return (sender, args) =>
                   {
                       handler(sender, args);
                       return Task.CompletedTask;
                   };
        }


        public static AsyncEventHandler<TEventArgs> ConvertToAsync<TEventArgs>
            (this EventHandler<TEventArgs> handler) where TEventArgs : EventArgs
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            return (sender, args) =>
                   {
                       handler(sender, args);
                       return Task.CompletedTask;
                   };
        }
    }
}