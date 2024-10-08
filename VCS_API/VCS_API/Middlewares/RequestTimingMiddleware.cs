﻿using System.Diagnostics;
using VCS_API.DirectoryDB.Repositories;

namespace VCS_API.Middlewares
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestTimingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                await _next(context);  // Call the next middleware in the pipeline
            }
            finally
            {
                stopwatch.Stop();
                var elapsedTime = stopwatch.ElapsedMilliseconds;
                Console.WriteLine($"Request [{context.Request.Method}] {context.Request.Path} took {elapsedTime} ms [operation ended at {DateTime.Now}].");
                AuditLogsRepo.LogStats(context.Request.Path, context.Request.Method, elapsedTime);
            }
        }
    }
}
