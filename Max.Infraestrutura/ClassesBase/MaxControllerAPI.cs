using Microsoft.AspNetCore.Mvc;

namespace Max.Infraestrutura.ClassesBase
{
    public abstract class MaxControllerAPI<T> : MaxController<T> where T : MaxEntidade
    {
        //// GET api/controller
        //[HttpGet]
        //public List<T> Get()
        //{
        //    var spec = CreateSpec<AllSpec<T>>();
        //    return Service.List(spec);
        //}

        // GET api/controller?codigo=123
        [HttpGet]
        public T Buscar([FromQuery] T entidate)
        {
            return Service.Find(entidate);
        }

        // POST api/controller
        [HttpPost]
        public T Adicionar([FromBody] T entidate)
        {
            return Service.Add(entidate);
        }

        // PUT api/controller
        [HttpPut]
        public T Atualizar([FromBody] T entidate)
        {
            return Service.Update(entidate);
        }

        //[HttpPut("UpdateRange")]
        //public object AtulizarVarios([FromBody] T[] entidades)
        //{
        //    return Service.UpdateRange(entidades);
        //}

        // DELETE api/controller
        [HttpDelete]
        public T Remover([FromBody] T entidate)
        {
            return Service.Remove(entidate);
        }

        [HttpDelete("DeleteRange")]
        public void RemoverVarios([FromBody] T[] entidades)
        {
            Service.RemoveRange(entidades);
        }
    }
}
