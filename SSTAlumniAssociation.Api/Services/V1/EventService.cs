using Event.V1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using SSTAlumniAssociation.Api.Authorization;

namespace SSTAlumniAssociation.Api.Services.V1;

[Authorize]
public class EventServiceV1 : EventService.EventServiceBase
{
    public override Task<ListEventsResponse> ListEvents(ListEventsRequest request, ServerCallContext context)
    {
        return base.ListEvents(request, context);
    }

    public override Task<Event.V1.Event> GetEvent(GetEventRequest request, ServerCallContext context)
    {
        return base.GetEvent(request, context);
    }

    public override Task<Event.V1.Event> GetAdmission(GetAdmissionRequest request, ServerCallContext context)
    {
        return base.GetAdmission(request, context);
    }

    [AuthorizeAdmin]
    public override Task<Event.V1.Event> UpdateEvent(UpdateEventRequest request, ServerCallContext context)
    {
        return base.UpdateEvent(request, context);
    }

    [AuthorizeAdmin]
    public override Task<Empty> DeleteEvent(DeleteEventRequest request, ServerCallContext context)
    {
        return base.DeleteEvent(request, context);
    }

    [AuthorizeAdmin]
    public override Task<Event.V1.Event> AddAttendee(AddAttendeeRequest request, ServerCallContext context)
    {
        return base.AddAttendee(request, context);
    }

    [AuthorizeAdmin]
    public override Task<Event.V1.Event> BatchAddAttendees(BatchAddAttendeesRequest request, ServerCallContext context)
    {
        return base.BatchAddAttendees(request, context);
    }
}
