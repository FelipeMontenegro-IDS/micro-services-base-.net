﻿using Application.Interfaces.Azure.ServicesBus;
using Shared.Enums.Queue.Services;

namespace Persistence.Wrappers.azure.ServicesBus;

public class ResponseQueue<TQueue> : IBaseQueue<MicroService,TQueue> where TQueue : Enum
{
    public Enum Microservice { get; set; }
    public TQueue Queue { get; set; }
    
    public ResponseQueue(Enum microservice, TQueue queue)
    {
        Microservice = microservice;
        Queue = queue;
    }


    public bool IsValidQueue(MicroService microservice, Enum queue)
    {
        throw new NotImplementedException();
    }
}