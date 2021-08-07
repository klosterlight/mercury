using System.Collections.Generic;

namespace Mercury.Reservations.Tests
{
    public class ErrorResponse
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string TraceId { get; set; }
        public IDictionary<string, object[]> Errors{ get; set; }
    }
}