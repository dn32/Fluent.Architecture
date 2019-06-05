using Fluent.Architecture.Controllers;
using Microsoft.AspNetCore.Mvc;

[Route("api/[Controller]")]
public class UserController : FluentController<User>
{
    // GET api/controller/Find?code=123
    [HttpGet("Find")]
    public User Find([FromQuery] User entity)
    {
        return Service.Find(entity);
    }

    // GET api/controller/Count
    [HttpGet("Count")]
    public object Count()
    {
        return Service.Count();
    }

    // POST api/controller
    [HttpPost]
    public User Add([FromBody] User entity)
    {
        return Service.Add(entity);
    }

    // PUT api/controller
    [HttpPut]
    public User Update([FromBody] User entity)
    {
        return Service.Update(entity);
    }

    // DELETE api/controller
    [HttpDelete]
    public User Remove([FromBody] User entity)
    {
        return Service.Remove(entity);
    }

    // DELETE api/controller/RemoveRange
    [HttpDelete("RemoveRange")]
    public void RemoveRange([FromBody] User[] entidades)
    {
        Service.RemoveRange(entidades);
    }
}

