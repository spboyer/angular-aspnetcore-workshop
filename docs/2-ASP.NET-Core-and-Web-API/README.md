# ASP.NET Core and Web API overview

Now that you are setup with .NET Core, we'll do an overview of ASP.NET Core and how to build your first ASP.NET Core app. Then we'll dive into building Web APIs with ASP.NET Core.

## Prerequisites
* [Visual Studio 2017](https://www.visualstudio.com/thank-you-downloading-visual-studio/?sku=Community&rel=15)
* [Visual Studio Code](https://code.visualstudio.com)
* [.NET Core 2.0 SDK](https://www.microsoft.com/net/download/core)

### Creating a new ASP.NET Core app

1. From the command-line, run `dotnet new web -o WebApp` or from Visual Studio create a new  ASP.NET Core web application using the Empty template
1. Type `dotnet run` to run the application or hit ctrl-F5 in Visual Studio
1. Browse to http://localhost:5000. You'll see a simple "Hello World" message.
1. Hit `ctrl-c` to stop the app from the command-line
1. Setup MVC
    1. In `ConfigureServices()` add `services.AddMvc()` to add the MVC services
    1. In `Configure()` replace the `app.Run(...)` code with `app.UseMvc()`
1. Add a `Pages` folder to your app
1. In the `Pages` folder add a new `Index.cshtml` file with the following content:

    ```html
    @page

    <h1>Hello from a Razor Page! The time is @DateTime.Now</h1>
    ```

    > Alternatively, create the page from the command-line by running `dotnet new page -o Pages -n Index -np` or use the Razor Page item template in Visual Studio

1. Run the app again using `dotnet run`
1. Browse to http://localhost:5000 and hit refresh a couple of times to see the content change
1. Check out https://docs.microsoft.com/en-us/aspnet/core/mvc/overview to learn more about building web apps with ASP.NET Core MVC and Razor Pages

### Create a Web API using ASP.NET Core

1. Add a `Controllers` folder to your project
1. In the `Controllers` folder add a `ValuesController.cs` file with the following content:

    ```c#
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    namespace WebApp.Controllers
    {
        [Route("api/[controller]")]
        public class ValuesController : ControllerBase
        {
            static Dictionary<int, Value> values = new Dictionary<int, Value>();

            // GET: api/values
            [HttpGet]
            public IEnumerable<Value> Get()
            {
                return values.Values;
            }

            // GET api/values/5
            [HttpGet("{id}")]
            public IActionResult Get(int id)
            {
                if (!values.ContainsKey(id))
                {
                    return NotFound();
                }
                return Ok(values[id]);
            }

            // POST api/values
            [HttpPost]
            public IActionResult Post([FromBody] Value value)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                value.Id = values.Keys.Count == 0 ? 0 : values.Keys.Max() + 1;
                values[value.Id] = value;

                return CreatedAtAction(nameof(Get), new { id = value.Id }, value);
            }

            // PUT api/values/5
            [HttpPut("{id}")]
            public IActionResult Put(int id, [FromBody] Value value)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                value.Id = id;
                values[id] = value;
                return Ok(value);
            }

            // DELETE api/values/5
            [HttpDelete("{id}")]
            public IActionResult Delete(int id)
            {
                if (!values.ContainsKey(id))
                {
                    return NotFound();
                }

                var value = values[id];
                values.Remove(id);
                return Ok(value);
            }
        }

        public class Value
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }
    }
    ```

    > You can alternatively create an ASP.NET Core project with a Web API by running `dotnet new webapi` from the command-line or using the ASP.NET Core Web API template in Visual Studio. Visual Studio also provides item templates and scaffolders for generating Web API controllers. We'll use the scaffolders in an later lab.

    > The values for this Web API are held in memory using a static field on the controller. These values will be lost each time the app restarts. In a later lab we will use a proper data store.

1. Using [Postman](https://www.getpostman.com/) or similar tool send some POST requests to create some values. Not the value of the Location header in the responses
1. Issue some GET requests to request individual values or all the values
1. Try deleting and updating some values using DELETE and PUT requests
1. Try requesting or updating a value that does not exist. What response code do you get?
1. Try specifying an ID that is not an integer. Update your routes to constrain the `id` route value to be an integer by adding `{id:int}`.
1. Try changing the path to the Web API by changing the route on the controller. 

    > The `[controller]` route token is automatically replaced with the controller name. Verify this by renaming the controller class.