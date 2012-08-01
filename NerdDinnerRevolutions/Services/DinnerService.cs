using System;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using Nancy;
using NerdDinnerRevolutions.Data;
using Newtonsoft.Json;

namespace NerdDinnerRevolutions.Services
{
    public class DinnerService : NancyModule
    {
        NerdDinners nerdDinners = new NerdDinners();

        public DinnerService()
            : base("/services")
        {
            Get["/dinners"] = x => GetDinners();
            Post["/dinner"] = x => CreateDinner();
        }

        private Response CreateDinner()
        {
            var dinner = JsonConvert.DeserializeObject<Dinner>(Request.Form.model);
            nerdDinners.Dinners.Add(dinner);
            try
            {
                nerdDinners.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var message = ex.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage;
                var response = new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Contents = stream =>
                                   {
                                       using (var writer = new StreamWriter(stream) { AutoFlush = true })
                                       {
                                           writer.WriteLine(message);
                                       }
                                   }
                };
                return response;
            }

            return new Response();
        }

        private Response GetDinners()
        {
            var dinners = (from d in nerdDinners.Dinners
                           where d.EventDate > DateTime.Now
                           select new
                                      {
                                          Address = d.Address,
                                          EventDate = d.EventDate,
                                          HostedBy = d.HostedBy,
                                          Title = d.Title
                                      }).ToArray();

            return Response.AsJson(dinners);
        }
    }
}