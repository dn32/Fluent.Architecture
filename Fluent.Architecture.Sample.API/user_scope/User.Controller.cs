using Fluent.Architecture.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("api/[Controller]")]
public class UserController : FluentController<User>
{
    // GET api/user/Find?code=123
    [HttpGet("Find")]
    public User Find([FromQuery] User entity)
    {
        return Service.Find(entity);
    }

    // GET api/user/FindByTerm?code=123
    [HttpGet("FindByTerm")]
    public object FindByTerm(string term)
    {
        return new { list = Service.FindByTerm(term), pagination = LastRequestPagination, term };
    }

    // GET api/user/GetUserByEMail?email=dn@dn32.com.br
    [HttpGet("GetUserByEMail")]
    public User GetUserByEMail(string email)
    {
        var spec = CreateSpec<UserByEmailSpec>().AddParameter(email);
        return Service.FirstOrDefault(spec);
    }

    // GET api/user/Count
    [HttpGet("Count")]
    public object Count()
    {
        return Service.Count();
    }

    // POST api/user
    [HttpPost]
    public User Add([FromBody] User entity)
    {
        return Service.Add(entity);
    }

    // PUT api/user
    [HttpPut]
    public User Update([FromBody] User entity)
    {
        return Service.Update(entity);
    }

    // DELETE api/user
    [HttpDelete]
    public User Remove([FromBody] User entity)
    {
        return Service.Remove(entity);
    }

    // DELETE api/user/RemoveRange
    [HttpDelete("RemoveRange")]
    public void RemoveRange([FromBody] User[] entidades)
    {
        Service.RemoveRange(entidades);
    }
}

