using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace FrontEnd.Data
{
    public static class MixSeedData
    {
        private class SessionData
        {
            public string Name { get; set; }

            public string[] Speakers { get; set; }

            public string Track { get; set; }

            public string Abstract { get; set; }
        }

        private class SessionGroup
        {
            public DateTimeOffset StartTime { get; set; }

            public SessionData[] Sessions { get; set; }
        }

        public static void Seed(IServiceProvider services)
        {
            // TODO: Make this like, good, and only in Dev
            using (var scope = services.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<ApplicationDbContext>();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                // Conference
                var conference = new Conference { Name = "DevIntersection Europe 2017" };
                db.Conferences.Add(conference);

                // Speakers
                var speakers = new[] {
                    "Carl Franklin",
                    "Don Wibier",
                    "Donna Malayeri",
                    "Jeff Fritz",
                    "Jessica Engström",
                    "Jimmy Engström",
                    "Luca Bolognese",
                    "Mads Torgersen",
                    "Miguel de Icaza",
                    "Mikkel Mork Heghoj",
                    "Paul Yuknewicz",
                    "Richard Campbell",
                    "Scott Cate",
                    "Scott Hunter",
                    "Seth Juarez",
                    "Shayne Boyer",
                    "Steve Guggenheimer",
                    "Tess Ferrandez",
                    "Tiberiu Covaci"
                    };

                var speakerLookup = new Dictionary<string, Speaker>();
                foreach (var s in speakers)
                {
                    var speaker = new Speaker
                    {
                        Name = s
                    };
                    db.Speakers.Add(speaker);
                    speakerLookup[s] = speaker;
                }

                // Tracks
                var tracks = new[] {
                    "Room 1",
                    "Room 2",
                    "Room 3"
                };

                var trackLookup = new Dictionary<string, Track>();
                foreach (var t in tracks)
                {
                    var track = new Track
                    {
                        Conference = conference,
                        Name = t
                    };
                    db.Tracks.Add(track);
                    trackLookup[t] = track;
                }

                void AddSessions(SessionGroup group)
                {
                    var end = group.StartTime + TimeSpan.FromHours(1);
                    foreach (var s in group.Sessions)
                    {
                        var session = new Session
                        {
                            Conference = conference,
                            Title = s.Name,
                            StartTime = group.StartTime,
                            EndTime = end,
                            Track = trackLookup[s.Track],
                            Abstract = s.Abstract
                        };

                        session.SessionSpeakers = new List<SessionSpeaker>();
                        foreach (var sp in s.Speakers)
                        {
                            session.SessionSpeakers.Add(new SessionSpeaker
                            {
                                Session = session,
                                Speaker = speakerLookup[sp]
                            });
                        }

                        db.Sessions.Add(session);
                    }
                }

                // Sessions

                var sessionGroups = new List<SessionGroup>();

                // 9:00 - 10:00
                var startTime = new DateTimeOffset(2017, 9, 18, 9, 0, 0, TimeSpan.FromHours(1));

                // Mon
                sessionGroups.Add(new SessionGroup
                {
                    StartTime = startTime,
                    Sessions = new[] {
                        new SessionData { Name = "Keynote: The History of .NET", Speakers = new[] { "Richard Campbell" }, Track = "Room 1",
                            Abstract = @"Join Richard Campbell as he sets the stage for our learning at DevIntersection by reviewing the history of the .NET Framework, Visual Studio .NET, and the supporting frameworks" }
                    }
                });

                // Tue
                sessionGroups.Add(new SessionGroup
                {
                    StartTime = startTime.AddDays(1),
                    Sessions = new[] {
                        new SessionData { Name = "Keynote: Transforming the World with AI", Speakers = new[] { "Steve Guggenheimer" }, Track = "Room 1" }
                    }
                });

                // 10:30 - 11:30
                startTime = startTime + TimeSpan.FromHours(1) + TimeSpan.FromMinutes(30);

                // Mon
                sessionGroups.Add(new SessionGroup
                {
                    StartTime = startTime,
                    Sessions = new[]
                    {
                         new SessionData { Name = "GETTING STARTED DEVELOPING .NET CLOUD APPLICATIONS ON AZURE USING VISUAL STUDIO", Speakers= new [] { "Mikkel Mork Heghoj"}, Track="Room 1",
                            Abstract=@"How easy is it to get .NET application to run in Azure? Do .NET applications run better in Azure, and how? In this talk we will walk through the unique enhancements we have built in to Azure for giving your .NET applications the best fit in the cloud. We’ll show you a fast path to get existing apps up on Azure. We’ll also show you how to build modern scalable cloud app patterns for Web App and worker microservices using Docker containers. We’ll also show you how to be incredibly productive building and debugging your app using Visual Studio."},
                        new SessionData { Name="What’s new in C#", Speakers= new[] {"Mads Torgersen"}, Track="Room 2",
                            Abstract=@"In this demo-filled talk, Mads goes over what’s new in C# 7.0 and 7.1. Showing tuples, deconstruction, local functions, pattern matching and more, he’ll focus both on the language features and the support for them in Visual Studio. At the end he’ll show a glimpse of what’s being worked on for future versions of C#." },
                        new SessionData { Name="Holo world - Create your first HoloLens app with Unity", Speakers=new [] {"Jimmy Engström"}, Track="Room 3",
                            Abstract=@"HoloLens breached a barrier, a barrier between the digital and the real world, bringing digital content into our world.
Over the past year the HoloLens has continued to create new ways to visualize, to change education and the medical industry.
HoloLens is not only for games it is a Business platform (that is game enabled)
Using Unity we can create an experience that lets you bring digital content and interact with the real world.
During this session we will take a look at the HoloLens hardware, the possibilities, the limitations and tooling.
We will take a look at the basics of Unity and also create a HoloLens app from scratch complete with Gaze, Air Tap, Spatial mapping and Voice using HoloToolkit." }
                    }
                });

                // Tue
                sessionGroups.Add(new SessionGroup
                {
                    StartTime = startTime.AddDays(1),
                    Sessions = new[]
                    {
                        new SessionData { Name = "Your Favorite JavaScript Frameworks Meet ASP.NET", Speakers = new[] { "Shayne Boyer" }, Track = "Room 1",
                            Abstract = @"Client-side JavaScript and progressive web applications have evolved significantly at the same time as ASP.NET Core. In this code-filled session, We will show you how to use React, Angular, and Knockout with the latest ASP.NET Core to deliver web applications your customers will love." },
                        new SessionData { Name="Powerful Productivity Tips and Tricks for Visual Studio 2017", Speakers=new[] { "Scott Hunter"}, Track="Room 2",
                            Abstract=@"With the third update to Visual Studio 2017, a new collection of tools are now available to you. In this demo-filled session, Scott Hunter will show you how you can use the new features like .NET Core 2.0, ASP.NET Core 2.0, Docker Container, Live Unit Testing, C# 7.1 and Code Style checking to make your software development efforts quicker, simpler and more fun."},
                        new SessionData { Name="Machine Learning 101 – just enough to impress your boss", Speakers=new[] {"Tess Ferrandez"}, Track="Room 3",
                            Abstract=@"If you know everything there is to know about regression algorithms, K-nearest neighbors and SVMs, this session is probably not for you. But if you want to know what they hype is all about, what problems you can solve, and how to create basic Machine Learning experiments then you are very welcome. We’ll also have a look at how you can impress your boss by using the results of other people’s machine learning experiments – like cognitive services."}
                    }
                });

                // 11:45 - 12:45
                startTime = startTime + TimeSpan.FromHours(1) + TimeSpan.FromMinutes(15);

                // Mon
                sessionGroups.Add(new SessionGroup
                {
                    StartTime = startTime,
                    Sessions = new[]
                    {
                        new SessionData { Name = "Quick Intro to Modern JavaScript", Speakers = new[] { "Tiberiu Covaci" }, Track = "Room 1" },
                        new SessionData { Name = "Introducing ASP.NET Core 2.0", Speakers = new[] { "Jeff Fritz" }, Track = "Room 2" },
                        new SessionData { Name = "Designing for Speech", Speakers = new[] { "Jessica Engström" }, Track = "Room 3" },
                    }
                });

                startTime = startTime + TimeSpan.FromMinutes(15);

                // Tue
                sessionGroups.Add(new SessionGroup
                {
                    StartTime = startTime.AddDays(1),
                    Sessions = new[]
                    {
                        new SessionData { Name = "Bulletproof Transient Error Handling with Polly", Speakers = new[] { "Carl Franklin" }, Track = "Room 1" },
                        new SessionData { Name = "Azure Diagnostics: Fixing Cloud Application Issues and Performance on Azure", Speakers = new[] { "Paul Yuknewicz" }, Track = "Room 2" },
                        new SessionData { Name = "Practical Machine Learning - Predicting Things", Speakers = new[] { "Seth Juarez" }, Track = "Room 3" },
                    }
                });

                foreach (var group in sessionGroups)
                {
                    AddSessions(group);
                }

                db.SaveChanges();
            }
        }
    }
}