using System;
using System.Collections.Generic;
using System.Text;

namespace NHWorkflow.Services.Events
{
    public interface IEventPublisher
    {
        void Publish<T>(T eventMessage);
    }
}
