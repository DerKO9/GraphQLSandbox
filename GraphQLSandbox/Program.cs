using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Abstractions.Websocket;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using GraphQL.Types;

namespace GraphQLSandbox
{
   class Program
   {
      static void Main(string[] args)
      {
         var personAndFilmsRequest = new GraphQLRequest
         {
            Query = @"
               query PersonAndFilms($id: ID) {
                   person(id: $id) {
                       name
                       filmConnection {
                           films {
                               title
                           }
                       }
                   }
               }",
            OperationName = "PersonAndFilms",
            Variables = new
            {
               id = "cGVvcGxlOjE=",
            }
         };

         var graphQLClient = new GraphQLHttpClient("https://swapi.apis.guru/", new NewtonsoftJsonSerializer());

         GetResponse(graphQLClient, personAndFilmsRequest);

         Console.ReadKey();
      }

      private static async void GetResponse(GraphQLHttpClient client, GraphQLRequest request)
      {
         var graphQLResponse = await client.SendQueryAsync<PersonAndFilmsResponse>(request);

         var personName = graphQLResponse.Data.Person.Name;
      }

      private class PersonAndFilmsResponse
      {
         public PersonContent Person { get; set; }

         public class PersonContent
         {
            public string Name { get; set; }
            public FilmConnectionContent FilmConnection { get; set; }

            public class FilmConnectionContent
            {
               public List<FilmContent> Films { get; set; }

               public class FilmContent
               {
                  public string Title { get; set; }
               }
            }
         }
      }
   }
}
