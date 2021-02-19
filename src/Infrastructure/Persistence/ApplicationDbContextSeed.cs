using DeveloperPath.Domain.Entities;
using DeveloperPath.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeveloperPath.Infrastructure.Persistence
{
  public static class ApplicationDbContextSeed
  {
    public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager)
    {
      var defaultUser = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

      if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
      {
        await userManager.CreateAsync(defaultUser, "Administrator1!");
      }
    }

    public static async Task SeedSampleDataAsync(ApplicationDbContext context)
    {
      // Seed, if necessary
      if (!context.TodoLists.Any())
      {
        //TODO: remove this block
        context.TodoLists.Add(new TodoList
        {
          Title = "Shopping",
          Items =
                    {
                        new TodoItem { Title = "Apples", Done = true },
                        new TodoItem { Title = "Milk", Done = true },
                        new TodoItem { Title = "Bread", Done = true },
                        new TodoItem { Title = "Toilet paper" },
                        new TodoItem { Title = "Pasta" },
                        new TodoItem { Title = "Tissues" },
                        new TodoItem { Title = "Tuna" },
                        new TodoItem { Title = "Water" }
                    }
        });

        await context.SaveChangesAsync();
      }


      if (!context.Paths.Any())
      {
        // Sources 
        var primitivesSrc = new Source
        {
          Title = "Types and variables",
          Url = "https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/introduction#types-and-variables",
          Order = 0,
          Type = Domain.Enums.SourceType.Documentation,
          Availability = Domain.Enums.AvailabilityLevel.Free,
          Relevance = Domain.Enums.RelevanceLevel.Relevant
        };
        var tagSrc1 = new Source
        {
          Title = "Tag Helpers",
          Url = "https://docs.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/intro",
          Order = 0,
          Type = Domain.Enums.SourceType.Documentation,
          Availability = Domain.Enums.AvailabilityLevel.Free,
          Relevance = Domain.Enums.RelevanceLevel.Relevant
        };
        var tagSrc2 = new Source
        {
          Title = "Tag-хелперы",
          Url = "https://metanit.com/sharp/aspnet5/10.1.php",
          Order = 1,
          Type = Domain.Enums.SourceType.Course,
          Availability = Domain.Enums.AvailabilityLevel.Free,
          Relevance = Domain.Enums.RelevanceLevel.Relevant
        };

        // Sections 
        var basics = new Section
        {
          Title = "Basics",
          Order = 0
        };
        var advanced = new Section
        {
          Title = "Advanced",
          Order = 2
        };

        // Themes
        var primitiveTypes = new Theme
        {
          Title = "Primitive types",
          Description = "Learn about C# built-in data types.",
          Complexity = Domain.Enums.ComplexityLevel.Beginner,
          Necessity = Domain.Enums.NecessityLevel.MustKnow,
          Section = basics,
          Sources = new List<Source> { primitivesSrc },
          Order = 0
        };
        var classes = new Theme
        {
          Title = "Classes and objects",
          Description = "Classes are the most fundamental of C#’s types.",
          Complexity = Domain.Enums.ComplexityLevel.Beginner,
          Necessity = Domain.Enums.NecessityLevel.MustKnow,
          Section = basics,
          Order = 1
        };
        var interfaces = new Theme
        {
          Title = "Interfaces",
          Description = "An interface contains definitions for a group of related functionalities that a non-abstract class or a struct must implement.",
          Complexity = Domain.Enums.ComplexityLevel.Beginner,
          Necessity = Domain.Enums.NecessityLevel.MustKnow,
          Section = basics,
          Prerequisites = new List<Theme> { classes },
          Order = 2
        };
        var asyncronous = new Theme
        {
          Title = "Asynchronous programming",
          Description = "The core of async programming is the Task and Task<T> objects, which model asynchronous operations.",
          Complexity = Domain.Enums.ComplexityLevel.Advanced,
          Necessity = Domain.Enums.NecessityLevel.MustKnow,
          Section = advanced,
          Order = 0
        };

        var razor = new Theme
        {
          Title = "Razor",
          Description = "Introduction to ASP.NET Web Programming Using the Razor Syntax.",
          Complexity = Domain.Enums.ComplexityLevel.Beginner,
          Necessity = Domain.Enums.NecessityLevel.MustKnow,
          Order = 2
        };
        var tagHelpers = new Theme
        {
          Title = "Tag Helpers",
          Description = "Tag Helpers enable server-side code to participate in creating and rendering HTML elements in Razor files.",
          Complexity = Domain.Enums.ComplexityLevel.Intermediate,
          Necessity = Domain.Enums.NecessityLevel.GoodToKnow,
          Prerequisites = new List<Theme> { razor },
          Sources = new List<Source> { tagSrc1, tagSrc2 },
          Order = 2
        };

        // Modules
        var cSharp = new Module
        {
          Title = "C#",
          Description = "C# is the most commonly used language for leveraging the .NET Framework. " +
          "As such, learning C# is a springboard to creating enterprise systems, desktop applications, websites and mobile applications.",
          Necessity = Domain.Enums.NecessityLevel.MustKnow,
          Themes = new List<Theme> { primitiveTypes, classes, interfaces, asyncronous },
          Sections = new List<Section> { basics, advanced },
          Tags =
              new List<string>() {
                "Development", "Programming", "Languages"
              }
        };

        var aspNet = new Module
        {
          Title = "ASP.NET Core",
          Description = "ASP.NET Core is Microsoft's modern, cross-platform framework for building web applications and web APIs. " +
                "In this path, you will learn everything you need to know about building ASP.NET Core applications, " +
                "from building web applications with Razor to creating APIs.",
          Themes = new List<Theme> { razor, tagHelpers },
          Tags =
              new List<string>() {
                "Web", "Development", "Programming"
              },
          Prerequisites = new List<Module>() { cSharp },
          Necessity = Domain.Enums.NecessityLevel.MustKnow
        };

        // Paths
        context.Paths.Add(new Path
        {
          Title = "ASP.NET Developer",
          Description = "Learn how to design modern web applications using ASP.NET",
          Tags =
                new List<string>() {
                  "Web", "Development", "Programming"
                },
          Modules = new List<Module>() { cSharp, aspNet }
        });

        context.Paths.Add(new Path
        {
          Title = "Game Developer",
          Description = "Intelligent game design is important to the success of a shipped game. " +
              "This skill path will help you take your game idea and really flesh out the world and mechanics and then document them in your game design document. " +
              "Once you have everything documented you will prototype levels and mechanics to make sure you find maximum fun. " +
              "When this is done you will be ready to plan, document, and deliver your ideas to a team of developers that will bring your vision to life.",
          Tags =
                new List<string>() {
                  "Games", "Development", "Programming"
                },
          Modules = new List<Module>() { cSharp }
        });

        await context.SaveChangesAsync();
      }
    }
  }
}
