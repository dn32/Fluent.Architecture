

# Fluent Architecture
#### Uma arquitetura que pretende simplificar a forma como se escreve códigos para o dia-a-dia
Ferramentas necessárias:
- Visual Studio 2017/2019 ou Visual Studio Code

---

### 1° Passo:
Crie uma aplicação web por meio do Visual Studio.
Os seguintes frameworks são compatíveis:
 - **Net Framework 4.6.1**
 - **Net Core 2.1**
 - **Net Core 2.2**

### 2° Passo:
Adicione a referencia do pacote ao seu projeto por meio do gerenciador de pacotes do nuget

{{ "{% endhighlight " }}%}  nuget
Install-Package Fluent.Architecture.Core
Install-Package Fluent.Architecture.EntityFramework
{{ "{% endhighlight " }}%}  
Escolha os pacotes abaixo de acordo com os bancos de dados a serem usados na aplicação:
{{ "{% endhighlight " }}%}  nuget
Install-Package Fluent.Architecture.EntityFramework.MySQL
Install-Package Fluent.Architecture.EntityFramework.Oracle
Install-Package Fluent.Architecture.EntityFramework.PostgreSQL
Install-Package Fluent.Architecture.EntityFramework.SqlServer
{{ "{% endhighlight " }}%}  

### 3° Passo:
Crie uma classe que será responsável pela inicialização da arquitetura, conforme exemplo abaixo:
{{ "{% highlight C#" }}%}  
using Fluent.Architecture;
using Fluent.Architecture.EntityFramework;
using Fluent.Architecture.EntityFramework.PostgreSQL;
using System;

using Fluent.Architecture;
using Fluent.Architecture.EntityFramework;
using Fluent.Architecture.EntityFramework.MySQL;
using System;

public class ArchitectureInit
{
    public static void Setup(IServiceProvider serviceProvider)
    {
        Fluent.Architecture.Setup
        .Init()
        .SetServiceProvider(serviceProvider)
        .UseEntityFramework()
        .AddConnectionString("Server=localhost;Database=testDb;Uid=root;Pwd=admin;", createDatabaseIfNotExists: true, typeof(EfContextMySQL))
        .Build()
        .Run();
    }
}
{{ "{% endhighlight " }}%}  
### 4º Passo:
À partir do Startup de sua aplicação, inicialize a arquitetura:

#### Exemplo no .Net Core
{{ "{% highlight C#" }}%}  
...
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseMvc();

    Inicializacao.Inicialize(app.ApplicationServices); //A inicialização da arquitetura
}
...
{{ "{% endhighlight " }}%}  
#### Exemplo no .Net Framework
{{ "{% highlight C#" }}%}  

public class WebApiApplication : System.Web.HttpApplication
{
    protected void Application_Start()
    {
        Inicializacao.Inicialize(null); //A inicialização da arquitetura
    }
}
{{ "{% endhighlight " }}%}  
### 5º Passo:
Crie uma entidade:
{{ "{% highlight C#" }}%}  
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Entities;
using Fluent.Architecture.EntityFramework;

[Table("Users"), DbType(FluentDbType.MYSQL)]
public class User : FluentEntity
{
    [Key]
    public int Code { get; set; }

    [FluentUniqueKey]
    public string Email { get; set; }

    public string Name { get; set; }
}
{{ "{% endhighlight " }}%}  
> Note que usamos o atributo _Table_ para indicar o nome da tabela do banco de dados e o atributo _DbType_ para indicar o tipo de banco de dados a ser usado por essa entidade.
> Uma entidade sempre deve ser criada para um tipo de banco de dados específico. Em casos em que se deseja criar um Model e não uma entidade, deve se tratar de forma um pouco diferente. Veja mais em:  [Entidade](Entidade)

### 6° passo:
Crie um controller  conforme abaixo:
{{ "{% endhighlight " }}%}  C++
using Fluent.Architecture.Controllers;
using Microsoft.AspNetCore.Mvc;

[Route("api/[Controller]")]
public class UserController : FluentController<User>
{
    // GET api/controller?code=123
    [HttpGet]
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
{{ "{% endhighlight " }}%}  

