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
        // Modules
        var cSharp = new Module
        {
          Title = "C#",
          Description = "C# is the most commonly used language for leveraging the .NET Framework. " +
          "As such, learning C# is a springboard to creating enterprise systems, desktop applications, websites and mobile applications.",
          Necessity = Domain.Enums.NecessityLevel.MustKnow,
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
