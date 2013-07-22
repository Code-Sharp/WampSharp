﻿using System;

namespace WampSharp.Rpc.Client
{
    public class WampRpcCall<TMessage>
    {
        public string CallId { get; set; }
        public string ProcUri { get; set; }

        public TMessage[] Arguments { get; set; }

        public Type ReturnType { get; set; }
    }
}