Todos os métodos do controller estão prontos para serem executados.
Execute a aplicação e teste os métodos.
Baixe o template o template do link abaixo e importe no seu postman para facilitar os testes.
[Template para importação](https://github.com/dn32/Fluent.Architecture/blob/master/Fluent.Architecture.Sample.API/postman/postman-user-v1.json)

### 7° Passo:
Crie uma especificação de consulta
{{ "{% highlight C#" }}%}  
using System;
using System.Linq;
using Fluent.Architecture.Specifications;

public class UserByEmailSpec : FluentSpecification<User>
{
    public string Email { get; set; }

    public UserByEmailSpec AddParameter(string email)
    {
        Email = email;
        return this;
    }

    public override IQueryable<User> Where(IQueryable<User> query)
    {
        return query.Where(x => x.Email.Equals(Email, StringComparison.InvariantCultureIgnoreCase));
    }

    public override IOrderedQueryable<User> Order(IQueryable<User> query)
    {
        return query.OrderBy(x => x.Name);
    }
}
{{ "{% endhighlight " }}%}  
> Uma especificação define um modelo de consulta e pode ser utilizada em vários métodos do FluentService<>.

### 8° passo:
Adicione esse novo método ao seu controller:
{{ "{% highlight C#" }}%}  
[HttpGet("GetUserByEMail")]
public User GetUserByEMail(string email)
{
    var spec = CreateSpec<UserByEmailSpec>().AddParameter(email);
    return Service.FirstOrDefault(spec);
}
{{ "{% endhighlight " }}%}  
Execute a aplicação e acesse o endereço da aplicação.
Exemplo: http://localhost:5000/api/user/GetUserByEMail?email=dn@dn32.com.br

---

### 9° passo:
Adicione o filtro **FluentExceptionHandlerAttribute** de controle de exceções à classe FilterConfig ou equivalente, ficando como a seguir:

#### Exemplo no .Net Core
{{ "{% highlight C#" }}%}  
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc(options => options.Filters.Add(new FluentExceptionHandlerAttribute()));
}
{{ "{% endhighlight " }}%}  

#### Exemplo no .Net Framework
{{ "{% highlight C#" }}%}  
using Fluent.Architecture.Filters;

public class FilterConfig
{
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
        filters.Add(new HandleErrorAttribute());
        filters.Add(new FluentExceptionHandlerAttribute());
    }
}
{{ "{% endhighlight " }}%}  
Eventuais erros agora serão apresentados da seguinte forma:
{{ "{% endhighlight " }}%}  C++
{
    Message:  "An entity with any of these keys already exists in the database: {Id:0}, {Email:'max@mail.com'}",
    ValidationError:  true
}
{{ "{% endhighlight " }}%}  
O erro de validação refere-se ao e-mail repetido no banco de dados. Quando decoramos a propriedade Email de User com **FluentUniqueKey**, informamos para o sistema que não é permitido duplicação do valor desse campo.

Veja mais sobre validação em [Validação](Validação).

### Está tudo pronto para iniciar o trabalho.
---
Não é obrigatória a criação de mais nenhum arquivo para o funcionamento básico da entidade *User*, mas se for necessário tratar uma regra de validação, regra de negócio, ou algo mais específico, faz-se necessário criar itens customizador.

É necessário entender cada ponto da arquitetura para executar com sucesso os procedimentos necessários para o desenvolvimento de um sistema limpo e bem feito com ela.
Vamos fazer uma abordagem mais detalhada nos próximos tópicos.

Sempre no rodapé da página será apresentado o item que deve ser visto após o atual para facilitar a sequência de atendimento.

Para facilitar o fluxo de aprendizado, sempre que uma informação não necessária para o entendimento geral da arquitetura, esse será marcado como Referência.
Veja um exemplo abaixo:

> **Referência**:
> Informações de referência são para consulta em momentos de necessidade e não para aprendizado imediato.

---
Pra prosseguir, veja o item [Entidade](Entidade)
