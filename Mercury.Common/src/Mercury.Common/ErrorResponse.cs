using System.Collections.Generic;

namespace Mercury.Common
{
    public class ErrorResponse
    {
        public ErrorResponse() { }
        public ErrorResponse(int statusCode, IEntity entity)
        {
            Status = statusCode;
            Errors = entity.Errors;
        }
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string TraceId { get; set; }
        public IDictionary<string, object[]> Errors { get; set; }
    }
}